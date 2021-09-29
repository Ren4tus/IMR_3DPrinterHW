using UnityEngine;

public class EvaluationCH2MaterialCart : EvaluationInteractiveObject
{
    public bool isEquip = false;
    public EvaluationCH2UpperBuildChamberDoor[] ChamberDoors;
    private Animation _animation;
    private bool AllOpen = true;

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

        AllOpen = true;

        for (int i=0; i<ChamberDoors.Length; i++)
        {
            if (!ChamberDoors[i].isOpen)
            {
                AllOpen = false;
                break;
            }
        }

        if (AllOpen)
        {
            EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
            CartEquip();
            return;
        }

        if(EvaluationSceneController.Instance.isSkip)
        {
            EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
            CartEquip();
        }
    }

    public void CartEquip()
    {
        isEquip = true;
        _animation.Play("MaterialCartIn");
    }
}
