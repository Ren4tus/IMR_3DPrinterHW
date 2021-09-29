using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationTimerPanel : TweenableCanvas
{
    private Animator _animator;
    private Button _button;

    public Text Timertxt;

    public bool IsOpen = false;
    public bool IsEvaluationStart = false;

    private void Awake()
    {
        base.Awake();

        if (_animator == null)
            _animator = GetComponent<Animator>();

        if (_button == null)
            _button = GetComponent<Button>();

        IsOpen = false;
    }

    private void Start()
    {
        FadeOut(0.5f);
    }

    public void SetTimerText(string text)
    {
        Timertxt.text = text;
    }

    public void PanelOpen()
    {
        IsOpen = true;
        _animator.SetTrigger("TimerPanelOpen");

    }
    public void PanelClose()
    {
        IsOpen = false;
        _animator.SetTrigger("TimerPanelClose");
    }
    public void PanelOpenCloseToggle()
    {
        if (IsOpen)
        {
            PanelClose();
        }
        else
        {
            PanelOpen();
        }
    }
}
