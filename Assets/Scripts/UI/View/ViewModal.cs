using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ViewModal : MonoBehaviour
{
    public GameObject modal;        // 모달창
    public Button CloseButton;      // 모달창 닫기
    public Transform ModalBackground;   // 모달창 백그라운드
    public Transform ModalTabParent;          // 하단 탭버튼 부모패널
    public GameObject TabButtonPrefabs;     // 하단 버튼 프리팹

    public GridLayoutGroup tabLayoutGroup; // 하단 탭의 레이아웃 그룹

    public Transform DataPanelParent;   // data 부모패널
    public GameObject DataPanel;    // json data 패널

    private void Awake()
    {
        JsonLoader.Instance.LoadJson("upgrade.json");
    }

    public void showModal(List<string> tabData)
    {
        if (modal == null)
        {
            Debug.LogError("모달 오브젝트가 인스펙터와 연결되지 않았음.");
            return;
        }

        ModalTabButton(ModalTabParent, tabData);
        UpdateTabLayout(tabData.Count);

        modal.SetActive(true);
        Debug.Log("Modal is now active");
    }

    public void hideModal()
    {
        modal.SetActive(false);
    }

    public void ModalTabButton(Transform parent, List<string> data)      // 탭 버튼 클릭 시
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        foreach (string text in data)
        {
            GameObject button = Instantiate(TabButtonPrefabs, parent);
            Text buttonText = button.GetComponentInChildren<Text>();

            buttonText.text = text;

            // 버튼 프리팹에 클릭 이벤트 추가
            Button buttonComponent = button.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => OnTabButtonClick(text));
            }
        }
    }

    private void OnTabButtonClick(string buttonText)        // 모달 하단 탭버튼 클릭 시 이벤트
    {
        Debug.Log($"button Text {buttonText}");

        foreach (Transform child in DataPanelParent)
        {
            Destroy(child.gameObject); // 기존 패널 제거
        }

        if (buttonText == "공격")
        {
            foreach (var attack in JsonLoader.Instance.RootData.AttackData)
            {
                Debug.Log($"{attack.Name} : {attack.Description}");
                GameObject panel = GameObject.Instantiate(DataPanel, DataPanelParent);

                if(panel != null)
                {
                    Text[] texts = panel.GetComponentsInChildren<Text>();

                    foreach(Text text in texts)
                    {
                        if (text.name == "Name")
                            text.text = attack.Name;
                        else if (text.name == "description")
                            text.text = attack.Description;
                    }
                }
                
            }
        }
    }

    private void UpdateTabLayout(int buttonCount)
    {
        if (tabLayoutGroup == null)
        {
            Debug.LogError("Tab Layout Group is not assigned in ViewModal!");
            return;
        }

        // 부모의 RectTransform 크기를 가져옵니다.
        RectTransform parentRect = tabLayoutGroup.GetComponent<RectTransform>();
        if (parentRect == null)
        {
            Debug.LogError("Tab Layout Group is missing a RectTransform!");
            return;
        }

        // 부모의 크기와 버튼 개수에 따라 Cell Size 계산
        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        // 한 줄에 3개 버튼 배치 (예시)
        int columnCount = buttonCount <= 3 ? buttonCount : 3; // 3열 레이아웃
        float cellWidth = parentWidth / columnCount; // 버튼의 가로 크기
        float cellHeight = parentHeight / 2;        // 버튼의 세로 크기 (2행 기준)

        // GridLayoutGroup의 Cell Size 설정
        tabLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        // Constraint 설정
        tabLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        tabLayoutGroup.constraintCount = columnCount;
    }

}
