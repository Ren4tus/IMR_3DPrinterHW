using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1BtnLink : EvaluationInteractiveUI
{
    public AnimatedUI CurrentUI;
    public AnimatedUI SequenceStatusCheck;

    public override void CompleteStep() {
        base.CompleteStep();

        CurrentUI.FadeOut(0f);
        SequenceStatusCheck.FadeIn();
    }
}
