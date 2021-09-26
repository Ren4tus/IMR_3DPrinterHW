using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;
using Newtonsoft.Json.Linq;
public class xAPISender : MonoBehaviour
{
    public static xAPISender instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            if (instance != this) 
                Destroy(this.gameObject); 
        }
    }

    private Dictionary<string, bool> send_dict;

    private string _endpoint = "http://www.vt-lrs.com/data/xAPI";
    private string _key = "812073176a6f25e4ddd1cb6c80fadac19e05221e";
    private string _secret_key = "2307f2b781f07c4aaaaede4ef5ee6cfce306f2f8";
    private IMR.IMRStatement imr_statement;
    public string debug_msg = "";

    private RemoteLRS lrs;
    public StatementLRSResponse lrs_res = null;

    private string actor_name = "korea_tech";

    public string ActorName => actor_name;

    
    public string NowLesson = "null";
    public string NowActivity = "null";
    public string UserID = "null";

    EvaluationSceneController evaluationSceneController;

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
        lrs = new RemoteLRS(_endpoint, _key, _secret_key);
    }
    public void SetRemoteLRS(string s)
    {
        lrs = new RemoteLRS(s, _key, _secret_key);
    }

    public void Init()
    {
        SetRemoteLRS();
        send_dict = new Dictionary<string, bool>();
        
        //lrs_res = lrs.SaveStatement(imr_statement.GetStatement());
    }
    public void SetLesson(string Lesson)
    {
        switch (Lesson)
        {
            case "EvaluationCH1":
                NowLesson = "Material-Jetting";
                SendIMRStatement("initialized", "");
                break;
        }
    }
    
    public void SendMessageWithTranslate(int seq, int step, JObject extension = null)
    {
        if(NowLesson == "Material-Jetting")
        {
            if(seq == 0)
            {
                switch (step)
                {
                    case 0:
                        SendIMRStatement("performed", "Connect_Computer");
                        break;
                }
            }
            else if(seq == 1)
            {
                switch (step)
                {
                    case 0:
                        SendIMRStatement("performed", "Check-PrinterState");
                        break;
                }
            }
        }



    }
    public void SendIMRStatement(string verb, string activity, JObject extension = null)
    {
        imr_statement = new IMR.IMRStatement();
        imr_statement.SetActor();
        imr_statement.SetVerb(verb);
        imr_statement.SetActivity(activity);
        if(extension != null)
            imr_statement.SetContextExtensions(extension);
        lrs_res = lrs.SaveStatement(imr_statement.GetStatement());
        if (lrs_res.success) //Success
        {
            //driving_lesson_statement_manager.ChangeNewStatement(name);
            Debug.Log("Save statement: " + lrs_res.content.id);
        }
        else
        {
            Debug.Log("Statement Failed: " + lrs_res.errMsg);
        }
    }
    public bool SendDrivingStatement(string name)
    {
        Debug.Log(name + "was send");
        var imr_statement = new IMR.IMRStatement();
        lrs_res = lrs.SaveStatement(imr_statement.GetStatement());
        if (lrs_res.success) //Success
        {
            //driving_lesson_statement_manager.ChangeNewStatement(name);
            Debug.Log("Save statement: " + lrs_res.content.id);
            return true;
        }
        else //Failure
        {
            Debug.Log("Statement Failed: " + lrs_res.errMsg);
            return false;
        }
    }

    //public bool SendExcavationLessonStatement(string name)
    //{
    //    Debug.Log(name);
    //    var imr_statement = excavation_lesson_statement_manager.GetIMRStatement(name);
    //    lrs_res = lrs.SaveStatement(imr_statement.GetStatement());
    //    if (lrs_res.success) //Success
    //    {
    //        excavation_lesson_statement_manager.ChangeNewStatement(name);
    //        Debug.Log("Save Excavation statement: " + lrs_res.content.id);
    //        return true;
    //    }
    //    else //Failure
    //    {
    //        Debug.Log("Excavation Statement Failed: " + lrs_res.errMsg);
    //        return false;
    //    }
    //}
}
