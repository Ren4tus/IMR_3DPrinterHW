
using IMR;
using Newtonsoft.Json.Linq;

public class InitalizeStatement : IMRStatement
{
    public InitalizeStatement() => Init();
    public sealed override void Init()
    {
        base.Init();
        SetActor();
        SetVerb("initialized");
        SetActivity("3DPrinterHWSetting/");

        //추가 10-6 버전 추가.
        var extensions = new JObject();
        var content = new JObject();
        var version = new JObject();
        content["content"] = "3DPrinterHWSetting";
        version["version"] = "temp";
        extensions.Add("https://www.koreatech.ac.kr/extension/context",content);
        SetContextExtensions(extensions);
    }
    //추가 10-6 레슨 추가 함수
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

            UnityEngine.Debug.Log("SetContextExtensionLesson: save lesson "+ lessonName);
        }

    }
}
