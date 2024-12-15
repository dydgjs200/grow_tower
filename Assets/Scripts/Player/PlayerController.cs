using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �⺻ ����
    public string PlayerID { get; private set; }
    public float HP { get; private set; }
    public float Damage { get; private set; }
    public float AttackSpeed { get; private set; }

    // �Ѿ� ������Ʈ
    public GameObject playerBullet;

    // ������Ʈ ����
    public HealthComponent healthComponent;     //ü�� �ý���
    public AttackComponent attackComponent;

    //�浹ó��
    public Rigidbody2D rg2D;


    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        attackComponent = GetComponent<AttackComponent>();
        rg2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        // PlayerManager���� ������ ��������
        PlayerInfo playerData = PlayerManager.Instance.GetPlayerInfo(0);
        if (playerData != null)
        {
            InitializedPlayer(playerData.PlayerID, playerData.HP, playerData.Damage, playerData.AttackSpeed);
        }
        else
        {
            Debug.LogError("Player data could not be loaded.");
        }
    }

    void Update()
    {
        AttackCoolDown();
    }

    public void InitializedPlayer(string playerid, float hp, float damage, float attackSpeed)
    {
        PlayerID = playerid;
        HP = hp;
        Damage = damage;
        AttackSpeed = attackSpeed;

        healthComponent.InitializedHP(HP);
        attackComponent.InitializedAttack(Damage, AttackSpeed, playerBullet);
        Debug.Log($"Init Player > {playerid}, {damage}, {attackSpeed}");
    }

    public void AttackCoolDown()
    {
        attackComponent.AttackCoolDown(AttackSpeed);
    }

    public void UpdateHP(float currentHP)
    {
        HP = currentHP;
        //PlayerLocalCache.Instance.SetPlayerData(PlayerId, "HP", HP); // ĳ�ÿ� ������Ʈ
        Debug.Log($"Player HP updated: {HP}");
    }
}
