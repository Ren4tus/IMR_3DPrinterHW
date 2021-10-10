
//using System.Collections.Generic;
//using IMR;
//using Newtonsoft.Json.Linq;
//using TinCan;
//using UnityEngine;

//public class JetLessonManager: Lesson
//{
//    private InitalizeStatement _init_statement; //시작
//    private ChoiceStatement _choice_statement; //힌트
//    private TimeLimiteStatement _time_limite_statement; //완성
//    private ResultStatement _result_statement; //완성

//    public BatteryLessonManager() => Init();
//    public sealed override void Init()
//    {
//        statement_dictionary = new Dictionary<string, IMRStatement>();

//        _init_statement = new InitalizeStatement();
//        _choice_statement = new ChoiceStatement();
//        _time_limite_statement = new TimeLimiteStatement();
//        _result_statement = new ResultStatement();
        
//        SetObject();
        
//        statement_dictionary.Add("Init", _init_statement);
//        statement_dictionary.Add("Choice", _choice_statement);
//        statement_dictionary.Add("Time", _time_limite_statement);
//        statement_dictionary.Add("Result", _result_statement);
//    }

//    public void SetObject()
//    {
//        _init_statement.SetActivity("battery-system");
//        _choice_statement.SetActivity("battery-system/select");
//        _time_limite_statement.SetActivity("battery-system/limit-time");
//        _result_statement.SetActivity("battery-system");
//    }
//    public override void ChangeNewStatement(string name)
//    {
//        switch(name)
//        {
//            case "Init":
//                _init_statement = new InitalizeStatement();
//                statement_dictionary["Init"]= _init_statement;
//                SetObject();

//                break;
//            case "Choice":
//                _choice_statement = new ChoiceStatement();
//                statement_dictionary["Choice"]= _choice_statement;
//                SetObject();

//                break;
//            case "Time":
//                _time_limite_statement = new TimeLimiteStatement();
//                statement_dictionary["Time"]= _time_limite_statement;
//                SetObject();

//                break;
//            case "Result":
//                _result_statement = new ResultStatement();
//                statement_dictionary["Result"]= _result_statement;
//                SetObject();

//                break;
//        }
//    }
//    public void SetChoiceStatementElements(params object[] list)
//    {
//        var super_job = new JObject();
//        var job = new JObject();
//        //job.Add("field", (string)list[0]);
//        job.Add("currentStep", (int)list[1]); //controlCountb
//        job.Add("totalStep", (int)list[2]); // evaluator.mapper.Length
//        job.Add("componentName", ComponentText.current.comp_text.text); //evaluator.mapper.GetObject(controlCount)
//       //job.Add("componentName", (string)list[3]); //evaluator.mapper.GetObject(controlCount)
//        super_job.Add("https://www.koreatech.ac.kr/extension/select", job);
//        _choice_statement.SetResultExtensions(super_job);
//        _choice_statement.SetSuccess((bool)list[0]);

//    }
    
//    public void SetLimitStatementResult(bool b)
//    {
//        _time_limite_statement.SetResultSuccess(b);
//    }

//    public void SetResultStatementExtention(bool b)
//    {
//        _result_statement.SetCompletion(b);
//    }

//    public void SetChoiceStatementExtension(bool b)
//    {
//        _choice_statement.SetSuccess(b);
//    }
//}

   