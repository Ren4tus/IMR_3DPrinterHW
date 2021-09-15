using UnityEngine;
using UnityEngine.UI;
using HighlightingSystem;

public class ToolTableHoverText : MonoBehaviour
{
    public GameObject ToolTableObj;
    public Text HoverText;

    private void OnMouseEnter()
    {
        ToolTableObj.SetActive(true);
        this.gameObject.GetComponent<Highlighter>().tween = true;
    }

    private void OnMouseExit()
    {
        ToolTableObj.SetActive(false);
        this.gameObject.GetComponent<Highlighter>().tween = false;
    }

    private void OnMouseOver()
    {
        switch (this.gameObject.name)
        {
            case "scraper":
            case "scraperS":
                HoverText.text = "스크래퍼";
                break;
            case "Nitril_gloves":
                HoverText.text = "니트릴장갑";
                break;
            case "Safety_glasses":
                HoverText.text = "보안경";
                break;
            case "Dustproof_mask":
                HoverText.text = "방진마스크";
                break;
            case "clean_fabric":
                HoverText.text = "세척용 천";
                break;
            case "Tissu_case":
                HoverText.text = "티슈";
                break;
            case "Moving_tray":
                HoverText.text = "이동용받침대";
                break;
            case "ethanol_case":
                HoverText.text = "에탄올";
                break;
            case "ethanol_bucket":
                HoverText.text = "에탄올 수조";
                break;
            case "Clipper":
                HoverText.text = "클리퍼";
                break;
            case "Sandpaper":
                HoverText.text = "사포";
                break;
            case "Grinder":
                HoverText.text = "그라인더";
                break;
            case "hand_file":
                HoverText.text = "줄";
                break;
            case "Resin":
                HoverText.text = "레진";
                break;
            case "Awl":
                HoverText.text = "송곳";
                break;
            case "clean_brush":
                HoverText.text = "세척용 브러쉬";
                break;
            case "Tissu":
                HoverText.text = "티슈";
                break;
            default:
                break;
        }
    }
}
