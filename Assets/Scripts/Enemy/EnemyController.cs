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

    public void AttackCoolDown()            // 공격 쿨타임
    {
        attackComponent.AttackCoolDown(AttackSpeed);
    }

    public void MoveEnemyToPlayer()     // 플레이어에게 이동
    {
        moveComponent.MoveEnemyToPlayer();
    }

    public void UpdateHP(float currentHP)
    {
        HP = currentHP;
        Debug.Log($"Enemy HP updated: {HP}");
    }
}
