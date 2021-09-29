/*
 * Sequence-Based Framework
 * 스크립트 버튼 활성화 기능 필요
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HighlightingSystem;
using SBFrameworkCore;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PracticeSceneController : MonoBehaviour
{
    private static PracticeSceneController instance = null;

    public enum ProgressBarStatus
    {
        Inactive,
        Process,
        Complete
    }

    private TBProgressBarGenerator tbGen = null;

    // 코루틴
    private IEnumerator typingCoroutine = null;
    private IEnumerator clickCoroutine = null;
    private IEnumerator cameraCoroutine = null;
    private IEnumerator delayCoroutine = null;

    // 카메라 위치를 저장하기 위한 변수
    private Vector3 tempPosition = Vector3.zero;
    private Vector3 tempRotation = Vector3.zero;

    [Header("Settings")]
    public string NarrationPath;
    public Camera mainCamera;
    public Text titleUI;
    public string TitleText;
    public BehaviorGuideUIController GuideUI;
    public CL_ScriptBoxController scriptBox;
    public TitleIndicatorController TitleIndicatorUI;

    [Space]
    public NextStepButtonController NextStepButton;
    public Button PrevStepButttn;

    [Header("Start Sequence")]
    public int TotalSequenceCount = 0;
    public int startSequence = 1;
    private bool IsInitialized = false;

    // State
    public SBFrameworkSequence[] Sequences;
    private Dictionary<int, int>[] scriptArr;
    private string[] sequenceNames;
    private ProgressBarStatus[] sequenceState;

    // 캐싱 관련
    private Dictionary<string, Transform> objectList;
    private Dictionary<string, Highlighter> highlighterList;
    private Dictionary<string, Animator> animatorList;

    // 진행 관련
    private int lastScriptIdx = 0;
    private int curSeqNum = 0;
    private int curJobNum = 0;
    private string language = null;
    
    private bool isMoving = false;
    private bool isBlocking = false;

    public bool startSeq = true;

    public static PracticeSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        Initiallize();
    }

    private void Initiallize()
    {
        if (instance == null)
            instance = this;

        int i = 0;
        Sequences = new SBFrameworkSequence[TotalSequenceCount + 1];

        // 스크립트 읽어오기
        for (i = 1; i <= TotalSequenceCount; i++)
        {
            Sequences[i] = new SBFrameworkSequence();
            Sequences[i].LoadSequenceByScript(i);
        }

        // 처음 시퀀스의 첫 Job에만 시작 위치 넣어주기(이후 자동 계산)
        Sequences[1].Jobs[0].CameraPosition = new Vector3(-1.5f, 1.5f, -0.8f);
        Sequences[1].Jobs[0].CameraRotation = Vector3.zero;

        // 캐싱용
        objectList = new Dictionary<string, Transform>();
        highlighterList = new Dictionary<string, Highlighter>();
        animatorList = new Dictionary<string, Animator>();

        sequenceNames = new string[TotalSequenceCount + 1];
        sequenceState = new ProgressBarStatus[TotalSequenceCount + 1];
        scriptArr = new Dictionary<int, int>[TotalSequenceCount + 1]; // scriptArr[1][0] = 1번 Sequence의 0번 Job이 시작될 때 스크립트 창에 떠있는 스크립트 번호

        // 프로그램 언어 KR로 고정(언어 설정, 현재 사용하지 않음)
        language = "KR";

        // 나레이션 파일 불러오기
        CL_CommonFunctionManager.Instance.LoadNarrationFile(NarrationPath);
        
        // 이하 작업 진행
        SBJob tempJob = null;
        Sequences[1].GetJob(0, out tempJob);

        Vector3 pos = tempJob.CameraPosition;
        Vector3 rotation = tempJob.CameraRotation;

        int j;

        // 인덱스는 1부터
        // 카메라 이동이 반영된 시퀀스별 초기 위치를 미리 저장해 놓고, 오브젝트를 캐싱해 놓는 작업
        for (i = 1; i <= TotalSequenceCount; i++)
        {
            sequenceNames[i] = Sequences[i].GetSequenceName();
            sequenceState[i] = ProgressBarStatus.Inactive;
            scriptArr[i] = new Dictionary<int, int>();

            lastScriptIdx = 0;

            for (j = 0; j < Sequences[i].TotalJobCount(); j++)
            {
                // 카메라 셋업
                Sequences[i].Jobs[j].CameraPosition = pos;
                Sequences[i].Jobs[j].CameraRotation = rotation;

                Transform gameObject = null;

                string name = Sequences[i].Jobs[j].ObjectName;

                if (name != null)
                {
                    if (objectList.ContainsKey(name))
                    {
                        gameObject = objectList[name];
                    }
                    else
                    {
                        if (name.Equals(this.name))
                        {
                            gameObject = this.transform;
                        }
                        else
                        {
                            gameObject = GameObject.Find(name).transform;
                        }
                    }
                }

                switch (Sequences[i].Jobs[j].Job)
                {
                    case SBStepJobType.CallMethod:
                        if (!objectList.ContainsKey(name))
                            objectList.Add(name, gameObject);

                        break;

                    case SBStepJobType.HighlightOn:
                        if (!highlighterList.ContainsKey(name))
                            highlighterList.Add(name, gameObject.GetComponent<Highlighter>());

                        break;

                    case SBStepJobType.AnimationPlay:
                    case SBStepJobType.AnimationTrigger:
                        if (!animatorList.ContainsKey(name))
                            animatorList.Add(name, gameObject.GetComponent<Animator>());

                        break;

                    case SBStepJobType.WaitForClickObject:
                        if (!objectList.ContainsKey(name))
                            objectList.Add(name, gameObject);

                        if (!highlighterList.ContainsKey(name))
                            highlighterList.Add(name, objectList[name].GetComponent<Highlighter>());

                        break;

                    case SBStepJobType.Typing:
                        lastScriptIdx = Sequences[i].Jobs[j].ScriptIndex;
                        break;

                    case SBStepJobType.CameraTransform:
                        pos = Sequences[i].Jobs[j].TargetPosition;
                        rotation = Sequences[i].Jobs[j].TargetRotation;
                        break;
                }

                scriptArr[i].Add(j, lastScriptIdx);
            }
        }

        // 탑바 프로그래스 바 생성
        tbGen = FindObjectOfType(typeof(TBProgressBarGenerator)) as TBProgressBarGenerator;
        tbGen.SetSequenceCount(TotalSequenceCount);
        tbGen.GenerateProgressBar(TotalSequenceCount, sequenceNames);

        // 탑바에 시퀀스 이동 EventTrigger 추가
        for (i = 0; i < TotalSequenceCount; i++)
        {
            int tempIndex = i + 1;
            EventTrigger.Entry entryClick = new EventTrigger.Entry();
            entryClick.eventID = EventTriggerType.PointerDown;
            entryClick.callback.AddListener((eventData) => SequenceChangeForBtn(tempIndex));

            tbGen.progressSteps[i].GetComponent<EventTrigger>().triggers.Add(entryClick);
        }

        // 스크립트창 열기
        scriptBox.ScriptBoxActive();

        // 퀵메뉴 사용
        // CL_CommonFunctionManager.Instance.QuickMenuActive(true);

        if (CL_CommonFunctionManager.Instance.UseTutorial()) // 튜토리얼 사용
        {
            CL_CommonFunctionManager.Instance.TutorialPanelOpen(8);
        }
        else // 미사용
        {
            StartInit();
        }

        IsInitialized = true;
    }

    private void OnDestroy()
    {
        tbGen = null;
        highlighterList = null;
        objectList = null;
        sequenceState = null;
        scriptArr = null;
        instance = null;
        Sequences = null;

        Resources.UnloadUnusedAssets();
    }

    public void StartInit()
    {
        startSeq = false;

        curSeqNum = startSequence;
        curJobNum = 0;

        if (!IsInitialized)
            Initiallize();
        
        // startSequence부터 시작
        if (LoadSceneParams.sequenceNumber.Equals(-1))
        {
            ChangeSteps(startSequence, 0);
        }
        else
        {
            // '다시 학습하기' 버튼으로 진입했을 경우
            ChangeSteps(LoadSceneParams.sequenceNumber, 0);
        }
    }

    // ------------------------------------------------------------------------------------------------------------------------------
    // UI 관련
    // ------------------------------------------------------------------------------------------------------------------------------
    public void RefreshScriptCounter()
    {
        scriptBox.SetCounter(scriptArr[curSeqNum][curJobNum] + 1, Sequences[curSeqNum].Scripts[language].Count);
    }

    // ------------------------------------------------------------------------------------------------------------------------------
    // 상태머신 관련
    // ------------------------------------------------------------------------------------------------------------------------------
    private void Run()
    {
        switch (Sequences[curSeqNum].Jobs[curJobNum].Job)
        {
            case SBStepJobType.UndoPoint:
            case SBStepJobType.End:
                if (!isBlocking)
                    NextStepButton.OnNext();
                break;

            case SBStepJobType.WaitForClickObject:
                if (clickCoroutine != null)
                    StopCoroutine(clickCoroutine);

                Highlight(Sequences[curSeqNum].Jobs[curJobNum].ObjectName, true);

                clickCoroutine = WaitForClick_Co(Sequences[curSeqNum].Jobs[curJobNum].ObjectName);
                StartCoroutine(clickCoroutine);
                break;

            case SBStepJobType.Delay:
                if (delayCoroutine != null)
                    StopCoroutine(delayCoroutine);

                delayCoroutine = Wait_Co(Sequences[curSeqNum].Jobs[curJobNum].DelayTime);
                StartCoroutine(delayCoroutine);
                break;

            case SBStepJobType.Typing:
                TypingStart(Sequences[curSeqNum].Scripts[language][scriptArr[curSeqNum][curJobNum]], CL_CommonFunctionManager.Instance.TypingSpeed());
                break;

            case SBStepJobType.CameraTransform:
                CameraMoveStart(Sequences[curSeqNum].Jobs[curJobNum].TargetPosition,
                                  Sequences[curSeqNum].Jobs[curJobNum].TargetRotation,
                                  Sequences[curSeqNum].Jobs[curJobNum].CameraMoveSpeed,
                                  Sequences[curSeqNum].Jobs[curJobNum].CameraRotateSpeed);
                break;
                
            case SBStepJobType.AnimationPlay:
                animatorList[Sequences[curSeqNum].Jobs[curJobNum].ObjectName].SetFloat("speed", 1.0f);
                animatorList[Sequences[curSeqNum].Jobs[curJobNum].ObjectName].Play(Sequences[curSeqNum].Jobs[curJobNum].MethodName, -1, 0f);
                goto default;

            case SBStepJobType.CallMethod:
                objectList[Sequences[curSeqNum].Jobs[curJobNum].ObjectName].SendMessage(Sequences[curSeqNum].Jobs[curJobNum].MethodName);
                goto default;

            case SBStepJobType.HighlightOn:
                Highlight(Sequences[curSeqNum].Jobs[curJobNum].ObjectName, true);
                goto default;

            case SBStepJobType.HighlightOff:
                Highlight(Sequences[curSeqNum].Jobs[curJobNum].ObjectName, false);
                goto default;

            case SBStepJobType.BlockingNext:
                NextStepButton.Init();
                NextStepButton.Blocking();
                isBlocking = true;
                goto default;

            case SBStepJobType.BlockingRelease:
                NextStepButton.Init();
                NextStepButton.BlockingRelease();
                isBlocking = false;
                goto default;

            default:
                ChangeSteps(curSeqNum, curJobNum + 1);
                break;
        }
    }

    // ------------------------------------------------------------------------------------------------------------------------------
    // 카메라 제어
    // ------------------------------------------------------------------------------------------------------------------------------
    public void CameraSet()
    {
        mainCamera.transform.position = Sequences[curSeqNum].Jobs[curJobNum].CameraPosition;
        mainCamera.transform.rotation = Quaternion.Euler(Sequences[curSeqNum].Jobs[curJobNum].CameraRotation);
    }

    public bool CameraMove(Vector3 targetPosition, float moveSpeed = 1.0f)
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * moveSpeed);

        if (Vector3.Distance(targetPosition, mainCamera.transform.position) <= 0.001f)
            return true;

        return false;
    }
    public bool CameraRotate(Vector3 TargetRotation, float rotateSpeed = 1.0f)
    {
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, Quaternion.Euler(TargetRotation), Time.deltaTime * rotateSpeed);

        if (Quaternion.Angle(Quaternion.Euler(TargetRotation), mainCamera.transform.rotation) <= 0.01f)
            return true;

        return false;
    }

    // ------------------------------------------------------------------------------------------------------------------------------
    // 하이라이트
    // ------------------------------------------------------------------------------------------------------------------------------
    public void Highlight(string name, bool isOn)
    {
        if (!highlighterList.ContainsKey(name))
            return;

        if (isOn)
        {
            highlighterList[name].TweenStart();
        }
        else
        {
            highlighterList[name].Off();
        }
    }

    // ------------------------------------------------------------------------------------------------------------------------------
    // 시퀀스 및 스텝 제어
    // ------------------------------------------------------------------------------------------------------------------------------    
    public void ChangeSteps(int sequence, int jobIdx)
    {
        if (sequence < 1 || sequence > TotalSequenceCount)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("더이상 이동할 수 없습니다.", CL_MessagePopUpController.DialogType.NOTICE);
            return;
        }

        if (jobIdx >= Sequences[sequence].TotalJobCount())
        {
            ChangeSteps(sequence + 1, 0);
            return;
        }

        if (jobIdx < 0)
        {
            if (sequence <= 1)
            {
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("더이상 이동할 수 없습니다.", CL_MessagePopUpController.DialogType.NOTICE);
                ChangeSteps(1, 0);
                return;
            }

            ChangeSteps(sequence - 1, Sequences[sequence - 1].TotalJobCount() - 1);
            return;
        }

        CompleteCurrentJob();

        if (curSeqNum < sequence)
        {
            RunDoMethod(curSeqNum, curJobNum, sequence, jobIdx);
        }
        else if (curSeqNum == sequence)
        {
            if (curJobNum < jobIdx)
            {
                RunDoMethod(curSeqNum, curJobNum, sequence, jobIdx);
            }
            else
            {
                RunUndoMethod(curSeqNum, curJobNum, sequence, jobIdx);
            }
        }
        else
        {
            // Undo의 경우
            RunUndoMethod(curSeqNum, curJobNum, sequence, jobIdx);
        }

        int temp = curSeqNum;

        curSeqNum = sequence;
        curJobNum = jobIdx;

        // 현재 작업 멈춤
        StopAllCoroutines();
        CL_CommonFunctionManager.Instance.StopNarration();

        if ((curSeqNum == startSequence && jobIdx == 0) || curSeqNum != temp)
        {
            TitleIndicatorUI.ShowTitleSlideIn(Sequences[curSeqNum].SequenceName[language], TitleText);
            StartCoroutine(DelayedRun(2.5f));
        }
        else
        {
            UpdateUI();
            CameraSet();

            NextStepButton.Init();
            Run();
        }
    }

    public void RunUndoMethod(int startSeq, int startJobIdx, int endSeq, int endJobIdx)
    {
        if (startSeq.Equals(endSeq))
        {
            // 시작 시퀀스와 목표 시퀀스가 같을 때
            UndoJob(startSeq, startJobIdx, endJobIdx);
        }
        else
        {
            UndoJob(startSeq, startJobIdx, 0);

            for (int i = startSeq - 1; i > endSeq; i--)
                UndoJob(i, Sequences[i].TotalJobCount() - 1, 0);

            UndoJob(endSeq, Sequences[endSeq].TotalJobCount() - 1, endJobIdx);
        }
    }
    public void RunDoMethod(int startSeq, int startJobIdx, int endSeq, int endJobIdx)
    {
        if (startSeq.Equals(endSeq))
        {
            DoJob(startSeq, startJobIdx + 1, endJobIdx);
        }
        else
        {
            DoJob(startSeq, startJobIdx + 1, Sequences[startSeq].TotalJobCount());

            for (int i = startSeq + 1; i < endSeq; i++)
                DoJob(i, 0, Sequences[i].TotalJobCount());

            DoJob(endSeq, 0, endJobIdx);
        }
    }

    private void UndoJob(int startSeq, int startIdx, int endIdx)
    {
        for (int i = startIdx; i >= endIdx; i--)
        {
            switch (Sequences[startSeq].Jobs[i].Job)
            {
                case SBStepJobType.CallMethod:
                    objectList[Sequences[startSeq].Jobs[i].ObjectName].SendMessage(Sequences[startSeq].Jobs[i].UndoMethodName);
                    break;
                case SBStepJobType.AnimationPlay:
                    animatorList[Sequences[startSeq].Jobs[i].ObjectName].SetFloat("speed", -10.0f);
                    animatorList[Sequences[startSeq].Jobs[i].ObjectName].Play(Sequences[startSeq].Jobs[i].MethodName, -1, 1f);
                    break;
                case SBStepJobType.BlockingNext:
                    isBlocking = false;
                    NextStepButton.BlockingRelease();
                    break;
                case SBStepJobType.BlockingRelease:
                    isBlocking = true;
                    NextStepButton.Blocking();
                    break;
            }
        }
    }

    private void DoJob(int startSeq, int startIdx, int endIdx)
    {
        for (int i = startIdx; i < endIdx; i++)
        {
            switch (Sequences[startSeq].Jobs[i].Job)
            {
                case SBStepJobType.BlockingNext:
                    isBlocking = true;
                    NextStepButton.Blocking();
                    break;
                case SBStepJobType.BlockingRelease:
                    isBlocking = false;
                    NextStepButton.BlockingRelease();
                    break;
                case SBStepJobType.CallMethod:
                    objectList[Sequences[startSeq].Jobs[i].ObjectName].SendMessage(Sequences[startSeq].Jobs[i].MethodName);
                    break;
                case SBStepJobType.AnimationPlay:
                    animatorList[Sequences[startSeq].Jobs[i].ObjectName].SetFloat("speed", 10.0f);
                    animatorList[Sequences[startSeq].Jobs[i].ObjectName].Play(Sequences[startSeq].Jobs[i].MethodName, -1, 0f);
                    break;
            }
        }
    }

    public void CompleteCurrentJob()
    {
        switch (Sequences[curSeqNum].Jobs[curJobNum].Job)
        {
            case SBStepJobType.BlockingRelease:
                isBlocking = false;
                NextStepButton.BlockingRelease();
                break;
            case SBStepJobType.BlockingNext:
                isBlocking = true;
                NextStepButton.Blocking();
                break;
            case SBStepJobType.WaitForClickObject:
            case SBStepJobType.HighlightOff:
                Highlight(Sequences[curSeqNum].Jobs[curJobNum].ObjectName, false);
                break;
            case SBStepJobType.Typing:
                scriptBox.SetText(Sequences[curSeqNum].Scripts[language][scriptArr[curSeqNum][curJobNum]]);
                break;

            default:
                break;
        }
    }

    public void NextStep()
    {
        StopAllCoroutines();
        ChangeSteps(curSeqNum, curJobNum + 1);
    }

    public void NextStepForBtn()
    {
        if (isBlocking)
            return;

        int i, j;

        StopAllCoroutines();
        CompleteCurrentJob();

        for (i = curJobNum + 1; i < Sequences[curSeqNum].TotalJobCount(); i++)
        {
            switch (Sequences[curSeqNum].Jobs[i].Job)
            {
                case SBStepJobType.End:
                case SBStepJobType.CameraTransform:
                case SBStepJobType.WaitForClickObject:
                case SBStepJobType.BlockingNext:
                case SBStepJobType.BlockingRelease:
                case SBStepJobType.AnimationPlay:
                case SBStepJobType.Typing:
                    ChangeSteps(curSeqNum, i);
                    return;
            }
        }

        for (i = curSeqNum + 1; i <= TotalSequenceCount; i--)
        {
            for (j = 0; j < Sequences[i].TotalJobCount(); j++)
            {
                switch (Sequences[i].Jobs[j].Job)
                {
                    case SBStepJobType.End:
                    case SBStepJobType.CameraTransform:
                    case SBStepJobType.WaitForClickObject:
                    case SBStepJobType.BlockingNext:
                    case SBStepJobType.BlockingRelease:
                    case SBStepJobType.AnimationPlay:
                    case SBStepJobType.Typing:
                        ChangeSteps(i, j);
                        return;
                }
            }
        }

#if UNITY_WEBGL_EACH
        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("학습이 종료되었습니다.\n다시 학습하시겠습니까?",
            CL_MessagePopUpController.DialogType.NOTICE,
            () => LoadPractice(),
            null);
#else
        //CL_CommonFunctionManager.Instance.MakePopUp().PopUp("실습이 완료되었습니다.", CL_MessagePopUpController.DialogType.NOTICE);
        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("실습 학습이 마무리되었습니다.\n실습 평가로 이동하시겠습니까?", 
            CL_MessagePopUpController.DialogType.NOTICE, 
            () => LoadEvaluation(), 
            null);
#endif


        return;
    }

    public void LoadPractice()
    {
        CL_CommonFunctionManager.Instance.LoadScene("TrainingCH3");
    }

    public void LoadEvaluation()
    {
        CL_CommonFunctionManager.Instance.LoadScene("EvaluationCH3");
    }

    public void PrevStepForBtn()
    {
        int i, j;

        for (i = curJobNum - 1; i >= 0; i--)
        {
            switch (Sequences[curSeqNum].Jobs[i].Job)
            {
                case SBStepJobType.Typing:
                    if (Sequences[curSeqNum].Jobs[i].IsAutoNext)
                        continue;

                    StopAllCoroutines();
                    CL_CommonFunctionManager.Instance.StopNarration();

                    ChangeSteps(curSeqNum, i);
                    return;

                case SBStepJobType.UndoPoint:
                case SBStepJobType.UndoPointAutoNext:
                case SBStepJobType.WaitForClickObject:
                    StopAllCoroutines();
                    CL_CommonFunctionManager.Instance.StopNarration();

                    ChangeSteps(curSeqNum, i);
                    scriptBox.SetText(Sequences[curSeqNum].Scripts[language][scriptArr[curSeqNum][i]]);
                    return;
            }
        }

        for (i = curSeqNum - 1; i >= 1; i--)
        {
            for (j = Sequences[i].TotalJobCount() - 1; j >= 0; j--)
            {
                switch (Sequences[i].Jobs[j].Job)
                {
                    case SBStepJobType.Typing:
                        StopAllCoroutines();
                        CL_CommonFunctionManager.Instance.StopNarration();

                        ChangeSteps(i, j);
                        return;

                    case SBStepJobType.UndoPoint:
                    case SBStepJobType.UndoPointAutoNext:
                    case SBStepJobType.WaitForClickObject:
                        StopAllCoroutines();
                        CL_CommonFunctionManager.Instance.StopNarration();

                        ChangeSteps(i, j);
                        scriptBox.SetText(Sequences[i].Scripts[language][scriptArr[i][j]]);
                        return;
                }
            }
        }

        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("더 이상 이동할 수 없습니다.", CL_MessagePopUpController.DialogType.NOTICE);
        return;
    }

    public void PrevSequenceForBtn()
    {
        if (curSeqNum <= 1)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("더 이상 이동할 수 없습니다.", CL_MessagePopUpController.DialogType.NOTICE);
            return;
        }

        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("<color='#436d88'>[" + Sequences[curSeqNum - 1].SequenceName[language] + "]</color> 단계로 이동하시겠습니까?\n현재 진행중인 단계가 자동 진행/초기화 됩니다.",
                                                                   CL_MessagePopUpController.DialogType.NOTICE,
                                                                   () =>
                                                                   {
                                                                       ChangeSteps(curSeqNum - 1, 0);
                                                                   },
                                                                   null);
    }

    public void NextSequenceForBtn()
    {
        if (curSeqNum >= TotalSequenceCount)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("더 이상 이동할 수 없습니다.", CL_MessagePopUpController.DialogType.NOTICE);
            return;
        }

        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("<color='#436d88'>[" + Sequences[curSeqNum + 1].SequenceName[language] + "]</color> 단계로 이동하시겠습니까?\n현재 진행중인 단계가 자동 진행/초기화 됩니다.",
                                                                       CL_MessagePopUpController.DialogType.NOTICE,
                                                                       () =>
                                                                       {
                                                                           ChangeSteps(curSeqNum + 1, 0);
                                                                       },
                                                                       null);
    }

    public void SequenceChangeForBtn(int sequence)
    {
        if (sequence > TotalSequenceCount || sequence < 1)
            return;

        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("<color='#436d88'>[" + Sequences[sequence].SequenceName[language] + "]</color> 단계로 이동하시겠습니까?\n현재 진행중인 단계가 자동 진행/초기화 됩니다.",
                                                                   CL_MessagePopUpController.DialogType.NOTICE,
                                                                   () =>
                                                                   {
                                                                       StopAllCoroutines();
                                                                       ChangeSteps(sequence, 0);
                                                                   },
                                                                   null);
    }


    public void TypingStart(string text, float typingSpeed = 0.03f)
    {
        TypingStop();

        typingCoroutine = RichTextTyping_Co(text, scriptBox.textBox, typingSpeed);

        CL_CommonFunctionManager.Instance.PlayNarration(GetNarrationIndex(curSeqNum, scriptArr[curSeqNum][curJobNum]) - 1);
        StartCoroutine(typingCoroutine);
    }
    public void TypingStop()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            CL_CommonFunctionManager.Instance.StopNarration();
        }
    }
    public void TypingStopButPlayNarration()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
    }

    public void CameraMoveStart(Vector3 targetPosition, Vector3 TargetRotation, float moveSpeed = 1.0f, float rotateSpeed = 1.0f)
    {
        CameraMoveStop();

        cameraCoroutine = CameraTransform_Co(Sequences[curSeqNum].Jobs[curJobNum].TargetPosition,
                                             Sequences[curSeqNum].Jobs[curJobNum].TargetRotation,
                                             Sequences[curSeqNum].Jobs[curJobNum].CameraMoveSpeed,
                                             Sequences[curSeqNum].Jobs[curJobNum].CameraRotateSpeed);

        StartCoroutine(cameraCoroutine);
    }
    public void CameraMoveStop()
    {
        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);

        isMoving = false;
    }
    // ------------------------------------------------------------------------------------------------------------------------------
    // 코루틴 (Coroutines)
    // ------------------------------------------------------------------------------------------------------------------------------
    IEnumerator CameraTransform_Co(Vector3 targetPosition, Vector3 TargetRotation, float moveSpeed = 1.0f, float rotateSpeed = 1.0f)
    {
        int tempSeq = curSeqNum;
        int tempJob = curJobNum;

        isMoving = true;

        while (isMoving)
        {
            yield return new WaitForFixedUpdate();
            bool bMoveComplete = CameraMove(targetPosition, moveSpeed);
            bool bRotateComplete = CameraRotate(TargetRotation, rotateSpeed);

            if (bMoveComplete && bRotateComplete)
            {
                isMoving = false;
                break;
            }
        }
        
        if (!isBlocking)
            NextStepButton.OnNext();

        if (tempSeq == curSeqNum && tempJob == curJobNum)
        {
            if (Sequences[tempSeq].Jobs[tempJob].IsAutoNext)
            {
                NextStep();
            }
        }
    }

    IEnumerator WaitForClick_Co(string target)
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                GameObject clickedObject = null;
                
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
                {
                    clickedObject = hit.collider.gameObject;

                    if (clickedObject != null && !IsPointerOverUIObject() && clickedObject.name.Contains(target))
                    {
                        // 하이라이팅 끄기
                        Highlight(target, false);
                        break;
                    }
                }
            }

            yield return null;
        }

        ChangeSteps(curSeqNum, curJobNum + 1);
    }

    IEnumerator Wait_Co(float time)
    {
        yield return new WaitForSeconds(time);

        NextStep();
    }

    IEnumerator DelayedRun(float time)
    {
        yield return new WaitForSeconds(time);
        
        UpdateUI();
        CameraSet();

        NextStepButton.Init();
        Run();
    }

    private IEnumerator RichTextTyping_Co(string text, Text textOut, float typingSpeed = 0.03f)
    {
        int tempSeq = curSeqNum;
        int tempJob = curJobNum;

        string[] richTextSymbols = { "b", "i" };
        string[] richTextCloseSymbols = { "b", "i", "size", "color" };

        //줄바꿈 처리
        text = text.Replace("\\n", "\n");
        string tempStr = text;

        int typingIndex = 0;
        float nowWaitTime = 0f;
        char[] splitTypingTexts = text.ToCharArray();
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        bool bold = false;
        bool italic = false;
        bool size = false;
        bool color = false;
        int fontSize = 0;
        string colorCode = null;

        StringBuilder stringBuilder = new StringBuilder();

        textOut.text = null;

        while (true)
        {
            yield return waitForEndOfFrame;
            nowWaitTime += Time.deltaTime;

            if (typingIndex == splitTypingTexts.Length)
                break;

            if (splitTypingTexts[typingIndex] == ' ')
            {
                stringBuilder.Append(splitTypingTexts[typingIndex]);
                typingIndex++;
                continue;
            }

            if (nowWaitTime >= typingSpeed)
            {
                if (textOut.supportRichText)
                {
                    bool symbolCatched = false;

                    for (int i = 0; i < richTextSymbols.Length; i++)
                    {
                        string symbol = string.Format("<{0}>", richTextSymbols[i]);
                        string closeSymbol = string.Format("</{0}>", richTextCloseSymbols[i]);

                        if (splitTypingTexts[typingIndex] == '<' && typingIndex + (1 + richTextSymbols[i].Length) < text.Length && text.Substring(typingIndex, 2 + richTextSymbols[i].Length).Equals(symbol))
                        {
                            switch (richTextSymbols[i])
                            {
                                case "b":
                                    typingIndex += symbol.Length;
                                    if (typingIndex + closeSymbol.Length + 3 <= text.Length)
                                    {
                                        if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                                        {
                                            bold = true;
                                            symbolCatched = true;
                                        }
                                    }
                                    break;
                                case "i":
                                    typingIndex += symbol.Length;
                                    if (typingIndex + closeSymbol.Length + 3 <= text.Length)
                                    {
                                        if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                                        {
                                            italic = true;
                                            symbolCatched = true;
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    if (splitTypingTexts[typingIndex] == '<' && typingIndex + 14 < text.Length && text.Substring(typingIndex, 8).Equals("<color=#") && splitTypingTexts[typingIndex + 14] == '>')
                    {
                        string closeSymbol = string.Format("</{0}>", "color");
                        string tempColorCode = text.Substring(typingIndex + 8, 6);

                        typingIndex += 15;
                        if (typingIndex + closeSymbol.Length <= text.Length)
                        {
                            if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                            {
                                color = true;
                                colorCode = tempColorCode;
                                symbolCatched = true;
                            }
                        }
                    }

                    if (splitTypingTexts[typingIndex] == '<' && typingIndex + 6 < text.Length && text.Substring(typingIndex, 6).Equals("<size="))
                    {
                        string closeSymbol = string.Format("</{0}>", "size");
                        string sizeSub = text.Substring(typingIndex + 6);
                        int symbolIndex = sizeSub.IndexOf('>');
                        string tempSize = sizeSub.Substring(0, symbolIndex);

                        typingIndex += 7 + tempSize.Length;
                        if (typingIndex + closeSymbol.Length <= text.Length)
                        {
                            if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                            {
                                size = true;
                                fontSize = int.Parse(tempSize);
                                symbolCatched = true;
                            }
                        }
                    }

                    bool closeSymbolCatched = false;

                    for (int i = 0; i < richTextCloseSymbols.Length; i++)
                    {
                        string closeSymbol = string.Format("</{0}>", richTextCloseSymbols[i]);

                        if (splitTypingTexts[typingIndex] == '<' && typingIndex + (1 + richTextCloseSymbols[i].Length) < text.Length && text.Substring(typingIndex, 3 + richTextCloseSymbols[i].Length).Equals(closeSymbol))
                        {
                            switch (richTextCloseSymbols[i])
                            {
                                case "b":
                                    typingIndex += closeSymbol.Length;
                                    bold = false;
                                    closeSymbolCatched = true;
                                    break;
                                case "i":
                                    typingIndex += closeSymbol.Length;
                                    italic = false;
                                    closeSymbolCatched = true;
                                    break;
                                case "size":
                                    typingIndex += closeSymbol.Length;
                                    size = false;
                                    fontSize = 0;
                                    closeSymbolCatched = true;
                                    break;
                                case "color":
                                    typingIndex += closeSymbol.Length;
                                    color = false;
                                    colorCode = null;
                                    closeSymbolCatched = true;
                                    break;
                            }
                        }

                        if (closeSymbolCatched)
                            break;
                    }

                    if (symbolCatched || closeSymbolCatched)
                        continue;

                    if (typingIndex < text.Length)
                    {
                        string convertedRichText = RichTextConvert(splitTypingTexts[typingIndex].ToString(), bold, italic, size, fontSize, color, colorCode);
                        stringBuilder.Append(convertedRichText);
                    }
                }
                else
                {
                    if (typingIndex < text.Length)
                        stringBuilder.Append(splitTypingTexts[typingIndex]);
                }

                textOut.text = stringBuilder.ToString();
                typingIndex++;
                nowWaitTime = 0f;

                // 타이핑 효과음 부분
                //if (typingIndex % 3 == 0)
                //    CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.Typing);
            }
        }

        // 타이핑 완료 시, 나레이션이 재생되었는지 확인
        if (Sequences[curSeqNum].Jobs[curJobNum].IsAutoNext)
        {
            if (tempSeq == curSeqNum && tempJob == curJobNum)
            {
                while (CL_CommonFunctionManager.Instance.IsPlayingNarration())
                {
                    yield return new WaitForFixedUpdate();
                }

                NextStep();
            }
        }

        if (!isBlocking)
            NextStepButton.OnNext();
    }

    private string RichTextConvert(string text, bool bold, bool italic, bool size, int fontSize, bool color, string colorCode)
    {
        StringBuilder stringBuilder = new StringBuilder();

        if (bold)
        {
            stringBuilder.Append("<b>");
            stringBuilder.Append(text);
            stringBuilder.Append("</b>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        if (italic)
        {
            stringBuilder.Append("<i>");
            stringBuilder.Append(text);
            stringBuilder.Append("</i>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        if (color)
        {
            stringBuilder.Append("<color=#");
            stringBuilder.Append(colorCode);
            stringBuilder.Append(">");
            stringBuilder.Append(text);
            stringBuilder.Append("</color>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        if (size)
        {
            stringBuilder.Append("<size=");
            stringBuilder.Append(fontSize);
            stringBuilder.Append(">");
            stringBuilder.Append(text);
            stringBuilder.Append("</size>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        return text;
    }

    // ------------------------------------------------------------------------------------------------------------------------------
    // Audio
    // ------------------------------------------------------------------------------------------------------------------------------
    private int GetNarrationIndex(int sequence, int scriptIdx)
    {
        int count = 1;

        for (int i=1;i<sequence;i++)
        {
            count += Sequences[i].Scripts[language].Count;
        }

        count += scriptIdx;

        return count;
    }

    // ------------------------------------------------------------------------------------------------------------------------------
    // UI
    // ------------------------------------------------------------------------------------------------------------------------------
    public void UpdateUI()
    {
        int i;

        // 타이틀 이탤릭체 적용
        titleUI.text = "<i>" + sequenceNames[curSeqNum] + "</i>";
        scriptBox.SetText(Sequences[curSeqNum].Scripts[language][scriptArr[curSeqNum][curJobNum]]);
        RefreshScriptCounter();

        // 진행상황 변경
        for (i = 1; i < curSeqNum; i++)
        {
            sequenceState[i] = ProgressBarStatus.Complete;
            tbGen.progressSteps[i - 1].GetComponent<Image>().color = CL_UIPresetData.ProgressBarComplete;
        }

        for (i = TotalSequenceCount; i > curSeqNum; i--)
        {
            sequenceState[i] = ProgressBarStatus.Inactive;
            tbGen.progressSteps[i - 1].GetComponent<Image>().color = CL_UIPresetData.ProgressBarInactive;
        }

        sequenceState[curSeqNum] = ProgressBarStatus.Process;
        tbGen.progressSteps[curSeqNum - 1].GetComponent<Image>().color = CL_UIPresetData.ProgressBarProcessing;
    }

    // 오브젝트를 클릭할때 UI 레이어를 함께 클릭했는지 검사하는 함수
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer.Equals(LayerMask.NameToLayer("UI")))
                return true;
        }

        return false;
    }
}