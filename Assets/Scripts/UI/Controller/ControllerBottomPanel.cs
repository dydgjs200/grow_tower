using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

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

        viewBottomPanel.Bottom_Upgrade.onClick.AddListener(() => OnButtonClick("Upgrade"));
        viewBottomPanel.Bottom_Skill.onClick.AddListener(() => OnButtonClick("Skill"));
        viewBottomPanel.Bottom_Equipment.onClick.AddListener(() => OnButtonClick("Equipment"));
        viewBottomPanel.Bottom_Store.onClick.AddListener(() => OnButtonClick("Store"));

        viewModal.CloseButton.onClick.AddListener(() => OnCloseButtonClick());
    }

    private void OnButtonClick(string tabType)      // up, skill, equip, store
    {
        if (viewModal == null)
        {
            Debug.LogError("viewModal is null in ControllerBottomPanel!");
            return;
        }

        List<string> tabData = modelModal.GetTabData(tabType);

        viewModal.showModal(tabData);
    }

    private void OnCloseButtonClick()
    {
        viewModal.hideModal();
    }
}
