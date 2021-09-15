using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class SimpleClickMethod : MonoBehaviour
{
    public GameObject gloves;
    public GameObject glasses;
    public GameObject mask;

    public Transform PrinterFront;
    public Transform PrevTransform;

    public GameObject Wrench;

    public GameObject power_handle;

    private void OnMouseUp()
    {
        switch(this.gameObject.name)
        {
            case "Printer01_Cap":
                // 애니메이션으로 바꾸기
                this.gameObject.transform.localRotation = Quaternion.Euler(-150f, 0f, 0f);
                break;
            case "Nitril_gloves":
                gloves.SetActive(true);
                this.gameObject.SetActive(false);
                break;
            case "Safety_glasses":
                glasses.SetActive(true);
                this.gameObject.SetActive(false);
                break;
            case "Dustproof_mask":
                mask.SetActive(true);
                this.gameObject.SetActive(false);
                break;
            case "scraperL":
                CameraController.instance.setCameraByTransform(PrevTransform, PrinterFront);
                this.gameObject.SetActive(false);
                break;
            case "glass":
                Wrench.SetActive(true);
                GoNextSeq();
                break;
            case "Power":
                power_handle.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                GoNextSeq();
                break;
            case "Nitril_gloves_WaterJet":
                GoNextSeq();
                this.gameObject.SetActive(false);
                break;
            case "Foot_Pedal":
                GoNextSeq();
                break;
            case "Wrench_inWaterJet":
                GoNextSeq();
                this.gameObject.SetActive(false);
                break;
            case "Printer01_Material_cabinet_door":
                this.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 150f);
                break;
            case "material002":
                GoNextSeq();
                this.gameObject.SetActive(false);
                break;
            case "material_add":
                GoNextSeq();
                this.gameObject.SetActive(false);
                break;
            default:
                Debug.Log("SimpleClickMethod Setting Error");
                break;
        }
    }

    void GoNextSeq()
    {
        MainController.instance.isRightClick = true;
        MainController.instance.goNextSeq();
        MainController.instance.isRightClick = false;
    }
}