using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjClickAnimation : MonoBehaviour
{
    [Header("Ethanol")]
    public Animation Ethanol;
    public GameObject EthanolObj;
    private Vector3 EthanolPos = new Vector3(8.625f, 1.045f, -0.78f);

    [Header("CleanFabric")]
    public Animation Clean;
    public GameObject CleanObj;
    private Vector3 CleanPos = new Vector3(8.225f, 0.7596f, -3.8129f);

    [Header("MJP")]
    public Animation MovingTray;

    private void OnMouseDown()
    {
        switch(this.gameObject.name)
        {
            case "ethanol_case":
                EthanolObj.transform.localPosition = EthanolPos;
                Ethanol.Play("drop_ethanol_legacy");
                break;
            case "PrinterBlock":
                this.gameObject.GetComponent<HighlightingSystem.Highlighter>().tween = false;
                this.gameObject.GetComponent<HighlightingSystem.Highlighter>().constant = false;
                CleanObj.transform.localPosition = CleanPos;
                Clean.Play("clean_towel_legacy");
                break;
            case "Wrench03_Complete":
                MovingTray.Play("MovingTray");
                break;
        }
    }
}
