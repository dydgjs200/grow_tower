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
    public GameObject TabButtonPrefabs;     // �ϴ� ��ư ������

    public GridLayoutGroup tabLayoutGroup; // �ϴ� ���� ���̾ƿ� �׷�

    public Transform DataPanelParent;   // data �θ��г�
    public GameObject DataPanel;    // json data �г�

    private void Awake()
    {
        JsonLoader.Instance.LoadJson("upgrade.json");
    }

    public void showModal(List<string> tabData)
    {
        if (modal == null)
        {
            Debug.LogError("��� ������Ʈ�� �ν����Ϳ� ������� �ʾ���.");
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

    public void ModalTabButton(Transform parent, List<string> data)      // �� ��ư Ŭ�� ��
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

            // ��ư �����տ� Ŭ�� �̺�Ʈ �߰�
            Button buttonComponent = button.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => OnTabButtonClick(text));
            }
        }
    }

    private void OnTabButtonClick(string buttonText)        // ��� �ϴ� �ǹ�ư Ŭ�� �� �̺�Ʈ
    {
        Debug.Log($"button Text {buttonText}");

        foreach (Transform child in DataPanelParent)
        {
            Destroy(child.gameObject); // ���� �г� ����
        }

        if (buttonText == "����")
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
