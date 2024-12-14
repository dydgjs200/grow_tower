using System.Collections.Generic;
using UnityEngine;

public class ModelModal
{
    // buttomPanel ��ư Ŭ���� �����Ǵ� ��� ��ư
    private Dictionary<string, List<string>> tabData = new Dictionary<string, List<string>>{
        {"Upgrade", new List<string> {"����", "���", "Ư��"} },
        {"Skill", new List<string> {"����", "�����"} },
    };

    public List<string> GetTabData(string tabType)
    {
        return tabData.ContainsKey(tabType) ? tabData[tabType] : new List<string>();
    }
}
