using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH3BtnRecoater : EvaluationInteractiveUI
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsInteractive)
            return;

        EvaluationCH3Controller.Instance.RecoaterToOrigin();
    }
}
