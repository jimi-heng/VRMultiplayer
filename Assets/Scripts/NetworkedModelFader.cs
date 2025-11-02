using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class NetworkedModelFader : NetworkBehaviour
{
    public GameObject currentModel;
    public GameObject newModel;
    public float fadeDuration = 2f;

    private void Start()
    {
        if (IsClient || IsServer)
        {
            // 初始状态（在所有客户端和服务器执行）
            newModel.SetActive(false);
            SetModelAlpha(newModel, 0f);
            SetModelAlpha(currentModel, 1f);
        }
    }

    // 客户端调用入口
    public void RequestFade()
    {
        StartFadeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartFadeServerRpc()
    {
        StartCoroutine(FadeModels());
    }

    private IEnumerator FadeModels()
    {
        // 通知所有客户端激活新模型
        SetModelActiveClientRpc(true, true); // 激活新模型
        SetModelAlphaClientRpc(true, 0f);    // 初始透明

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            // 服务器本地更新
            SetModelAlpha(currentModel, 1f - progress);
            SetModelAlpha(newModel, progress);

            // 同步到所有客户端
            SetModelAlphaClientRpc(false, 1f - progress); // 旧模型渐隐
            SetModelAlphaClientRpc(true, progress);       // 新模型渐显

            yield return null;
        }

        // 完成后禁用旧模型
        SetModelActiveClientRpc(false, false);
    }

    [ClientRpc]
    private void SetModelAlphaClientRpc(bool isNewModel, float alpha)
    {
        GameObject target = isNewModel ? newModel : currentModel;
        SetModelAlpha(target, alpha);
    }

    [ClientRpc]
    private void SetModelActiveClientRpc(bool isNewModel, bool active)
    {
        GameObject target = isNewModel ? newModel : currentModel;
        target.SetActive(active);
    }

    private void SetModelAlpha(GameObject model, float alpha)
    {
        if (model == null) return;

        foreach (var renderer in model.GetComponentsInChildren<Renderer>())
        {
            foreach (var material in renderer.materials)
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;

                if (alpha < 1f)
                {
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.renderQueue = 3000;
                }
            }
        }
    }
}