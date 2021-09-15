using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class PopupController : MonoBehaviour
{
    public MainController mainController;
    [Space]
    public Button[] button;                     //시점별 버튼
    public TextAsset[] componentName;           //부품명
    public TextAsset[] componentDescription;    //부품명 설명
    public Text compName;
    public Text compDescription;
    public GameObject popupPanel;
    public GameObject popupPanel2;
    public GameObject popupPanel3;
    public GameObject TestSequencePanel;
    public GameObject TestResultPanel;

    public void firstSceneInit()
    {
        //처음 시작 시 첫 팝업 버튼 제외하고 전부 가림
        button[0].gameObject.SetActive(true);
        button[0].onClick.AddListener(showAndHidePopup);
        for(int i = 1; i < button.Length; i++)
        {
            button[i].gameObject.SetActive(false);
            if (i != button.Length - 1)
                button[i].onClick.AddListener(showAndHidePopup);
            else
                continue;
        }
        popupPanel.SetActive(false);
        popupPanel2.SetActive(false);
        popupPanel3.SetActive(false);
        TestSequencePanel.SetActive(false);
        TestResultPanel.SetActive(false);
        mainController.finishedByCont(Controllers.Popup);
    }

    

    public void viewPopupButton(int nextSeq)
    {
        //시퀀스 전환마다 해당 팝업 버튼 제외하고 전부 가림
        for(int i = 0; i < button.Length; i++)
        {
            if (i == nextSeq)
                button[i].gameObject.SetActive(true);
            else
                button[i].gameObject.SetActive(false);
        }
    }

    public void viewPopupBySeq(int nextSeq)
    {
        //땜빵 문자열 변수
        string tempName = componentName[nextSeq].text;
        string tempDescription = componentDescription[nextSeq].text;
        //한글일 때 글자 출력
        if(ConfigurationController.Language == "KR")
        {
            int startNameIdx = tempName.LastIndexOf("<KR>") + "<KR>".Length;
            int startDescriptionIdx = tempDescription.LastIndexOf("<KR>") + "<KR>".Length;
            int lastNameIdx = tempName.IndexOf("</KR>");
            int lastDescriptionIdx = tempDescription.IndexOf("</KR>");
            tempName = tempName.Substring(startNameIdx, lastNameIdx - startNameIdx);
            tempDescription = tempDescription.Substring(startDescriptionIdx, lastDescriptionIdx - startDescriptionIdx);
        }
        //영어일 때 글자 출력
        else if(ConfigurationController.Language == "EN")
        {
            int startNameIdx = tempName.LastIndexOf("<EN>") + "<EN>".Length;
            int startDescriptionIdx = tempDescription.LastIndexOf("<EN>") + "<EN>".Length;
            int lastNameIdx = tempName.IndexOf("</EN>");
            int lastDescriptionIdx = tempName.IndexOf("</EN>");
            tempName = tempName.Substring(startNameIdx, lastNameIdx - startNameIdx);
            tempDescription = tempDescription.Substring(startDescriptionIdx, lastDescriptionIdx - startDescriptionIdx);
        }

        compName.text = tempName;
        compDescription.text = tempDescription;
        
    }

    public void showAndHidePopup()
    {
        if (popupPanel.activeSelf == false)
            popupPanel.SetActive(true);
        else
            popupPanel.SetActive(false);
    }

    //두 번째 팝업 활성화 함수
    public void viewSecondPopup()
    {
        popupPanel2.SetActive(true);
    }

    //세 번째 팝업 활성화 함수
    public void viewThirdPopup()
    {
        popupPanel2.SetActive(false);
        popupPanel3.SetActive(true);
    }
}
