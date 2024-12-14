using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ViewModal : MonoBehaviour
{
    public GameObject modal;        // ���â
    public Button CloseButton;      // ���â �ݱ�
    public Transform ModalBackground;   // ���â ��׶���
    public Transform ModalTabParent;          // �ϴ� �ǹ�ư �θ��г�
    public GameObject DataPanel;    // ���׷��̵� �г�
    public GameObject TabButtonPrefabs;     // �ϴ� ��ư ������

    public GridLayoutGroup tabLayoutGroup; // �ϴ� ���� ���̾ƿ� �׷�


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

    /// �� ��ư Ŭ�� �� 

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

        // �θ��� RectTransform ũ�⸦ �����ɴϴ�.
        RectTransform parentRect = tabLayoutGroup.GetComponent<RectTransform>();
        if (parentRect == null)
        {
            Debug.LogError("Tab Layout Group is missing a RectTransform!");
            return;
        }

        // �θ��� ũ��� ��ư ������ ���� Cell Size ���
        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        // �� �ٿ� 3�� ��ư ��ġ (����)
        int columnCount = buttonCount <= 3 ? buttonCount : 3; // 3�� ���̾ƿ�
        float cellWidth = parentWidth / columnCount; // ��ư�� ���� ũ��
        float cellHeight = parentHeight / 2;        // ��ư�� ���� ũ�� (2�� ����)

        // GridLayoutGroup�� Cell Size ����
        tabLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        // Constraint ����
        tabLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        tabLayoutGroup.constraintCount = columnCount;
    }

}
