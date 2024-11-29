using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSceneChanger : MonoBehaviour
{
    public void OnButtonClick(Button button)
    {
        // ��ư�� �̸� ��������
        string buttonName = button.name;

        Debug.Log($"Clicked Button Name: {buttonName}");

        // ��ư �̸��� ���� �� ����
        ChangeScene(buttonName);
    }

    private void ChangeScene(string sceneName)
    {
        // ���� �ε� �������� Ȯ�� (�ɼ�)
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"�� '{sceneName}'�� �������� �ʽ��ϴ�!");
        }
    }
}
