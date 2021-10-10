
using System.Collections.Generic;
using IMR;
using Newtonsoft.Json.Linq;
using TinCan;
//TODO : 나머지 스테이트 처리
public class PrinterStateControlManager : Lesson
{
    private InitalizeStatement _init_statement; //시작
    //private HintStatement _hint_statement; //힌트
    //private CompletionStatement _comp_statement; //완성
    //private ResultStatement _result_statement; //완성

    public PrinterStateControlManager() => Init();
    public sealed override void Init()
    {
        statement_dictionary = new Dictionary<string, IMRStatement>();

        _init_statement = new InitalizeStatement();
        //_hint_statement = new HintStatement();
        //_comp_statement = new CompletionStatement();
        //_result_statement = new ResultStatement();

        statement_dictionary.Add("Init", _init_statement);
        //statement_dictionary.Add("Hint", _hint_statement);
        //statement_dictionary.Add("Comp", _comp_statement);
        //statement_dictionary.Add("Result", _result_statement);
    }

    public override void ChangeNewStatement(string name)
    {
        switch (name)
        {
            case "Init":
                _init_statement = new InitalizeStatement();
                statement_dictionary["Init"] = _init_statement;
                break;
            //case "Hint":
            //    _hint_statement = new HintStatement();
            //    statement_dictionary["Hint"] = _hint_statement;
            //    break;
            //case "Comp":
            //    _comp_statement = new CompletionStatement();
            //    statement_dictionary["Comp"] = _comp_statement;
            //    break;
            //case "Result":
            //    _result_statement = new ResultStatement();
            //    statement_dictionary["Result"] = _result_statement;
            //    break;
        }
    }
    //public void SetHintStatementElements(int views)
    //{
    //    var super_job = new JObject();
    //    var job = new JObject();
    //    job.Add("count", views);
    //    super_job.Add("https://www.koreatech.ac.kr/extension/hint-count", job);
    //    _hint_statement.SetResultExtensions(super_job);
    //}
    //번호 수정해야됨 힌트
    public void SetStatementObject(IMRStatement statement, int num, string obj)
    {
        statement.SetActivity(string.Format("lesson/connect-evaluation-{0}/{1}", num + 1, obj));
    }
    public void SetHintStatementObject(IMRStatement statement, int num)
    {
        statement.SetActivity(string.Format("lesson/connect-evaluation-{0}/hint", num + 1));
    }
    //lesson이름
    public void SetCompletionStatementContext(IMRStatement statement, string lesson_name)
    {
        var super_job = new JObject();
        var job = new JObject();
        job.Add("content", "Pure Pneumatic Control");
        job.Add("lesson", lesson_name);
        super_job["https://www.koreatech.ac.kr/extension/context"] = job;
        statement.SetContextExtensions(super_job);
    }

    //public void SetCompletionStatementResultExtentions(bool complete, bool time_limit, string require_time)
    //{
    //    var job = new JObject();
    //    var super_job = new JObject();
    //    job["circuitComplete"] = complete;
    //    job["timeLimit"] = time_limit;
    //    job["requiredTime"] = require_time;
    //    super_job["https://www.koreatech.ac.kr/extension/completion"] = job;
    //    _comp_statement.SetResultExtensions(super_job);
    //}
    //public void SetResultStatementExtention(bool b)
    //{
    //    _result_statement.SetCompletion(b);
    //}
}

