using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CH1MethodSet : MonoBehaviour
{
    const bool CORRECT = true;
    const bool WRONG = false;

    // 여기 모든 메소드를 모으고 여기서 점수를 쏜다?
    // 대략 26개의 메소드가 나올 것 같음
    [Header("FloatingMsg")]
    public Animation FloatingMsg;
    public Image Icon;
    public Text FloatingMsgText;
    public Sprite[] icons;

    [Header("HelpText")]
    public Animation HelpTextAni;
    public Text HelpText;

    [Header("Print Complete obj")]
    public GameObject Wrench;

    // 
    private bool step2_1 = true;
    private bool step3_1 = true;
    private bool step3_2 = true;
    private bool step4 = true;
    private bool step5_2 = true;
    private bool step6 = true;
    private bool step7 = true;
    private bool step8 = true;
    private bool step8_2 = true;

    public static CH1MethodSet instance;

    public GameObject MaterialPanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Step1_Correct()
    {
        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("컴퓨터와 프린터가 연결되었습니다.", CL_MessagePopUpController.DialogType.NOTICE, () => ShowFloatingMsg("컴퓨터와 프린터를 연결 : +10점", CORRECT));

        // 점수 보내기
        ExamController.instance.SetScore(0, 10);

        // 클리어
        ExamController.instance.ExamStepClear(0);
    }

    public void Step1_Wrong()
    {
        ShowFloatingMsg("컴퓨터와 프린터가 연결되지 않음 : -5점", WRONG);
        // CL_CommonFunctionManager.Instance.MakePopUp().PopUp("컴퓨터와 프린터가 연결되었습니다.", CL_MessagePopUpController.DialogType.NOTICE, () => ShowFloatingMsg("프린터가 연결되지 않았습니다. : -5점", WRONG));
        // PopUp_Warning("프린터가 연결되지 않았습니다.");
        // 점수 보내기
        ExamController.instance.SetScore(0, -5);
    }

    public void Step2_1_Correct()
    {
        if (step2_1)
        {
            step2_1 = false;

            ShowFloatingMsg("프린터 상태를 확인 : +10점", CORRECT);
            RandomTemp();

            // 점수 보내기
            ExamController.instance.SetScore(1, 10);
        }
    }

    public void Step2_2_Correct()
    {
        ShowFloatingMsg("프린터 상태를 확인한 후 정상조치 : +10점", CORRECT);

        // 점수 보내기
        ExamController.instance.SetScore(1, 10);

        // 클리어
        ExamController.instance.ExamStepClear(1);
    }

    public void Step3_1_Check()
    {
        if (SlicerBuildTrayView.instance != null)
            if (SlicerBuildTrayView.instance.InnerFill() == 1 &&
                SlicerBuildTrayObjColorChange.instance.isArrange())
            {
                step3_1 = false;
                ShowFloatingMsg("모델의 위치가 정상이고 내부 채움 옵션을 만족 : +10점", CORRECT);

                // 점수 보내기
                ExamController.instance.SetScore(2, 10);
            }
    }

    public void Step3_2_Check()
    {
        if (SlicerBuildTrayObjColorChange.instance != null)
            if (SlicerBuildTrayObjColorChange.instance.ErrorCheck() &&
                SlicerBuildTrayObjColorChange.instance.TimerCheck())
            {
                step3_2 = false;
                ShowFloatingMsg("배치 오류 확인과 소요시간을 확인 : +10점", CORRECT);

                // 점수 보내기
                ExamController.instance.SetScore(2, 10);

                // 클리어
                ExamController.instance.ExamStepClear(2);
            }
    }

    public void Step4_Correct()
    {
        if (SlicerBuildTrayObjColorChange.instance != null)
        {
            if (SlicerBuildTrayObjColorChange.instance.isArrange() && step4)
            {
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("조형물이 프린팅 되었습니다.\n프린터로 이동하여 확인하세요.", CL_MessagePopUpController.DialogType.NOTICE, () => ShowFloatingMsg("정상적으로 프린팅 : +10점", CORRECT));
                step4 = false;
                MJPComputerAS.instance.SlicerClose();
                GameObject.Find("monitor_3dObj").SetActive(false);
                Wrench.SetActive(true);

                // 점수 보내기
                ExamController.instance.SetScore(3, 10);

                // 클리어
                ExamController.instance.ExamStepClear(3);
            }
            else
            {
                ShowFloatingMsg("모델이 트레이 범위 안에 있지 않음 : -5점", WRONG);

                // 점수 보내기
                ExamController.instance.SetScore(3, -5);
            }
        }
    }

    // 스크래퍼를 사용하여 조형물 회수
    public bool Step5_1_Check()
    {
        if(EquipList.instance.EquipCheck("scraperL"))
        {
            ShowFloatingMsg("스크래퍼를 사용하여 조형물 분리 : +5점", CORRECT);

            // 점수 보내기
            ExamController.instance.SetScore(4, 5);

            // 클리어
            ExamController.instance.ExamStepClear(4);

            SafetyEquipCheck();

            return false;
        }
        else
        {
            PopUp_Warning("스크래퍼를 사용하여 조형물을 분리하세요.");
        }

        return true;
    }

    // 회수 후 트레이 정리
    public void Step5_2_Correct()
    {
        if (ExamController.instance.IsClear(3) && step5_2 && ExamController.instance.IsClear(4))
        {
            step5_2 = false;

            ShowFloatingMsg("회수 후 트레이 정리 : +5점", CORRECT);

            // 점수 보내기
            ExamController.instance.SetScore(4, 5);
        }
        else if(ExamController.instance.IsClear(3) && !ExamController.instance.IsClear(4))
        {
            PopUp_Warning("조형물을 먼저 회수해주세요.");
        }
        else
        {
            PopUp_Warning("트레이에 회수할 조형물이 없습니다.");
        }
    }

    // 조형물 후처리(워터젯 사용)
    public void Step6_Correct()
    {
        if (ExamController.instance.IsClear(4) && step6)
        {
            step6 = false;

            ShowFloatingMsg("워터젯을 사용하여 서포트 제거 : +10점", CORRECT);

            // 점수 보내기
            ExamController.instance.SetScore(5, 10);
        }
        else
        {
            PopUp_Warning("서포트를 제거할 조형물이 없습니다.");
        }
    }

    // 헤드 청소
    public void Step7_Correct()
    {
        if (ExamController.instance.IsClear(4) && step7)
        {
            step7 = false;

            ShowFloatingMsg("헤드 청소 완료 : +10점", CORRECT);

            // 점수 보내기
            ExamController.instance.SetScore(6, 10);
        }
        else
        {
            PopUp_Warning("헤드 청소를 할 필요가 없습니다.");
        }
    }

    // 재료 보충
    public void Step8_1_Correct(int n)
    {
        if (ExamController.instance.IsClear(4) && step8)
        {
            if(step8)
            {
                step8 = false;

                MJPASController.Instance.MaterialToggle(n);
                MJPASController.Instance.MaterialToggle(n);

                ShowFloatingMsg("소진된 재료를 제거하고 새 재료 투입 : +5점", CORRECT);

                // 점수 보내기
                ExamController.instance.SetScore(7, 5);

                // 클리어
                ExamController.instance.ExamStepClear(7);
            }
            else
            {
                // 지금 안댐
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("재료 보충이 완료되었습니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
            }
        }
        else
        {
            PopUp_Warning("재료를 보충할 필요가 없습니다.");
        }
    }

    // 재료 보충된 것 확인
    public void Step8_2_Correct()
    {
        if(ExamController.instance.IsClear(7))
        {
            if(MaterialPanel.activeSelf)
            {
                ShowFloatingMsg("재료 보충 후 재료 상태 확인 : +5점", CORRECT);

                // 점수 보내기
                ExamController.instance.SetScore(7, 5);

                // 끝
                ExamController.instance.Finish();
            }
        }
    }

    private void SafetyEquipCheck()
    {
        if(!EquipList.instance.EquipCheck("Dustproof_mask") ||
           !EquipList.instance.EquipCheck("Nitril_gloves") ||
           !EquipList.instance.EquipCheck("Safety_glasses"))
        {
            ExamController.instance.SetEtcScore(0, false);
        }
    }

    private void Update()
    {
        if(step3_1)
        {
            Step3_1_Check();
        }
        if(step3_2)
        {
            Step3_2_Check();
        }
        if (step8_2)
        {
            Step8_2_Correct();
        }
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

    private void ShowHelpText()
    {

    }

    private void RandomTemp()
    {
        Color wrongColor = new Color(253/255f, 178/255f, 180/255f);
        Color correctColor = new Color(172/255f, 219/255f, 185/255f);
        int n = Random.Range(0, 10);

        // 헤드 온도
        if (n >= 5)
        {
            GameObject HeadTemp1 = GameObject.Find("ExamHeadTemp1");
            GameObject HeadTemp2 = GameObject.Find("ExamHeadTemp2");
            Text HeadTemp1Text = GameObject.Find("ExamHeadTempText1").GetComponent<Text>();
            Text HeadTemp2Text = GameObject.Find("ExamHeadTempText2").GetComponent<Text>();

            HeadTemp1.GetComponent<Image>().color = wrongColor;
            HeadTemp2.GetComponent<Image>().color = wrongColor;

            n = Random.Range(0, 10);
            int m;

            if (n >= 5)
            {
                m = Random.Range(0, 60);

                HeadTemp1Text.text = m.ToString();
                HeadTemp2Text.text = m.ToString();
            }
            else
            {
                m = Random.Range(100, 120);

                HeadTemp1Text.text = m.ToString();
                HeadTemp2Text.text = m.ToString();
            }

            HeadTemp1.GetComponent<Button>().onClick.AddListener(() =>
            {
                HeadTemp1.GetComponent<Image>().color = correctColor;
                HeadTemp2.GetComponent<Image>().color = correctColor;
                HeadTemp1Text.text = "72";
                HeadTemp2Text.text = "72";

                HeadTemp1.GetComponent<Button>().interactable = false;
                HeadTemp2.GetComponent<Button>().interactable = false;

                Step2_2_Correct();
            });
            HeadTemp2.GetComponent<Button>().onClick.AddListener(() =>
            {
                HeadTemp1.GetComponent<Image>().color = correctColor;
                HeadTemp2.GetComponent<Image>().color = correctColor;
                HeadTemp1Text.text = "72";
                HeadTemp2Text.text = "72";

                HeadTemp1.GetComponent<Button>().interactable = false;
                HeadTemp2.GetComponent<Button>().interactable = false;

                Step2_2_Correct();
            });
        }
        else // 챔버온도
        {
            GameObject ChamberTemp = GameObject.Find("ExamChamberTemp");
            Text ChamberTempText = GameObject.Find("ExamChamberTempText").GetComponent<Text>(); ;

            ChamberTemp.GetComponent<Image>().color = wrongColor;

            n = Random.Range(0, 10);
            int m;

            if (n >= 5)
            {
                m = Random.Range(0, 60);

                ChamberTempText.text = m.ToString();
            }
            else
            {
                m = Random.Range(100, 120);

                ChamberTempText.text = m.ToString();
            }

            ChamberTemp.GetComponent<Button>().onClick.AddListener(() =>
            {
                ChamberTemp.GetComponent<Image>().color = correctColor;
                ChamberTempText.text = "43";

                ChamberTemp.GetComponent<Button>().interactable = false;

                Step2_2_Correct();
            });
        }
    }
}
