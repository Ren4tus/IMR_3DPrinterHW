using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH3LoadScene : MonoBehaviour
{
    public void ReturnHome()
    {
        CL_CommonFunctionManager.Instance.ReturnHome();
    }

    public void ReLoad()
    {
        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("평가를 다시 시작할까요?", 
            CL_MessagePopUpController.DialogType.NOTICE,
            () => CL_CommonFunctionManager.Instance.LoadScene("EvaluationCH3"),
        null);
    }
}
