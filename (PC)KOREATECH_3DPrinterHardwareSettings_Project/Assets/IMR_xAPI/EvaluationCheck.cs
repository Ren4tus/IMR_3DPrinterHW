//using System.Collections;
//using System.Collections.Generic;
//using TinCan;
//using UnityEngine;
//using UnityEngine.UI;
//using IMR;
//public class EvaluationCheck : MonoBehaviour
//{
//    //public Evaluation evaluator;

//    public Text shortMessager;
//    private int tmpIdx = -1;

//    public Transform startCameraPos;

//    public Button[] buttons; // Motor : 3 buttons / Index 0, 1, 2, 3, 4, 5
//    public GameObject[] buttons3D; // Motor : 2 buttons / Index 0, 1

//    // 멀티미터 이미지 팝업창
//    public GameObject popupMultimeter;
//    public GameObject popupMultimeter3d;
//    public GameObject directionalLight;
//    // 성공 팝업창
//    public GameObject popupSuccess;
//    // 실패 팝업창
//    public GameObject popupFailure;
//    // 경고 팝업창
//    public GameObject popupAlert;

//    // N극, S극 와이어 오브젝트
//    public GameObject[] wires;

//    // 현재 스텝을 표시하는 텍스트
//    public Text currentState;

//    // 타이머 관련 변수
//    //public EvaluationTimer timer;

//    // public Button3DMouseEvent[] button3dMouses;
//    // bool[] button3dClicked = { false, false };

//    public GameObject resultPanel;
    
//    // 각 스텝마다 눌러야 하는 버튼의 인덱스
//    int[] stages = { 0, 0, 1, 1, 4, 5 };
//    // 3D 버튼 사용 여부 (True : 3D 버튼 사용 / False : 2D 버튼 사용)
//    bool[] use3d = { false, true, false, true, false, false };

//    public GameObject[] beforeSuccessObjs;
//    public GameObject[] afterSuccessObjs;

//    // 부품 인덱스
//    public int partIndex;

//    // 현재 스텝
//    public int currentStep = 0;
//    // 실패 횟수
//    public int failNum = 0;

//    // 잘못된 버튼 선택으로 인한 실패로 인한 감산 점수
//    public int failScoreButton = 0;
//    // N극 와이어 부착 실패로 인한 감산 점수
//    public int failScoreN = 0;
//    // S극 와이어 부착 실패로 인한 감산 점수
//    public int failScoreS = 0;

//    private bool firstErrorFlag = false;

//    void Start()
//    {
//        // 2D 버튼들의 Listener 부착
//        buttons[0].onClick.AddListener(() => CheckNon3dButton(0));
//        buttons[1].onClick.AddListener(() => CheckNon3dButton(1));
//        buttons[2].onClick.AddListener(() => CheckNon3dButton(2));
//        buttons[3].onClick.AddListener(() => CheckNon3dButton(3));
//        buttons[4].onClick.AddListener(() => CheckNon3dButton(4));
//        buttons[5].onClick.AddListener(() => CheckNon3dButton(5));

//        // button3dMouses[0] = buttons3D[0].GetComponent<Button3DMouseEvent>();
//        // button3dMouses[1] = buttons3D[1].GetComponent<Button3DMouseEvent>();


//        //GCameraDirector.G.SetGhostCameraTransform(startCameraPos);
//    }

//    private void OnEnable()
//    {
//        failNum = 0;
//        //MessageListSet.Instance.ClearList();
//        //StaticClassEvaulateResult.StartTime = StaticClassEvaulateResult.CurrentTime;
//    }

//    void Update()
//    {
//        UpdateShortMessage(currentStep);

//        if (currentStep >= 4)
//        {
//            for (int i = 0; i < beforeSuccessObjs.Length; i++)
//                beforeSuccessObjs[i].SetActive(false);

//            for (int i = 0; i < afterSuccessObjs.Length; i++)
//                afterSuccessObjs[i].SetActive(true);
//        }

//        // 현재 스텝에 따른 N, S극 와이어 활성화
//        switch (currentStep)
//        {
//            case 2: // N극 와이어
//                wires[0].SetActive(true);
//                break;
//            case 4: // S극 와이어
//                wires[1].SetActive(true);
//                break;
//            default:
//                break;
//        }

//        // 현재 스텝을 텍스트창에 표시
//        if (currentStep > 5)
//            currentStep = 5;
//        string curStep = "Step " + (currentStep + 1) + " / Step 6";
//        currentState.text = curStep;

//        // 모든 스텝을 완료하고, 멀티미터 팝업창이 꺼져있고, 성공 팝업창이 꺼져있는 경우
//        if (currentStep >= 5 && !popupMultimeter.activeSelf && !popupSuccess.activeSelf)
//        {
//            // 성공 팝업창을 띄운다
//            //popupSuccess.SetActive(true);
//            // 총 합산 점수를 계산한다
//            CalculateTotalScore();
//            // 결과 데이터를 결과창에 전달한다
//            SendFailTextResult(false);
//            // 시간을 일시 정지하고, 남은 시간을 저장한다
//            timer.StoreTimeForNextScene();

