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

    private readonly string encryptionKey = "YourEncryptionKey123"; // 16, 24, �Ǵ� 32 ������ Ű ���

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ���� �����͸� �а� Prefab �ʱ�ȭ
            LoadLocalPlayerData().ContinueWith(_ =>
            {
                Debug.Log("Player Prefab �ʱ�ȭ �Ϸ�");
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
                Debug.LogWarning($"���� JSON ������ �������� �ʽ��ϴ�: {jsonFilePath}");
                return;
            }

            string encryptedJson = await Task.Run(() => File.ReadAllText(jsonFilePath));
            string decryptedJson = Decrypt(encryptedJson);
            Debug.Log($"��ȣȭ�� JSON ������: {decryptedJson}");

            // JSON �����͸� Dictionary<string, PlayerInfo>�� ������ȭ
            LocalPlayerDict = JsonConvert.DeserializeObject<Dictionary<string, PlayerInfo>>(decryptedJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"���� JSON ������ �ε� ����: {ex.Message}");
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
            Debug.Log("���� JSON ������ ���� �Ϸ�");
        }
        catch (Exception ex)
        {
            Debug.LogError($"���� JSON ������ ���� ����: {ex.Message}");
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
