using UnityEngine;

public class EvaluationCH2Resin : EvaluationInteractiveObject
{
    private Animation _animation;
    public EvaluationCH2MaterialBottle MaterialBottle;

    public Transform Bottle;

    private void Awake()
    {
        base.Awake();

        _animation = GetComponent<Animation>();
    }

    public override void OnMouseDown()
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();

        if (MaterialBottle.isBottleOut)
        {
            EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);

            this.transform.SetParent(Bottle, true);

            ResinIn();
        }

        if(EvaluationSceneController.Instance.isSkip)
        {
            EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);

            this.transform.SetParent(Bottle, true);

            ResinIn();
        }
    }

    public void ResinIn()
    {
        _animation.Play("ResinIn");
    }
}
