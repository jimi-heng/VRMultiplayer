using System.IO;
using UnityEngine;
using TMPro;

public class SaveToCSV : MonoBehaviour
{
    public TMP_InputField inputField; // assign in the inspector
    public string fileName = "output.csv"; // file name to save

    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log("CSV will be saved to: " + filePath);
    }

    public void SaveInputToCSV()
    {
        if (inputField == null)
        {
            Debug.LogWarning("InputField not assigned!");
            return;
        }

        string text = inputField.text;

        // If you have multiple columns, split or prepare them here
        // For example: string csvLine = $"\"{column1}\",\"{column2}\"";
        string csvLine = $"\"{text}\"";

        // Save to file (append mode, so each input is a new line)
        File.AppendAllText(filePath, csvLine + "\n");

        Debug.Log("Saved to CSV: " + csvLine);
    }
}
