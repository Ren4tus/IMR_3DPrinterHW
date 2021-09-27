using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Token
{
    public string access_token;
}


[Serializable]
public class Authenticate 
{
    public string code;
    public Token body;
    public string message;
}

