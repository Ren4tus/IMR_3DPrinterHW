using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH1ModelMaterial1L : EvaluationInteractiveUI
{
    public EvaluationCH1Material NeedSupplyMaterial;
    public override void CompleteStep()
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();
        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);

        NeedSupplyMaterial.IsInteractive = true;
    }
}
