using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConfigurationUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum UIType
    {
        CheckBox,
        Switch
    }

    public UIType uiType;

    [Header("CheckBox")]
    public Sprite CheckboxHighlight;
    public Sprite CheckboxOn;

    [Header("Switch")]
    public Sprite[] SwitchHighlight;
    public Sprite SwitchOn;

    [Header("TargetImage")]
    public Image targetImage;
    private Sprite tmpImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tmpImage = targetImage.sprite;

        if(uiType == UIType.CheckBox)
        {
            if (targetImage.sprite == CheckboxOn)
                return;
            else
                targetImage.sprite = CheckboxHighlight;
        }
        else
        {
            if (targetImage.sprite == SwitchOn)
                targetImage.sprite = SwitchHighlight[0];
            else
                targetImage.sprite = SwitchHighlight[1];
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetImage.sprite = tmpImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tmpImage = targetImage.sprite;
    }
}
