using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CL_ConfigurationFullScreenForWebGL : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void Test()
    {
        CL_CommonFunctionManager.Instance.SetFullScreen();
    }
}