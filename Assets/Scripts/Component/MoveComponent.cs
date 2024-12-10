using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveComponent : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rg2D;
    public float dist;
    public float speed;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rg2D = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        MoveEnemyToPlayer();
    }

    public void InitializedDistance(float d, float s)           // enemy�� distance�� speed ����
    {
        dist = d;
        speed = s;
    }

    public void MoveEnemyToPlayer()
    {
        float d = Vector3.Distance(gameObject.transform.position, player.transform.position);

        // ���� ���� �÷��̾� ������ �Ÿ��� ��Ÿ����� ũ�� �۰� ����
        if (d >= dist)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            rg2D.MovePosition(transform.position + dir * speed * Time.deltaTime);
        }
    }


    // Gizmos�� dist ������ ǥ��
    private void OnDrawGizmos()
    {
        if (player == null) return;

        // Gizmos ���� ����
        Gizmos.color = Color.red;

        // ���� ��ġ���� dist ���� �׸���
        Gizmos.DrawWireSphere(transform.position, dist);
    }
}
