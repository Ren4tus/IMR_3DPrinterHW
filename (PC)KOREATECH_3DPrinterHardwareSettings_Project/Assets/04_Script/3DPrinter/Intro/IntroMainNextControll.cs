using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;

public class IntroMainNextControll : MonoBehaviour
{
    public Camera cam;

    public Animation[] guideTextAnim;

    public Highlighter[] highlighters;

    public Text[] GuideTexts;

    public BoxCollider[] PrinterCollider;

    public PostProcessVolume PPV;
    private Vignette vignette;

    public Animation[] Btns;
    public GameObject[] SpotLights;

    private Transform zoom;

    private Quaternion[] targetRotation = {   Quaternion.Euler(10, 180, 0), Quaternion.Euler(10, 140, 0), Quaternion.Euler(10, 180, 0), Quaternion.Euler(10, 220, 0) };
    private Quaternion[] targetLocalRotation = { Quaternion.Euler(0, 0, 0),    Quaternion.Euler(5, 0, 0),    Quaternion.Euler(0, 0, 0),  Quaternion.Euler(5, 0, 0) };
    private Vector3[] targetZoom = {                new Vector3(0, 0, -10),    new Vector3(-1.6f, 0, 0f),        new Vector3(0, 0, -4),       new Vector3(1.8f, 3, -6) };

    private Transform camPivot;

    private float timer = 0f;

    private int target = 0;

    private bool isCameraMove = false;
    private int clickTarget = 0;
    private bool btnAniFinish = false;

    private Coroutine VignettCoroutine;

    private int targetIdx
    {
        get { return target; }
        set
        {
            if (value != target)
            {
                if (value == 1)
                {
                    highlighters[0].tween = true;
                    highlighters[1].tween = false;
                    highlighters[2].tween = false;
                    clickTarget = 1;
                }
                else if (value == 2)
                {
                    highlighters[1].tween = true;
                    highlighters[0].tween = false;
                    highlighters[2].tween = false;
                    clickTarget = 2;
                }
                else if (value == 3)
                {
                    highlighters[2].tween = true;
                    highlighters[0].tween = false;
                    highlighters[1].tween = false;
                    clickTarget = 3;
                }
                else
                {
                    foreach (Highlighter hl in highlighters)
                    {
                        hl.tween = false;
                    }
                }
            }
            target = value;
        }
    }
    
    void Start()
    {
        /*
#if UNITY_WEBGL
        exitbutton.SetActive(false);
#endif
*/
        /*
        foreach (Animator ani in guideTextAnim)
        {
            ani.SetBool("on", true);
        }
        */
        // Post Process Rendering
        PPV.profile.TryGetSettings(out vignette);

        zoom = cam.transform;
        camPivot = zoom.parent;

        StartCoroutine(SelectionInit());
    }

    void Update()
    {
        if (timer < 1f)
            timer += Time.deltaTime;
        
        if (Input.GetMouseButtonUp(0) && !isCameraMove)
        {
            timer = 0f;

            if (clickTarget >= 1 && clickTarget <= 3)
            {
                isCameraMove = true;
                StartCoroutine(VignetteSet());
            }
            else if (clickTarget == 0)
            {
                isCameraMove = true;
                StopCoroutine(VignetteSet());
                vignette.intensity.value = 0.5f;
                Invoke("DelayReset", 0.5f);
            }
        }

        if (isCameraMove)
            CamMove();
    }

    private void CamMove()
    {
        camPivot.rotation = Quaternion.Slerp(camPivot.rotation, targetRotation[clickTarget], timer);
        zoom.localPosition = Vector3.Lerp(zoom.localPosition, targetZoom[clickTarget], timer);
        zoom.localRotation = Quaternion.Slerp(zoom.localRotation, targetLocalRotation[clickTarget], timer);

        GuideTexts[0].text = "학습을 진행할\n단계를 선택하세요.";
        GuideTexts[1].text = "학습을 진행할\n단계를 선택하세요.";

        foreach(BoxCollider BC in PrinterCollider)
        {
            BC.enabled = false;
        }
    }

    public void ResetValue()
    {
        clickTarget = 0;
        btnAniFinish = false;

        for (int i=0; i<3; i++)
        {
            Btns[i].gameObject.SetActive(false);
            SpotLights[i].SetActive(false);
        }

        Invoke("DelayReset", 0.5f);
    }

    private void DelayReset()
    {
        foreach (BoxCollider BC in PrinterCollider)
        {
            BC.enabled = true;
        }

        isCameraMove = false;

        GuideTexts[0].text = "학습을 진행할\n프린터를 선택하세요.";
        GuideTexts[1].text = "학습을 진행할\n프린터를 선택하세요.";
    }

    private IEnumerator SelectionInit()
    {
        yield return new WaitForSeconds(1.5f);
        /*
        foreach (Animator ani in guideTextAnim)
        {
            ani.SetBool("on", false);
        }
        */
    }

    private IEnumerator VignetteSet()
    {
        while(vignette.intensity.value <= 0.7f)
        {
            vignette.intensity.value += 0.01f;

            yield return new WaitForSeconds(0.01f);
        }

        SpotLights[clickTarget - 1].SetActive(true);

        Btns[clickTarget - 1].gameObject.SetActive(true);
        if (!btnAniFinish)
        {
            Btns[clickTarget - 1].Play("Btns");
            btnAniFinish = true;
        }

        isCameraMove = false;
    }

    public void SetTarget(int a)
    {
        targetIdx = a;
    }

    public void EnableHighlightTarget(int a)
    {
        for (int i = 0; i < 3; i++)
            highlighters[i].tween = (i == (a-1) ? true : false);
    }

    public void DisableHighlightTarget()
    {
        for (int i = 0; i < 3; i++)
            highlighters[i].tween = false;
    }

    public void PlayTextAnimation(int a)
    {
        for (int i = 0; i < 3; i++)
        {
            guideTextAnim[i].Stop();
        }

        guideTextAnim[a - 1].Play(); 
    }

    public void StopAllTextAnimation()
    {
        for (int i = 0; i < 3; i++)
        {
            guideTextAnim[i].Stop();
        }
    }    
}
