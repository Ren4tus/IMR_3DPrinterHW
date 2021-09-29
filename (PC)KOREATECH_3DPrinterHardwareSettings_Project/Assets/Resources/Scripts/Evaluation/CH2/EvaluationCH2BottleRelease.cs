using UnityEngine;

public class EvaluationCH2BottleRelease : EvaluationInteractiveObject
{
    public Animation _animation;

    public bool isHolderOut = false;

    private void Awake()
    {
        base.Awake();
    }

    public override void OnMouseDown()
    {
        if (!IsInteractive)
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();

        HolderToggle();
    }

    public void HolderToggle()
    {
        if(isHolderOut)
        {
            isHolderOut = false;
            _animation.Play("MaterialHolderIn");
        }
        else
        {
            isHolderOut = true;
            _animation.Play("MaterialHolderOut");
        }
    }
}
