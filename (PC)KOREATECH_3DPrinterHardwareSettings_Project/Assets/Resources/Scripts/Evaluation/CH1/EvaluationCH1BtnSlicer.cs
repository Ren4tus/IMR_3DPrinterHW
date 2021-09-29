using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH1BtnSlicer : EvaluationInteractiveUI
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        EvaluationSceneController.Instance.CameraControllOff();
        EvaluationSceneController.Instance.SlicerController.SlicerOpen();

        base.OnPointerClick(eventData);
    }
}