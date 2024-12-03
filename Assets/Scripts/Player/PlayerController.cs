using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CircleCollider2D circleCollider;
    public GameObject playerBullet;
    float intersection_time;
    float shoot_time;
    float growIntersectionRate;
    float growBulletSpeed;

    // Player info
    public float HP;
    public float Damage;
    public float AttackSpeed;


    private void Awake()
    {

        if (LocalPlayerDataManager.Instance == null)
        {
            Debug.LogError("LocalPlayerDataManager.Instance가 null입니다.");
            return;
        }
        CreatePlayer("admin");
    }

    void Start()
    {
        intersection_time = 0;
        circleCollider = GetComponent<CircleCollider2D>();
        growIntersectionRate = 5f;
        growBulletSpeed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GrowIntersection();
        ShootPlayerBullet();
    }

    public virtual void InitializedPlayer(PlayerInfo info)
    {
        HP = info.HP;
        Damage = info.Damage;
        AttackSpeed = info.AttackSpeed;
    }

    public void CreatePlayer(string id)
    {
        PlayerInfo playerinfo = LocalPlayerDataManager.Instance.GetPlayerInfo(id);
        GameObject playerPrefab = LocalPlayerDataManager.Instance.PlayerPrefab;

        if (playerPrefab == null)
        {
            Debug.LogError("PlayerPrefab이 null입니다. LocalPlayerDataManager에서 확인하세요.");
            return;
        }

        if (playerinfo == null)
        {
            Debug.LogError($"PlayerInfo가 null입니다. ID: {id}에 해당하는 정보가 없습니다.");
            return;
        }

        Debug.Log($"PlayerPrefab: {playerPrefab.name}, PlayerInfo: {playerinfo}");

        GameObject.Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        InitializedPlayer(playerinfo);
    }

    void GrowIntersection()
    {
        intersection_time += Time.deltaTime;

        if (intersection_time >= growIntersectionRate)
        {
            circleCollider.radius += 1f;
            intersection_time -= growIntersectionRate;
        }
    }

    void ShootPlayerBullet()
    {
        shoot_time += Time.deltaTime;

        if(shoot_time >= growBulletSpeed)
        {
            if(GameObject.FindWithTag("Enemy"))
                Instantiate(playerBullet, transform.position, Quaternion.identity);
            shoot_time -= growBulletSpeed;
        }
    }
}
