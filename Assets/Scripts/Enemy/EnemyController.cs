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

        Debug.Log($"{Name}��(��) �ʱ�ȭ�Ǿ����ϴ�!");
    }

    void MoveEnemy()
    {
        // ��ǥ���� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > 0.8f) // �ּ� �Ÿ� ����
        {
            // ��ǥ ���� ���
            Vector3 dir = (target.position - transform.position).normalized;

            // Rigidbody�� ����� �̵�
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
