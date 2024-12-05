using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 기본 스탯
    public float HP { get; private set; }
    public float Damage { get; private set; }
    public float AttackSpeed { get; private set; }

    // 스탯 초기화 확인
    public bool isInit = false;

    // 총알 오브젝트
    public GameObject playerBullet;
    float shoot_time;
    float growBulletSpeed;


    private void Awake()
    {

    }

    void Start()
    {
        growBulletSpeed = 1.0f;
    }

    void Update()
    {
        if (!isInit) return;


        ShootPlayerBullet();
        Debug.Log($"playerController {HP}, {Damage}");
    }

    public void InitializedPlayer(float hp, float damage, float attackSpeed)
    {
        HP = hp;
        Damage = damage;
        AttackSpeed = attackSpeed;

        isInit = true;      // 이 함수 실행여부 확인
    }

    void ShootPlayerBullet()
    {
        shoot_time += Time.deltaTime;

        if (shoot_time >= growBulletSpeed)
        {
            if (GameObject.FindWithTag("Enemy"))
                Instantiate(playerBullet, transform.position, Quaternion.identity);

            shoot_time -= growBulletSpeed;
        }
    }
}
