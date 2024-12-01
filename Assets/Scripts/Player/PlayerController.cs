using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CircleCollider2D circleCollider;
    public GameObject playerBullet;
    float intersection_time;
    float shoot_time;
    float growIntersectionRate;
    float growBulletSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        intersection_time = 0;
        circleCollider = GetComponent<CircleCollider2D>();
        growIntersectionRate = 5f;
        growBulletSpeed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GrowIntersection();
        ShootPlayerBullet();
    }

    void GrowIntersection()
    {
        intersection_time += Time.deltaTime;

        if (intersection_time >= growIntersectionRate)
        {
            circleCollider.radius += 1f;
            intersection_time -= growIntersectionRate;
        }
    }

    void ShootPlayerBullet()
    {
        shoot_time += Time.deltaTime;

        if(shoot_time >= growBulletSpeed)
        {
            if(GameObject.FindWithTag("Enemy"))
                Instantiate(playerBullet, transform.position, Quaternion.identity);
            shoot_time -= growBulletSpeed;
        }
    }
}
