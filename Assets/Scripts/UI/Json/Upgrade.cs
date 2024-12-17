using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    public string Name;          // ������ �̸�
    public string Variable;      // ������
    public string Type;          // ������ Ÿ��
    public string Description;   // ����
    public float? Increment;     // ���� �� (nullable)
}

[System.Serializable]
public class DefenseData
{
    public string Name;          // ������ �̸�
    public string Variable;      // ������
    public string Type;          // ������ Ÿ��
    public string Description;   // ����
    public float? Increment;     // ���� �� (nullable)
}

[System.Serializable]
public class Root
{
    public List<AttackData> AttackData;
    public List<DefenseData> DefenseData;
}
