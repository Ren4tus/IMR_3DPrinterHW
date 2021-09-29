using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepInfoController : TweenableCanvas
{
    private static StepInfoController instance = null;

    public Text text;
    private RectTransform rectTemp = null;
    private CanvasGroup canvasGroup = null;

    // Singleton
    public static StepInfoController Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        base.Awake();

        if (instance == null)
            instance = this;

        if (rectTemp == null)
            rectTemp = GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    public void DisplayStepName(string str)
    {
        text.text = str;

        Vector2 pos = new Vector2(Input.mousePosition.x, rectTemp.position.y);
        rectTemp.position = pos;

        FadeIn(0.2f);
    }
    public void HideStepName()
    {
        FadeOut(0.2f);
    }
}
