using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using HighlightingSystem;
using UnityEngine.UI;

public class HoverHighlightController : MonoBehaviour
{
    public bool IsInteractive = true;
    public string ObjectName;

    private Highlighter _highlighter = null;
    public PracticeToolTipController ToolTipController;

    private void Awake()
    {
        if (_highlighter == null)
            _highlighter = GetComponent<Highlighter>();
    }
    
    public void OnMouseEnter()
    {
        if (IsPointerOverUIObject())
            return;

        if (!IsInteractive)
            return;

        _highlighter.ConstantOn();
        ToolTipController.SetContentsByName(ObjectName);
        ToolTipController.SetPanelMousePosition();
        ToolTipController.Show();
    }
    public void OnMouseOver()
    {
        ToolTipController.SetPanelMousePosition();
    }

    public void OnMouseExit()
    {
        if (!IsInteractive)
            return;

        _highlighter.Off();
        ToolTipController.Hide();
    }

    public void HighlighterInit()
    {
        _highlighter.Off();
        ToolTipController.Hide();
    }

    // 오브젝트를 클릭할때 UI 레이어를 함께 클릭했는지 검사하는 함수
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer.Equals(LayerMask.NameToLayer("UI")))
                return true;
        }

        return false;
    }
}
