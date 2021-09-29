using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH3ChamberInDoor : EvaluationInteractiveObject
{
    public override void OnMouseDown()
    {
        EvaluationSceneController.Instance.FollowGuideController.Hide();

        if (!IsInteractive || IsPointerOverUIObject())
            return;

        EvaluationCH3Controller.Instance.InDoorToggle();
    }
}
