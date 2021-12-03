using System.Collections.Generic;
using IMR;
using Newtonsoft.Json.Linq;
using TinCan;
using UnityEngine;
using UnityEngine.UI;
//TODO: 나머지 요소들 채워넣기.
public class PowderBedFusionLessonManager : Lesson
{
    public VerticalLayoutGroup resultCanvas;
    private List<Dictionary<string, string>> result_statements;
    private string _hintText;

    //TODO: 이름 바꿔놓기 Select로
    private InitalizeStatement _init_statement;
    private ChoiceStatement _choice_statement;
    private HintStatement _hint_Statement;
    private TimeLimiteStatement _time_limite_statement;
    //private ResultStatement _result_statement;
    private TerminateStatement _jettingTerminateStatement;

    public PowderBedFusionLessonManager() => Init();
    public sealed override void Init()
    {
        lessonName = "powder-bed-fusion-3Dprinter";

        statement_dictionary = new Dictionary<string, IMRStatement>();
        result_statements = new List<Dictionary<string, string>>();

        _init_statement = new InitalizeStatement();
        _choice_statement = new ChoiceStatement();
        _hint_Statement = new HintStatement();
        _time_limite_statement = new TimeLimiteStatement();
        //_result_statement = new ResultStatement();
        _jettingTerminateStatement = new TerminateStatement();


        SetObject();

        statement_dictionary.Add("Init", _init_statement);
        statement_dictionary.Add("Choice", _choice_statement);
        statement_dictionary.Add("Hint", _hint_Statement);
        statement_dictionary.Add("Time", _time_limite_statement);
        //statement_dictionary.Add("Result", _result_statement);
        statement_dictionary.Add("Terminate", _jettingTerminateStatement);
    }

    public void SetObject()
    {
        _init_statement.SetActivity("3DPrinterHWSetting/powder-bed-fusion-3Dprinter");
        _choice_statement.SetActivity("3DPrinterHWSetting/verbs/powder-bed-fusion-3Dprinter/selection");
        _time_limite_statement.SetActivity("3DPrinterHWSetting/verbs/powder-bed-fusion-3Dprinter/limit-time");
        //_result_statement.SetActivity("3DPrinterHWSetting");
        _jettingTerminateStatement.SetActivity("3DPrinterHWSetting/powder-bed-fusion-3Dprinter");
        _hint_Statement.SetActivity("3DPrinterHWSetting/verbs/powder-bed-fusion-3Dprinter/hint");
        _jettingTerminateStatement.SetResultExtensionFromResultStatements(result_statements, lessonName);
        _hint_Statement.SetHintExtensions(_hintText, _hintCount);

        _init_statement.SetContextExtensionLesson("powder-bed-fusion-3Dprinter");
        _choice_statement.SetContextExtensionLesson("powder-bed-fusion-3Dprinter");
        _time_limite_statement.SetContextExtensionLesson("powder-bed-fusion-3Dprinter");
        _jettingTerminateStatement.SetContextExtensionLesson("powder-bed-fusion-3Dprinter");

        _jettingTerminateStatement.SetSuccess(_completion);
        _jettingTerminateStatement.SetScore(_score);
    }
    public override void ChangeNewStatement(string name)
    {
        switch (name)
        {
            case "Init":
                _init_statement = new InitalizeStatement();
                statement_dictionary["Init"] = _init_statement;
                SetObject();
                break;
            case "Choice":
                _choice_statement = new ChoiceStatement();
                statement_dictionary["Choice"] = _choice_statement;
                SetObject();
                break;
            case "Hint":
                _hint_Statement = new HintStatement();
                statement_dictionary["Hint"] = _hint_Statement;
                SetObject();
                _hint_Statement.SetHintExtensions(_hintText, _hintCount);
                break;
            case "Time":
                _time_limite_statement = new TimeLimiteStatement();
                statement_dictionary["Time"] = _time_limite_statement;
                SetObject();
                break;
            case "Terminate":
                _jettingTerminateStatement = new TerminateStatement();
                statement_dictionary["Terminate"] = _jettingTerminateStatement;
                SetObject();
                _jettingTerminateStatement.SetResultExtensionFromResultStatements(result_statements, lessonName);
                break;
        }
    }
    public override void SetEvaluationItemElement(string _item, string _step, bool sucess)
    {
        JObject resultExtension = new JObject();

        JObject tempProperty = new JObject();
        JProperty item = new JProperty("evaluation-item", _item);
        JProperty score = new JProperty("evaluation-step", _step);

        tempProperty = new JObject(item, score);

        resultExtension.Add("https://www.koreatech.ac.kr/extension/selection", tempProperty);

        _choice_statement.SetResultExtensions(resultExtension);

        _choice_statement.SetSuccess(sucess);

    }

    public override void SetHintStatementResultExtensions(string hintText)
    {
        _hintText = hintText;
    }
    public override void SetLimitStatementResult(bool b)
    {
        _time_limite_statement.SetResultSuccess(b);
    }

    public override void CloneResultCanvas(EvaluationCore.EvaluationContainer SequenceConatiner)
    {
        int sum = 0;
        bool isComplete = true;
        foreach (KeyValuePair<int, EvaluationCore.EvaluationSequence> item in SequenceConatiner._sequenceList)
        {
            int score = 0;
            for (int i = 1; i <= item.Value.TotalGainScores(); i++)
            {
                if (!item.Value.IsSkip)
                    score = i;
                else
                    score = 0;

            }
            Dictionary<string, string> newItem = new Dictionary<string, string>()
        {
            {item.Value.Name, score.ToString()}
        };
            result_statements.Add(newItem);
            sum += score;
            if (item.Value.TotalScores() != score)
            {
                isComplete = false;
            }
        }
        Dictionary<string, string> newItem2 = new Dictionary<string, string>()
        {
            {"안전하게 작업할 수 있다.", (EvaluationSceneController.Instance.IsPassSafety) ? "PASS" : "FAIL"}
        };
        result_statements.Add(newItem2);

        newItem2 = new Dictionary<string, string>()
        {
            {"시간 내 작업할 수 있다.", (EvaluationSceneController.Instance.IsCompleteInTime) ? "PASS" : "FAIL"}
        };
        result_statements.Add(newItem2);
        _score = sum;
        _completion = isComplete;
    }

}

