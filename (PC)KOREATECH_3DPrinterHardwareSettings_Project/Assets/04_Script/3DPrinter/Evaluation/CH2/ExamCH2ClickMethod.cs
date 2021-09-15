using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using HighlightingSystem;

public class ExamCH2ClickMethod : MonoBehaviour
{
    private bool buildPlatCheck = false;
    private bool use_UVoven = false;

    private void OnMouseDown()
    {
        if(!IsOverUI())
        {
            switch(this.gameObject.name)
            {
                case "Build_Chamber_Door_upper":
                    ChamberDoorUpperToggle();
                    break;
                case "Build_Chamber_Door_lower":
                    ChamberDoorLowerToggle();
                    break;
                case "Material_Cart":
                    CH2MethodSet.instance.Step1_Check();
                    break;
                case "Build_Platform":
                    if (ExamController.instance.IsClear(0) && !ExamController.instance.IsClear(1))
                    {
                        CH2MethodSet.instance.Step2_Check();
                    }
                    else if(ExamController.instance.IsClear(8) && !ExamController.instance.IsClear(9) && !buildPlatCheck)
                    {
                        buildPlatCheck = true;
                        CH2MethodSet.instance.Step10_Check1();
                    }
                    else if (ExamController.instance.IsClear(8) && !ExamController.instance.IsClear(9) && buildPlatCheck)
                    {
                        buildPlatCheck = false;
                        CH2MethodSet.instance.Step10_Check2();
                    }
                    break;
                case "Power_switch":
                    CH2MethodSet.instance.Step3_Check();
                    break;
                case "LogInPopUpBtn":
                    CH2MethodSet.instance.Step4_Check();
                    break;
                case "Bottle_Release":
                    BottleHolderToggle();
                    break;
                case "Material_Bottle":
                    if (CH2MethodSet.instance.isBottleOut)
                    {
                        this.GetComponent<Animation>().Play("MaterialBottleOut");
                        CH2MethodSet.instance.isMaterialBottleOut = true;
                        Invoke("InActiveGameObject", 0.5f);
                    }
                    break;
                case "Resin":
                    if(CH2MethodSet.instance.isMaterialBottleOut)
                    {
                        CH2MethodSet.instance.isMaterialBottleOut = false;
                        this.GetComponent<Animation>().Play("ResinIn");
                        this.transform.SetParent(GameObject.Find("Material_Bottle_Holder").transform);
                        CH2MethodSet.instance.Step6_Check();
                    }
                    break;
                case "usb01":
                    if (ExamController.instance.IsClear(6))
                    {
                        CH2MethodSet.instance.USB.Play("USB_PrinterIn");
                        CH2MethodSet.instance.usbIn = true;
                    }
                    break;
                case "UV_oven":
                    if (ExamController.instance.IsClear(9) && !use_UVoven)
                    {
                        use_UVoven = true;
                        CH2MethodSet.instance.Step11_Check2();
                    }
                    break;
                case "Grinder":
                    CH2MethodSet.instance.UseToolCheck("Grinder");
                    break;
                case "Sandpaper":
                    CH2MethodSet.instance.UseToolCheck("Sandpaper");
                    break;
                case "hand_file":
                    CH2MethodSet.instance.UseToolCheck("hand_file");
                    break;
                default:
                    break;
            }
        }
    }

    public void ChamberDoorUpperToggle()
    {
        if(CH2MethodSet.instance.isUpperDoorOpen)
        {
            this.GetComponent<Animation>().Play("ChamberDoorClose");
            CH2MethodSet.instance.isUpperDoorOpen = false;
        }
        else
        {
            this.GetComponent<Animation>().Play("ChamberDoorOpen");
            CH2MethodSet.instance.isUpperDoorOpen = true;
        }
    }

    public void ChamberDoorLowerToggle()
    {
        if (CH2MethodSet.instance.isLowerDoorOpen && !CH2MethodSet.instance.isBottleOut)
        {
            this.GetComponent<Animation>().Play("ChamberDoorClose");
            CH2MethodSet.instance.isLowerDoorOpen = false;
        }
        // 재료홀더가 탈거되어 있으면 하단 챔버도어를 닫을 수 없다.
        else if(CH2MethodSet.instance.isLowerDoorOpen && CH2MethodSet.instance.isBottleOut)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("재료 홀더가 탈거되어있습니다.", CL_MessagePopUpController.DialogType.NOTICE);
        }
        else
        {
            this.GetComponent<Animation>().Play("ChamberDoorOpen");
            CH2MethodSet.instance.isLowerDoorOpen = true;
        }
    }

    public void BottleHolderToggle()
    {
        if(!CH2MethodSet.instance.isBottleOut && CH2MethodSet.instance.isLowerDoorOpen)
        {
            GameObject.Find("Material_Bottle_Holder").GetComponent<Animation>().Play("MaterialHolderOut");
            CH2MethodSet.instance.isBottleOut = true;
        }
        // 챔버도어가 닫혀있으면 재료 홀더를 탈거할 수 없다.
        else if (!CH2MethodSet.instance.isBottleOut && !CH2MethodSet.instance.isLowerDoorOpen)
        {
            return;
        }
        else
        {
            GameObject.Find("Material_Bottle_Holder").GetComponent<Animation>().Play("MaterialHolderIn");
            CH2MethodSet.instance.isBottleOut = false;
        }
    }

    private void InActiveGameObject()
    {
        this.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (!IsOverUI())
            this.GetComponent<Highlighter>().tween = true;
    }

    private void OnMouseExit()
    {
        this.GetComponent<Highlighter>().tween = false;
    }

    public bool IsOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}