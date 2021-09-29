using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureTitleController : MonoBehaviour
{
    public string TitleText;
    public string SubTitleText;
    public TitleIndicatorController TitleIndicator;

    public static StructureTitleController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    public void ShowIndicator()
    {
        TitleIndicator.ShowTitleSlideIn(TitleText, SubTitleText);
    }
}