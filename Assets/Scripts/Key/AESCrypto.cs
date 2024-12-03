using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AESCrypto
{
    private byte[] key; // ��ȣȭ�� ���Ǵ� Ű
    private byte[] iv; // �ʱ�ȭ ���� 
    private readonly string keyPath = Path.Combine(Application.persistentDataPath, "aesKey.dat");
    private readonly string ivPath = Path.Combine(Application.persistentDataPath, "aesIV.dat");

    public AESCrypto()
    {
        if (File.Exists(keyPath) && File.Exists(ivPath)) // Ű�� IV�� �����ϴ� �� Ȯ��
        {
            // �����Ѵٸ� �ش� Ű�� IV�� �о��
            key = File.ReadAllBytes(keyPath);
            iv = File.ReadAllBytes(ivPath);
        }
        else
        {
            // ���ٸ� �ٽ� ����
            key = GenerateRandomBytes(32); // 256-bit key
            iv = GenerateRandomBytes(16);  // 128-bit IV

            File.WriteAllBytes(keyPath, key);
            File.WriteAllBytes(ivPath, iv);
        }
    }

    // ������ ������ ���� ����Ʈ �迭�� ����
    private byte[] GenerateRandomBytes(int length)
    {
        byte[] randomBytes = new byte[length];

        // RNGCryptoServiceProvider : ���� �߻���
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }
        return randomBytes;
    }

    public string EncryptString(string plainText)
    {
        // AES ��ü�� ���� ��ȣȭ ��ü�� ����Ƽ�� ���ҽ��� ����ϹǷ� ����Ŀ� �ݵ�� �����ؾ��Ѵ�.
        // using�� ����Ͽ� �۾��� ���� �Ŀ� �ڵ����� ���ҽ��� �����ǰ� �����ߴ�.
        // ����Ƽ�� ���ҽ��� � ü���� �ϵ����� ���������� ��ȣ�ۿ��ϴ� ���ҽ� (GC�� ���� �ڵ����� �������� �ʱ� ������ �������� ��������� ��)
        using (Aes aesAlg = Aes.Create()) // AES �˰��� ����
        {
            // Ű, IV ����
            aesAlg.Key = key;
            aesAlg.IV = iv;

            // ��ȣȭ ��ȯ�⸦ ����
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            // �� �ؽ�Ʈ�� ��ȣȭ
            byte[] encrypted = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);

            // ��ȣȭ�� ����Ʈ �迭�� Base64 ���ڿ��� ��ȯ �� ��ȯ
            return System.Convert.ToBase64String(encrypted);
        }
    }

    public string DecryptString(string cipherText) // ��ȣȭ �Լ�
    {
        // Base64 ���ڿ��� ����Ʈ �迭�� ��ȯ
        byte[] buffer = System.Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create()) // AES �˰��� ����
        {
            // Ű, IV ����
            aesAlg.Key = key;
            aesAlg.IV = iv;

            // ��ȣȭ ��ȯ�⸦ ����
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            // ��ȣȭ�� ����Ʈ �迭�� ��ȣȭ 
            byte[] decrypted = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

            // ��ȣȭ�� ����Ʈ �迭�� UTF-8 ���ڿ��� ��ȯ �� ��ȯ
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}