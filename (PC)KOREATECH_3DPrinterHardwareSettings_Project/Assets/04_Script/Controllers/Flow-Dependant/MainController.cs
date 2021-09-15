using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Enums;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    [SerializeField]
    private int currentSeq = 0;
    private Dictionary<Controllers, bool> isFinished;
    public int NumberOfSeq;

    [Header("Controllers")]
    public CSVController scriptController;
    public ObjectController objectController;
    public CameraController cameraController;
    public MaterialController materialController;
    public ConfigurationController configurationController;
    public TutorialController tutorialController;
    public PopupController popupController;
    public SequenceChangeController sequenceChangeController;
    public ColliderController colliderController;
    public ActionController actionController;
    public ExamController examController;
    public AnimationController animationController;
    public AnimationSeqController animationSeqController;
    public OutLineController outLineController;

    // ActionData에서 쓰는 싱글턴
    public static MainController instance;
    public bool isRightClick = false;
    ActionData AD;

    [Header("ScriptButton")]
    public Button NextBtn;

    [Header("TutorialNum")]
    public int tutorialNum = 0; // 1 - 지식, 2 - 구조, 3 - 실습, 4 - 평가
    private bool startSeq = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (LoadSceneParams.sequenceNumber >= 0)
        {
            currentSeq = LoadSceneParams.sequenceNumber;
            LoadSceneParams.sequenceNumber = -1;
        }            
    }
    // 여기까지 싱글턴 관련 코드

    public void Start()
    {
        if(CL_CommonFunctionManager.Instance.UseTutorial()) // 튜토리얼 사용
        {
            CL_CommonFunctionManager.Instance.TutorialPanelOpen(tutorialNum);
        }
        else // 미사용
        {
            StartInit();
        }
    }

    public bool IsStart()
    {
        return startSeq;
    }

    public void StartInit()
    {
        startSeq = false;

        isFinished = new Dictionary<Controllers, bool>();

        // 스크립트창 로드
        //CL_CommonFunctionManager.Instance.ScriptBoxActive(true);

        // 스크립트 컨트롤러 초기화
        if (scriptController != null)
        {
            isFinished.Add(Controllers.Script, false);

            scriptController.StepProgressBar();
            scriptController.ShowTitleIndicatorStart(currentSeq);

            // 구조
            if (StructureTitleController.instance != null)
                StructureTitleController.instance.ShowIndicator();

            StartCoroutine(DelayStartSeq(currentSeq));
        }

        // 컬라이더 컨트롤러 초기화
        if (colliderController != null)
        {
            isFinished.Add(Controllers.Collider, false);
            colliderController.firstSceneInit();
            if (colliderController.sequenceGroupData.AllDataBySeq.Length != 0)
                colliderController.activateCollider(currentSeq, currentSeq);
        }

        // 매터리얼 컨트롤러 초기화
        if (materialController != null)
        {
            isFinished.Add(Controllers.Material, false);
            materialController.firstSceneInit();
            if (materialController.sequenceGroupData.AllDataBySeq.Length != 0)
                materialController.setOnOffBySeq(currentSeq, currentSeq);
        }

        // 아웃라인 컨트롤러 초기화
        if (outLineController != null)
        {
            isFinished.Add(Controllers.OutLine, false);
            outLineController.firstSceneInit();
            if (outLineController.sequenceGroupData.AllDataBySeq.Length != 0)
                outLineController.setOnOffBySeq(currentSeq, currentSeq);
        }

        // 오브젝트 컨트롤러 초기화
        if (objectController != null)
        {
            isFinished.Add(Controllers.Object, false);
            objectController.firstSceneInit();
            if (objectController.sequenceGroupData.AllDataBySeq.Length != 0)
                objectController.setOnOffBySeq(currentSeq, currentSeq);
        }

        // 카메라 컨트롤러 초기화
        if (cameraController != null)
        {
            cameraController.setCameraBySeq(currentSeq, currentSeq);
        }

        // 튜토리얼 컨트롤러 초기화
        if (tutorialController != null)
        {
            isFinished.Add(Controllers.Tutorial, true);
            tutorialController.TutorialBtnON();
        }

        // 팝업 컨트롤러 초기화
        if (popupController != null)
        {
            isFinished.Add(Controllers.Popup, false);
            popupController.firstSceneInit();
        }

        // 시퀀스 이동 컨트롤러 초기화
        if (sequenceChangeController != null)
        {
            sequenceChangeController.firstSceneInit();
        }

        // 액션 컨트롤러 초기화
        if (actionController != null)
        {
            actionController.firstSceneInit();
            if (actionController.sequenceGroupData.AllDataBySeq.Length != 0)
                actionController.setOnOffBySeq(currentSeq, currentSeq);
        }
        if (animationController != null)
        {
            isFinished.Add(Controllers.Animation, true);
        }
    }

    public void Update()
    {
        // 테스트를 위한 코드 주석처리
        /*
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            // 다음 시퀀스 이동
            goNextSeq();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // 이전 시퀀스 이동
            goPrevSeq();
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 클릭 버튼. 컬라이더 입력에 사용
            if (colliderController == null)
               return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            GameObject[] currentObjects = colliderController.sequenceGroupData.AllDataBySeq[currentSeq].GetComponent<ColliderData>().ColliderOnObjects;
            BoxCollider[] boxColliders;
            for(int i = 0; i < currentObjects.Length; i++)
            {
                boxColliders = currentObjects[i].GetComponents<BoxCollider>();
                for (int j = 0; j < boxColliders.Length; j++)
                {
                    if (boxColliders[j].Raycast(ray, out hit, 100f))
                    {
                        materialController.showComponent(currentSeq);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // 임시 키보드 입력. 특정 시퀀스 이동 구현을 위한 참고로 남겨둠
            int nextSeq = 0;
            startByCont(Controllers.Script);
            scriptController.viewScriptBySeq(nextSeq);

            startByCont(Controllers.Object);
            objectController.setOnOffBySeq(currentSeq, nextSeq);

            startByCont(Controllers.Material);
            materialController.setOnOffBySeq(currentSeq, nextSeq);

            cameraController.setCameraOnlyByNext(nextSeq);
            currentSeq = nextSeq;
        }

        if (popupController != null)
        {
            if (Vector3.Distance(cameraController.cam.transform.localPosition, cameraController.sequenceGroupData.AllDataBySeq[currentSeq].transform.localPosition) < 0.1f)
            {
                if (currentSeq == 0)
                    return;
                else
                {
                    //popupController.viewFirstPopup();
                    popupController.viewPopupBySeq(currentSeq);
                }
            }
            else
            {
                if (currentSeq == 0)
                    return;
                else
                {
                    //popupController.viewFirstPopup();
                    popupController.showAndHidePopup();
                }
            }
        }
        */
    }

    public void goNextSeq()
    {
        // ActionData가 존재하면 넘기지 않는다.
        // 어떤 상황에서든 스크립트 다음버튼으로 다음으로 넘어가게 주석처리
        if(actionController.sequenceGroupData.AllDataBySeq[currentSeq].GetComponent<ActionData>().ObjName.Length !=0 &&
            !isRightClick)
        {
            int nextSeq = currentSeq;

            // 스크립트의 경우, 작업 처리가 되지 않았을 수 있다.
            if (!isFinished[Controllers.Script])
                scriptController.skipEffectBySeq(nextSeq);

            // 카메라의 경우, 시퀀스에 영향을 주지 않지만 시퀀스에 영향은 받는다.
            if (cameraController.getIsCamMove())
                cameraController.skipCamMoveBySeq(nextSeq);

            return;
        }

        // 마지막 시퀀스라면 리턴
        if (isAllFinished() && currentSeq == NumberOfSeq - 1)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            string ch = "";
            string message = "";
            string nextSceneName = "";

#if UNITY_WEBGL_EACH
            message = "학습이 종료되었습니다.\n다시 학습하시겠습니까?";
            nextSceneName = currentSceneName;
#else
            if (currentSceneName.Contains("Structure"))
            {
                // 구조
                ch = currentSceneName.Replace("StructureCH", "");
                message = "구조 학습이 마무리되었습니다.\n실습으로 이동하시겠습니까?";
                nextSceneName = "TrainingCH"+ch;
            }
            else
            {
                ch = currentSceneName.Replace("TrainingCH", "");
                message = "실습 학습이 마무리되었습니다.\n실습 평가로 이동하시겠습니까?";
                nextSceneName = "EvaluationCH"+ch;
            }
#endif
            // 존재하지 않는 씬인지 체크
            if (!Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp(message,
                    CL_MessagePopUpController.DialogType.NOTICE,
                    () => CL_CommonFunctionManager.Instance.LoadScene("Main"),
                    null);
            }
            else
            {
                CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp(message,
                    CL_MessagePopUpController.DialogType.NOTICE,
                    () => CL_CommonFunctionManager.Instance.LoadScene(nextSceneName),
                    null);
            }

            return;
        }

        // 구조면 실습으로, 실습이면 평가로

        // 평가 컨트롤러가 존재하는 경우, 다음 시퀀스 넘어가는 메소드를 사용하지 않음
        // if (examController != null) return;

        if(!isAllFinished())
        {
            if (!isFinished[Controllers.Script])
                scriptController.skipEffectBySeq(currentSeq);

            if (cameraController.getIsCamMove())
                cameraController.skipCamMoveBySeq(currentSeq);

            return;
        }

        foreach(int n in scriptController.stepSeq)
        {
            if(n - 1 == currentSeq)
            {
                scriptController.ShowTitleIndicatorStart(currentSeq + 1);
                StartCoroutine(DelayLoadSeq(currentSeq + 1, false));
                return;
            }
        }

        LoadSeq(currentSeq + 1, false);

        /*
        if (scriptController.SyncTitleIndicator() == 1)
        {
            scriptController.ShowTitleIndicatorStart();
            StartCoroutine(DelayLoadSeq(currentSeq + 1, false));
        }
        */
    }

    public void goPrevSeq()
    {
        // 첫 시퀀스라면 리턴
        if (isAllFinished() && currentSeq == 0)
            return;

        //평가에서는 이전 시퀀스로 넘어갈 수 없음
        // if (examController != null) return;

        LoadSeq(currentSeq - 1, false);
    }

    IEnumerator DelayLoadSeq(int _nextSeq, bool _forcedNext)
    {
        yield return new WaitForSeconds(2.5f);

        LoadSeq(_nextSeq, _forcedNext);
    }

    IEnumerator DelayStartSeq(int currentSeq)
    {
        yield return new WaitForSeconds(2.5f);

        scriptController.viewScriptBySeq(currentSeq);
        scriptController.StepProgressBar();
    }

    public void LoadSeq(int _nextSeq, bool _forcedNext)
    {
        NextBtn.interactable = true;

        if(currentSeq == _nextSeq)
        {
            return;
        }
        if (isAllFinished() || _forcedNext)
        {
            int prevSeq = currentSeq;

            currentSeq = _nextSeq;

            if (scriptController != null)
            {
                startByCont(Controllers.Script);
                scriptController.viewScriptBySeq(prevSeq, _nextSeq);
            }

            if (objectController != null)
            {
                startByCont(Controllers.Object);
                objectController.setOnOffBySeq(prevSeq, _nextSeq);
            }

            if (colliderController != null)
            {
                startByCont(Controllers.Collider);
                colliderController.activateCollider(prevSeq, _nextSeq);
            }

            if (materialController != null)
            {
                startByCont(Controllers.Material);
                materialController.setOnOffBySeq(prevSeq, _nextSeq);
                materialController.hideComponent(_nextSeq);
            }
            if (cameraController != null)
            {
                cameraController.setCameraOnlyByNext(_nextSeq);
            }

            if (actionController != null)
            {
                actionController.setOnOffBySeq(prevSeq, _nextSeq);
            }

            if (animationSeqController != null)
            {
                animationSeqController.ClearAnimPlaying();
            }

            if(outLineController != null)
            {
                outLineController.setOnOffBySeq(prevSeq, _nextSeq);
            }
            //이후 시퀀스들은 별개로 다뤄야 하므로 추가하지 않는다.
        }
        else
        {
            // 스크립트의 경우, 작업 처리가 되지 않았을 수 있다.
            if (!isFinished[Controllers.Script])
                scriptController.skipEffectBySeq(currentSeq);

            // 오브젝트의 경우, 작업 처리가 되지 않을 수 없다.

            // 매터리얼(하이라이트)의 경우, 작업 처리가 되지 않을 수 없다.

            // 카메라의 경우, 시퀀스에 영향을 주지 않지만 시퀀스에 영향은 받는다.
            if (cameraController.getIsCamMove())
                cameraController.skipCamMoveBySeq(currentSeq);

            // 팝업의 경우 작업 처리가 끝난 상태이므로 무시한다.
        }

        // 위의 진행바 조정
        scriptController.StepProgressBar();

        // 다음 시퀀스의 액션 데이터 존재 시 다음 버튼 인터랙션 막기
        if (actionController.sequenceGroupData.AllDataBySeq[currentSeq].GetComponent<ActionData>().ObjName.Length != 0)
        {
            NextBtn.interactable = false;
        }
    }

    public void finishedByCont(Controllers cont)
    {
        // 메인 컨트롤러에서 호출하지 않는 메소드입니다.
        // 각 서브 컨트롤러에서 사용할 것입니다.
        // 시퀀스 제어에 영향을 주는 컨트롤러만 사용하는 메소드입니다.
        isFinished[cont] = true;
    }

    public void startByCont(Controllers cont)
    {
        // 서브 컨트롤러에서 호출하지 않는 메소드입니다.
        // 메인 컨트롤러에서 사용할 것입니다.
        // 시퀀스 제어에 영향을 주는 컨트롤러만 사용하는 메소드입니다.
        isFinished[cont] = false;
    }

    public bool isContFinished(Controllers cont)
    {
        if (isFinished[cont])
            return true;
        else
            return false;
    }

    public bool isAllFinished()
    {
        foreach(bool val in isFinished.Values)
        {
            if (!val)
                return false;
        }

        return true;
    }

    public void GoJumpSeq(int seqNum)
    {
        LoadSeq(seqNum, true);
    }

    public void GoJumpStep(int index)
    {
        int nextSeq = index;

        if (index < scriptController.stepSeq.Length - 1)
        {
            if (scriptController.stepSeq[index] <= currentSeq && scriptController.stepSeq[index + 1] > currentSeq)
            {
                return;
            }
        }
        else
        {
            if (scriptController.stepSeq[index] <= currentSeq)
            {
                return;
            }
        }

        if(CL_CommonFunctionManager.Instance.GetProgramLanguage() == "KR")
        {
            CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("<color='#436d88'>[" + scriptController.stepText[index] + "]</color> 단계로 이동하시겠습니까?\n현재 진행중인 단계가 자동 진행/초기화 됩니다.",
                CL_MessagePopUpController.DialogType.NOTICE, 
               () => {
                   scriptController.ShowTitleIndicatorStart(scriptController.stepSeq[index]);
                   StartCoroutine(DelayLoadSeq(scriptController.stepSeq[index], true));
               },
               null);
        }
        else
        {
            CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("Are you sure you want to go to the\n<color='#436d88'>[" + scriptController.stepText[index] + "]</color> step?", CL_MessagePopUpController.DialogType.NOTICE, () => LoadSeq(scriptController.stepSeq[index], true), null);
        }
    }

    public void GoNextStepWithPopUp()
    {
        if(GoNextStepNum() != -1)
            CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("<color='#436d88'>[" + scriptController.stepText[GoNextStepNum()] + "]</color> 단계로 이동하시겠습니까?\n현재 진행중인 단계가 자동 진행/초기화 됩니다.", 
                CL_MessagePopUpController.DialogType.NOTICE, 
                () => {
                    scriptController.ShowTitleIndicatorStart(scriptController.stepSeq[GoNextStepNum()]);
                    StartCoroutine(DelayLoadSeq(scriptController.stepSeq[GoNextStepNum()], true));
                },
                null);
    }

    public void GoPrevStepWithPopUp()
    {
        if (GoPrevStepNum() != -1)
            CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("<color='#436d88'>[" + scriptController.stepText[GoPrevStepNum()] + "]</color> 단계로 이동하시겠습니까?\n현재 진행중인 단계가 자동 진행/초기화 됩니다.", 
                CL_MessagePopUpController.DialogType.NOTICE, 
                () => {
                    scriptController.ShowTitleIndicatorStart(scriptController.stepSeq[GoPrevStepNum()]);
                    StartCoroutine(DelayLoadSeq(scriptController.stepSeq[GoPrevStepNum()], true));
                },
                null);
    }

    public int GoNextStepNum()
    {
        bool isEnd = true;

        int nextSeq = 0;

        for (int i = 0; i < scriptController.stepSeq.Length; i++)
        {
            if(currentSeq < scriptController.stepSeq[i])
            {
                isEnd = false;
                nextSeq = i;
                break;
            }
        }

        if (isEnd)
        {
            return -1;
        }

        return nextSeq;
    }

    public int GoPrevStepNum()
    {
        int nextSeq = 0;
        
        if (currentSeq <= scriptController.stepSeq[0])
        {
            return -1;
        }

        for (int i = scriptController.stepSeq.Length - 1; i > 0; i--)
        {
            if(currentSeq > scriptController.stepSeq[i])
            {
                nextSeq = i;
                break;
            }
        }

        return nextSeq;
    }

    public void GoNextStep()
    {
        bool isEnd = true;

        int nextSeq = 0;

        foreach (int num in scriptController.stepSeq)
        {
            if(currentSeq < num)
            {
                isEnd = false;
                nextSeq = num;
                break;
            }
        }

        if(isEnd)
        {
            return;
        }

        LoadSeq(nextSeq, true);
    }

    public void GoPrevStep()
    {
        int nextSeq = 0;

        foreach (int num in scriptController.stepSeq)
        {
            if(currentSeq <= num)
            {
                break;
            }
            else
            {
                nextSeq = num;
            }
        }

        LoadSeq(nextSeq, true);
    }

    public int GetCurrentSeq()
    {
        return currentSeq;
    }
    
    public void SetCurrentSeq(int seqNum)
    {
        this.currentSeq = seqNum;
    }
}