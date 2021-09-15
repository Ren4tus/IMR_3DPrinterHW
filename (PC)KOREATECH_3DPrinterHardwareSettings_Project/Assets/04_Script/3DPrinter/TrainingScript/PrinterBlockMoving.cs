using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterBlockMoving : MonoBehaviour
{
    // left : 0.432
    // right : -0.273

    int move = 0;

    Animation ani;
    public static Animation ani2;

    public GameObject mirror;

    private void Start()
    {
        ani = GetComponent<Animation>();
        ani2 = ani;
    }
    
    public static void AniStart()
    {
        ani2.clip = ani2.GetClip("PrinterBlcok");
        ani2.Play();
    }

    public static void AniStop()
    {
        ani2.Stop();
    }

    public static void HeadCleanStart()
    {
        float time = 0f;

        while (time <= 3f)
            time += Time.deltaTime;

        ani2.clip = ani2.GetClip("HeadClean");
        ani2.Play();
    }

    public void FinalAniStart()
    {
        mirror.SetActive(false);

        ani2.clip = ani2.GetClip("FinalAni");
        ani2.Play();

        MainController.instance.isRightClick = true;
        MainController.instance.goNextSeq();
        MainController.instance.isRightClick = false;
    }
}