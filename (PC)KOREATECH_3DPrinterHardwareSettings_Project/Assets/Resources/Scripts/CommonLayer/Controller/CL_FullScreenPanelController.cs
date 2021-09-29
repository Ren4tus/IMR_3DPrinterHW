using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CL_FullScreenPanelController : MonoBehaviour
{
    private bool isFullScreen;

    void Start()
    {
        isFullScreen = Screen.fullScreen;
    }

    public void FullScreenPanelActive()
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    public void FullScreenPanelInActive()
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
