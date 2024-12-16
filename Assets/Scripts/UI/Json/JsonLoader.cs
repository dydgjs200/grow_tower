using System.IO;
using UnityEngine;

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
        filePath = Path.Combine(Application.streamingAssetsPath, filePath);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"JSON 파일이 존재하지 않습니다: {filePath}");
            RootData = null;
            return;
        }

        string jsonData = File.ReadAllText(filePath);
        Debug.Log($"Loaded JSON Data: {jsonData}"); // JSON 데이터 출력

        RootData = JsonUtility.FromJson<Root>(jsonData);

        if (RootData == null)
        {
            Debug.LogError("JSON 데이터를 파싱하는 데 실패했습니다.");
        }
    }

}
