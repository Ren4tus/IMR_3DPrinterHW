using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberPinAni : MonoBehaviour
{
    [Header("ChamberPin")]
    public Animation ChamberPin;
    public int chamberPinSeqNum; // 8
    private Vector3 chamberPinPos = new Vector3(0f, -0.012f, -0.3635f);
    private bool canPlay = true;
    private bool init = true;

    void Init()
    {
        ChamberPin.transform.localPosition = chamberPinPos;
        ChamberPin.gameObject.SetActive(false);
        canPlay = true;
    }

    void Update()
    {
        if (MainController.instance.GetCurrentSeq() < chamberPinSeqNum)
        {
            Init();
        }
        else if (MainController.instance.GetCurrentSeq() == chamberPinSeqNum && init)
        {
            init = false;
            Init();
        }
        else if (MainController.instance.GetCurrentSeq() > chamberPinSeqNum)
        {
            init = true;
        }
    }

    private void OnMouseDown()
    {
        if (canPlay)
        {
            canPlay = false;
            this.GetComponent<Highlighter>().ConstantOff();
            ChamberPin.gameObject.SetActive(true);
            ChamberPin.Play("ChamberPin");
        }
    }
}
