using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJPMovingObjectsAS : MonoBehaviour
{
    private Animator animator;

    public int FastSeq;

    private int printCount;
    private Vector3 trayOriginalPos;

    public int ResetSeq;

    public GameObject buildTray;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        trayOriginalPos = buildTray.transform.localPosition;
    }

    public void PrintStart()
    {
        printCount = 0;
        animator.SetTrigger("Printing");
    }
    public void PrintStop()
    {
        animator.SetTrigger("PrintingStop");
    }

    public void PositionReset()
    {
        animator.SetTrigger("PrintingStop");
        TrayReset();
    }

    public void CleanPosition()
    {
        animator.SetTrigger("CleanPosition");
        TrayReset();
        buildTray.transform.localPosition = new Vector3(0f, 0.1f, 0.2f);
    }

    public void TrayDown()
    {
        if (printCount < 20)
        {
            buildTray.transform.Translate(0f, 0f, -0.0015f);
            printCount++;
        }
        else
        {
            animator.speed = 1f;
            PrintStop();

            MainController.instance.isRightClick = true;
            MainController.instance.goNextSeq();
            MainController.instance.isRightClick = false;

            buildTray.transform.localPosition = new Vector3(-0.005f, 0.2f, 0.2f);
        }

        if(printCount == 5 && MainController.instance.GetCurrentSeq() == 35)
        {
            MainController.instance.isRightClick = true;
            MainController.instance.goNextSeq();
            MainController.instance.isRightClick = false;
        }
    }

    public void FasterPrinting()
    {
        animator.speed = 20f;
    }

    public void ResetSpeed()
    {
        animator.speed = 1f;
    }

    public void IntroPrinting()
    {
        animator.speed = 3f;
    }

    /*
    public void Update()
    {
        if (MainController.instance != null)
        {
            if (MainController.instance.GetCurrentSeq() == FastSeq)
            {
                FasterPrinting();
            }
        }
        else
        {
            animator.speed = 1f;
        }
    }
    */

    public void TrayDownPos()
    {
        buildTray.transform.localPosition = new Vector3(-0.005f, 0.15f, 0.2f);
    }

    public void TrayReset()
    {
        buildTray.transform.localPosition = trayOriginalPos;
    }

    public void PrintingSound1Play()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_MoveBlock1);
    }
    public void PrintingSound2Play()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_MoveBlock2);
    }
    public void PrintingSound3Play()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_MoveBlockCrank);
    }
    public void PrintingSound4Play()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_MoveBlockReturn);
    }
}
