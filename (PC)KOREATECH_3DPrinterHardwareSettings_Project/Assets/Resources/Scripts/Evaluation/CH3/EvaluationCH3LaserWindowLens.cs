using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH3LaserWindowLens : EvaluationInteractiveObject
{
    public bool IsEquip = true;
    public EvaluationCH3LaserWindow LaserWindow;
    public Animator _animator;
    public Animator _bafflePlugAnim;

    private void Awake()
    {
        base.Awake();
    }

    public override void OnMouseDown()
    {
        if (!IsInteractive || IsPointerOverUIObject())
            return;

        if (!EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence))
        {
            // 이전 시퀀스를 완료하지 않았을 경우
            int[] tempTarget = EvaluationSceneController.Instance.GetParentSequenceIndex(TargetSequence);

            tempStr.Clear();

            for (int i = 0; i < tempTarget.Length; i++)
            {
                tempStr.Append("[");
                tempStr.Append(EvaluationSceneController.Instance.GetSequenceName(tempTarget[i]));
                tempStr.Append("]");
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
            tempStr.Append("[");
            tempStr.Append(EvaluationSceneController.Instance.GetStepName(TargetSequence, parentIdx));
            tempStr.Append("]\n작업을 먼저 완료해야 합니다.");

            CL_CommonFunctionManager.Instance.MakePopUp().PopUp(tempStr.ToString(), CL_MessagePopUpController.DialogType.CAUTION);
            return;
        }

        if (!EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
        {
            EvaluationSceneController.Instance.FollowGuideController.Hide();
            EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);

            StartCoroutine(Split());
            return;
        }
    }

    IEnumerator Split()
    {
        _bafflePlugAnim.Play("Remove");
        yield return new WaitForSeconds(2f);

        _animator.Play("SplitCover");
        yield return new WaitForSeconds(5f);

        _animator.Play("Assembly");
        yield return new WaitForSeconds(5f);

        IsInteractive = false;
        LaserWindow.EquipToPrinter();
    }
}
