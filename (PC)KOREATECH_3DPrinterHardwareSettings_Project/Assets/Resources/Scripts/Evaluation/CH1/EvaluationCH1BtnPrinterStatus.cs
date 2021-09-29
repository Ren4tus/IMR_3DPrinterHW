using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH1BtnPrinterStatus : EvaluationInteractiveUI
{
    public AnimatedUI PrinerInterface;

    public override void OnPointerClick(PointerEventData eventData)
    {
        PrinerInterface.FadeIn();

        base.OnPointerClick(eventData);
    }
}
