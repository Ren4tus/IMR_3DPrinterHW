using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH2BtnAdvanced : EvaluationInteractiveUI
{
    public AnimatedUI AdvancedUI;

    public override void OnPointerClick(PointerEventData eventData)
    {
        AdvancedUI.FadeIn(0.5f);

        base.OnPointerClick(eventData);
    }
}