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

    public void InitializedDistance(float d, float s)           // enemy의 distance와 speed 설정
    {
        dist = d;
        speed = s;
    }

    public void MoveEnemyToPlayer()
    {
        float d = Vector3.Distance(gameObject.transform.position, player.transform.position);

        // 현재 적과 플레이어 사이의 거리가 사거리보다 크면 작게 만듦
        if (d >= dist)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            rg2D.MovePosition(transform.position + dir * speed * Time.deltaTime);
        }
    }


    // Gizmos로 dist 범위를 표시
    private void OnDrawGizmos()
    {
        if (player == null) return;

        // Gizmos 색상 설정
        Gizmos.color = Color.red;

        // 적의 위치에서 dist 범위 그리기
        Gizmos.DrawWireSphere(transform.position, dist);
    }
}
