using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ResSequence
{
    public string sah_seq;
}

[Serializable]
public class ResRegisterUserLoginInfo
{

    public string code;
    public string message;
    public ResSequence body;
   
}
