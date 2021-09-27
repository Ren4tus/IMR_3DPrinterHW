using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ResEmail
{
    public string email;
}

[Serializable]
public class ResFindPassword 
{

    public string code;
    public ResEmail body;
    public string message;

}
