using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH1PrinterStatusClose : EvaluationInteractiveUI
{
    public AnimatedUI PrinerInterface;

    public override void OnPointerClick(PointerEventData eventData)
    {
        PrinerInterface.FadeOut();

        base.OnPointerClick(eventData);
    }
}
