using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBarEvaluation : MonoBehaviour
{
    public static TopBarEvaluation instance;

    public string[] StepTitle;
    public Text stepText;

    private int stepCount = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void StepTitleUpdate()
    {
        stepText.text = StepTitle[stepCount];
        stepCount++;
    }
}
