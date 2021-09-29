using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1BuildTray : EvaluationInteractiveObject
{
    public Transform Hand;
    public Animator HandAnimator;
    public GameObject CleanHand;
    public GameObject GloveHand;

    public override void OnMouseDown()
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();

        if (!EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence))
            return;

        int parentidx = EvaluationSceneController.Instance.SequenceConatiner._sequenceList[TargetSequence].scoreItems[TargetStep].ParentIndex;

        if (!EvaluationSceneController.Instance.SequenceConatiner._sequenceList[TargetSequence].scoreItems[parentidx].IsFinished)
        {
            IsInteractive = false;
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("먼저 조형물을 회수해 주세요.", CL_MessagePopUpController.DialogType.CAUTION);
            return;
        }
        
        if (EvaluationSceneController.Instance.IsEquip("NitrilGloves"))
        {
            CleanHand.SetActive(false);
            GloveHand.SetActive(true);
        }
        else
        {
            CleanHand.SetActive(true);
            GloveHand.SetActive(false);

            // 안전 작업 실패
            EvaluationSceneController.Instance.IsPassSafety = false;
        }

        Hand.gameObject.SetActive(true);
        HandAnimator.Play("CleanBuildTray");
        StartCoroutine(AnimationOff(2.5f));

        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
    }

    IEnumerator AnimationOff(float time)
    {
        yield return new WaitForSeconds(time);
        Hand.gameObject.SetActive(false);
    }
}
