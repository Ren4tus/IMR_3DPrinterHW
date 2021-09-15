using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainSpriteSwap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image Printer;
    public Image Button;

    public Sprite[] printerSwap;
    public Sprite[] buttonSwap;

    public string sceneName;

    /*
    public void Awake()
    {
        UISetting();
    }

    public void UISetting()
    {
        CL_CommonFunctionManager.Instance.ScriptBoxInActive(); 
        CL_CommonFunctionManager.Instance.QuickMenuActive(false);
        CL_CommonFunctionManager.Instance.TopBarInActive();
    }
    */

    public void OnPointerClick(PointerEventData eventData)
    {
        Button.sprite = buttonSwap[2];

        CL_CommonFunctionManager.Instance.LoadScene(sceneName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Printer.sprite = printerSwap[0];
        Button.sprite = buttonSwap[0];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Printer.sprite = printerSwap[1];
        Button.sprite = buttonSwap[1];
    }
}
