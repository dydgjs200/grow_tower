using UnityEngine;

public class AttackComponent : MonoBehaviour
{

    [SerializeField] private float Damage;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private GameObject Bullet;
    public float time;

    public string targetTag;        // 부착 오브젝트의 반대

    private void Awake()
    {
        time = 0;
        SetTag();
    }

    public void SetTag()            // 부착 오브젝트와 반대되는 상대 태그 설정
    {
        if (gameObject.tag == "Enemy")
            targetTag = "Player";
        else if (gameObject.tag == "Player")
            targetTag = "Enemy";
    }

    public void InitializedAttack(float damage, float attackSpeed, GameObject bullet)
    {
        Damage = damage;
        AttackSpeed = attackSpeed;
        Bullet = bullet;
    }

    public void AttackCoolDown(float AttackSpeed)        // 공격 쿨타임(총알 생성 후 AttackSpeed만큼 대기)
    {
        time += Time.deltaTime;

        if(time > AttackSpeed)
        {
            if (GameObject.FindWithTag(targetTag))
            {
                Instantiate(Bullet, transform.position, Quaternion.identity);
            }

            time -= AttackSpeed;
        }
    }
}
