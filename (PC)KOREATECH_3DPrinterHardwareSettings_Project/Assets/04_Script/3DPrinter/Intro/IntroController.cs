using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public CameraPathAnimator CameraController;
    public float cameraSpeed;
    public float fadeOutPercentage = 0f;

    public CameraPath CP;

    [Header("MJP")]
    public MJPMovingObjectsAS MJP;

    [Header("PP")]
    public Animation PP;

    [Header("PBP")]
    public Animator PBP;

    [Header("Fade")]
    public Animation Fade;
    public GameObject SkipBtn;

    private bool isFadeOut = false;

    [Header("SceneLoad")]
    public string NextSceneName;
    private bool _isIntroEnd = false;

    void Start()
    {
        // CameraController.pathSpeed = cameraSpeed;
        StartCoroutine(LoadScene());
    }

    public void MJPAni()
    {
        MJP.PrintStart();
        MJP.IntroPrinting();
    }

    public void PBPAni()
    {
        PBP.Play("PrintStart");
    }

    public void PPAni()
    {
        PP.Play("PP_Applicator_Arm");
    }

    public void FadeOut()
    {
        if (!isFadeOut)
        {
            Fade.Play("FadeOut");
            SkipBtn.SetActive(false);
            isFadeOut = true;
            Invoke("LoadNextScene", 0.6f);
        }
    }

    public void FadeIn()
    {
        Fade.Play("FadeIn");
        isFadeOut = false;
    }

    public void Skip()
    {
        CameraController.Seek(fadeOutPercentage);
        SkipBtn.SetActive(false);
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(NextSceneName);
        async.allowSceneActivation = false;

        while(!async.isDone)
        {
            if(_isIntroEnd && async.progress >= 0.9f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void LoadNextScene()
    {
        _isIntroEnd = true;
    }
}