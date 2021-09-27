using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LoginMemberInfo
{
    public string date_of_birth;
    public string email;
    public string gender_code;
    public string gender_name;
    public string id;
    public string idx;
    public string job;
    public string job_code;
    public string mobile_phone_number;
    public string name;
    public string receive_email;
    public string receive_text_message;
    public string role_code;
    public string role_name;
    public string status_code;
    public string status_name;
    public string user_properties;
}

[Serializable]
public class ResMemberInfo
{
    public string code;
    public LoginMemberInfo body;
    public string message;
}
