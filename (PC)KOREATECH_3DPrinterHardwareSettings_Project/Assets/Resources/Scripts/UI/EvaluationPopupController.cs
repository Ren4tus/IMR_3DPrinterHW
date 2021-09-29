using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationPopupController : AnimatedUI
{
    public void Show(float time = 0.5f)
    {
        FadeIn(time);
    }

    public void Hide(float time = 0.5f)
    {
        FadeOut(time);
    }
}
