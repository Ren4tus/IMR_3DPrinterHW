
using IMR;
using Newtonsoft.Json.Linq;

public class ChoiceStatement : IMRStatement
{
    public ChoiceStatement() => Init();
    public sealed override void Init()
    {
        base.Init();
        SetActor();
        SetVerb("performed");
        SetActivity("selection");
        var extensions = new JObject();
        var content = new JObject();
        content["content"] = "3DPrinterHWSetting";
        extensions.Add("https://www.koreatech.ac.kr/extension/context",content);
        SetContextExtensions(extensions);
    }

    public void SetSuccess(bool b)
    {
        _result.success = b;
    }
    public void SetScore(double i)
    {
        _result.score.raw = i;

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
