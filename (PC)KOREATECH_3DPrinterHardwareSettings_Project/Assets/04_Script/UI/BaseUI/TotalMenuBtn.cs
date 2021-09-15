using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TotalMenuBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Chapter Move Data")]
    public string NextChapterStr;
    public string NextSceneNameStr;
    public Image BGImage;
    public Text text;
    public BaseUIController baseUIController;
    private Color BGColor = new Color(220.0f / 255, 220.0f / 255, 220.0f / 255);
    private Color BGColorHovered = new Color(22.0f / 255, 120.0f / 255, 231.0f / 255);

    public void OnPointerClick(PointerEventData eventData)
    {
        baseUIController.OnChapMovePanel(NextChapterStr, NextSceneNameStr);
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        BGImage.color = BGColorHovered;
        text.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BGImage.color = BGColor;
        text.color = Color.black;
    }
    
}
