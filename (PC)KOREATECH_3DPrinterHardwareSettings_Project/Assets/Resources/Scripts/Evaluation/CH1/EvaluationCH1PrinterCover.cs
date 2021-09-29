using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1PrinterCover : EvaluationInteractiveObject
{
    public override void OnMouseDown()
    {
        if (!IsInteractive)
            return;
        
        EvaluationCH1AniamtionController.Instance.CoverToggle();
    }
}
