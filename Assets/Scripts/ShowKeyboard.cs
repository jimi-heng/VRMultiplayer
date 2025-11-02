using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEditor.ShaderGraph.Internal;

public class ShowKeyboard : MonoBehaviour
{
    private TMP_InputField inputField;

    public float distance = 0.5f;
    public float verticalOffset = -0.5f;
    public Transform positionSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener(x => OpenKeyboard());
    }

    // Update is called once per frame
    public void OpenKeyboard()
    {
        NonNativeKeyboard.Instance.InputField = inputField;
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);

        Vector3 direction = positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targertPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;

        NonNativeKeyboard.Instance.RepositionKeyboard(targertPosition);

        SetCaretColorAlpha(1);

        NonNativeKeyboard.Instance.OnClosed += Instance_Onclosed;
    }

    private void Instance_Onclosed(object sender, System.EventArgs e)
    {
        SetCaretColorAlpha (0);
        NonNativeKeyboard.Instance.OnClosed -= Instance_Onclosed;
    }

    public void SetCaretColorAlpha(float value)
    {
        inputField.customCaretColor = true;
        Color caretColor = inputField.caretColor;
        caretColor.a = value;
        inputField.caretColor = caretColor;
    }
}
