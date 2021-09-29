using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH1BtnHeadClean : EvaluationInteractiveUI
{
    private bool isProceed = false;
    public EvaluationInteractiveObject PrinterBlock;
    public Animator PrinterBlockAnimator;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        if (isProceed)
            return;

        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("프린터 블록이 청소 위치로 이동합니다.", CL_MessagePopUpController.DialogType.NOTICE);
        PrinterBlockAnimator.Play("CleanPosition");
        PrinterBlock.IsInteractive = true;

        isProceed = true;
    }
}