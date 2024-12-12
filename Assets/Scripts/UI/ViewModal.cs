using UnityEngine;
using UnityEngine.UI;

public class ViewModal : MonoBehaviour
{
    public GameObject modal;
    public Text contentText;
    public Button CloseButton;

    public void showModal(string content)
    {
        Debug.Log("showModal called");

        if (modal == null)
        {
            Debug.LogError("Modal GameObject is not assigned in ViewModal!");
            return;
        }

        if (contentText == null)
        {
            Debug.LogError("ContentText is not assigned in ViewModal!");
            return;
        }

        contentText.text = content;
        modal.SetActive(true);
        Debug.Log("Modal is now active");
    }


    public void hideModal()
    {
        modal.SetActive(false);
    }
}
