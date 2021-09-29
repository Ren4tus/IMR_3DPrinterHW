using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH3LaserWindow : EvaluationInteractiveObject
{
    public bool IsEquip = true;
    public EvaluationCH3LaserWindowLens Lens;
    private Animator _animator;

    private void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
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

            StartCoroutine(Separate());
            return;
        }
    }

    public void EquipToPrinter()
    {
        StopAllCoroutines();
        StartCoroutine(Equip());
    }

    IEnumerator Equip()
    {
        IsEquip = true;
        _animator.Play("EquipChamber");

        yield return new WaitForSeconds(1.1f);

        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("레이저윈도우 장착이 완료되었습니다.", CL_MessagePopUpController.DialogType.CAUTION);
    }

    IEnumerator Separate()
    {
        IsEquip = false;
        _animator.Play("Remove");

        yield return new WaitForSeconds(1.1f);

        _animator.Play("PutOnTable");
        IsInteractive = false;
        Lens.IsInteractive = true;
    }
}
