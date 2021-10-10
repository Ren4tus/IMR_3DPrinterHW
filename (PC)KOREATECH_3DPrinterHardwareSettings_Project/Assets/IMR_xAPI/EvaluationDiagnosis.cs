//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//// 진단기 평가
//public class EvaluationDiagnosis : MonoBehaviour
//{
//    // 진단기에서 테스트하고자 하는 기준 1
//    // (모터 진단일 경우 모터 관련이 띄워져야 함)
//    // 진단기 DTC검색 내에서 필요한 텍스트를 표시했는가?
//    public bool scoreParamText = false;
//    // 진단기 DTC검색 내에서 필요한 팝업창을 띄웠는가?
//    public bool scoreParamPopup = false;

//    // 평가 타입에 따른 인덱스
//    // (0 : 모터, 1 : 감속기, 2 : 배터리)
//    public int partIndex;
//    // DTC검색 내에서 확인해야 하는 텍스트
//    public Text[] needCheckTexts;
//    // DTC검색 내에서 확인해야 하는 팝업창
//    public GameObject[] needCheckPopup;
//    // 코드별 진단 내에서 확인해야 하는 버튼
//    public Button[] needCheckButton;

//    // 다음 씬으로 이동하는 팝업창
//    public GameObject popupNext;
//    // 경고 팝업창
//    public GameObject popupAlert;
//    // 실패 팝업창
//    public GameObject popupFail;

//    // 타이머 관련 변수
//    public EvaluationTimer timer;

//    // 실패한 횟수
//    public int failNum = 0;

//    // 각 부품별 코드
//    string[] codeName =
//    {
//        "구동 모터 모듈 전원 이상",
//        "SBW 제어기 이상",
//        "배터리 전류 이상"
//    };

//    void Start()
//    {
//        // 점수 초기화
//        StaticClassEvaulateResult.TotalScore = 0;

//        // 코드별 진단 내에서 확인해야 하는 모든 버튼에 대해서
//        for (int i =0; i < needCheckButton.Length; i++)
//        {
//            // 현재 진단하는 부품과 버튼의 부품 코드가 동일할 경우
//            if (i == partIndex)
//                needCheckButton[i].onClick.AddListener(SuccessToSelectButton); // 성공
//            // 그렇지 않을 경우
//            else
//                needCheckButton[i].onClick.AddListener(FailToSelectButton); // 실패
//        }

//        MessageListSet.Instance.SetMessageListBuff();
//    }

//    // Update is called once per frame
//    void Update()
//    {        
//        // DTC검색 진단 내에서 확인해야 하는 모든 텍스트에 대해서
//        for (int i =0; i < needCheckTexts.Length; i++)
//        {
//            // 해당 텍스트가 표시되어 있고, 해당 텍스트가 진단하는 부품 코드와 일치할 경우
//            if (needCheckTexts[i].gameObject.activeSelf && 
//                needCheckTexts[i].text == codeName[partIndex])
//            {
//                scoreParamText = true; // 통과
//            }
//        }
        
//        // 부품 코드와 일치하는 팝업이 켜질 경우
//        if (needCheckPopup[partIndex].gameObject.activeSelf)
//        {
//            scoreParamPopup = true; // 통과
//        }
//    }

//    // 코드별 진단 내에서 잘못된 버튼을 선택한 경우
//    public void FailToSelectButton()
//    {
//        failNum++; // 실패 수 증가

//        // 일정 범위 이상 실패시 실패 팝업 띄우고 다음으로 이동
//        if (failNum > 4)
//        {
//            popupFail.SetActive(true);
//            CalculateTotalScore(); // 진단기 내 총 점수 계산
//            SendFailTextResult(false); // 결과 창에 실패 내역 전달
//            timer.StoreTimeForNextScene(); // 타이머 일시 정지
//        }
//        else
//        {
//            popupAlert.SetActive(true); // 경고 팝업창 띄우기
//            timer.StoreTimeForNextScene(); // 타이머 일시 정지
//        }
//    }

//    void SuccessToSelectButton()
//    {
//        popupNext.SetActive(true); // 다음 씬으로 가는 팝업 창 표시
//        CalculateTotalScore(); // 진단기 내 총 점수 계산
//        SendFailTextResult(false); // 결과 창에 실패 내역 전달
//        timer.StoreTimeForNextScene(); // 타이머 일시 정지
//    }

//    void CalculateTotalScore()
//    {
//        int startScore = 10; // 시작 점수

//        if (scoreParamText) startScore += 5; // 통과 시 점수 추가
//        if (scoreParamPopup) startScore += 5; // 통과 시 점수 추가

//        startScore -= failNum * 5; // 실패 횟수 * 5 만큼 점수 감산
//        if (startScore < 0) startScore = 0; // 최소 0점에서 종료
        
//        Debug.Log(startScore);
//        StaticClassEvaulateResult.TotalScore += startScore; // 총 점수에 반영
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
//        if (!scoreParamText)
//            MessageListSet.Instance.AddList(currentTypeString + " DTC 비확인", timeout ? -1 : 5,
//                currentTypeIndex + 2);//currentTypeString + " 훈련 - 진단기"
//        if (!scoreParamPopup)
//            MessageListSet.Instance.AddList(currentTypeString + " DTC 팝업 비확인", timeout ? -1 : 5,
//                currentTypeIndex + 2);//currentTypeString + " 훈련 - 진단기"
//        if (failNum > 0)
//            MessageListSet.Instance.AddList("진단기 선택 오류", timeout ? -1 : 5,
//                currentTypeIndex + 2);//currentTypeString + " 훈련 - 진단기"
//    }
//}
