using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH1BtnToggle : EvaluationInteractiveUI
{
    public bool IsDefaultTemp = true;

    public AnimatedUI PrinerInterfaceTemp;
    public AnimatedUI PrinerInterfaceMaterial;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (IsDefaultTemp)
        {
            PrinerInterfaceTemp.FadeOut(0f);
            PrinerInterfaceMaterial.FadeIn(0.3f);
        }
        else
        {
            PrinerInterfaceTemp.FadeIn(0.3f);
            PrinerInterfaceMaterial.FadeOut(0f);
        }

        base.OnPointerClick(eventData);
    }
}
