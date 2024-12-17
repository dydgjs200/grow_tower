using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public List<PlayerInfo> playerInfos; // 플레이어 데이터를 저장하는 리스트
    private string persistentPath;       // 저장된 데이터 경로
    private string streamingPath;        // 초기 데이터 경로

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 경로 설정
        persistentPath = Path.Combine(Application.persistentDataPath, "grow_tower_players.json");
        streamingPath = Path.Combine(Application.streamingAssetsPath, "grow_tower_players.json");

        // 플레이어 데이터 불러오기
        StartCoroutine(LoadPlayerData());
    }

    private IEnumerator LoadPlayerData()
    {
        // 먼저 persistentDataPath에 파일이 있는지 확인
        if (File.Exists(persistentPath))
        {
            LoadFromFile(persistentPath);
        }
        else
        {
            // StreamingAssets에서 JSON 파일 불러오기
            UnityWebRequest request = UnityWebRequest.Get(streamingPath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Failed to load player data from StreamingAssets: " + request.error);
            }
            else
            {
                string jsonContent = request.downloadHandler.text;

                // persistentDataPath로 파일 복사
                File.WriteAllText(persistentPath, jsonContent);
                Debug.Log("Player data copied to persistentDataPath.");

                // 파일 로드
                LoadFromFile(persistentPath);
            }
        }
    }

    private void LoadFromFile(string filePath)
    {
        try
        {
            string jsonContent = File.ReadAllText(filePath);
            playerInfos = JsonConvert.DeserializeObject<List<PlayerInfo>>(jsonContent);

            if (playerInfos != null && playerInfos.Count > 0)
            {
                Debug.Log("Player data loaded successfully.");
            }
            else
            {
                Debug.LogError("No player data found in JSON.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading player data: {ex.Message}");
        }
    }

    public void SavePlayerData()
    {
        try
        {
            string jsonContent = JsonConvert.SerializeObject(playerInfos, Formatting.Indented);
            File.WriteAllText(persistentPath, jsonContent);
            Debug.Log("Player data saved successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving player data: {ex.Message}");
        }
    }

    public PlayerInfo GetPlayerInfo(int index)
    {
        if (playerInfos != null && index >= 0 && index < playerInfos.Count)
        {
            return playerInfos[index];
        }
        else
        {
            Debug.LogError($"PlayerInfo at index {index} not found!");
            return null;
        }
    }
}
