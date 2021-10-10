using System;
using System.Collections;
using System.Collections.Generic;
using IMR;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;

public class XAPIApplication : Singleton<XAPIApplication>
{

    public static XAPIApplication current;

    public LoginLessonManager loginLessonManager;
    public JettingLessonManager jettingLessonManager;
    //public MotorLessonManager motor_lesson_manager;
    //public ReducerLessonManager reducer_lesson_manager;
   
    private string _endpoint = "http://www.vt-lrs.com/data/xAPI";
    private string _key = "812073176a6f25e4ddd1cb6c80fadac19e05221e";
    private string _secret_key = "2307f2b781f07c4aaaaede4ef5ee6cfce306f2f8";
    public string debug_msg = "";
    
    private RemoteLRS lrs;
    public StatementLRSResponse lrs_res = null;
    
    private string actor_name = "korea_tech";

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
    }
    //IMRLAB 10-07 추가
    public void SendInitStatementByRowSceneName(string sceneName)
    {
        switch (sceneName)
        {
            case "EvaluationCH1":
                SendJettingStatement("Init");
                break;
        }
    }
    
    public bool SendJettingStatement(string name)
    {
        var imr_statement = jettingLessonManager.GetIMRStatement(name);
        lrs_res = lrs.SaveStatement(imr_statement.GetStatement());
        if (lrs_res.success) //Success
        {
            jettingLessonManager.ChangeNewStatement(name);
            Debug.Log("Save statement: " + lrs_res.content.id);
            return true;
        }
        else //Failure
        {
            Debug.Log("Statement Failed: " + lrs_res.errMsg);
            return false;
        }
    }

    //IMRLAB 10-07 추가
    //로그인된 유저 아이디 ACTOR로 저장/ 로그인 문장 전송
    public bool SendLoginStatement(string userID)
    {
        
        string[] splitText = userID.Split('@');
        ActorName = splitText[0];
        loginLessonManager.SetContextExtensionUserID(ActorName);
        var imr_statement = loginLessonManager.GetIMRStatement("Init");
        
        lrs_res = lrs.SaveStatement(imr_statement.GetStatement());

        LessonManagerInit();

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

    //public bool SendReducerStatement(string name)
    //{
    //    var imr_statement = reducer_lesson_manager.GetIMRStatement(name);
    //    lrs_res = lrs.SaveStatement(imr_statement.GetStatement());
    //    if (lrs_res.success) //Success
    //    {
    //        reducer_lesson_manager.ChangeNewStatement(name);
    //        Debug.Log("Save statement: " + lrs_res.content.id);
    //        return true;
    //    }
    //    else //Failure
    //    {
    //        Debug.Log("Statement Failed: " + lrs_res.errMsg);
    //        return false;
    //    }
    //}
    //public bool SendMotorStatement(string name)
    //{
    //    var imr_statement = motor_lesson_manager.GetIMRStatement(name);
    //    lrs_res = lrs.SaveStatement(imr_statement.GetStatement());
    //    if (lrs_res.success) //Success
    //    {
    //        motor_lesson_manager.ChangeNewStatement(name);
    //        Debug.Log("Save statement: " + lrs_res.content.id);
    //        return true;
    //    }
    //    else //Failure
    //    {
    //        Debug.Log("Statement Failed: " + lrs_res.errMsg);
    //        return false;
    //    }
    //}
    //
    // public void ReceiveStartEvent()
    // {
    //     
    //     _pure_pneumatic_controll_lesson_manager.SetStatementObject(
    //             _pure_pneumatic_controll_lesson_manager.GetIMRStatement("Init")
    //         ,CircuitPracticeUIManager.current.GetCurrentIndexNumber());
    //     
    //     _pure_pneumatic_controll_lesson_manager.SetCompletionStatementContext(
    //             _pure_pneumatic_controll_lesson_manager.GetIMRStatement("Init")
    //         , CircuitPracticeUIManager.current._courseName);
    //
    //     SendStatement("Init");
    //
    // }
    //
    // public void ReceiveHintEvent()
    // {
    //     _pure_pneumatic_controll_lesson_manager.SetStatementObject(
    //         _pure_pneumatic_controll_lesson_manager.GetIMRStatement("Hint")
    //         ,CircuitPracticeUIManager.current.GetCurrentIndexNumber());
    //     
    //     _pure_pneumatic_controll_lesson_manager.SetCompletionStatementContext(
    //         _pure_pneumatic_controll_lesson_manager.GetIMRStatement("Hint"),
    //         CircuitPracticeUIManager.current._courseName);
    //     
    //     _pure_pneumatic_controll_lesson_manager.SetHintStatementElements(
    //         CircuitPracticeUIManager.current._tutorModeCnt);
    //     
    //     SendStatement("Hint");
    // }
    //
    // public void ReceiveCompEvent()
    // {
    //     _pure_pneumatic_controll_lesson_manager.SetStatementObject(
    //         _pure_pneumatic_controll_lesson_manager.GetIMRStatement("Comp")
    //         ,CircuitPracticeUIManager.current.GetCurrentIndexNumber());
    //     
    //     _pure_pneumatic_controll_lesson_manager.SetCompletionStatementContext(
    //         _pure_pneumatic_controll_lesson_manager.GetIMRStatement("Comp"),
    //         CircuitPracticeUIManager.current._courseName);
    //     
    //     _pure_pneumatic_controll_lesson_manager.SetCompletionStatementResultExtentions(
    //         CircuitPracticeUIManager.current.isSuccess,
    //         CircuitPracticeUIManager.current.time.ToString());
    //     
    //     SendStatement("Comp");
    //
    // }
    //
    // public void ReceiveResultEvent()
    // {
    //     _pure_pneumatic_controll_lesson_manager.SetStatementObject(
    //         _pure_pneumatic_controll_lesson_manager.GetIMRStatement("Result")
    //         ,CircuitPracticeUIManager.current.GetCurrentIndexNumber());
    //     
    //     _pure_pneumatic_controll_lesson_manager.SetCompletionStatementContext(
    //         _pure_pneumatic_controll_lesson_manager.GetIMRStatement("Result"),
    //         CircuitPracticeUIManager.current._courseName);
    //
    //     _pure_pneumatic_controll_lesson_manager.SetResultStatementExtention(
    //         CircuitPracticeUIManager.current.isSuccess);
    //     
    //     SendStatement("Result");
    //
    // }
}
