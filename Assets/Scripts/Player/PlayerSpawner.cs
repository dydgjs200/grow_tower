using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Vector3 spawnPosition;

    private void Start()
    {
        FirebaseDataLoader dataLoader = FindObjectOfType<FirebaseDataLoader>();
        dataLoader.OnDataLoaded += OnPlayerDataLoaded;
        dataLoader.LoadPlayerData();
    }

    private void OnPlayerDataLoaded(Dictionary<string, Dictionary<string, object>> playersData)
    {
        PlayerLocalCache.Instance.InitializeCache(playersData);

        foreach (var playerId in playersData.Keys)
        {
            Dictionary<string, object> playerData = playersData[playerId];

            float hp = (float)playerData["HP"];
            float damage = (float)playerData["Damage"];
            float attackSpeed = (float)playerData["AttackSpeed"];

            Debug.Log($"PlayerId > {playerId}");

            CreatePlayer(playerId, hp, damage, attackSpeed);
        }
    }

    private void CreatePlayer(string playerId, float hp, float damage, float attackSpeed)
    {
        GameObject newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        PlayerController playerController = newPlayer.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.InitializedPlayer(playerId, hp, damage, attackSpeed);
        }
    }
}
