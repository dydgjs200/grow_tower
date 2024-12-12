using Unity.VisualScripting;
using UnityEngine;

public class ControllerBottomPanel : MonoBehaviour
{
    // View
    public ViewBottomPanel viewBottomPanel;
    public ViewModal viewModal;

    // Model
    private ModelModal modelModal;

    private void Start()
    {
        modelModal = new ModelModal();

        viewBottomPanel.Bottom_Upgrade.onClick.AddListener(() => OnButtonClick("You clicked Up!"));
        viewBottomPanel.Bottom_Skill.onClick.AddListener(() => OnButtonClick("You clicked Skill!"));
        viewBottomPanel.Bottom_Equipment.onClick.AddListener(() => OnButtonClick("You clicked Equip!"));
        viewBottomPanel.Bottom_Store.onClick.AddListener(() => OnButtonClick("You clicked Store!"));

        viewModal.CloseButton.onClick.AddListener(() => OnCloseButtonClick());
    }

    private void OnButtonClick(string content)
    {
        if (viewModal == null)
        {
            Debug.LogError("viewModal is null in ControllerBottomPanel!");
            return;
        }
        modelModal.Content = content;

        viewModal.showModal(content);
    }

    private void OnCloseButtonClick()
    {
        viewModal.hideModal();
    }
}
