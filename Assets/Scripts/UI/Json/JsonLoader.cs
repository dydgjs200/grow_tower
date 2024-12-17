using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

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
        StartCoroutine(LoadJsonFromFile(filePath));
    }

    private IEnumerator LoadJsonFromFile(string filePath)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, filePath);

        UnityWebRequest request = UnityWebRequest.Get(fullPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"JSON ������ �ҷ����� �� �����߽��ϴ�: {request.error}");
            RootData = null;
        }
        else
        {
            string jsonData = request.downloadHandler.text;
            Debug.Log($"JSON ������ �ε� ����: {jsonData}");
            RootData = JsonUtility.FromJson<Root>(jsonData);

            if (RootData == null)
            {
                Debug.LogError("JSON �����͸� �Ľ��ϴ� �� �����߽��ϴ�.");
            }
        }
    }
}
