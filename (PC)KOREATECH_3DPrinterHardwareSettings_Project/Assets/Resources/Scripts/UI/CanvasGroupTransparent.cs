using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupTransparent : MonoBehaviour
{
    public bool isUseToolTip = true;

    private bool isTransparent = true;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        SetAlpha();
    }

    private void SetAlpha()
    {
        canvasGroup.alpha = (isTransparent) ? 0 : 1;
    }

    public void UseTooltip()
    {
        isUseToolTip = true;
    }
    public void DontUseTooltip()
    {
        isUseToolTip = false;
        Hide();
    }

    public void Show()
    {
        if (!isUseToolTip)
            return;

        isTransparent = false;
        SetAlpha();
    }
    public void Hide()
    {
        isTransparent = true;
        SetAlpha();
    }
}
