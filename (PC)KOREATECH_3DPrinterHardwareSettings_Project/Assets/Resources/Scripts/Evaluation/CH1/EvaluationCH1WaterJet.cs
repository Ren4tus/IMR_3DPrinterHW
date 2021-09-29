using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1WaterJet : EvaluationInteractiveObject
{
    public Transform Hand;

    public override void OnMouseDown()
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();
        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);

        Hand.gameObject.SetActive(true);
        StartCoroutine(AnimationOff(2f));
    }
    
    IEnumerator AnimationOff(float time)
    {
        yield return new WaitForSeconds(time);
        Hand.gameObject.SetActive(false);
    }
}
