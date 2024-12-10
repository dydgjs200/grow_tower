using UnityEngine;

public class AttackComponent : MonoBehaviour
{

    [SerializeField] private float Damage;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private GameObject Bullet;
    public float time;

    public string targetTag;        // ���� ������Ʈ�� �ݴ�

    private void Awake()
    {
        time = 0;
        SetTag();
    }

    public void SetTag()            // ���� ������Ʈ�� �ݴ�Ǵ� ��� �±� ����
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

    public void AttackCoolDown(float AttackSpeed)        // ���� ��Ÿ��(�Ѿ� ���� �� AttackSpeed��ŭ ���)
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
