using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH2BtnAdvancedStart : EvaluationInteractiveUI
{
    public AnimatedUI AdvancedUI;
    public AnimatedUI Advanced_ELUI;

    public override void CompleteStep()
    {
        base.CompleteStep();

        AdvancedUI.FadeOut(0.5f);
        Advanced_ELUI.FadeOut(0.5f);
    }
}