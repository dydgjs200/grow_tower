using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public string PlayerID;
    public float HP;
    public float Damage;
    public float AttackSpeed;

    public PlayerInfo(string playerid, float hp, float damage, float attackspeed)
    {
        PlayerID = playerid;
        HP = hp;
        Damage = damage;
        AttackSpeed = attackspeed;
    }   
}