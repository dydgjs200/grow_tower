using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // 프리팹
    public Vector3 spawnPosition;  // 생성 위치

    private void Start()
    {
        PlayerManager playerManager = PlayerManager.Instance;

        if (playerManager != null)
        {
            // JSON 배열에서 첫 번째 플레이어 데이터를 가져와 생성
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
        // 프리팹 생성
        GameObject newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        PlayerController playerController = newPlayer.GetComponent<PlayerController>();

        if (playerController != null)
        {
            // PlayerController 초기화
            playerController.InitializedPlayer(playerId, hp, damage, attackSpeed);
            Debug.Log($"Player {playerId} created successfully.");
        }
        else
        {
            Debug.LogError("PlayerController not found on the prefab.");
        }
    }
}
