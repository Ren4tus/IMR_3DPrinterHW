using UnityEngine;

public class EvaluationCH2BuildPlatformOut : EvaluationInteractiveObject
{
    public Animation _animation;
    public EvaluationCH2BuildPlatformToolTable NextStep;

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

        OutBuildPlatform();
        
        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
    }
    
    public void OutBuildPlatform()
    {
        _animation.Play("BuildPlatformOut");
        Invoke("NextStepActive", 0.4f);
    }

    public void NextStepActive()
    {
        NextStep.IsInteractive = true;
    }
}
