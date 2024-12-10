using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D rg2D;

    // Enemy Info
    public string Name;
    public bool Melee;
    public float HP;
    public float Damage;
    public float Armor;
    public float Magic_resist;
    public float AttackSpeed;
    public float Speed;
    public float Distance;

    //Component
    public HealthComponent healthComponent;
    public MoveComponent moveComponent;
    public AttackComponent attackComponent;

    //Bullet
    public GameObject EnemyBullet;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        moveComponent = GetComponent<MoveComponent>();
        attackComponent = GetComponent<AttackComponent>();

        rg2D = GetComponent<Rigidbody2D>();
    }


    void Start()
    {
        healthComponent.InitializedHP(HP);
        moveComponent.InitializedDistance(Distance, Speed);
        attackComponent.InitializedAttack(Damage, AttackSpeed, EnemyBullet);
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemyToPlayer();
        AttackCoolDown();
        
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
        AttackSpeed = info.AttackSpeed;
        Speed = info.Speed;
        Distance = info.distance;
    }

    public void AttackCoolDown()
    {
        attackComponent.AttackCoolDown(AttackSpeed);
    }

    public void MoveEnemyToPlayer()
    {
        moveComponent.MoveEnemyToPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)         // 플레이어 bullet과 충돌했을 때 데미지 입음.
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            healthComponent.TakeDamage(50);     // 플레이어의 공격 임시값 50
        }
    }

}
