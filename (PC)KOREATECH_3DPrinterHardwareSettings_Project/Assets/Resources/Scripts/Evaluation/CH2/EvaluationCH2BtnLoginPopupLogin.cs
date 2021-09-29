using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH2BtnLoginPopupLogin : EvaluationInteractiveUI
{
    public AnimatedUI CurrentUI;
    public AnimatedUI PrinterStatus;

    public override void OnPointerClick(PointerEventData eventData)
    {



        CurrentUI.FadeOut();

        base.OnPointerClick(eventData);
    }
}