using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 기본 스탯
    public string PlayerID { get; private set; }
    public float HP { get; private set; }
    public float Damage { get; private set; }
    public float AttackSpeed { get; private set; }

    // 총알 오브젝트
    public GameObject playerBullet;

    // 컴포넌트 관리
    public HealthComponent healthComponent;     //체력 시스템
    public AttackComponent attackComponent;

    //충돌처리
    public Rigidbody2D rg2D;


    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        attackComponent = GetComponent<AttackComponent>();
        rg2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        // PlayerManager에서 데이터 가져오기
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
        //PlayerLocalCache.Instance.SetPlayerData(PlayerId, "HP", HP); // 캐시에 업데이트
        Debug.Log($"Player HP updated: {HP}");
    }
}
