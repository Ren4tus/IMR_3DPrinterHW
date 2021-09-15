using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using HighlightingSystem;
using UnityEngine.UI;

public class MJP_HoverHighlighter : MonoBehaviour
{
    public string ObjectName;

    private Highlighter _highlighter = null;
    public MJP_ToolTipController ToolTipController;

    private void Awake()
    {
        if (_highlighter == null)
            _highlighter = GetComponent<Highlighter>();
    }

    public void OnMouseEnter()
    {
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
        _highlighter.Off();
        ToolTipController.Hide();
    }
}
