using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationCH2BtnLogin : EvaluationInteractiveUI
{
    public AnimatedUI SequenceStatusCheck;

    public override void OnPointerClick(PointerEventData eventData)
    {
        SequenceStatusCheck.FadeIn();

        base.OnPointerClick(eventData);
    }
}