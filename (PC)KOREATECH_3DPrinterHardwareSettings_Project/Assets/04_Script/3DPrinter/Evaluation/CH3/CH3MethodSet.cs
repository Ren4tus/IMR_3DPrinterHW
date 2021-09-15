using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CH3MethodSet : MonoBehaviour
{
    const bool CORRECT = true;
    const bool WRONG = false;

    // 
    // 기본 공통
    [Header("FloatingMsg")]
    public Animation FloatingMsg;
    public Image Icon;
    public Text FloatingMsgText;
    public Sprite[] icons;

    [Header("HelpText")]
    public Animation HelpTextAni;
    public Text HelpText;

    public static CH3MethodSet instance;
    private int stepCount = 0;
    /* -------------- */

    [Header("Step1")]
    public Text ScoreStep1;

    [Header("Step2")]
    public Text ScoreStep2_1;
    public Text ScoreStep2_2;

    [Header("Step3")]
    public Text ScoreStep3;

    [Header("Step4")]
    public Text ScoreStep4_1;
    public Text ScoreStep4_2;

    [Header("Step5")]
    public Text ScoreStep5_1;
    public Text ScoreStep5_2;

    [Header("Step6")]
    public Text ScoreStep6_1;
    public Text ScoreStep6_2;

    [Header("Step7")]
    public Text ScoreStep7;

    [Header("Step8")]
    public Text ScoreStep8;

    [Header("Step9")]
    public Text ScoreStep9;

    [Header("ETC")]
    private List<string> toolList = new List<string>();
    private bool toolCheck = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }




    /*
    public void Step10_Check1()
    {
        BuildPlatform.Play("BuildPlatformOut");

        ShowFloatingMsg("빌드 플랫폼 탈거 : +5점", CORRECT);
        ScoreStep10_1.text = " - 빌드 플랫폼 탈거 (<color=#2CA936>5</color>/5)";

        ExamController.instance.SetScore(9, 5);

        StartCoroutine(ShowHelpText("프린팅 된 조형물을 회수해주세요.", 2f));
    }
    */

    public void StartExam()
    {
        StartCoroutine(ShowHelpText("프리터 상태를 확인해주세요.", 2f));
    }

    public bool PrevStepClearCheck(int stepNum)
    {
        bool flag = true;

        for (int i = 0; i < stepNum - 1; i++)
        {
            if (!ExamController.instance.IsClear(i))
            {
                flag = false;
            }
        }

        return flag;
    }

    private void PopUp_Warning(string text)
    {
        CL_CommonFunctionManager.Instance.MakePopUp().PopUp(text, CL_MessagePopUpController.DialogType.WARNING, null);
    }

    // 최대 글자
    // ShowFloatingMsg("가나다라마바사아자차카타파하가나다가나다라마바사아자차카타-", CORRECT);
    private void ShowFloatingMsg(string context, bool isCorrect)
    {
        Icon.sprite = isCorrect ? icons[0] : icons[1];
        FloatingMsgText.text = context;

        if (FloatingMsg.isPlaying)
            FloatingMsg.Stop();

        FloatingMsg.Play("FloatingMsg");
    }

    // 최대 글자
    // ShowHelpText("가나다라마바사아자차카타파하가나다라");
    private IEnumerator ShowHelpText(string context, float time)
    {
        HelpText.text = context;

        yield return new WaitForSeconds(time);

        if (HelpTextAni.isPlaying)
            HelpTextAni.Stop();

        HelpTextAni.Play("HelpTextAni");
    }
}
