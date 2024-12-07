using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public Transform target;
    public Rigidbody2D rg2D;

    // Enemy Info
    public string Name;
    public bool Melee;
    public float HP;
    public float Damage;
    public float Armor;
    public float Magic_resist;
    public float Speed;

    //Component
    public HealthComponent healthComponent;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
    }


    void Start()
    {
        healthComponent.InitializedHP(HP);

        player = GameObject.FindWithTag("Player");
        rg2D = GetComponent<Rigidbody2D>();
        target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }

    public virtual void InitializeEnemy(EnemyInfo info)
    {
        if (info == null)
            return;

        Name = info.Name;
        Melee = info.Melee;
        HP = info.HP;
        Damage = info.Damage;
        Armor = info.Armor;
        Magic_resist = info.Magic_resist;
        Speed = info.Speed;
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
            rg2D.MovePosition(transform.position + dir * Speed * Time.deltaTime);
        }
    }

    public virtual void Attack() { }
    public virtual void SkillAttack() { }

    private void OnTriggerEnter2D(Collider2D collision)         // �÷��̾� bullet�� �浹���� �� ������ ����.
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            healthComponent.TakeDamage(50);     // �÷��̾��� ���� �ӽð� 50
        }
    }

}
