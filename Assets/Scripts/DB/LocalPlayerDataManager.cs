using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class LocalPlayerDataManager : MonoBehaviour
{
    public static LocalPlayerDataManager Instance { get; private set; }
    public Dictionary<string, PlayerInfo> LocalPlayerDict = new Dictionary<string, PlayerInfo>();
    public GameObject PlayerPrefab;

    private readonly string encryptionKey = "YourEncryptionKey123"; // 16, 24, 또는 32 글자의 키 사용

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 로컬 데이터를 읽고 Prefab 초기화
            LoadLocalPlayerData().ContinueWith(_ =>
            {
                Debug.Log("Player Prefab 초기화 완료");
            });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async Task LoadLocalPlayerData()
    {
        try
        {
            string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_players.json");

            if (!File.Exists(jsonFilePath))
            {
                Debug.LogWarning($"로컬 JSON 파일이 존재하지 않습니다: {jsonFilePath}");
                return;
            }

            string encryptedJson = await Task.Run(() => File.ReadAllText(jsonFilePath));
            string decryptedJson = Decrypt(encryptedJson);
            Debug.Log($"복호화된 JSON 데이터: {decryptedJson}");

            // JSON 데이터를 Dictionary<string, PlayerInfo>로 역직렬화
            LocalPlayerDict = JsonConvert.DeserializeObject<Dictionary<string, PlayerInfo>>(decryptedJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"로컬 JSON 데이터 로드 실패: {ex.Message}");
        }
    }

    public async Task SaveLocalPlayerData()
    {
        try
        {
            string jsonFilePath = Path.Combine(Application.persistentDataPath, "grow_tower_players.json");
            string json = JsonConvert.SerializeObject(LocalPlayerDict, Formatting.Indented);
            string encryptedJson = Encrypt(json);

            await Task.Run(() => File.WriteAllText(jsonFilePath, encryptedJson));
            Debug.Log("로컬 JSON 데이터 저장 완료");
        }
        catch (Exception ex)
        {
            Debug.LogError($"로컬 JSON 데이터 저장 실패: {ex.Message}");
        }
    }

    public PlayerInfo GetPlayerInfo(string id)
    {
        if (LocalPlayerDict.ContainsKey(id))
        {
            return LocalPlayerDict[id];
        }
        else
        {
            return null;
        }
    }

    private string Encrypt(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
        byte[] ivBytes = new byte[16];
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using (StreamWriter writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(plainText);
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    private string Decrypt(string encryptedText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
        byte[] ivBytes = new byte[16];
        byte[] cipherBytes = Convert.FromBase64String(encryptedText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
            {
                using (StreamReader reader = new StreamReader(cryptoStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
