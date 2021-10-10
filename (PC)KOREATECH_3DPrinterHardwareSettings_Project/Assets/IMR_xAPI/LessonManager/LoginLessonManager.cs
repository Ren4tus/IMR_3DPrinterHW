using System.Collections.Generic;
using IMR;
using Newtonsoft.Json.Linq;
using TinCan;
using UnityEngine;
//TODO: 나머지 요소들 채워넣기.
public class LoginLessonManager : Lesson
{
    private LoginStatement _login_statement;

    public LoginLessonManager() => Init();
    public sealed override void Init()
    {
        Debug.Log("LoginLessonManager init");

        statement_dictionary = new Dictionary<string, IMRStatement>();

        _login_statement = new LoginStatement();


        SetObject();

        statement_dictionary.Add("Init", _login_statement);

        
    }

    public void SetObject()
    {
        _login_statement.SetActivity("3DPrinterHWSetting/login");

    }
    public override void ChangeNewStatement(string name)
    {
        switch (name)
        {
            case "Init":
                _login_statement = new LoginStatement();
                statement_dictionary["Init"] = _login_statement;
                SetObject();

                break;

        }
    }
   
    public void SetContextExtensionUserID(string userID)
    {
        _login_statement._SetContextExtensionUserID(userID);

    }
}

