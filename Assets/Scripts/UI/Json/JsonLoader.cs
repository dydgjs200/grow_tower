using System.IO;
using UnityEngine;

public class JsonLoader : MonoBehaviour
{
    public static JsonLoader Instance { get; private set; } // �̱��� �ν��Ͻ�
    public Root RootData { get; private set; }              // �ε�� JSON ������

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadJson(string filePath)
    {
        filePath = Path.Combine(Application.streamingAssetsPath, filePath);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"JSON ������ �������� �ʽ��ϴ�: {filePath}");
            RootData = null;
            return;
        }

        string jsonData = File.ReadAllText(filePath);
        Debug.Log($"Loaded JSON Data: {jsonData}"); // JSON ������ ���

        RootData = JsonUtility.FromJson<Root>(jsonData);

        if (RootData == null)
        {
            Debug.LogError("JSON �����͸� �Ľ��ϴ� �� �����߽��ϴ�.");
        }
    }

}
