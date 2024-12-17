using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class JsonLoader : MonoBehaviour
{
    public static JsonLoader Instance { get; private set; } // 싱글톤 인스턴스
    public Root RootData { get; private set; }              // 로드된 JSON 데이터

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
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
            Debug.LogError($"JSON 파일을 불러오는 데 실패했습니다: {request.error}");
            RootData = null;
        }
        else
        {
            string jsonData = request.downloadHandler.text;
            Debug.Log($"JSON 데이터 로드 성공: {jsonData}");
            RootData = JsonUtility.FromJson<Root>(jsonData);

            if (RootData == null)
            {
                Debug.LogError("JSON 데이터를 파싱하는 데 실패했습니다.");
            }
        }
    }
}
