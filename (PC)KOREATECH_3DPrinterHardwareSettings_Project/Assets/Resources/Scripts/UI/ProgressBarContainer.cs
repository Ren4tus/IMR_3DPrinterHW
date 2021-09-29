using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProgressBarContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline outline;
    public string stepName;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void SetString(string str)
    {
        stepName = str;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StepInfoController.Instance.DisplayStepName(stepName);
        outline.enabled = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StepInfoController.Instance.HideStepName();
        outline.enabled = false;
    }
}
