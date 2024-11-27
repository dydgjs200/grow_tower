using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public GameObject player;
    public Transform target;
    public Rigidbody2D rg2D;

    // Monster Info
    public int ID;
    public string Name;
    public bool Melee;
    public float HP;
    public float Damage;
    public float Armor;
    public float Magic_resist;
    public float Speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = 5.0f;
        player = GameObject.FindWithTag("Player");
        rg2D = GetComponent<Rigidbody2D>();
        target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }

    public virtual void InitializeEnemy(MonsterInfo info)
    {
        if (info == null)
            return;

        ID = info.ID;
        Name = info.Name;
        Melee = info.Melee;
        HP = info.HP;
        Damage = info.Damage;
        Armor = info.Armor;
        Magic_resist = info.Magic_resist;
        Speed = info.Speed;

        Debug.Log($"{Name}이(가) 초기화되었습니다!");
    }

    void MoveEnemy()
    {
        // 목표와의 거리 계산
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > 0.8f) // 최소 거리 설정
        {
            // 목표 방향 계산
            Vector3 dir = (target.position - transform.position).normalized;

            // Rigidbody를 사용해 이동
            rg2D.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
        }
    }

    protected virtual void Die(int takeDamage) {
        if (takeDamage >= HP)
        {
            Destroy(gameObject);
        }
        else
        {
            HP -= takeDamage;
        }
    }
    public virtual void Attack() { }
    public virtual void SkillAttack() { }
}
