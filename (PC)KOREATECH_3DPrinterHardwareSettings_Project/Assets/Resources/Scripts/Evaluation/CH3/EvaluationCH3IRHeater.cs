using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH3IRHeater : EvaluationInteractiveObject
{
    public override void OnMouseDown()
    {
        if (!IsInteractive || IsPointerOverUIObject())
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();
        EvaluationCH3Controller.Instance.HeaterToggle();
    }
}