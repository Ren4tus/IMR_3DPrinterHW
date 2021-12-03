using System.Collections;
using System.Collections.Generic;
using IMR;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class TimeLimiteStatement : IMRStatement
{
    public TimeLimiteStatement() => Init();
    public sealed override void Init()
    {
        base.Init();
        SetActor();
        SetVerb("performed");
        SetActivity("3DPrinterHWSetting/limit-time");
        SetResultSuccess(true);
        var extensions = new JObject();
        var content = new JObject {{"content", "3DPrinterHWSetting" } };
        extensions.Add("https://www.koreatech.ac.kr/extension/context",content);
        SetContextExtensions(extensions);
    }

    public void SetResultSuccess(bool b)
    {
        _result.success = b;
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
}
