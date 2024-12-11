using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHP;
    public float currentHP;

    private PlayerController playerController;
    private EnemyController enemyController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        enemyController = GetComponent<EnemyController>();
    }

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

    public void TakeDamage(float damage, string type)       // type = player or enemy
    {
        currentHP -= damage;

        if (playerController != null && type == "player")
            playerController.UpdateHP(currentHP);
        if (enemyController != null && type == "enemy")
            enemyController.UpdateHP(currentHP);

    }
}
