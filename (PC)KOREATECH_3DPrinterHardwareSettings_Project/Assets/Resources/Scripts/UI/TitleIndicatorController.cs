using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TitleIndicatorController : MonoBehaviour
{
    public Text TitleText;
    public Text SubtitleText;

    private Canvas _canvas;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        _canvas.enabled = false;
    }

    public void ShowTitleSlideIn(string titleStr, string subtitleStr)
    {
        SetTitle(titleStr);
        SetSubtitle(subtitleStr);

        _animator.Rebind();
        _animator.Play("DoubleSlide");
    }

    public void SetTitle(string titleStr)
    {
        TitleText.text = titleStr;
    }
    public void SetSubtitle(string subtitleStr)
    {
        SubtitleText.text = subtitleStr;
    }

    public void StartSound()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.IndicatorStart);
    }

    public void EndSound()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.IndicatorEnd);
    }
}