//            //GMainStoryBrowser.G.OnNext();
//            GoToResult();

//            //StoryLineListControl.S.OnSelectStoryLine();
//        }

//        // 실패 수가 한도 (7번)을 넘었을 경우
//        if (failNum >= 6)
//        {
//            // 실패 팝업 창을 띄운다
//            //popupFailure.SetActive(true);
//            // 추가 점수 없음
//            //StaticClassEvaulateResult.TotalScore = 0;
//            // 결과 데이터를 결과창에 전달한다
//            SendFailTextResult(false);
//            // 시간을 일시 정지하고, 남은 시간을 저장한다
//            timer.StoreTimeForNextScene();

//            //GMainStoryBrowser.G.OnNext();
//            GoToResult();

//            //StoryLineListControl.S.OnSelectStoryLine();
//        }
//    }

//    public void GoToResult()
//    {
//        //XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(true,currentStep+1
//        //    ,6,CheckerText.current.comp_text.text);
//        XAPIApplication.current.SendMotorStatement("Choice");
//        //evaluator.calculateResult();
//        resultPanel.SetActive(true);
//        this.gameObject.SetActive(false);
//        XAPIApplication.current.motor_lesson_manager.SetLimitStatementResult(true);
//        XAPIApplication.current.SendMotorStatement("Time");
//        XAPIApplication.current.motor_lesson_manager.SetResultStatementExtention(true);
//        XAPIApplication.current.SendMotorStatement("Result");
//        //GMainStoryBrowser.G.OnNext();
//        // IMRStatement.SetActor();
//        // IMRStatement.SetVerb("passed");
//        // IMRStatement.SetActivity(SStoryManager.G.m_MainStoryName);
//    }

//    public void Check3dButton(int buttonNum)
//    {
//        // 현재 스텝에서 3D 버튼을 사용하지 않는 경우
//        if (!use3d[currentStep])
//        {
//            // 버튼 선택을 실패한 경우
//            FailCase();
//        }
//        else
//        {
//            // 현재 스텝에서 사용해야 할 3D 버튼 이외의 다른 3D 버튼을 누른 경우
//            if (buttonNum != stages[currentStep])
//            {
//                // 버튼 선택을 실패한 경우
//                FailCase();
//            }
//            else
//            {
//                //XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(true,currentStep+1
//                //    ,6,CheckerText.current.comp_text.text);
//                XAPIApplication.current.SendMotorStatement("Choice");
//                // 다음 스텝으로 이동
//                //evaluator.addSuccessScore(1);
//                currentStep++;
//                firstErrorFlag = false;
                
//            }
//        }
//    }

//    void CheckNon3dButton(int buttonNum)
//    {
//        // 현재 스텝에서 2D 버튼을 사용하지 않는 경우
//        if (use3d[currentStep])
//        {
//            // 멀티미터 팝업창을 켜는 버튼 이외에는
//            if (buttonNum != 4)
//            {
//                // 버튼 선택을 실패한 경우
//                FailCase();
//            }
//            else
//            {
//                // 멀티미터 팝업창을 활성화한다
//                // 점수 감산 없음
//                popupMultimeter.SetActive(true);
//                popupMultimeter3d.SetActive(true);
//                directionalLight.SetActive(false);
//            }
//        }
//        else
//        {
//            // 현재 스텝에서 사용해야 할 2D 버튼 이외의 다른 2D 버튼을 누른 경우
//            if (buttonNum != stages[currentStep])
//            {
//                // 멀티미터 팝업창을 켜는 버튼 이외에는
//                if (buttonNum != 4)
//                {
//                    // 버튼 선택을 실패한 경우
//                    FailCase();
//                }
//                else
//                {
//                    // 멀티미터 팝업창을 활성화한다
//                    // 점수 감산 없음
//                    popupMultimeter.SetActive(true);
//                    popupMultimeter3d.SetActive(true);
//                    directionalLight.SetActive(false);
//                }
//            }
//            else
//            {
//                // 멀티미터 팝업창을 활성화 여부
//                if (buttonNum == 4)
//                {
//                    popupMultimeter.SetActive(true);
//                    popupMultimeter3d.SetActive(true);
//                    directionalLight.SetActive(false);
//                }                    
//                else
//                {
//                    popupMultimeter.SetActive(false);
//                    popupMultimeter3d.SetActive(false);
//                    directionalLight.SetActive(true);
//                }
//                // 다음 스텝으로 이동
//                //XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(true,currentStep+1
//                //    ,6,CheckerText.current.comp_text.text);
//                XAPIApplication.current.SendMotorStatement("Choice");
//                //evaluator.addSuccessScore(1);
//                currentStep++;
//            }
//        }
//    }

