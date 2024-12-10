using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{

    public GameObject player;
    public Rigidbody2D rg2D;

    public float BulletSpeed = 5.0f;        //�ӽ� �Ѿ� �ӵ�

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

    public void MoveBullet()            // �� �Ѿ� �߻�
    {
        if(player != null)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;

            transform.Translate(dir * Time.deltaTime * BulletSpeed);
        }
    }
    public void FindPlayerAndStopShooting()     // ���� ���� �� �Ѿ� ������Ʈ �ı�
    {
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
