using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberDoorControll : MonoBehaviour
{
    [Header("ChamberDoorRatate")]
    public Animation ChamberDoorUpper;
    public Animation ChamberDoorLower;
    public int chamberDoorCloseSeqNum;
    public int chamberDoorLowerCloseSeqNum;
    private bool canPlay = true;
    
    void Update()
    {
        if (MainController.instance.GetCurrentSeq() != chamberDoorCloseSeqNum &&
            MainController.instance.GetCurrentSeq() != chamberDoorLowerCloseSeqNum)
        {
            canPlay = true;
        }
    }

    private void OnMouseDown()
    {
        if (canPlay)
        {
            canPlay = false;
            ChamberDoorUpper.gameObject.GetComponent<Highlighter>().ConstantOff();
            ChamberDoorLower.gameObject.GetComponent<Highlighter>().ConstantOff();

            if(MainController.instance.GetCurrentSeq() == chamberDoorCloseSeqNum)
            {
                ChamberDoorUpper.Play("ChamberDoorClose");
                ChamberDoorLower.Play("ChamberDoorClose");
            }
            else if(MainController.instance.GetCurrentSeq() == chamberDoorLowerCloseSeqNum)
            {
                ChamberDoorLower.Play("ChamberDoorClose");
            }
        }
    }
}
