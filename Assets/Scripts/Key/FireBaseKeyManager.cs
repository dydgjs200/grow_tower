using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Firebase.RemoteConfig;
using Firebase.Extensions;

public class FirebaseKeyManager : MonoBehaviour
{
    public static FirebaseKeyManager Instance { get; private set; }
    private string encryptionKey;

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
        }
    }

    private void Start()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Firebase initialized successfully.");
                FetchAndActivate();
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {task.Result}");
            }
        });
    }

    private void FetchAndActivate()
    {
        FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(fetchTask =>
        {
            if (fetchTask.IsCompleted && FirebaseRemoteConfig.DefaultInstance.Info.LastFetchStatus == LastFetchStatus.Success)
            {
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(activateTask =>
                {
                    encryptionKey = FirebaseRemoteConfig.DefaultInstance.GetValue("JsonEncryptor").StringValue;

                    if (string.IsNullOrEmpty(encryptionKey))
                    {
                        Debug.LogError("Encryption key is empty or null.");
                        return;
                    }

                    Debug.Log($"Encryption Key retrieved: {encryptionKey}");

                    // 복호화 및 로그 출력
                    string filePath = Path.Combine(Application.persistentDataPath, "grow_tower_enemies.txt");
                    DecryptAndLogJson(filePath);
                });
            }
            else
            {
                Debug.LogError("Failed to fetch remote config values.");
            }
        });
    }

    private void EncryptAndSaveJson(string json, string filePath)
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            Debug.LogError("Encryption key is not available. Encryption aborted.");
            return;
        }

        string encryptedJson = EncryptJson(json);

        if (!string.IsNullOrEmpty(encryptedJson))
        {
            File.WriteAllText(filePath, encryptedJson);
            Debug.Log($"Encrypted JSON saved to: {filePath}");
        }
        else
        {
            Debug.LogError("Failed to encrypt JSON.");
        }
    }

    private string EncryptJson(string json)
    {
        try
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptionKey.PadRight(32).Substring(0, 32)); // AES-256 키 조정
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length); // IV를 JSON 앞부분에 저장
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(json);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Encryption failed: {ex.Message}");
            return null;
        }
    }

    private void DecryptAndLogJson(string filePath)
    {
        if (string.IsNullOrEmpty(encryptionKey))
        {
            Debug.LogError("Encryption key is not available. Decryption aborted.");
            return;
        }

        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        try
        {
            string encryptedJson = File.ReadAllText(filePath);
            string decryptedJson = DecryptJson(encryptedJson);

            if (!string.IsNullOrEmpty(decryptedJson))
            {
                Debug.Log($"Decrypted JSON content:\n{decryptedJson}");
            }
            else
            {
                Debug.LogError("Decryption failed. Decrypted content is null or empty.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error reading or decrypting file: {ex.Message}");
        }
    }

    private string DecryptJson(string encryptedJson)
    {
        try
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedJson);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptionKey.PadRight(32).Substring(0, 32)); // AES-256 키 조정
                byte[] iv = new byte[16];
                Array.Copy(encryptedBytes, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Decryption failed: {ex.Message}");
            return null;
        }
    }
}
