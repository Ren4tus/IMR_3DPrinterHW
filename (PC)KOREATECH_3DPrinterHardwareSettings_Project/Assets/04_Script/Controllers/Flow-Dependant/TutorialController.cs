using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class TutorialController : MonoBehaviour
{
    [Header("Main Controller")]
    public MainController mainController;

    [Header("Select Panel")]
    public GameObject TutorialButton;
    public GameObject TutorialSelectPanel;

    [Header("Components and Parameters for Scripting")]
    public TextAsset[] TutorialsInFile;
    public bool[] NeedClick;
    public Text scriptWindow;
    public GameObject HighlightRect;
    private IEnumerator Effect;
    private bool isScriptPlay;
    private int currentIdx;

    [Header("Tutorial Start Panel")]
    // 튜토리얼 패널은 튜토리얼 중에는 언제나 켜져 있어야 합니다.
    public GameObject TutorialPanel;
    public Animator TutorialAnim;

    [Header("Fake Panels For Tutorial")]
    public GameObject HomePanel;
    public GameObject HelpPanel;
    public GameObject TotalMenuPanel;
    public GameObject ConfigurationPanel;
    public GameObject ExitPanel;

    [Header("Base Menu and Buttons")]
    public Button TotalMenuBtn;
    public Button TotlaMenuExitBtn;
    public Button ConfigBtn;
    public Button ExitBtn;
    public GameObject OriginalBaseMenu;
    public Animator baseMenuAnim;
    private bool baseMenuOn = false;

    [Header("Continue Button")]
    public Button ContinueBtn;

    public void Start()
    {
        isScriptPlay = false;
        currentIdx = 0;
    }

    public void TutorialBtnON()
    {
        TutorialButton.SetActive(true);
    }

    public void TutorialBtnOFF()
    {
        TutorialButton.SetActive(false);
    }

    // 튜토리얼 소개 버튼 눌렀을 때
    public void TutorialBtnClicked()
    {
        TutorialSelectPanel.SetActive(true);
        mainController.startByCont(Controllers.Tutorial);
        TutorialButton.SetActive(false);
    }

    // 튜토리얼 시작 여부 창에서 아니오 눌렀을 때
    public void TutorialCancelled()
    {
        TutorialSelectPanel.SetActive(false);
        mainController.finishedByCont(Controllers.Tutorial);
    }

    // 튜토리얼 시작 여부 창에서 예 눌렀을 때
    public void StartTutorial()
    {
        TutorialSelectPanel.SetActive(false);
        TutorialPanel.SetActive(true);
        TutorialAnim.SetBool("Enable", true);
        Invoke("StartNextScript", 1.0f);
    }


    // 현재 인덱스에 해당하는 튜토리얼 시작할 때 필요한 작업 수행
    // 튜토리얼 변경에 따라 수정될 수 있음
    public void StartNextScript()
    {
        switch(currentIdx)
        {
            case 0:
                OriginalBaseMenu.SetActive(false);
                break;

            case 1:
                baseMenuOn = true;
                baseMenuAnim.SetBool("enable", true);
                break;

            case 2:
                TotalMenuBtn.interactable = true;
                HighlightRect.SetActive(true);
                HighlightRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(-842, -18);
                break;

            case 3:
                HighlightRect.SetActive(false);
                TotalMenuBtn.interactable = false;
                TotlaMenuExitBtn.interactable = false;
                break;

            case 4:
                TotlaMenuExitBtn.interactable = true;
                break;

            case 5:
                HighlightRect.SetActive(true);
                HighlightRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(-820.8f, 25);
                ConfigBtn.interactable = true;                
                break;

            case 6:
                HighlightRect.SetActive(false);
                ConfigBtn.interactable = false;
                break;

            case 7:
                HighlightRect.SetActive(true);
                HighlightRect.GetComponent<RectTransform>().anchoredPosition = new Vector2(-830.5f, 73);
                ExitBtn.interactable = true;
                break;

            case 8:
                HighlightRect.SetActive(false);
                ExitBtn.interactable = false;
                break;

            case 10:
                ContinueBtn.gameObject.SetActive(true);
                break;

            default:
                break;
        }

        viewScriptBySeq(currentIdx);
    }

    // 요구되는 버튼 클릭 시 튜토리얼 시퀀스 진행 메소드
    public void StartNextScriptByClick()
    {
        currentIdx++;
        StartNextScript();
    }

    // 첫 튜토리얼(훈련보조도구) 종료 후 학습 진행 버튼 클릭 시
    public void ClickContinueBtn()
    {
        // 튜토리얼이 종료되었으므로 원래 베이스 메뉴 On. 기존 베이스메뉴 Off
        OriginalBaseMenu.SetActive(true);
        TutorialAnim.SetBool("Enable", false);

        // 튜토리얼 스크립트 창 OFF
        scriptWindow.text = "";
        StopCoroutine(Effect);

        // 학습 진행 버튼 끄기
        ContinueBtn.gameObject.SetActive(false);
        Invoke("FinishTutorial", 1.0f);
    }

    // 튜토리얼 종료하고 다음 시퀀스로 넘어가는 메소드
    public void FinishTutorial()
    {
        // 튜토리얼 패널 종료
        TutorialPanel.SetActive(false);

        // 튜토리얼 종료 후 바로 다음 시퀀스 진행
        mainController.finishedByCont(Controllers.Tutorial);
        mainController.goNextSeq();
    }

    // 튜토리얼 스크립트 출력 메소드
    public void viewScriptBySeq(int nextSeq)
    {
        if (isScriptPlay)
        {
            StopCoroutine(Effect);
        }

        string tempStr = TutorialsInFile[nextSeq].text;
        if (ConfigurationController.Language == "KR")
        {
            int startIdx = tempStr.LastIndexOf("<KR>") + "<KR>".Length;
            int lastIdx = tempStr.IndexOf("</KR>");
            tempStr = tempStr.Substring(startIdx, lastIdx - startIdx);
        }
        else if (ConfigurationController.Language == "EN")
        {
            int startIdx = tempStr.LastIndexOf("<EN>") + "<EN>".Length;
            int lastIdx = tempStr.IndexOf("</EN>");
            tempStr = tempStr.Substring(startIdx, lastIdx - startIdx);
        }

        Effect = DialogueEffect(tempStr);
        StartCoroutine(Effect);
    }

    // 튜토리얼 스크립트 이펙트 코루틴 메소드
    private IEnumerator DialogueEffect(string str)
    {
        scriptWindow.text = "";

        isScriptPlay = true;
        int charIdx = 0;
        while (str.Length > charIdx)
        {
            scriptWindow.text += str[charIdx];
            charIdx++;
            yield return new WaitForSeconds(0.05f);
        }
        scriptWindow.text = str;
        isScriptPlay = false;

        if(!NeedClick[currentIdx])
        {
            currentIdx++;
            Invoke("StartNextScript", 1.0f);
        }
    }
}
