using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

public class BuildPlatformAni : MonoBehaviour
{
    [Header("BuildPlatform")]
    public Animation BuildPlat;
    public int buildPlatSeqNum;
    private bool isBuildPlatPlay = true;
    private bool init = true;

    void Init()
    {
        isBuildPlatPlay = true;
        BuildPlat.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        BuildPlat.gameObject.transform.GetChild(0).position = new Vector3(0.085f, 1.051f, -0.06529331f);
    }

    void Update()
    {
        if (MainController.instance.GetCurrentSeq() < buildPlatSeqNum)
        {
            Init();
        }
        else if(MainController.instance.GetCurrentSeq() == buildPlatSeqNum && init)
        {
            init = false;
            Init();
        }
        else if(MainController.instance.GetCurrentSeq() > buildPlatSeqNum)
        {
            init = true;
        }
    }

    void OnMouseDown()
    {
        if (isBuildPlatPlay)
        {
            isBuildPlatPlay = false;
            this.GetComponent<Highlighter>().ConstantOff();
            BuildPlat.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            BuildPlat.Play("BuildPlatform");
        }
    }
}