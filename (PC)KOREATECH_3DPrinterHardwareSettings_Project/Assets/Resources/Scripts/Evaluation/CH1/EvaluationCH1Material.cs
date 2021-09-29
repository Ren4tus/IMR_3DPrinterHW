using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1Material : EvaluationInteractiveObject
{
    public int MaterialIndex = 0;

    public override void OnMouseDown()
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();
        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);

        EvaluationCH1AniamtionController.Instance.MaterialOut(MaterialIndex);
        StartCoroutine(AnimationOff(3f));
    }

    IEnumerator AnimationOff(float time)
    {
        yield return new WaitForSeconds(time);
        EvaluationCH1AniamtionController.Instance.MaterialIn(MaterialIndex);
    }
}
