using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1MaterialCabinetCover : EvaluationInteractiveObject
{
    public void OnMouseDown()
    {
        if (!IsInteractive)
            return;

        EvaluationCH1AniamtionController.Instance.CabinetDoorToggle();
    }
}
