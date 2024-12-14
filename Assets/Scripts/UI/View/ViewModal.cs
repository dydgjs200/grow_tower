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
    public GameObject DataPanel;    // 업그레이드 패널
    public GameObject TabButtonPrefabs;     // 하단 버튼 프리팹

    public GridLayoutGroup tabLayoutGroup; // 하단 탭의 레이아웃 그룹


    public void showModal(List<string> tabData)
    {
        Debug.Log("showModal called");

        if (modal == null)
        {
            Debug.LogError("Modal GameObject is not assigned in ViewModal!");
            return;
        }

        UpgradeButton(ModalTabParent, tabData);
        UpdateTabLayout(tabData.Count);

        modal.SetActive(true);
        Debug.Log("Modal is now active");
    }


    public void hideModal()
    {
        modal.SetActive(false);
    }

    /// 탭 버튼 클릭 시 

    public void UpgradeButton(Transform parent, List<string> data)
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
