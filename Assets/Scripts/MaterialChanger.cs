using UnityEngine;
using System.Collections;

public class MaterialChanger : MonoBehaviour
{
    public Material[] materials; // 要切换的材质数组
    public float changeInterval = 1f; // 切换间隔时间（秒）

    private Renderer rend;
    private int currentIndex = 0;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (materials.Length > 0)
        {
            rend.material = materials[0]; // 初始化为第一个材质
            StartCoroutine(ChangeMaterialOverTime());
        }
    }

    IEnumerator ChangeMaterialOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeInterval); // 等待指定时间

            currentIndex++;
            if (currentIndex >= materials.Length)
            {
                currentIndex = 0; // 如果到末尾就循环
            }

            rend.material = materials[currentIndex];
        }
    }
}
