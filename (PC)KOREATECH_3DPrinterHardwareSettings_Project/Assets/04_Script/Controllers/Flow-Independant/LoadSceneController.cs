using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneController : MonoBehaviour
{
    public Text progressText;
    public Text loadingText;

    public MainController mainController;
    public GameObject LoadingPage;

    private float loadingDuration = 2.0f;
    private Animator animator;

    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    public void LoadScene(string sceneName)
    {
        LoadingPage.SetActive(true);
        // 애니메이션 재생 대기시간
        StartCoroutine(LoadSceneAsync(sceneName, 0.5f));
    }

    private IEnumerator LoadSceneAsync(string sceneName, float delay)
    {
        progressText.text = "0%";
        //animator.SetTrigger("LoadingPanelOpen");

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
        //animator.SetTrigger("LoadingPanelClose");
    }
}
