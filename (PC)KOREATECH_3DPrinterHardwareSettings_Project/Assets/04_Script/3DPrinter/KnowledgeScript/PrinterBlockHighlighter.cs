using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

public class PrinterBlockHighlighter : MonoBehaviour
{
    enum btnState
    {
        UV,
        Head,
        Pump,
        Vaccum,
        Idle
    }

    public Highlighter UVLeft;
    public Highlighter UVRight;

    public Highlighter PrinterHead;

    public Highlighter Pump;

    public Highlighter Vaccum;

    private btnState BtnState;
    private bool check = false;
    private bool isClear = false;

    private void Start()
    {
        BtnState = btnState.Idle;
    }

    private void Update()
    {
        if (isClear)
        {
            isClear = false;

            switch (BtnState)
            {
                case btnState.UV:
                    UVLeft.tween = true;
                    UVRight.tween = true;
                    StartCoroutine(WaitTime(2f));
                    break;
                case btnState.Head:
                    PrinterHead.tween = true;
                    StartCoroutine(WaitTime(2f));
                    break;
                case btnState.Pump:
                    Pump.tween = true;
                    StartCoroutine(WaitTime(2f));
                    break;
                case btnState.Vaccum:
                    Vaccum.tween = true;
                    StartCoroutine(WaitTime(2f));
                    break;
                case btnState.Idle:
                    break;
            }
        }
    }

    public void UVHighlight()
    {
        AllHighlightOff();
        StopAllCoroutines();
        BtnState = btnState.UV;
        isClear = true;
    }

    public void HeadHighlight()
    {
        AllHighlightOff();
        isClear = true;
        StopAllCoroutines();
        BtnState = btnState.Head;
    }

    public void VaccumHighlight()
    {
        AllHighlightOff();
        isClear = true;
        StopAllCoroutines();
        BtnState = btnState.Vaccum;
    }

    public void PumpHighlight()
    {
        AllHighlightOff();
        StopAllCoroutines();
        isClear = true;
        BtnState = btnState.Pump;
    }

    public void AllHighlightOff()
    {
        UVLeft.tween = false;
        UVRight.tween = false;
        Pump.tween = false;
        Vaccum.tween = false;
        PrinterHead.tween = false;
    }


    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);

        switch (BtnState)
        {
            case btnState.UV:
                UVLeft.tween = false;
                UVRight.tween = false;
                BtnState = btnState.Idle;
                break;
            case btnState.Head:
                PrinterHead.tween = false;
                BtnState = btnState.Idle;
                break;
            case btnState.Pump:
                Pump.tween = false;
                BtnState = btnState.Idle;
                break;
            case btnState.Vaccum:
                Vaccum.tween = false;
                BtnState = btnState.Idle;
                break;
            case btnState.Idle:
                break;
        }
    }
}
