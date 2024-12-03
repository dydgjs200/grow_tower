using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public float HP;
    public float Damage;
    public float AttackSpeed;

    public PlayerInfo(float hp, float damage, float attackspeed)
    {
        HP = hp;
        Damage = damage;
        AttackSpeed = attackspeed;
    }   
}