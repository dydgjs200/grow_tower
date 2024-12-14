using System.Collections.Generic;
using UnityEngine;

public class ModelModal
{
    // buttomPanel 버튼 클릭에 대응되는 모달 버튼
    private Dictionary<string, List<string>> tabData = new Dictionary<string, List<string>>{
        {"Upgrade", new List<string> {"공격", "방어", "특수"} },
        {"Skill", new List<string> {"버프", "디버프"} },
    };

    public List<string> GetTabData(string tabType)
    {
        return tabData.ContainsKey(tabType) ? tabData[tabType] : new List<string>();
    }
}
