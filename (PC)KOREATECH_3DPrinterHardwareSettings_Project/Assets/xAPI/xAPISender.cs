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

    public int hint_count = 0;

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
        
        //lrs_res = lrs.SaveStatement(imr_statement.GetStatement());
    }
    public void setActorName(string actorName)
    {
        string[] splitText = actorName.Split('@');
        actor_name = splitText[0];
    }
    public void SetLesson(string Lesson)
    {
        hint_count = 0;
        switch (Lesson)
        {
            case "EvaluationCH1":
                NowLesson = "Material-Jetting";
                SendIMRStatement("initialized", "");
                break;
        }
    }
    public string DecodeSequence(int seq, int step)
    {
        if (NowLesson == "Material-Jetting")
        {
            if (seq == 0)
            {
                switch (step)
                {
                    case 0:
                        return "Connect_Computer";
                }
            }
            else if (seq == 1)
            {
                switch (step)
                {
                    case 0:
                        return "Check-PrinterState";
                }
            }
        }

        return "sequence decode error";
    }
    public void SendMessageWithTranslate(int seq, int step, JObject extension = null)
    {
        SendIMRStatement("performed", DecodeSequence(seq, step));

    }
    public void SendLoginMessageStatement(string id)
    {
        //xAPISender.instance.UserID = loginSceneControl.m_Id.text;
        JObject extension = new JObject();
        JObject sources = new JObject();
        sources.Add("id", id);
        extension.Add("https://www.koreatech.ac.kr/extension/loginInfo", sources);
        NowLesson = "login_page";
        SendIMRStatement("login", "Client", extension);
        setActorName(id);
        Debug.Log("Login Success");
    }
    public void SendHintStatement(string currentStepAction)
    {
        hint_count++;
        JObject extension = new JObject();
        JObject hintcount = new JObject();
        hintcount.Add("hint-count", hint_count);
        extension.Add("https://www.koreatech.ac.kr/extension/hint", hintcount);
        JObject evealutionStep = new JObject();
        evealutionStep.Add("evaluation-step", currentStepAction);
        extension.Add("https://www.koreatech.ac.kr/extension/evaluation-step", evealutionStep);

        SendIMRStatement("performed", "hint", extension);
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
