using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHP;
    public float currentHP;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Die();
    }

    public void InitializedHP(float hp)
    {
        maxHP = hp;
        currentHP = hp;
    }

    public void Die()
    {
        if (currentHP <= 0)
        {
            Debug.Log("오브젝트 die");
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
    }
}
