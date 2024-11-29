using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSceneChanger : MonoBehaviour
{
    public void OnButtonClick(Button button)
    {
        // 버튼의 이름 가져오기
        string buttonName = button.name;

        Debug.Log($"Clicked Button Name: {buttonName}");

        // 버튼 이름에 따라 씬 변경
        ChangeScene(buttonName);
    }

    private void ChangeScene(string sceneName)
    {
        // 씬이 로드 가능한지 확인 (옵션)
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"씬 '{sceneName}'이 존재하지 않습니다!");
        }
    }
}
