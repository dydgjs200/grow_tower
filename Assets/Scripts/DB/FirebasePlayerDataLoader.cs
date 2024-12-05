using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FirebasePlayerDataLoader : MonoBehaviour
{
    public GameObject playerPrefab; // 준비된 프리팹


    void Start()
    {
        FirebaseDatabase.DefaultInstance.GetReference("players").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    float HP = float.Parse(child.Child("HP").Value.ToString());
                    float Damage = float.Parse(child.Child("Damage").Value.ToString());
                    float AttackSpeed = float.Parse(child.Child("AttackSpeed").Value.ToString());

                    Debug.Log($"snapShot > {child}, {HP}");

                    // 데이터로 프리팹 생성
                    CreatePlayer(HP, Damage, AttackSpeed);
                }
            }
            else
            {
                Debug.LogError("Failed to load data from Firebase.");
            }
        });
    }

    void CreatePlayer(float HP, float Damage, float AttackSpeed)
    {
        Debug.Log("Create Player");
        GameObject newPlayer = Instantiate(playerPrefab, new Vector3(0, 0 ,0), Quaternion.identity);
        PlayerController playerScript = newPlayer.GetComponent<PlayerController>();
    }
}
