using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class ExamController : MonoBehaviour
{
    public Text Timer;

    private float time = 0f;

    public float speed = 1f;

    public string m_Timer = @"남은 시간 00:00";
    private bool m_IsPlaying = false;
    private bool m_IsEnd = false;
    public float m_TotalSeconds = 30 * 60; // 카운트 다운 전체 초(5분 X 60초), 인스펙트 창에서 수정해야 함. 

    public static ExamController instance;

    public bool[] ExamStep;

    [Header("단계별 점수")]
    public int[] StepScore;
    public int[] MaxScore; // 최대점수
    public bool[] etcScore; // 안전, 시간내 등

    [Header("평가표")]
    public GameObject RatingPanel;
    public GameObject ScoreList;

    [Header("평가 기준")]
    public int InternalOption;
    public int ImportOption;
    public Sprite[] ImportOptionImage;
    public string[] ImportOptionName;
    public Image StartImage;
    public Text StartText;

    private string[] InternalOptionTexts = { "내부채움 : 속빈",
                                             "내부채움 : 가볍게",
                                             "내부채움 : 보통",
                                             "내부채움 : 무겁게"};
    


    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        ResetValue();

        for (int i = 0; i < ExamStep.Length; i++)
            ExamStep[i] = false;

        OptionSetting();
    }

    void Update()
    {
        if (m_IsPlaying)
        {
            m_Timer = CountdownTimer();

            if(m_IsEnd)
            {
                EndFunction();
            }
        }

        if (m_TotalSeconds <= 0)
        {
            etcScore[1] = false;
            EndFunction();
            SetZero();
        }

        Timer.text = m_Timer;
    }

    private void OptionSetting()
    {
        // 처음 랜덤에 맞춰 화면 구성
        // 필요) 이미지, 텍스트
        InternalOption = UnityEngine.Random.Range(0, 4); // 내부 채움 옵션
        ImportOption = UnityEngine.Random.Range(0, 2);   // 모델 종류
        StringBuilder sb = new StringBuilder();

        if(ImportOption == 0)
        {
            sb.Append("모델 : "+ ImportOptionName[0] +"\n");
        }
        else
        {
            sb.Append("모델 : " + ImportOptionName[1] + "\n");

            StartImage.SetNativeSize();
            StartImage.rectTransform.sizeDelta = new Vector2(240f, 240f);
        }

        StartImage.sprite = ImportOptionImage[ImportOption];

        sb.Append(InternalOptionTexts[InternalOption] + "\n");
        sb.Append("제한시간 : 30분");

        StartText.text = sb.ToString();

        sb.Clear();
    }

    private string CountdownTimer(bool IsUpdate = true)
    {
        if (IsUpdate)
            m_TotalSeconds -= Time.deltaTime * speed;

        TimeSpan timespan = TimeSpan.FromSeconds(m_TotalSeconds);
        string timer = "";

        if (timespan.Minutes <= 15 && timespan.Minutes > 10)
        {
            timer = string.Format("남은시간 <color='#ff7e10'>{0:00}:{1:00}</color>", timespan.Minutes, timespan.Seconds);
        }
        else if(timespan.Minutes <= 10)
        {
            timer = string.Format("남은시간 <color='#cd2b24'>{0:00}:{1:00}</color>", timespan.Minutes, timespan.Seconds);
        }
        else
        {
            timer = string.Format("남은시간 {0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
        }

        return timer;
    }

    private void EndFunction()
    {
        m_IsPlaying = false;

        for(int i=0; i<ExamStep.Length; i++)
        {
            ScoreList.transform.GetChild(i).GetComponent<Text>().text = StepScore[i]+"/"+MaxScore[i];
        }
        for(int j=ExamStep.Length; j<ExamStep.Length+etcScore.Length; j++)
        {
            if(etcScore[j-ExamStep.Length])
            {
                ScoreList.transform.GetChild(j).GetComponent<Text>().text = "Pass";
            }
            else
            {
                ScoreList.transform.GetChild(j).GetComponent<Text>().text = "Fail";
            }
        }

        RatingPanel.SetActive(true);
    }

    public bool isStart()
    {
        return m_IsPlaying;
    }

    private void SetZero()
    {
        m_Timer = @"남은시간 00:00";
        m_TotalSeconds = 0;
        m_IsPlaying = false;
    }

    private void ResetValue()
    {
        m_IsPlaying = false;
        speed = 1f;
    }

    public void StartTimer()
    {
        m_IsPlaying = true;
    }

    public void ExamStepClear(int n)
    {
        ExamStep[n] = true;
    }

    public bool IsClear(int n)
    {
        return ExamStep[n];
    }

    public void SetScore(int n, int score)
    {
        StepScore[n] += score;

        for(int i=0; i<n; i++)
        {
            if (StepScore[i] < 0)
                StepScore[i] = 0;
        }
    }

    public void SetEtcScore(int n, bool pass)
    {
        etcScore[n] = pass;
    }

    public void Finish()
    {
        m_IsEnd = true;
    }

    public void LoadScene(string sceneName)
    {
        CL_CommonFunctionManager.Instance.LoadScene(sceneName);
    }
}
