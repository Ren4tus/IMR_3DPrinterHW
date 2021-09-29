using UnityEngine;

public class EvaluationCH2BuildPlatformToolTable : EvaluationInteractiveObject
{
    public GameObject BuildPlatInPrinter;
    public GameObject BuildPlatToolTable;

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

        ChangeBuildPlatform();
        
        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
    }
    
    public void ChangeBuildPlatform()
    {
        BuildPlatInPrinter.SetActive(false);
        BuildPlatToolTable.SetActive(true);
    }
}
