using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingPanelController : MonoBehaviour
{
    // 씬 이동
    public void LoadScene(string sceneName)
    {
        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("지식 학습이 마무리되었습니다.\n구조로 이동하시겠습니까?",
                                                                    CL_MessagePopUpController.DialogType.NOTICE,
                                                                    () => CL_CommonFunctionManager.Instance.LoadScene(sceneName),
                                                                    null);
    }

    // 씬 이동 with parameter
    public void LoadTrain2SceneWithParameter(int param)
    {
        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("지식 학습이 마무리되었습니다.\n구조로 이동하시겠습니까?",
                                                                    CL_MessagePopUpController.DialogType.NOTICE,
                                                                    () => CL_CommonFunctionManager.Instance.LoadTrain2SceneWithParameter(param),
                                                                    null);        
    }
}
