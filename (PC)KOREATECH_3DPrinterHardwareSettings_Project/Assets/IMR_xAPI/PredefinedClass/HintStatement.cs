using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMR;
using Newtonsoft.Json.Linq;
public class HintStatement : IMRStatement
{
    public HintStatement() => Init();
    public sealed override void Init()
    {
        base.Init();
        SetActor();
        SetVerb("performed");
        SetActivity("hint");
        var extensions = new JObject();
        var content = new JObject();
        content["content"] = "3DPrinterHWSetting";
        extensions.Add("https://www.koreatech.ac.kr/extension/context", content);
        SetContextExtensions(extensions);
    }

    public void SetHintExtensions(string hintText, int hintCount)
    {
        JObject resultExtension = new JObject();

        JProperty hintTextProperty = new JProperty("hint-text", hintText);
        JProperty hintCountProperty = new JProperty("hint-count", hintCount);
        JObject tempJOBject = new JObject(hintTextProperty, hintCountProperty);
       
        resultExtension.Add("https://www.koreatech.ac.kr/extension/hint", tempJOBject);

        //_result.success = true ;
        SetResultExtensions(resultExtension);

        Debug.Log("SetHintExtensions, hintText : "+ hintText);
        Debug.Log("resultExtension : " + tempJOBject);
    }
}
