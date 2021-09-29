using UnityEngine;

public class EvaluationCH2BuildPlatform : EvaluationInteractiveObject
{
    public Animation _animation;


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

        EquipBuildPlatform();
        
        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
    }
    
    public void EquipBuildPlatform()
    {
        _animation.Play("BuildPlatform");
    }
}
