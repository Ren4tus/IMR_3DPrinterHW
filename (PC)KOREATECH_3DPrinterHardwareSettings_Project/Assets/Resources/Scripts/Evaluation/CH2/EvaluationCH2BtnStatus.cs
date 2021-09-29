using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH2BtnStatus : EvaluationInteractiveUI
{
    public AnimatedUI AdvancedUI;
    public AnimatedUI Advanced_ELUI;

    public override void OnPointerClick(PointerEventData eventData)
    {
        AdvancedUI.FadeOut(0.5f);
        Advanced_ELUI.FadeOut(0.5f);

        base.OnPointerClick(eventData);
    }
}