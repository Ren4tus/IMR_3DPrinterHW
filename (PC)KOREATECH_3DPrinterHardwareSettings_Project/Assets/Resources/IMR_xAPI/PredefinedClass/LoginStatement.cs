
using IMR;
using Newtonsoft.Json.Linq;

public class LoginStatement : IMRStatement
{
    public LoginStatement() => Init();
    public sealed override void Init()
    {
        UnityEngine.Debug.Log("LoginStatement INIT");
        base.Init();
        SetActor();
        SetVerb("logged-in");
        SetActivity("3DPrinterHWSetting/login");

        //추가 10-6 버전 추가.
        var extensions = new JObject();
        var content = new JObject();
        //var version = new JObject();
        content["content"] = "3DPrinterHWSetting";
        //version["version"] = "temp";
        extensions.Add("https://www.koreatech.ac.kr/extension/context", content);
        SetContextExtensions(extensions);
    }
    public void _SetContextExtensionUserID(string userID)
    {
        var extensions = new JObject();
        var ID = new JObject();
        ID["ID"] = userID;
        extensions.Add("https://www.koreatech.ac.kr/extension/context", ID);
        SetContextExtensions(extensions);
    }

}
