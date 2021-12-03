using System.Collections;
using System.Collections.Generic;
using IMR;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ResultStatement : IMRStatement
{
    public ResultStatement() => Init();
    public sealed override void Init()
    {
        base.Init();
        SetActor();
        SetVerb("terminated");
        SetActivity("motor-system");
        var extensions = new JObject();
        var content = new JObject();
        content["content"] = "3DPrinterHWSetting";
        extensions.Add("https://www.koreatech.ac.kr/extension/context",content);
        SetContextExtensions(extensions);
    }

    public void SetCompletion(bool b)
    {
        _result.completion = b;
    }
}
