
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
        SetActivity("select");
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
        TinCan.Score score = new TinCan.Score();
        score.raw = i;
        _result.score = score;

    }
}
