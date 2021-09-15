using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicerBuildTrayColorCheck : MonoBehaviour
{
    public Color redColor;
    public Color blueColor;

    public GameObject[] RenderChild;

    public void AllCheckColor()
    {
        bool check = false;

        for (int i = 0; i < RenderChild.Length; i++)
        {
            if (RenderChild[i].GetComponent<Renderer>().material.color == blueColor)
            {
                check = true;
            }
        }

        if (check)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("배치가 올바르게 되었습니다.\n예상 프린팅 시간은 15시간 입니다.", CL_MessagePopUpController.DialogType.NOTICE);
        }
        else
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("배치가 올바르게 되지 않았습니다.", CL_MessagePopUpController.DialogType.NOTICE);
        }
    }
}
