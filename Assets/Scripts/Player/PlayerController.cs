using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �⺻ ����
    public float HP { get; private set; }
    public float Damage { get; private set; }
    public float AttackSpeed { get; private set; }

    // ���� �ʱ�ȭ Ȯ��
    public bool isInit = false;

    // �Ѿ� ������Ʈ
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

        isInit = true;      // �� �Լ� ���࿩�� Ȯ��
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
