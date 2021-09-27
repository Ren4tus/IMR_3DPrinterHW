using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ResUserIndex
{
	public string latest_sah_seq;
}

[Serializable]
public class ResLoginConditionCheck
{
	public string code;
	public string message;
	public ResUserIndex body;
}