using UnityEngine;

public class EvaluationCH2PowerSwitch : EvaluationInteractiveObject
{
    private Animation _animation;

    public GameObject GUI_ClickCollier;
    public GameObject GUI_Login;

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

        PowerSwitchOn();

        GUI_ClickCollier.SetActive(true);
        GUI_Login.SetActive(true);

        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
    }

    public void PowerSwitchOn()
    {
        _animation.Play("PowerSwitch");
    }
}
