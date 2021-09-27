using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegisterCourse : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Text m_Text;
	public Color m_Normal;
	public Color m_Hover;
	public Color m_Pressed;

	public void OnPointerExit(PointerEventData eventData)
	{
		m_Text.color = m_Normal;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		m_Text.color = m_Hover;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OpenStepPage();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		m_Text.color = m_Pressed;
	}

	void OpenStepPage()
	{
		Application.OpenURL("https://e-koreatech.step.or.kr/page/lms/?m1=course%25&m2=course_list%25&startCount=0%25&filter_list=sort%3Dcreation_day%2FDESC%26contentCd%3D2%25");
	}
    
    void Start()
    {
        m_Text.text ="수강신청 하러가기";
    }
}
