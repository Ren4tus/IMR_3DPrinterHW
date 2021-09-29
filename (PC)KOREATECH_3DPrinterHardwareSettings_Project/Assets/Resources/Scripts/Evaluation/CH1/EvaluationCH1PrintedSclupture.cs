using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1PrintedSclupture : EvaluationInteractiveObject
{
    /*
    public void OnMouseDown()
    {
        base.OnMouseDown();

        transform.gameObject.SetActive(false);
        BuildTray.IsInteractive = true;
    }
    */

    public override void OnMouseDown()
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();

        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("이동용 트레이를 이용하여 조형물을 회수해 주세요.", CL_MessagePopUpController.DialogType.NOTICE, null);
    }
}
