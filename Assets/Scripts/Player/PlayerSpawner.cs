using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // ������
    public Vector3 spawnPosition;  // ���� ��ġ

    private void Start()
    {
        PlayerManager playerManager = PlayerManager.Instance;

        if (playerManager != null)
        {
            // JSON �迭���� ù ��° �÷��̾� �����͸� ������ ����
            PlayerInfo playerData = playerManager.GetPlayerInfo(0); // Index 0

            if (playerData != null)
            {
                CreatePlayer(playerData.PlayerID, playerData.HP, playerData.Damage, playerData.AttackSpeed);
            }
            else
            {
                Debug.LogError("Player data could not be loaded.");
            }
        }
        else
        {
            Debug.LogError("PlayerManager is not initialized!");
        }
    }

    private void CreatePlayer(string playerId, float hp, float damage, float attackSpeed)
    {
        // ������ ����
        GameObject newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        PlayerController playerController = newPlayer.GetComponent<PlayerController>();

        if (playerController != null)
        {
            // PlayerController �ʱ�ȭ
            playerController.InitializedPlayer(playerId, hp, damage, attackSpeed);
            Debug.Log($"Player {playerId} created successfully.");
        }
        else
        {
            Debug.LogError("PlayerController not found on the prefab.");
        }
    }
}
