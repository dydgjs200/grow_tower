using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    public string Name;          // 아이템 이름
    public string Variable;      // 변수명
    public string Type;          // 데이터 타입
    public string Description;   // 설명
    public float? Increment;     // 증가 값 (nullable)
}

[System.Serializable]
public class DefenseData
{
    public string Name;          // 아이템 이름
    public string Variable;      // 변수명
    public string Type;          // 데이터 타입
    public string Description;   // 설명
    public float? Increment;     // 증가 값 (nullable)
}

[System.Serializable]
public class Root
{
    public List<AttackData> AttackData;
    public List<DefenseData> DefenseData;
}
