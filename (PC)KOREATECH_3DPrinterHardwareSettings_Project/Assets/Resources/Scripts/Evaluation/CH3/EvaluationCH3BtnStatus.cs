using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH3BtnStatus : EvaluationInteractiveUI
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        CompleteStep();
        EvaluationCH3Controller.Instance.StatusPanelToggle();
    }
}
