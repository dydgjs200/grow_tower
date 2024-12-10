using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{

    public GameObject player;
    public Rigidbody2D rg2D;

    public float BulletSpeed = 5.0f;        //임시 총알 속도

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        rg2D = GetComponent<Rigidbody2D>();
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
}
