using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMR;
using Newtonsoft.Json.Linq;
public class TerminateStatement : IMRStatement
{
    public TerminateStatement() => Init();
    public sealed override void Init()
    {
        base.Init();

        SetActor();
        SetVerb("terminated");
        SetActivity("3DPrinterHWSetting/");
        _result.score = new TinCan.Score();
        

        //추가 10-6 버전 추가.
        var extensions = new JObject();
        var content = new JObject();
        var version = new JObject();
        content["content"] = "3DPrinterHWSetting";
        version["version"] = "temp";
        extensions.Add("https://www.koreatech.ac.kr/extension/context", content);
        SetContextExtensions(extensions);
    }
    public void SetResultExtensionFromResultStatements(List<Dictionary<string, string>> resultStatements,string lessonName)
    {
        JObject resultExtension = new JObject();
        List<JObject> results = new List<JObject>();
        JObject tempObject = new JObject();
        JArray jArray = new JArray();
        
        for (int i = 0; i < resultStatements.Count; i++)
        {
            foreach (KeyValuePair<string, string> element in resultStatements[i])
            {
                JObject tempProperty = new JObject();
                JProperty item = new JProperty("evaluation-item", element.Key);
                JProperty score = new JProperty("evaluation-score", element.Value);

                tempProperty = new JObject(item,score);
                results.Add(tempProperty);

            }
            if (i < 9)
            {
                tempObject.Add("result0"+(i + 1).ToString(), results[i]);

            }
            else
            {
                tempObject.Add("result" + (i + 1).ToString(), results[i]);
            }


        }
        resultExtension.Add("https://www.koreatech.ac.kr/extension/3DPrinterHWSetting/"+lessonName+"/result", tempObject);

        SetResultExtensions(resultExtension);
    }
    //추가 10-6 레슨 추가 함수
    public void SetContextExtensionLesson(string lessonName)
    {
        var extensions = new JObject();
        JProperty content = new JProperty("content", "3DPrinterHWSetting");
        JProperty lesson = new JProperty("lesson", lessonName);

        JObject tempProperty = new JObject(content, lesson);
        extensions.Add("https://www.koreatech.ac.kr/extension/context", tempProperty);
        SetContextExtensions(extensions);


    }
    public void SetScore(double i)
    {
        _result.score.raw = i;

    }
    public void SetSuccess(bool b)
    {
        _result.completion = b;
    }

}
