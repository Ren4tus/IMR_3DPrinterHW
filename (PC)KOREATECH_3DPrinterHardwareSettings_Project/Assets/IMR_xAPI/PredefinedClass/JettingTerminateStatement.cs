using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMR;
using Newtonsoft.Json.Linq;
public class JettingTerminateStatement : IMRStatement
{


    public JettingTerminateStatement() => Init();
    public sealed override void Init()
    {
        base.Init();

        SetActor();
        SetVerb("terminated");
        SetActivity("3DPrinterHWSetting/");

        //추가 10-6 버전 추가.
        var extensions = new JObject();
        var content = new JObject();
        var version = new JObject();
        content["content"] = "3DPrinterHWSetting";
        version["version"] = "temp";
        extensions.Add("https://www.koreatech.ac.kr/extension/context", content);
        SetContextExtensions(extensions);
    }
    public void SetResultExtensionFromResultStatements(List<Dictionary<string, int>> resultStatements)
    {
        JObject resultExtension = new JObject();
        List<JObject> results = new List<JObject>();
        JObject tempObject = new JObject();
        for (int i = 0; i < resultStatements.Count; i++)
        {

            foreach (KeyValuePair<string, int> element in resultStatements[i])
            {
                JObject tempProperty = new JObject();
                JProperty item = new JProperty("evaluation-item", element.Key);
                JProperty score = new JProperty("evaluation-score", element.Value);

                tempProperty = new JObject(item,score);
                results.Add(tempProperty);

            }           
            tempObject.Add("result"+(i + 1).ToString(), results[i]);


        }
        resultExtension.Add("https://www.koreatech.ac.kr/extension/final-result", tempObject);

        SetResultExtensions(resultExtension);
    }
    public void SetContextExtensionLesson(string lessonName)
    {
        JObject extensionsJO = _context.extensions.ToJObject();

        if (extensionsJO.ContainsKey("lesson"))
        {
            UnityEngine.Debug.Log("statement already have lesson key");
            return;
        }
        else
        {
            
            JObject lesson = new JObject();
            lesson["lesson"] = lessonName;
            extensionsJO.Add(lesson);

            _context.extensions = new TinCan.Extensions(extensionsJO);

            UnityEngine.Debug.Log("SetContextExtensionLesson: save lesson " + lessonName);
        }

    }
}
