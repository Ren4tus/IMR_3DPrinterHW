using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH3ChamberLightSwitch : EvaluationInteractiveObject
{
    public override void OnMouseDown()
    {
        EvaluationSceneController.Instance.FollowGuideController.Hide();

        if (IsPointerOverUIObject())
            return;

        EvaluationCH3Controller.Instance.ChamberLightToggle();
    }
}