//    public void OnEmptyCallback()
//    {

//    }

//    void FailCase_Callback()
//    {
//        popupMultimeter.SetActive(false);
//        popupMultimeter3d.SetActive(false);
//        directionalLight.SetActive(true);
//        currentStep++;
//    }

//    void FailCase()
//    {
//        // 실패 횟수 누적
//        failNum++;
//        // 실패 횟수에 따른 감산 점수 계산
//        if (firstErrorFlag)
//        {
//            CalculateFailScore();
//            firstErrorFlag = false;
//        }
//        else
//            firstErrorFlag = true;
//        // 실패 수가 한도 (7번)을 넘지 않았을 경우
//        //if (failNum < 6)
//        //{
//        //    if (firstErrorFlag)
//        //        UserConfirmPopupControl.G.RequestConfirm(
//        //         "다시 선택해주세요!"
//        //         , null);
//        //    else
//        //    {
//        //        string errorStr = "틀렸습니다. 다음 절차로 넘어갑니다.";
//        //        XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(false,currentStep+1
//        //            ,6,CheckerText.current.comp_text.text);
//        //        XAPIApplication.current.SendMotorStatement("Choice");

//        //        if (currentStep >= 5)
//        //            errorStr = "틀렸습니다. 점검기 평가를 종료합니다.";

//        //        UserConfirmPopupControl.G.RequestConfirm(errorStr , FailCase_Callback, true);
//        //        //popupMultimeter.SetActive(false);
//        //        //popupMultimeter3d.SetActive(false);
//        //        //directionalLight.SetActive(true);
//        //        //currentStep++;
//        //    }
//        //    // 경고 팝업창을 띄운다
//        //    //popupAlert.SetActive(true);
//        //}
//    }

//    void CalculateTotalScore()
//    {
//        // 시작 점수 설정
//        int startScore = 15;

//        // 실패 횟수에 따라서 점수 감산
//        startScore -= ((failNum) / 2) * 5;
//        if (startScore < 0) startScore = 0; // 최저 점수 0

//        Debug.Log(startScore);
//        // 총 점수에 반영
//        //StaticClassEvaulateResult.TotalScore = startScore;
//    }

//    void CalculateFailScore()
//    {
//        //현재 스텝에 따라서
//        switch (currentStep)
//        {
//            case 1:
//                failScoreN += 5; // N극 선택 오류
//                break;
//            case 3:
//                failScoreS += 5; // S극 선택 오류
//                break;
//            default:
//                failScoreButton += 5; // 버튼 선택 오류
//                break;
//        }

//        //evaluator.addFailScore(1);
//    }

//    public void SendFailTextResult(bool timeout)
//    {
//        // 부품 타입에 따른 문자열 설정
//        string currentTypeString = "";
//        int currentTypeIndex = -1;
//        switch (partIndex)
//        {
//            case 0:
//                currentTypeString = "모터";
//                currentTypeIndex = 10;
//                break;
//            case 1:
//                currentTypeString = "감속기";
//                currentTypeIndex = 20;
//                break;
//            case 2:
//                currentTypeString = "배터리";
//                currentTypeIndex = 30;
//                break;
//            default:
//                currentTypeString = "Error";
//                break;
//        }

//        // 결과 창에 해당 실패 내역 전달
//        if (failScoreButton > 0)
//            MessageListSet.Instance.AddList(
//                currentTypeString + " 점검 " + "버튼 선택 오류"
//                , timeout ? -1 : failScoreButton, currentTypeIndex);// currentTypeString + " 훈련 - 점검"
//        if (failScoreN > 0)
//            MessageListSet.Instance.AddList(
//                currentTypeString + " 점검 " + "N극 연결 오류"
//                , timeout ? -1 : failScoreN, currentTypeIndex);// currentTypeString + " 훈련 - 점검"
//        if (failScoreS > 0)
//            MessageListSet.Instance.AddList(
//                currentTypeString + " 점검 " + "S극 연결 오류"
//                , timeout ? -1 : failScoreS, currentTypeIndex);// currentTypeString + " 훈련 - 점검"
//    }

//    public void UpdateShortMessage(int stepIdx)
//    {
//        if (shortMessager == null)
//            return;

//        string sMessage = null;

//        switch(stepIdx)
//        {
//            case 0:
//                sMessage = "N극 연결부 선택";
//                break;
//            case 1:
//                sMessage = "N극 와이어 연결";
//                break;
//            case 2:
//                sMessage = "S극 연결부 선택";
//                break;
//            case 3:
//                sMessage = "S극 와이어 연결";
//                break;
//            case 4:
//                sMessage = "멀티미터기 확인";
//                break;
//            case 5:
//                sMessage = "점검값 확인";
//                break;
//            default:
//                sMessage = "모터 실습 평가 종료";
//                break;
//        }

//        shortMessager.text = sMessage;
//    }

//}
