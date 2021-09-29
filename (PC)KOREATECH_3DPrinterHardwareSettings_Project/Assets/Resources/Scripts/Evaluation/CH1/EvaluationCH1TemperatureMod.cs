using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationCH1TemperatureMod : EvaluationInteractiveUI
{
    public Image ImageUI;
    public Text TextUI;

    public override void CompleteStep()
    {
        ImageUI.color = new Color32(172, 219, 185, 255);
        TextUI.text = "72";

        base.CompleteStep();
    }
}
