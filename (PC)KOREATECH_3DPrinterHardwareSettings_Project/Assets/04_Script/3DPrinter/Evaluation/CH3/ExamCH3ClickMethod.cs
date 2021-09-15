using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using HighlightingSystem;

public class ExamCH3ClickMethod : MonoBehaviour
{
    private void OnMouseDown()
    {

    }

    private void InActiveGameObject()
    {
        this.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (!IsOverUI())
            this.GetComponent<Highlighter>().tween = true;
    }

    private void OnMouseExit()
    {
        this.GetComponent<Highlighter>().tween = false;
    }

    public bool IsOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
