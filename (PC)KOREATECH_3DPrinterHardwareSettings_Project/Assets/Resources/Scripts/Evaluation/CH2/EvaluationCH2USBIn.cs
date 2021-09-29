using UnityEngine;

public class EvaluationCH2USBIn : EvaluationInteractiveObject
{
    public Animation _animation;
    public CanvasGroup GUI;
    public GameObject ClickCollider;

    private void Awake()
    {
        base.Awake();
    }

    public override void OnMouseDown()
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();

        USBIn();

        Invoke("SlicerOpen", 0.5f);
    }

    public void USBIn()
    {
        _animation.Play("USB_PrinterIn");
    }

    public void SlicerOpen()
    {
        EvaluationSceneController.Instance.SlicerController.SlicerOpen();
        GUI.blocksRaycasts = false;
        ClickCollider.SetActive(false);
    }
}
