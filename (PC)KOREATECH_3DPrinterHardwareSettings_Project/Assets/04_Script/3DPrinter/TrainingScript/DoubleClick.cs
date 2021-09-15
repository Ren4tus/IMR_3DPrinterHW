using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleClick : MonoBehaviour
{
    float doubleClickStart = 0;
    void OnMouseUp()
    {
        if ((Time.time - doubleClickStart) < 0.3f)
     {
            this.OnDoubleClick();
            doubleClickStart = -1;
        }
     else
        {
            doubleClickStart = Time.time;
        }
    }
    void OnDoubleClick()
    {
        switch (this.gameObject.name)
        {
            case "Connect":
                MainController.instance.isRightClick = true;
                MainController.instance.goNextSeq();
                MainController.instance.isRightClick = false;
                break;
            case "Slicer":
                MainController.instance.isRightClick = true;
                MainController.instance.goNextSeq();
                MainController.instance.isRightClick = false;
                break;
            case "ViewStat":
                MainController.instance.isRightClick = true;
                MainController.instance.goNextSeq();
                MainController.instance.isRightClick = false;
                break;
            case "HeadClean":
                MainController.instance.isRightClick = true;
                MainController.instance.goNextSeq();
                MainController.instance.isRightClick = false;
                break;
            default:
                break;
        }
    }
}