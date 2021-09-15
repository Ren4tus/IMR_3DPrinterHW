using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCLResolution : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        CL_CommonFunctionManager.Instance.SetResolution();
    }
}
