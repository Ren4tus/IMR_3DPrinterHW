using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH3BtnPartExtract : EvaluationInteractiveUI
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        CompleteStep();
    }

    public override void CompleteStep()
    {
        EvaluationSceneController.Instance.FollowGuideController.Hide();

        if (!IsInteractive || IsPointerOverUIObject())
            return;

        if (TargetSequence.Equals(-1) || TargetStep.Equals(-1))
            return;

        if (EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        if (!EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence))
        {
            // 이전 시퀀스를 완료하지 않았을 경우
            int[] tempTarget = EvaluationSceneController.Instance.GetParentSequenceIndex(TargetSequence);

            tempStr.Clear();

            for (int i = 0; i < tempTarget.Length; i++)
            {
                tempStr.Append("<b>[");
                tempStr.Append(EvaluationSceneController.Instance.GetSequenceName(tempTarget[i]));
                tempStr.Append("]</b>");
            }

            tempStr.Append("\n작업을 먼저 완료해야 합니다.");
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp(tempStr.ToString(),
                                                                CL_MessagePopUpController.DialogType.CAUTION);
            return;
        }

        if (!EvaluationSceneController.Instance.IsTargetStepParentComplete(TargetSequence, TargetStep))
        {
            // 이전 단계를 완료하지 않았을 경우
            int parentIdx = EvaluationSceneController.Instance.GetParentStepIndex(TargetSequence, TargetStep);

            tempStr.Clear();
            tempStr.Append("<b>[");
            tempStr.Append(EvaluationSceneController.Instance.GetStepName(TargetSequence, parentIdx));
            tempStr.Append("]</b>\n작업을 먼저 완료해야 합니다.");

            CL_CommonFunctionManager.Instance.MakePopUp().PopUp(tempStr.ToString(), CL_MessagePopUpController.DialogType.CAUTION);
            return;
        }

        EvaluationCH3Controller.Instance.Extract();
    }
}
