using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1Printer : MonoBehaviour
{
    private Animator animator;

    private int printCount;
    private Vector3 trayOriginalPos;
    
    public GameObject buildTray;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        trayOriginalPos = buildTray.transform.localPosition;
    }

    public void PrintStart()
    {
        printCount = 0;
        animator.Play("Printing");
    }
    public void PrintStop()
    {
        animator.Play("PrintingStop");
    }

    public void PositionReset()
    {
        animator.Play("PrintingStop");
        TrayReset();
    }

    public void CleanPosition()
    {
        animator.Play("CleanPosition");
        TrayReset();
        buildTray.transform.localPosition = new Vector3(0f, 0.15f, 0.2f);
    }

    public void TrayDown()
    {
        Debug.Log(printCount);
        if (printCount < 20)
        {
            buildTray.transform.Translate(0f, 0f, -0.003f);
            printCount++;
        }
        else
        {
            animator.speed = 1f;
            PrintStop();
        }
    }

    public void FasterPrinting()
    {
        animator.speed = 20f;
    }

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
