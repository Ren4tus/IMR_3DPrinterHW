using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopbarAS : MonoBehaviour
{
    public void PanelOpenSound()
    {
        //CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.TopbarDown);
    }

    public void InActive()
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    public void Active()
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }
}
