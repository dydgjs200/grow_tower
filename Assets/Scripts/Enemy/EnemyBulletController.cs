using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{

    public GameObject player;
    public Rigidbody2D rg2D;


    public float Damage;        // 데미지 값 설정
    public float BulletSpeed = 5.0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        rg2D = GetComponent<Rigidbody2D>();

        Damage = GetComponentInParent<EnemyController>().Damage;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        FindPlayerAndStopShooting();
    }

    public void MoveBullet()            // 적 총알 발사
    {
        if(player != null)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;

            transform.Translate(dir * Time.deltaTime * BulletSpeed);
        }
    }
    public void FindPlayerAndStopShooting()     // 적이 없을 시 총알 오브젝트 파괴
    {
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //총알과 부딪힌 플레이어의 healthComponent 착용 여부 확인 -> takeDamage 
            HealthComponent health = collision.GetComponent<HealthComponent>();
            PlayerController controller = collision.GetComponent<PlayerController>();
            if(health != null)
            {
                health.TakeDamage(Damage, "player");
                //PlayerLocalCache.Instance.SetPlayerData(controller.PlayerId, "HP", controller.HP);          // player의 정보 로컬 저장
            }

            Destroy(gameObject);         // 플레이어 없을 시 EnemyBullet 삭제
        }
    }
}
