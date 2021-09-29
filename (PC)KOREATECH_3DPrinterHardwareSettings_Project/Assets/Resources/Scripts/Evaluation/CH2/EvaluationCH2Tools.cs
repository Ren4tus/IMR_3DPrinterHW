using UnityEngine;

public class EvaluationCH2Tools : EvaluationInteractiveObject
{
    public GameObject Tools;

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

        ToolsOn();

        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);

        Invoke("ToolsOff", 12f);
    }

    public void ToolsOn()
    {
        Tools.SetActive(true);
    }

    public void ToolsOff()
    {
        Tools.SetActive(false);
    }
}