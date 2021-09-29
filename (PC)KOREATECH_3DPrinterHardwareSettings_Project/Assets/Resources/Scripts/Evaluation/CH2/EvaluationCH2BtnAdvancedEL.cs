using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH2BtnAdvancedEL : EvaluationInteractiveUI
{
    public AnimatedUI AdvancedELUI;

    public override void OnPointerClick(PointerEventData eventData)
    {
        AdvancedELUI.FadeIn(0.5f);

        base.OnPointerClick(eventData);
    }
}