using UnityEngine;

public class EvaluationCH2UVOven : EvaluationInteractiveObject
{

    private void Awake()
    {
        base.Awake();

        // _animation = GetComponent<Animation>();
    }

    public override void OnMouseDown()
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();

        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
    }
}