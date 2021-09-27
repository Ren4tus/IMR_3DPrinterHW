using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FindPassword : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Text m_Text;
    public FindPasswordPopup m_Popup;

    public Color m_Normal;
    public Color m_Hover;
    public Color m_Pressed;



    public void OnPointerClick(PointerEventData eventData) 
    {
        m_Popup.PopupOpen();
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        m_Text.color = m_Hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Text.color = m_Normal;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_Text.color = m_Pressed;
    }

}
