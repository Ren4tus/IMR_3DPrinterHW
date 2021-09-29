using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH2BtnLoginPopupClose : EvaluationInteractiveUI
{
    public AnimatedUI CurrentUI;

    public override void OnPointerClick(PointerEventData eventData)
    {
        CurrentUI.FadeOut();

        base.OnPointerClick(eventData);
    }
}