using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

public class MJP_OtherHoverHighlighter : MonoBehaviour
{
    public Highlighter Highlighter;
    
    public void OnMouseEnter()
    {
        Highlighter.ConstantOn();
    }

    public void OnMouseExit()
    {
        Highlighter.Off();
    }
}
