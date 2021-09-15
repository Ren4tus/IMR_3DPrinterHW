using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrinterInterface : MonoBehaviour
{
    public GameObject PrinterInterfaceHoverObj;
    public Text HoverText;

    public Toggle[] CheckList;
    public Toggle[] CheckList2;

    private void OnMouseEnter()
    {
        PrinterInterfaceHoverObj.SetActive(true);
    }

    private void OnMouseExit()
    {
        PrinterInterfaceHoverObj.SetActive(false);
    }

    private void OnDisable()
    {
        PrinterInterfaceHoverObj.SetActive(false);
    }

    private void OnMouseOver()
    {
        switch(this.gameObject.name)
        {
            case "1Heads":
                HoverText.text = "헤드 온도";
                CheckList[0].isOn = false;
                CheckList[0].isOn = true;
                break;
            case "2Supports":
                HoverText.text = "서포트 노즐온도";
                break;
            case "3Models":
                HoverText.text = "모델 온도";
                break;
            case "4Waste":
                HoverText.text = "폐기물용기 잔량";
                CheckList[1].isOn = true;
                break;
            case "5Ambient":
                HoverText.text = "챔버 온도";
                CheckList[2].isOn = true;
                break;
            case "6UVLamps":
                HoverText.text = "UV램프";
                CheckList[3].isOn = true;
                break;
            case "7Vaccum":
                HoverText.text = "진공";
                CheckList[4].isOn = true;
                break;
            case "8Toggle":
                HoverText.text = "토글버튼";
                break;
            case "9Heater":
                HoverText.text = "히터 온도";
                CheckList[5].isOn = true;
                break;
            case "1State":
                HoverText.text = "현재 프린터 상태";
                break;
            case "2Model":
                HoverText.text = "모델 재료 잔량";
                CheckList2[0].isOn = false;
                CheckList2[0].isOn = true;
                break;
            case "3Support":
                HoverText.text = "서포트 재료 잔량";
                CheckList2[1].isOn = false;
                CheckList2[1].isOn = true;
                break;
            case "4Toggle":
                HoverText.text = "토글 버튼";
                break;
            default:
                break;
        }
    }
}
