using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EvaluationCore;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EvaluationSceneController : MonoBehaviour
{
    private static EvaluationSceneController instance = null;

    public static EvaluationSceneController Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    [Header("Settings")]
    public int EvaluationTime = 1800;
    public int StartSequence = 0;
    public int StartStep = 0;

    public string TitleText;
    public string SubtitleText;

    [Space]
    public int AssignmentModel = 1; // 1 또는 2
    public int AssignmentInternalOption = 0;

    [Space]
    public float GuidePopupDuraion = 3.0f;

    [Header("평가표")]
    [SerializeField]
    public List<EvaluationSequence> Sequences;
    public EvaluationContainer SequenceConatiner = null;
    public EvaluationInteractiveObject[] InteractiveObjects;
    public EvaluationInteractiveUI[] InteractiveUIs;
    public EvaluationCH3Controller EvaluationCH3Controller;

    [Header("튜토리얼")]
    public EvaluationTutorialController TutorialController;
    public TitleIndicatorController TitleIndicator;

    [Space]
    [Header("평가 과제 안내")]
    public EvaluationPopupController AssignmentPopup;
    public EvaluationScoreMessage ScoreMsgController;
    public EvaluationObjectInfo FollowGuideController;
    public BehaviorGuideUIController GuideUIController;
    public ChecklistController Checklist;
    public EvaluationRatingCanvasController RatingPanel;

    public bool IsPassSafety = true;
    public bool IsCompleteInTime = true;

    [Header("슬라이서")]
    public EvaluationSlicerController SlicerController;
    public bool IsPrintingComplete = false;

    [Header("타이머")]
    public bool isEvaluationStart = false;
    public EvaluationTimerPanel Timer;
    private IEnumerator _timerCoroutine = null;
    private WaitForSeconds _waitTime = new WaitForSeconds(1f);

    [Header("카메라")]
    public CameraControlByMouse cameraController;

    [Header("장비 장착 확인")]
    public List<EvaluationEquipableItem> EquipableItems;

    public bool startSeq = true;

    [Header("CameraMove")]
    public Transform cam_obj;
    public Transform cam;

    [Header("SlicerSeq")]
    public int slicerSeq;
    public int printSeq;
    public bool isSkip = false;
    public int CH = 0;

    [Header("Tutorial Num")]
    public int tutorialNum = 0;

    // -------------------------------------------------------------------------------------
    // Initialize
    // -------------------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        SequenceConatiner = new EvaluationContainer();
        SequenceConatiner.MakeIndex(Sequences);

        GetEvaluationInteractiveObjects();
    }

    private void Start()
    {
        IsPassSafety = true;
        IsCompleteInTime = true;

        Checklist.MakeEvaluationCheckList(SequenceConatiner);
        Checklist.HideChecklist(0f);
        Checklist.ActiveChecklist();

        Timer.SetTimerText("평가 시작 전");

        if (CL_CommonFunctionManager.Instance.UseTutorial()) // 튜토리얼 사용
        {
            CL_CommonFunctionManager.Instance.TutorialPanelOpen(tutorialNum);
        }
        else // 미사용
        {
            StartInit();
        }
    }

    public void GetEvaluationInteractiveObjects()
    {
        // 모든 상호작용 오브젝트 불러오기, Awake 시 한번만 호출
        InteractiveObjects = GameObject.FindObjectsOfType<EvaluationInteractiveObject>();
        InteractiveUIs = GameObject.FindObjectsOfType<EvaluationInteractiveUI>();
    }

    public void StopEvaluationInteractiveObjectsCoroutine()
    {
        // 평가 종료시 모든 상호작용 오브젝트 코루틴 중지
        int i = 0;

        for (i = 0; i < InteractiveObjects.Length; i++)
        {
            Debug.Log(InteractiveObjects[i].GetType().Name);
            InteractiveObjects[i].StopAllCoroutines();
        }

        for (i = 0; i < InteractiveUIs.Length; i++)
        {
            Debug.Log(InteractiveUIs[i].GetType().Name);
            InteractiveUIs[i].StopAllCoroutines();
        }

        if (EvaluationCH3Controller != null)
            EvaluationCH3Controller.StopAllCoroutines();
    }


    public void StartInit()
    {
        TitleIndicator.ShowTitleSlideIn(TitleText, SubtitleText);
        StartCoroutine(DelayedStart(2.5f));
    }

    private void OnDestroy()
    {
        instance = null;
        SequenceConatiner = null;
        Sequences = null;

        Destroy(this);
    } 


    public void ShowTips()
    {
        GuideUIController.Popup(GuidePopupDuraion);

        //IMRLAB 10-21
        //팁 보기 문장 전송

        XAPIApplication.current.AddHintCount(1);
        XAPIApplication.current.SendIMRXAPIStatement("Hint");
        Debug.Log("click help" + GuideUIController.TextUI.text);
    }

    // -------------------------------------------------------------------------------------
    // Process
    // -------------------------------------------------------------------------------------
    public void EvaluationStart()
    {
        isEvaluationStart = true;
        //IMRLAB 10-07 씬 로드시 init 문장 전송
        XAPIApplication.current.SendInitStatementBySceneName();
         
        //IMRLAB 11-04 힌트 텍스트 init
        XAPIApplication.current.nowLessonManager.SetHintStatementResultExtensions(SequenceConatiner._sequenceList[0].scoreItems[0].Tip);
        XAPIApplication.current.nowLessonManager.ChangeNewStatement("Hint");
        GuideUIController.SetMessage(SequenceConatiner._sequenceList[0].scoreItems[0].Tip);

        if (CL_CommonFunctionManager.Instance.UseTutorial())
        {
            // 튜토리얼 보기 옵션이 체크되어 있을 경우, 튜토리얼 먼저 진행 
            CameraControllOn();
            TimerStart(EvaluationTime);
        }
        else
        {
            // 튜토리얼을 사용하지 않을 경우 바로 평가 진행
            CameraControllOn();
            TimerStart(EvaluationTime);
        }
    }
    public void EvaluationRetry()
    {
        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("평가를 다시 시작할까요?",
                                                                   CL_MessagePopUpController.DialogType.NOTICE,
                                                                   () =>
                                                                   {
                                                                       CL_CommonFunctionManager.Instance.QuickMenuActive(true);
                                                                       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                                                                   },
                                                                   null);
    }

    public void EvaluationEnd()
    {
        TimerStop();
        StopEvaluationInteractiveObjectsCoroutine();

        isEvaluationStart = false;

        if (IsPassSafety)
            IsPassSafety = IsAllEquipSafetyEquipment();

        IsCompleteInTime = SequenceConatiner.IsAllComplete();
        cameraController.CameraControllOff();

        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("평가가 종료되었습니다.", CL_MessagePopUpController.DialogType.NOTICE, RatingPanelOpen);
        //IMRLAB - 10/14 완료 문장 전송
        if (!XAPIApplication.current.terminatied)
        {
            XAPIApplication.current.GetResultCanvas(SequenceConatiner);
            Debug.Log("Terminated send");
            XAPIApplication.current.terminatied = true;

        }
    }
    
    public int[] GetParentSequenceIndex(int targetSeq)
    {
        return SequenceConatiner._sequenceList[targetSeq].postProcessSequences;
    }
    public string GetSequenceName(int targetSeq)
    {
        return SequenceConatiner._sequenceList[targetSeq].Name;
    }
    public string GetStepName(int targetSeq, int targetStep)
    {
        return SequenceConatiner._sequenceList[targetSeq].scoreItems[targetStep].Name;
    }
    public int GetParentStepIndex(int targetSeq, int targetStep)
    {
        return SequenceConatiner._sequenceList[targetSeq].scoreItems[targetStep].ParentIndex;
    }
    public bool IsTargetStepComplete(int targetSeq, int targetStep)
    {
        if (SequenceConatiner._sequenceList[targetSeq].scoreItems[targetStep].IsFinished)
            return true;

        return false;
    }
    public bool IsTargetStepParentComplete(int targetSeq, int targetStep)
    {
        int index = SequenceConatiner._sequenceList[targetSeq].scoreItems[targetStep].ParentIndex;

        if (index.Equals(-1))
            return true;

        if (SequenceConatiner._sequenceList[targetSeq].scoreItems[index].IsFinished)
            return true;

        return false;
    }
    public bool IsPostSequenceComplete(int targetSeq)
    {
        int[] postSequences = SequenceConatiner._sequenceList[targetSeq].postProcessSequences;

        for (int i = 0; i < postSequences.Length; i++)
        {
            if (!SequenceConatiner._sequenceList[postSequences[i]].IsComplete)
                return false;
        }

        return true;
    }
    public void CompleteStep(int targetSeq, int targetStep)
    {
        bool result = SequenceConatiner._sequenceList[targetSeq].StepComplete(targetStep);

        if (!result)
            return;

        if (!isSkip)
        {
            // 점수창 표시
            ScoreMsgController.GainScoreMsgPopup(SequenceConatiner._sequenceList[targetSeq].scoreItems[targetStep].Name, SequenceConatiner._sequenceList[targetSeq].scoreItems[targetStep].Score);

            // 체크리스트 업데이트
            Checklist.evaluationChecklist[targetSeq][targetStep + 1].SetBackgroundColor(Checklist.CompleteColor);

            if (SequenceConatiner._sequenceList[targetSeq].IsAllComplete())
                Checklist.evaluationChecklist[targetSeq][0].SetBackgroundColor(Checklist.CompleteColor);


            //IMRLAB 09-28
            //행동 문장 전송
            Debug.Log("Test: " + Checklist.evaluationChecklist[targetSeq][targetStep+1].Text.text);
            Debug.Log("Test: " + SequenceConatiner._sequenceList[targetSeq].Name);
            Debug.Log("targetSeq: " + targetSeq + "targetStep: " + targetStep);


            XAPIApplication.current.nowLessonManager.SetEvaluationItemElement(
                SequenceConatiner._sequenceList[targetSeq].Name,
                Checklist.evaluationChecklist[targetSeq][targetStep+1].Text.text, true);
           
            //XAPIApplication.S.jettingLessonManager.AddResultStatement(SequenceConatiner._sequenceList[targetSeq].Name);
            XAPIApplication.current.SendIMRXAPIStatement("Choice");

            //xAPISender.instance.SendMessageWithTranslate(targetSeq,targetStep);
        }
        else
        {
            // 점수창 표시
            ScoreMsgController.GainScoreMsgPopup(SequenceConatiner._sequenceList[targetSeq].scoreItems[targetStep].Name, 0, true);
            //XAPIApplication.S.jettingLessonManager.AddResultStatement(SequenceConatiner._sequenceList[targetSeq].Name);
            // 체크리스트 업데이트
            Checklist.evaluationChecklist[targetSeq][targetStep + 1].SetBackgroundColor(Checklist.SkipColor);
            XAPIApplication.current.nowLessonManager.SetEvaluationItemElement(
                SequenceConatiner._sequenceList[targetSeq].Name,
                Checklist.evaluationChecklist[targetSeq][targetStep + 1].Text.text, false);

            XAPIApplication.current.SendIMRXAPIStatement("Choice");

            if (SequenceConatiner._sequenceList[targetSeq].IsAllComplete())
                Checklist.evaluationChecklist[targetSeq][0].SetBackgroundColor(Checklist.SkipColor);
        }

        if (SequenceConatiner.IsAllComplete()) // 모든 단계를 완료했을 시
        {
            EvaluationEnd();
            XAPIApplication.current.SetTimeLimitSucces(true);
            XAPIApplication.current.SendIMRXAPIStatement("Time");
            return;
        }

        //IMRLAB - 수정 예정
        if (SequenceConatiner._sequenceList[targetSeq].scoreItems.Count - 1 <= targetStep)
        {
            if (targetSeq + 1 >= SequenceConatiner._sequenceList.Count)
                return;

            GuideUIController.SetMessage(SequenceConatiner._sequenceList[targetSeq + 1].scoreItems[0].Tip);

            XAPIApplication.current.nowLessonManager.SetHintStatementResultExtensions(SequenceConatiner._sequenceList[targetSeq + 1].scoreItems[0].Tip);

        }
        else
        {
            GuideUIController.SetMessage(SequenceConatiner._sequenceList[targetSeq].scoreItems[targetStep + 1].Tip);

            XAPIApplication.current.nowLessonManager.SetHintStatementResultExtensions(SequenceConatiner._sequenceList[targetSeq].scoreItems[targetStep + 1].Tip);
        }
    }

    public void SetGuideText()
    {
        for (int i = 0; i < SequenceConatiner._sequenceList.Count; i++) { 
            if(!SequenceConatiner._sequenceList[i].IsComplete)
            {
                if(!SequenceConatiner._sequenceList[i].scoreItems[0].IsFinished)
                    GuideUIController.SetMessage(SequenceConatiner._sequenceList[i].scoreItems[0].Tip);
                else
                    GuideUIController.SetMessage(SequenceConatiner._sequenceList[i].scoreItems[1].Tip);

                break;
            }
        }
    }

    // -------------------------------------------------------------------------------------
    // Equip
    // -------------------------------------------------------------------------------------
    public bool IsAllEquipSafetyEquipment()
    {
        foreach (EvaluationEquipableItem item in EquipableItems)
        {
            if (!item.IsEquip)
                return false;
        }

        return true;
    }
    public bool IsEquip(string name)
    {
        foreach (EvaluationEquipableItem item in EquipableItems)
        {
            if (item.ObjectName.Equals(name))
            {
                if (item.IsEquip)
                    return true;
            }
        }

        return false;
    }

    // -------------------------------------------------------------------------------------
    // Rating
    // -------------------------------------------------------------------------------------
    public void RatingPanelOpen()
    {


        RatingPanel.MakeScoreBoard(SequenceConatiner);
        RatingPanel.OpenRatingPanel();
    }
    public void RatingPanelClose()
    {
        RatingPanel.CloseRatingPanel();
    }

    // -------------------------------------------------------------------------------------
    // Camera
    // -------------------------------------------------------------------------------------
    public void CameraControllOn()
    {
        cameraController.CameraControllOn();
    }
    public void CameraControllOff()
    {
        cameraController.CameraControllOff();
    }

    // -------------------------------------------------------------------------------------
    // Timer
    // -------------------------------------------------------------------------------------
    public void TimerStart(int seconds)
    {
        TimerStop();

        _timerCoroutine = Timer_co(EvaluationTime);
        StartCoroutine(_timerCoroutine);

        Timer.FadeIn(0.3f);
    }
    public void TimerStop()
    {
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);
    }

    private IEnumerator DelayedStart(float time)
    {
        startSeq = false;
        yield return new WaitForSeconds(time);
        AssignmentPopup.Show(1f); // 과제 설명 팝업

        //IMRLAB - 10/14
        //종료 문장 result list init
        //XAPIApplication.S.jettingLessonManager.InitResultStatement(SequenceConatiner);
    }

    private IEnumerator Timer_co(int seconds)
    {
        TimeSpan times;
        int timeRemaining = seconds;

        while (timeRemaining > 0)
        {
            timeRemaining--;

            times = TimeSpan.FromSeconds(timeRemaining);

            if (times.Minutes <= 14 && times.Minutes > 9)
            {
                Timer.SetTimerText(string.Format("남은시간 <color='#ff7e10'>{0:00}:{1:00}</color>", times.Minutes, times.Seconds));
            }
            else if (times.Minutes <= 9)
            {
                Timer.SetTimerText(string.Format("남은시간 <color='#cd2b24'>{0:00}:{1:00}</color>", times.Minutes, times.Seconds));
            }
            else
            {
                Timer.SetTimerText(string.Format("남은시간 <color='#8AD28D'>{0:00}:{1:00}</color>", times.Minutes, times.Seconds));
            }

            yield return _waitTime;
        }

        if (isEvaluationStart)
        {
            XAPIApplication.current.SetTimeLimitSucces(false);
            XAPIApplication.current.SendIMRXAPIStatement("Time");
            EvaluationEnd();
        }
    }

    // -------------------------------------------------------------------------------------
    // Utilities
    // -------------------------------------------------------------------------------------
    
    public void ReturnHorm()
    {
        CL_CommonFunctionManager.Instance.ReturnHome();
    }

    // 
    public IEnumerator MoveToTarget(Transform next)
    {
        float startTime = Time.time;
        float normalizsedTime = 0f;

        Vector3 startPos = cam_obj.transform.position;
        Quaternion startRot = cam.transform.rotation;

        float sinVal = 0f;
        float halfPI = Mathf.PI * 0.6f;

        float time = 1f;

        cameraController.CameraControllOff();

        while (normalizsedTime < 0.8f)
        {
            normalizsedTime = (Time.time - startTime) / time;

            sinVal = Mathf.Sin(normalizsedTime * halfPI);

            cam_obj.position = Vector3.Lerp(startPos, next.position, sinVal);
            cam.rotation = Quaternion.Lerp(startRot, next.rotation, sinVal);

            yield return null;
        }

        cam_obj.position = next.position;
        cam.rotation = next.rotation;

        cameraController.SetCurrentPosition();
        cameraController.CameraControllOn();
    }

    // Skip
    public void SkipSequence()
    {
        if (!isEvaluationStart)
            return;

        isSkip = true;

        EvaluationInteractiveObject[] AllObjs = GameObject.FindObjectsOfType<EvaluationInteractiveObject>();
        EvaluationInteractiveUI[] AllUI = GameObject.FindObjectsOfType<EvaluationInteractiveUI>();

        int currentSequenceNum = 0;

        for(int i=0; i<Sequences.Count; i++)
        {
            if(!Sequences[i].IsComplete)
            {
                currentSequenceNum = i;
                break;
            }
        }

        Sequences[currentSequenceNum].IsSkip = true;

        // CH2전용
        if(CH == 2 && currentSequenceNum == 4)
        {
            CompleteStep(4, 0);
        }

        // CH3전용
        if(CH == 3)
        {
            switch (currentSequenceNum)
            {
                case 3:
                case 4:
                case 5:
                case 7:
                    CH3Method(currentSequenceNum);
                    return;
            }
        }

        if (currentSequenceNum == slicerSeq)
        {
            EvaluationSlicerController.instance.SlicerSkip();

            // CompleteStep(currentSequenceNum, 0);
            // CompleteStep(currentSequenceNum, 1);

            return;
        }

        if (currentSequenceNum == printSeq)
        {
            EvaluationSlicerController.instance.PrintingSkip();
            // CompleteStep(currentSequenceNum, 0);

            return;
        }

        // 3D Obj
        for (int i=0; i<AllObjs.Length; i++)
        {
            if(AllObjs[i].TargetSequence == currentSequenceNum)
            {
                if(!AllObjs[i].IsInteractive)
                {
                    AllObjs[i].IsInteractive = true;
                }
                AllObjs[i].OnMouseDown();
            }
            // Debug.Log(AllObjs[i].gameObject.name + " : " + AllObjs[i].TargetSequence);
        }
        // UI
        for (int i=0; i<AllUI.Length; i++)
        {
            if(AllUI[i].TargetSequence == currentSequenceNum)
            {
                if (!AllUI[i].IsInteractive)
                {
                    AllUI[i].IsInteractive = true;
                }
                AllUI[i].OnPointerClick(null);
                AllUI[i].CompleteStep();
            }
            // Debug.Log(AllUI[i].gameObject.name + " : " + AllUI[i].TargetSequence);
        }

        //CompleteSequence(currentSequenceNum);
        isSkip = false;
    }

    public void CH3Method(int seq)
    {
        if(seq == 3)
        {
            EvaluationCH3Controller.Instance.Seq3Skip();
        }

        if(seq == 4)
        {
            EvaluationCH3Controller.Instance.Seq4Skip();
        }

        if(seq == 5)
        {
            EvaluationCH3Controller.Instance.Seq5Skip();
        }

        if(seq == 7)
        {
            EvaluationCH3Controller.Instance.Seq7Skip();
        }
    }
}