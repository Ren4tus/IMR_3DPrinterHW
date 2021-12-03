using System;
using System.Collections;
using System.Collections.Generic;
using IMR;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;

public class XAPIApplication : MonoBehaviour
{

    public static XAPIApplication current;
    public bool terminatied = false;

    private string _endpoint = "http://www.vt-lrs.com/data/xAPI";
    private string _key = "812073176a6f25e4ddd1cb6c80fadac19e05221e";
    private string _secret_key = "2307f2b781f07c4aaaaede4ef5ee6cfce306f2f8";
    public string debug_msg = "";
    
    private RemoteLRS lrs;
    public StatementLRSResponse lrs_res = null;
    
    public string actor_name = "korea_tech";
    //private string currentLesson = "NULL";
    public string tempUserName = "null";

    public LoginLessonManager loginLessonManager;
    public JettingLessonManager jettingLessonManager;
    public PhotopolymerizationLessonManager photopolymerizationLessonManager;
    public PowderBedFusionLessonManager powderBedFusionLessonManager;
    public Lesson nowLessonManager;
    //IMRLAB 
    public string ActorName
    {
        get => actor_name;
        set => actor_name = value;
    }

    public string EndPoint
    {
        get => _endpoint;
        set => _endpoint = value;
    }
    public string UserName
    {
        get => _key;
        set => _key = value;
    }
    public string Password
    {
        get => _secret_key;
        set => _secret_key = value;
    }

    public void SetRemoteLRS()
    {
        lrs = new RemoteLRS(_endpoint,_key,_secret_key);
    }
    public void SetRemoteLRS(string s)
    {
        lrs = new RemoteLRS(s,_key,_secret_key);
    }

    private void Awake()
    {
        if (current == null)
        {
            current = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public void Init()
    {
        SetRemoteLRS();
        
        loginLessonManager = new LoginLessonManager();
        
    }
    //IMRLAB 10-07 추가
    public void LessonManagerInit()
    {
        jettingLessonManager = new JettingLessonManager();
        photopolymerizationLessonManager = new PhotopolymerizationLessonManager();
        powderBedFusionLessonManager = new PowderBedFusionLessonManager();
    }
    //IMRLAB 10-07 추가
    public void SendInitStatementBySceneName()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "EvaluationCH1":
                terminatied = false;
                nowLessonManager = jettingLessonManager;
                SendIMRXAPIStatement("Init");
                break;
            case "EvaluationCH2":
                terminatied = false;
                nowLessonManager = photopolymerizationLessonManager;
                SendIMRXAPIStatement("Init");
                break;
            case "EvaluationCH3":
                terminatied = false;
                nowLessonManager = powderBedFusionLessonManager;
                SendIMRXAPIStatement("Init");
                break;
        }
    }
    public void SetNowLessonFromNowActiveScene()
    {
        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            case "EvaluationCH1":
                nowLessonManager = jettingLessonManager;
                break;
            case "EvaluationCH2":
                nowLessonManager = photopolymerizationLessonManager;
                break;
            case "EvaluationCH3":
                nowLessonManager = powderBedFusionLessonManager;
                break;
        }
        
    }
    public bool SendIMRXAPIStatement(string name)
    {
        SetNowLessonFromNowActiveScene();
        var imr_statement = nowLessonManager.GetIMRStatement(name);
        lrs_res = lrs.SaveStatement(imr_statement.GetStatement());
        if (lrs_res.success) //Success
        {
            nowLessonManager.ChangeNewStatement(name);
            Debug.Log("Save statement2: " + lrs_res.content.id);
            return true;
        }
        else //Failure
        {
            Debug.Log("Statement Failed: " + lrs_res.errMsg);
            return false;
        }
    }
    public void AddHintCount(int n)
    {
        nowLessonManager.AddHintCount(n);
    }

    //IMRLAB 10-07 추가
    //로그인된 유저 아이디 ACTOR로 저장/ 로그인 문장 전송
    public bool SendLoginStatement(string userID)
    {
        Init();
        string[] splitText = userID.Split('@');
        ActorName = splitText[0];
        loginLessonManager.SetContextExtensionUserID(ActorName);
        LessonManagerInit();
        var imr_statement = loginLessonManager.GetIMRStatement("Init");
        
        lrs_res = lrs.SaveStatement(imr_statement.GetStatement());


        if (lrs_res.success) //Success
        {
            loginLessonManager.ChangeNewStatement("Init");
            Debug.Log("Save statement: " + lrs_res.content.id);
            return true;
        }
        else //Failure
        {
            Debug.Log("Statement Failed: " + lrs_res.errMsg);
            return false;
        }

    }
    public void GetResultCanvas(EvaluationCore.EvaluationContainer SequenceConatiner)
    {
        StartCoroutine(callResultCanvas(SequenceConatiner));
    }

    IEnumerator callResultCanvas(EvaluationCore.EvaluationContainer SequenceConatiner)
    {
        yield return new WaitForSeconds(0.1f);
        nowLessonManager.CloneResultCanvas(SequenceConatiner);
        nowLessonManager.ChangeNewStatement("Terminate");
        XAPIApplication.current.SendIMRXAPIStatement("Terminate");

    }
    public void SetTimeLimitSucces(bool b)
    {
        nowLessonManager.SetLimitStatementResult(b);
    }

}
