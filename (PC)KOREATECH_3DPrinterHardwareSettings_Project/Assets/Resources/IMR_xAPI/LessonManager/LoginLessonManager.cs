using IMR;
using System.Collections.Generic;
using UnityEngine;

public class LoginLessonManager : Lesson
{
    private LoginStatement _login_statement;

    public LoginLessonManager() => Init();

    public override sealed void Init()
    {
        Debug.Log("LoginLessonManager init");

        statement_dictionary = new Dictionary<string, IMRStatement>();

        _login_statement = new LoginStatement();

        SetObject();

        statement_dictionary.Add("Init", _login_statement);
    }

    public void SetObject()
    {
        _login_statement.SetActivity("3DPrinterHWSetting/logged-in");
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