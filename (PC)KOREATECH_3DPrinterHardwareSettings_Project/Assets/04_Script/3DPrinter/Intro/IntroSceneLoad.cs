using UnityEngine;

public class IntroSceneLoad : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {

        CL_CommonFunctionManager.Instance.LoadSceneWithPopup(sceneName);
    }
}
