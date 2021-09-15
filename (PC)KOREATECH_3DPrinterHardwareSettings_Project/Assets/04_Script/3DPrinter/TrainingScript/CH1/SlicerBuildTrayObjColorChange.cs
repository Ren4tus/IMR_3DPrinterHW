using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicerBuildTrayObjColorChange : MonoBehaviour
{
    public Color redColor;
    public Color blueColor;

    public GameObject[] RenderChild;

    public SlicerBuildTrayRotateObj trayRotate;

    private bool check = false;
    private bool errorCheck = false;
    private bool timerCheck = false;

    public static SlicerBuildTrayObjColorChange instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        int rotateIndex = trayRotate.getRotateIndex();

        float zMin = rotateIndex % 2 == 0 ? -0.066f : 0.03f;
        float zMax = rotateIndex % 2 == 0 ? 0.215f : 0.12f;
        float xMin = rotateIndex % 2 == 0 ? -0.068f : -0.18f;
        float xMax = rotateIndex % 2 == 0 ? 0.093f : 0.17f;

        if (this.gameObject.transform.localPosition.z >= zMin && 
            this.gameObject.transform.localPosition.z <= zMax &&
            this.gameObject.transform.localPosition.x >= xMin &&
            this.gameObject.transform.localPosition.x <= xMax)
        {
            foreach (GameObject render in RenderChild)
                render.GetComponent<Renderer>().material.color = blueColor;
        }
        else
        {
            foreach (GameObject render in RenderChild)
                render.GetComponent<Renderer>().material.color = redColor;
        }

        for (int i = 0; i < RenderChild.Length; i++)
        {
            if (RenderChild[i].GetComponent<Renderer>().material.color == blueColor)
            {
                check = true;
            }
            else
            {
                check = false;
            }
        }
    }

    public bool isArrange()
    {
        return check;
    }

    public bool ErrorCheck()
    {
        return errorCheck;
    }

    public void BtnTimer()
    {
        timerCheck = true;
    }

    public bool TimerCheck()
    {
        return timerCheck;
    }

    public void CheckColor()
    {
        if (check)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("배치가 올바르게 되었습니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
            errorCheck = true;
        }
        else
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("배치가 올바르게 되지 않았습니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
        }
    }
}
