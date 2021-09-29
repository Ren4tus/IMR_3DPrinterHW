using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH3ShifterSwitch : EvaluationInteractiveObject
{
    public override void OnMouseDown()
    {
        EvaluationSceneController.Instance.FollowGuideController.Hide();

        if (IsPointerOverUIObject())
            return;

        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep) )
            return;

        EvaluationCH3Controller.Instance.ShifterOn();
    }
}
