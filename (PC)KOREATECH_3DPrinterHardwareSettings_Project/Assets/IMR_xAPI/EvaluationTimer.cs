//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class EvaluationTimer : MonoBehaviour
//{
//    // 시간이 표시되는 텍스트
//    public Text timeText;
//    // 초기에 시작되는 타이머 시간
//    public float startTime;
//    // True : 초기에 해당 시간으로 시작, False : 다른 씬의 남은 시간을 가져옴
//    public bool startingPoint = false;
//    // True : 시간 진행, False : 시간 일시 중지
//    bool tickTimer = false;
//    // 타임 아웃 팝업
//    //public GameObject popupTimeout;
//    // 팝업이 켜질 경우 시간이 일시 중지되는 팝업
//    //public List<GameObject> popups;
//    // 현재 남은 시간
//    float currentTime;
//    // 진단기 평가 관련 변수
//    public EvaluationDiagnosis diag;
//    // 점검기 평가 관련 변수
//    public EvaluationCheck check;
//    bool endFailSend = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        // popups = new List<GameObject>();

//        // True : 초기에 해당 시간으로 시작
//        if (startingPoint)
//        {
//            // 초기 시작 시간 결정
//            currentTime = startTime;
//            // Static Class에 저장
//            StaticClassEvaulateResult.StartTime = startTime;
//        }
//        // False : 다른 씬의 남은 시간을 가져옴
//        else
//        {
//            // Static Class에서 남은 시간을 가져옴
//            currentTime = StaticClassEvaulateResult.CurrentTime;
//        }            
//        tickTimer = true;
//    }

//    // Update is called once per frame 
//    void Update()
//    {
//        //bool isActiveFlag = false;

//        //// 시간이 유효한 상황에서
//        //if (currentTime >= 0)
//        //{
//        //    //// 지정된 팝업 중 
//        //    //for (int i = 0; i < popups.Count; i++)
//        //    //{
//        //    //    // 하나라도 켜졌을 경우
//        //    //    if (popups[i].activeSelf)
//        //    //    {
//        //    //        // 일시 정지 관련 플래그
//        //    //        isActiveFlag = true;
//        //    //        break;
//        //    //    }

//        //    //}

//        //    // 시간 일시 정지 관련 변수 설정
//        //    tickTimer = !isActiveFlag;
//        //}

//        // True : 시간 진행
//        if (tickTimer)
//        {
//            currentTime -= Time.deltaTime;
//            // 해당 시간 관련 문자열 저장
//            timeText.text = (string.Format("{0:D2}", (int)currentTime / 60).ToString()) + " : " +
//                (string.Format("{0:D2}", (int)currentTime % 60).ToString());
//        }
//        // 시간이 모두 소진되었을 경우
//        if (currentTime < 0)
//        {
//            Debug.Log("Time over");
//            // False : 시간 일시 정지
//            tickTimer = false;
//            timeText.text = "0:00";
//            // 타임 아웃 팝업 표시
//            //popupTimeout.SetActive(true);

//            // 진단기 평가 중에서 타임 아웃되었을 경우
//            //if (diag && !endFailSend)
//            //{
//            //    // 실패 내용 전달
//            //    diag.SendFailTextResult(true);
//            //    endFailSend = true;
//            //}

//            //// 점검기 평가 중에서 타임 아웃되었을 경우
//            //if (check && !endFailSend)
//            //{
//            //    // 실패 내용 전달
//            //    check.SendFailTextResult(true);
//            //    endFailSend = true;
//            //}                
//        }            
//    }

//    public void StoreTimeForNextScene()
//    {
//        // False : 시간 일시 정지
//        tickTimer = false;
//        // 해당 남은 시간을 Static Class에 저장
//        StaticClassEvaulateResult.CurrentTime = currentTime;
//        Debug.Log("time : " + StaticClassEvaulateResult.CurrentTime.ToString());
//        // SceneManager.LoadScene(nextScene);
//    }

//    public void StoreTimeForNextSceneWithLoad(string nextScene)
//    {
//        // False : 시간 일시 정지
//        tickTimer = false;
//        // 해당 남은 시간을 Static Class에 저장
//        StaticClassEvaulateResult.CurrentTime = currentTime;
//        Debug.Log("time : " + StaticClassEvaulateResult.CurrentTime.ToString());
//        // 다음 씬으로 이동
//        //SceneManager.LoadScene(nextScene);
//    }

//    public void StoreTimeForNextSceneWithLW(string nextScene)
//    {
//        // False : 시간 일시 정지
//        tickTimer = false;
//        // 해당 남은 시간을 Static Class에 저장
//        StaticClassEvaulateResult.CurrentTime = currentTime;
//        Debug.Log("time : " + StaticClassEvaulateResult.CurrentTime.ToString());
//        // 다음 씬으로 이동
//        //LoadingSceneManager.LoadScene(nextScene);
//    }
//}
