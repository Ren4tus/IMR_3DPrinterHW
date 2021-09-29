using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH3BtnSlicerExecute : EvaluationInteractiveUI
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        EvaluationCH3Controller.Instance.SlicerPanelToggle();
    }
}
