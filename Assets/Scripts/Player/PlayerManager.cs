using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public List<PlayerInfo> playerInfos; // �÷��̾� �����͸� �����ϴ� ����Ʈ
    private string persistentPath;       // ����� ������ ���
    private string streamingPath;        // �ʱ� ������ ���

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

        // ��� ����
        persistentPath = Path.Combine(Application.persistentDataPath, "grow_tower_players.json");
        streamingPath = Path.Combine(Application.streamingAssetsPath, "grow_tower_players.json");

        // �÷��̾� ������ �ҷ�����
        StartCoroutine(LoadPlayerData());
    }

    private IEnumerator LoadPlayerData()
    {
        // ���� persistentDataPath�� ������ �ִ��� Ȯ��
        if (File.Exists(persistentPath))
        {
            LoadFromFile(persistentPath);
        }
        else
        {
            // StreamingAssets���� JSON ���� �ҷ�����
            UnityWebRequest request = UnityWebRequest.Get(streamingPath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Failed to load player data from StreamingAssets: " + request.error);
            }
            else
            {
                string jsonContent = request.downloadHandler.text;

                // persistentDataPath�� ���� ����
                File.WriteAllText(persistentPath, jsonContent);
                Debug.Log("Player data copied to persistentDataPath.");

                // ���� �ε�
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
