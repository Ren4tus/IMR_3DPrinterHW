
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
        SetVerb("login");
        SetActivity("3DPrinterHWSetting/login");

       
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
