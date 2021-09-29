using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1PrinterBlock : EvaluationInteractiveObject
{
    public EvaluationCH1ModelMaterial1L SupplyMaterial;

    public override void OnMouseDown()
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();

        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);

        SupplyMaterial.IsInteractive = true;
    }
}
