using UnityEngine;
using UnityEngine.UI;
using System;

public class PrintingInterfaceScript : MonoBehaviour
{
    public Text Percent;
    public Text Timer;
    public MainController main;
    private float time = 0f;

    public float speed = 1f;

    public static PrintingInterfaceScript instance;

    public string m_Timer = @"00:00:00";
    private bool m_IsPlaying=true;
    public float m_TotalSeconds = 5 * 60; // 카운트 다운 전체 초(5분 X 60초), 인스펙트 창에서 수정해야 함. 
    public int ResetSeq;

    public int FastSeq;
    private void ResetValue()
    {
        m_IsPlaying = true;
        m_TotalSeconds = 21600f;
        Percent.text = "0%";
        Timer.text = "06:00:00";
        speed = 1f;
    }

    private void OnEnable()
    {
        ResetValue();
    }

    private void OnDisable()
    {
        ResetValue();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        time += Time.deltaTime * speed;

        if ((int)time / 216 >= 100)
            Percent.text = "100%";
        else
            Percent.text = (int)time/216+"%";

        //Timer.text = string.Format("{0:00}:{1:00}:{2:00}", (int)time / 3600, (int)time / 60 & 60, (int)time % 60);

        if (m_IsPlaying)
        {
            m_Timer = CountdownTimer();
        }

        if (m_TotalSeconds <= 0)
        {
            SetZero();
        }

        Timer.text = m_Timer;
        if(FastSeq == MainController.instance.GetCurrentSeq())
        {
            speed = 1500f;
        }
        else
        {
            speed = 1f;
        }
    }

    public void Faster()
    {
        // 다음시퀀스로
        MainController.instance.isRightClick = true;
        MainController.instance.goNextSeq();
        MainController.instance.isRightClick = false;

        speed = 1500f;
    }

    private string CountdownTimer(bool IsUpdate = true)
    {
        if (IsUpdate)
            m_TotalSeconds -= Time.deltaTime*speed;

        TimeSpan timespan = TimeSpan.FromSeconds(m_TotalSeconds);
        string timer = string.Format("{0:00}:{1:00}:{2:00}",
            timespan.Hours, timespan.Minutes, timespan.Seconds);

        return timer;
    }

    private void SetZero()
    {
        m_Timer = @"00:00:00";
        m_TotalSeconds = 0;
        m_IsPlaying = false;
    }
}