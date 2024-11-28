using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    public int ID { get; set; }
    public bool Melee { get; set; }
    public float HP { get; set; }
    public float Damage { get; set; }
    public float Armor { get; set; }
    public float Magic_resist { get; set; }
    public float Speed { get; set; }

    public EnemyInfo(int id, bool melee, float hp, float damage, float armor, float magicResist, float speed)
    {
        ID = id;
        Melee = melee;
        HP = hp;
        Damage = damage;
        Armor = armor;
        Magic_resist = magicResist;
        Speed = speed;
    }
}
