using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorHoverClick : MonoBehaviour
{
    public GameObject ConnectBG;
    public GameObject SlicerBG;
    public GameObject ViewStatBG;
    public GameObject HeadCleanBG;

    /*
    private void OnGUI()
    {
        Debug.Log("OnGui");

        switch (this.gameObject.name)
        {
            case "Connect":
                if (Event.current.clickCount == 2 && Input.GetMouseButtonDown(0))
                {
                    Debug.Log("OnGui in Connect");
                    MainController.instance.isRightClick = true;
                    MainController.instance.goNextSeq();
                    MainController.instance.isRightClick = false;
                }
                break;
            case "Slicer":
                break;
            default:
                break;
        }
    }
    */

    private void OnMouseOver()
    {
        switch(this.gameObject.name)
        {
            case "Connect":
                ConnectBG.SetActive(true);
                break;
            case "Slicer":
                SlicerBG.SetActive(true);
                break;
            case "ViewStat":
                ViewStatBG.SetActive(true);
                break;
            case "HeadClean":
                HeadCleanBG.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnMouseExit()
    {
        ConnectBG.SetActive(false);
        SlicerBG.SetActive(false);
        ViewStatBG.SetActive(false);
        HeadCleanBG.SetActive(false);
    }
}
