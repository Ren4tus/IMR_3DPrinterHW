using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CH2MethodSet : MonoBehaviour
{
    const bool CORRECT = true;
    const bool WRONG = false;

    const string CORRECT_USER_ID = "guest";
    const string CORRECT_USER_PASSWORD = "0";

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

    public static CH2MethodSet instance;
    private int stepCount = 0;

    /* -------------- */
    [Header("Step1")]
    public Button MaterialCartBtn;
    public Text ScoreStep1;
    public Animation MaterialCart;
    public Animation ChamberPin;
    public Animation Connector;
    public bool isUpperDoorOpen = false;
    public bool isLowerDoorOpen = false;

    [Header("Step2")]
    public Animation BuildPlatform;
    public Text ScoreStep2;

    [Header("Step3")]
    public Animation PowerSwitch;
    public Button PrinterBackBtn;
    public GameObject GUI_ClickCollider;
    public Text ScoreStep3;

    [Header("Step4")]
    public InputField UserID;
    public InputField UserPasssword;
    public GameObject GUI_Login;
    public GameObject GUI_PrinterStatus;
    public Text ScoreStep4;

    [Header("Step5")]
    public Text ScoreStep5;

    [Header("Step6")]
    public bool isBottleOut = false;
    public bool isMaterialBottleOut = false;
    public Text ScoreStep6;

    [Header("Step7")]
    public Text ScoreStep7;

    [Header("Step8")]
    public GameObject Slicer;
    public GameObject ExpandUIPanel;
    public GameObject UICamera;
    public GameObject UI_BlackBG_Clicks;
    public GameObject UI_BlackBG_Camera;
    public Text ScoreStep8_1;
    public Text ScoreStep8_2;
    public PPSlicerController slicerController;
    public Animation USB;
    public bool usbIn = false;
    private bool isSlicerOpen = false;
    public bool isAllCollocated = false;

    [Header("Step9")]
    public Text ScoreStep9;
    public GameObject[] PrintedObj;

    [Header("Step10")]
    public Text ScoreStep10_1;
    public Text ScoreStep10_2;
    public GameObject BuildPlatform_Obj;
    private Vector3 BuildPlatform_Pos = new Vector3(-0.025f, 0.7892f, 3.41f);
    private Vector3 BuildPlatform_Rot = new Vector3(-90f, 0f, 0f);

    [Header("Step11")]
    public Text ScoreStep11_1;
    public Text ScoreStep11_2;
    
    [Header("ETC")]
    private List<string> toolList = new List<string>();
    private bool toolCheck = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (MainController.instance.GetCurrentSeq() == 2 && !isSlicerOpen && usbIn)
        {
            isSlicerOpen = true;
            Slicer.SetActive(true);
            ExpandUIPanel.SetActive(false);
            UICamera.SetActive(false);
            UI_BlackBG_Clicks.SetActive(false);
            UI_BlackBG_Camera.SetActive(false);
            GUI_ClickCollider.SetActive(false);
            GUI_Login.SetActive(false);
            GUI_PrinterStatus.SetActive(false);
            slicerController.SlicerOpen();
        }

        if(isAllCollocated)
        {
            isAllCollocated = false;

            Step8_Check1();

            if(ExamController.instance.ImportOption == slicerController.ImportOption &&
                ExamController.instance.InternalOption == slicerController.InternalOption)
            {
                Invoke("Step8_Check2", 2f);
            }
        }
    }

    public void StartExam()
    {
        StartCoroutine(ShowHelpText("재료카트를 프린터에 장착하세요.", 2f));
    }

    public void Step1_Check()
    {
        if(isUpperDoorOpen && isLowerDoorOpen)
        {
            MaterialCart.Play("MaterialCartIn");
            ChamberPin.Play("ChamberPin");
            Connector.Play("ConnectorIn");

            ShowFloatingMsg("재료카트 장착 : +10점", CORRECT);
            ScoreStep1.text = " - 재료카트 장착 (<color=#2CA936>10</color>/10)";
            MaterialCartBtn.onClick.AddListener(() => MainController.instance.GoJumpSeq(7));

            ExamController.instance.SetScore(0, 10);
            ExamController.instance.ExamStepClear(0);

            StartCoroutine(ShowHelpText("빌드 플랫폼을 장착하세요.", 2f));
        }
        else
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("챔버 도어가 닫혀있습니다.", CL_MessagePopUpController.DialogType.WARNING);
        }
    }

    public void Step2_Check()
    {
        if(ExamController.instance.IsClear(0) && !ExamController.instance.IsClear(1))
        {
            BuildPlatform.Play("BuildPlatform");

            ShowFloatingMsg("빌드 플랫폼 장착 : +10점", CORRECT);
            ScoreStep2.text = " - 빌드 플랫폼 장착 (<color=#2CA936>10</color>/10)";
            
            ExamController.instance.SetScore(1, 10);
            ExamController.instance.ExamStepClear(1);

            StartCoroutine(ShowHelpText("프린터의 전원을 공급하세요.", 2f));
        }
        else if(!ExamController.instance.IsClear(0))
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("재료카트를 장착해야합니다.", CL_MessagePopUpController.DialogType.WARNING);
        }
    }

    public void Step3_Check()
    {
        if(PrevStepClearCheck(2) && !ExamController.instance.IsClear(2))
        {
            PowerSwitch.Play("PowerSwitch");

            ShowFloatingMsg("전원 공급 : +10점", CORRECT);
            ScoreStep3.text = " - 전원 공급 (<color=#2CA936>10</color>/10)";

            ExamController.instance.SetScore(2, 10);
            ExamController.instance.ExamStepClear(2);

            GUI_Login.SetActive(true);
            GUI_ClickCollider.SetActive(true);

            StartCoroutine(ShowHelpText("프린터 시스템에 로그인하세요.", 2f));

            PrinterBackBtn.transform.GetChild(0).GetComponent<Text>().text = "프린터 측면";
            PrinterBackBtn.onClick.AddListener(() => MainController.instance.GoJumpSeq(8));
        }
        else if(PrevStepClearCheck(3))
        {
            return;
        }
        else
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("이전 단계를 완료하여 주세요.", CL_MessagePopUpController.DialogType.WARNING);
        }
    }

    public void Step4_Check()
    {
        if(PrevStepClearCheck(3))
        {
            if(UserID.text == CORRECT_USER_ID && UserPasssword.text == CORRECT_USER_PASSWORD)
            {
                GUI_Login.SetActive(true);
                GUI_PrinterStatus.SetActive(true);

                ShowFloatingMsg("시스템 로그인 : +5점", CORRECT);
                ScoreStep4.text = " - 프린터 시스템 로그인 (<color=#2CA936>5</color>/5)";

                ExamController.instance.SetScore(3, 5);
                ExamController.instance.ExamStepClear(3);

                Invoke("Step5_Check", 2f);
            }
            else
            {
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("아이디와 비밀번호를 확인해주세요.", CL_MessagePopUpController.DialogType.WARNING);
            }
        }
        else
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("이전 단계를 완료하여 주세요.", CL_MessagePopUpController.DialogType.WARNING);
        }
    }

    public void Step5_Check()
    {
        ShowFloatingMsg("프린터 상태 확인 : +10점", CORRECT);
        ScoreStep5.text = " - 챔버와 재료의 온도 확인 (<color=#2CA936>10</color>/10)";

        ExamController.instance.SetScore(4, 10);
        ExamController.instance.ExamStepClear(4);

        StartCoroutine(ShowHelpText("재료카트의 재료를 보충해주세요.", 2f));
    }

    public void Step6_Check()
    {
        ShowFloatingMsg("재료 보충 : +10점", CORRECT);
        ScoreStep6.text = " - 재료카트의 재료 보충 (<color=#2CA936>10</color>/10)";

        ExamController.instance.SetScore(5, 10);
        ExamController.instance.ExamStepClear(5);

        StartCoroutine(ShowHelpText("빌드 플랫폼 자동 레벨링을 하세요.", 2f));
    }

    public void Step7_Check()
    {
        if (PrevStepClearCheck(6) && !ExamController.instance.IsClear(6))
        {
            ShowFloatingMsg("빌드 플랫폼 자동 레벨링 : +10점", CORRECT);
            ScoreStep7.text = " - 빌드 플랫폼 레벨링 (<color=#2CA936>10</color>/10)";

            ExamController.instance.SetScore(6, 10);
            ExamController.instance.ExamStepClear(6);

            StartCoroutine(ShowHelpText("슬라이싱 단계를 완료하세요.", 2f));
        }
        else
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("이전 단계를 완료하여 주세요.", CL_MessagePopUpController.DialogType.WARNING);
        }
    }

    public void Step8_Check1()
    {
        ShowFloatingMsg("배치오류 확인 : +5점", CORRECT);
        ScoreStep8_2.text = " - 배치오류 확인 (<color=#2CA936>5</color>/5)";

        ExamController.instance.SetScore(7, 5);
        ExamController.instance.ExamStepClear(7);
    }

    public void Step8_Check2()
    {
        ShowFloatingMsg("조형물 및 내부채움 만족 : +5점", CORRECT);
        ScoreStep8_1.text = " - 조형물 및 내부채움 만족 (<color=#2CA936>5</color>/5)";

        ExamController.instance.SetScore(7, 5);
    }

    public void Step9_Popup()
    {
        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("프린팅이 진행되면 다시 슬라이싱을 진행할 수 없습니다. 이대로 프린팅을 진행할까요?",
                                                            CL_MessagePopUpController.DialogType.NOTICE,
                                                            Step9_Check,
                                                            null);
    }

    public void Step9_Check()
    {
        // 팝업창 추가로 인해 추가된 코드
        slicerController.ExitEditMode();

        Slicer.SetActive(false);
        PrintedObj[slicerController.ImportOption].SetActive(true);

        ShowFloatingMsg("프린팅 : +10점", CORRECT);
        ScoreStep9.text = " - 조형물 프린팅 (<color=#2CA936>10</color>/10)";

        ExamController.instance.SetScore(8, 5);
        ExamController.instance.ExamStepClear(8);

        StartCoroutine(ShowHelpText("빌드 플랫폼을 탈거해주세요.", 2f));
    }

    public void Step10_Check1()
    {
        BuildPlatform.Play("BuildPlatformOut");

        ShowFloatingMsg("빌드 플랫폼 탈거 : +5점", CORRECT);
        ScoreStep10_1.text = " - 빌드 플랫폼 탈거 (<color=#2CA936>5</color>/5)";

        ExamController.instance.SetScore(9, 5);

        StartCoroutine(ShowHelpText("프린팅 된 조형물을 회수해주세요.", 2f));
    }

    public void Step10_Check2()
    {
        BuildPlatform_Obj.transform.localPosition = BuildPlatform_Pos;
        BuildPlatform_Obj.transform.localRotation = Quaternion.Euler(BuildPlatform_Rot);

        ShowFloatingMsg("조형물 회수 : +5점", CORRECT);
        ScoreStep10_2.text = " - 조형물 회수 (<color=#2CA936>5</color>/5)";

        ExamController.instance.SetScore(9, 5);
        ExamController.instance.ExamStepClear(9);

        StartCoroutine(ShowHelpText("조형물 후처리를 진행해주세요.", 2f));
    }

    public void Step11_Check1()
    {
        ShowFloatingMsg("공구를 이용하여 후처리 : +5점", CORRECT);
        ScoreStep11_1.text = " - 공구를 이용하여 후처리 (<color=#2CA936>5</color>/5)";

        ExamController.instance.SetScore(10, 5);

        StartCoroutine(ShowHelpText("경화장치를 이용하여 후처리하세요.", 2f));
    }

    public void Step11_Check2()
    {
        ShowFloatingMsg("경화장치 사용 : +5점", CORRECT);
        ScoreStep11_2.text = " - 경화장치 사용 (<color=#2CA936>5</color>/5)";

        ExamController.instance.SetScore(10, 5);
        ExamController.instance.ExamStepClear(10);

        ExamController.instance.Finish();
    }

    public bool PrevStepClearCheck(int stepNum)
    {
        bool flag = true;

        for(int i=0; i<stepNum-1; i++)
        {
            if(!ExamController.instance.IsClear(i))
            {
                flag = false;
            }
        }

        return flag;
    }

    public void UseToolCheck(string obj)
    {
        if (ExamController.instance.IsClear(9))
        {
            if (toolList.Contains(obj))
            {
                return;
            }
            else
            {
                // 애니메이션 재생
                toolList.Add(obj);
            }

            if (toolList.Contains("Grinder") && toolList.Contains("Sandpaper") && toolList.Contains("hand_file") && !toolCheck)
            {
                toolCheck = true;
                Step11_Check1();
            }
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