using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CL_LoadSceneController : TweenableCanvas
{
    public Text progressText;
    public Text loadingText;

    public MainController mainController;

    private float loadingDuration = 3.0f;

    public void LoadScene(string sceneName)
    {
        // 애니메이션 재생 대기시간
        StartCoroutine(LoadSceneAsync(sceneName, 0.5f));
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public string SetSubTitleText(string sceneName)
    {
        string loadingMsg = "";

        if(sceneName.Contains("1"))
        {
            loadingMsg = "재료분사프린터 ";
        }
        else if(sceneName.Contains("2"))
        {
            loadingMsg = "광중합프린터 ";
        }
        else if(sceneName.Contains("3"))
        {
            loadingMsg = "분말적층용융프린터 ";
        }

        if(sceneName.Contains("Main"))
        {
            loadingMsg += "메인화면으로 이동합니다.";
        }
        else if(sceneName.Contains("Knowledge"))
        {
            loadingMsg += "지식 학습으로 이동합니다.";
        }
        else if(sceneName.Contains("Structure"))
        {
            loadingMsg += "구조 학습으로 이동합니다.";
        }
        else if(sceneName.Contains("Training"))
        {
            loadingMsg += "실습 학습으로 이동합니다.";
        }
        else if(sceneName.Contains("Evaluation"))
        {
            loadingMsg += "실습 평가로 이동합니다.";
        }

        return loadingMsg;
    }

    private IEnumerator LoadSceneAsync(string sceneName, float delay)
    {
        progressText.text = "0%";
        loadingText.text = SetSubTitleText(sceneName);

        FadeIn();

        yield return new WaitForSeconds(delay);

        Time.timeScale = 1.0f;

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(sceneName);
        asyncOper.allowSceneActivation = false;

        float totalPercentage = 0.0f;
        float multiplier = 90.0f;
        float startTime = Time.time;
        float elapsedTime = 0f;
        float progressPercentage = 0f;

        while(asyncOper.progress < 0.9f)
        {
            while (elapsedTime < loadingDuration)
            {
                elapsedTime = Time.time - startTime;
                progressPercentage = elapsedTime / loadingDuration * multiplier + totalPercentage;
                progressText.text = progressPercentage.ToString("F0") + "%";

                yield return null;
            }
            totalPercentage = progressPercentage;
            multiplier /= 10f;
            startTime = Time.time;
            elapsedTime = 0f;
        }

        startTime = Time.time;

        while (progressPercentage < 100)
        {
            elapsedTime = Time.time - startTime;
            progressPercentage = elapsedTime / loadingDuration * multiplier + totalPercentage;
            progressText.text = progressPercentage.ToString("F0") + "%";

            yield return null;
        }

        asyncOper.allowSceneActivation = true;

        FadeOut();
    }
}
