using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public List<PlayerInfo> playerInfos; // 플레이어 데이터를 저장하는 리스트
    public string jsonPath;

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

        jsonPath = Path.Combine(Application.streamingAssetsPath, "grow_tower_players.json");
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        if (File.Exists(jsonPath))
        {
            string jsonContent = File.ReadAllText(jsonPath);

            // JSON 배열을 List<PlayerInfo>로 변환
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
        else
        {
            Debug.LogError("Player data file not found: " + jsonPath);
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
