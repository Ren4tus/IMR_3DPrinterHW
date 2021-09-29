using UnityEngine;

public class EvaluationCH2MaterialBottle : EvaluationInteractiveObject
{
    private Animation _animation;
    public EvaluationCH2BottleRelease BottleRelease;

    public bool isBottleOut = false;

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

        if(BottleRelease.isHolderOut)
        {
            MaterialBottleOut();
            Invoke("InActiveBottle", 0.5f);
        }

        if (EvaluationSceneController.Instance.isSkip)
        {
            MaterialBottleOut();
            Invoke("InActiveBottle", 0.5f);
        }
    }

    public void MaterialBottleOut()
    {
        isBottleOut = true;
        _animation.Play("MaterialBottleOut");
    }

    public void InActiveBottle()
    {
        this.gameObject.SetActive(false);
    }
}
