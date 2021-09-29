using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH2BtnPrintStart : EvaluationInteractiveUI
{
    public AnimatedUI CurrentUI;
    public AnimatedUI PrintCompleteUI;
    public GameObject printedSculture;
    

    public override void CompleteStep()
    {
        if (!IsInteractive || TargetSequence.Equals(-1) || TargetStep.Equals(-1) ||
            !EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence) ||
            EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        base.CompleteStep();

        CurrentUI.FadeOut(0f);
        PrintCompleteUI.FadeIn();

        EvaluationSceneController.Instance.SlicerController.Printing();

        printedSculture.SetActive(true);
    }
}