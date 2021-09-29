using AnimationCoroutines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnimatedImage : MonoBehaviour
{
    public RectTransform _rectTransform;

    protected IEnumerator _transitionScale = null;
    protected IEnumerator _transitionPosition = null;
    protected IEnumerator _transitionAlpha = null;
    protected IEnumerator _transitionColor = null;

    protected Image _Image = null;

    protected Vector2 originPosition;
    protected Vector2 originSizeDelta;
    protected Vector2 originScale;

    [Header("Animation")]
    public AnimationCurve m_animaitonCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float m_duration = 0.3f;

    protected void Awake()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        if (_Image == null)
            _Image = GetComponent<Image>();

        SetOrigin();
    }

    public void SetOrigin()
    {
        originPosition = _rectTransform.anchoredPosition;
        originScale = _rectTransform.localScale;
        originSizeDelta = _rectTransform.sizeDelta;
    }

    public void SetColor(Color targetColor)
    {
        _Image.color = targetColor;
    }

    public virtual void FadeIn(float time = 0.5f)
    {
        if (_transitionAlpha != null)
            StopCoroutine(_transitionAlpha);

        _Image.enabled = true;

        _transitionAlpha = AlphaTo(_Image.color.a, 1f, time);
        StartCoroutine(_transitionAlpha);
    }

    public virtual void FadeOut(float time = 0.5f)
    {
        if (_transitionAlpha != null)
            StopCoroutine(_transitionAlpha);

        _transitionAlpha = AlphaTo(_Image.color.a, 0f, time);
        StartCoroutine(_transitionAlpha);
    }

    public virtual void ColorChange(Color targetColor, float time = 0.5f, UnityAction done = null)
    {
        if (_transitionColor != null)
            StopCoroutine(_transitionColor);

        _transitionColor = ColorTo(_Image.color, targetColor, time, done);
        StartCoroutine(_transitionColor);
    }
    public virtual void ColorChange(Color32 targetColor, float time = 0.5f, UnityAction done = null)
    {
        if (_transitionColor != null)
            StopCoroutine(_transitionColor);

        Color color = new Color32(targetColor.r, targetColor.g, targetColor.b, targetColor.a);

        _transitionColor = ColorTo(_Image.color, color, time, done);
        StartCoroutine(_transitionColor);
    }

    public virtual void Resize(Vector2 start, Vector2 end, float time = 0.5f)
    {
        if (_transitionScale != null)
            StopCoroutine(_transitionScale);

        _transitionScale = ScaleTo(start, end, time);
        StartCoroutine(_transitionScale);
    }

    public virtual void Move(Vector2 start, Vector2 end, float time = 0.5f)
    {
        if (_transitionPosition != null)
            StopCoroutine(_transitionPosition);

        _transitionPosition = MoveTo(start, end, time);
        StartCoroutine(_transitionPosition);
    }

    public virtual void Blink(float time = 0.5f)
    {
        if (_transitionAlpha != null)
            StopCoroutine(_transitionAlpha);

        _transitionAlpha = BlinkTo(time);
        StartCoroutine(_transitionAlpha);
    }
    public virtual void BlinkStop()
    {
        if (_transitionAlpha != null)
            StopCoroutine(_transitionAlpha);
    }

    protected IEnumerator MoveTo(Vector2 s, Vector2 e, float time)
    {
        yield return DefaultCoroutines.Vector2LerpUnclamped((v) => { _rectTransform.anchoredPosition = v; }, s, e, time, m_animaitonCurve);
    }

    protected IEnumerator ScaleTo(Vector2 s, Vector2 e, float time)
    {
        yield return DefaultCoroutines.Vector2LerpUnclamped((v) => { _rectTransform.localScale = v; }, s, e, time, m_animaitonCurve);
    }

    protected IEnumerator AlphaTo(float s, float e, float time, UnityAction done = null)
    {
        yield return DefaultCoroutines.MathfLerpUnclamped((v) => { _Image.color = new Color(_Image.color.r, _Image.color.g, _Image.color.b, v); }, s, e, time, m_animaitonCurve, done);
    }
    protected IEnumerator BlinkTo(float time, UnityAction done = null)
    {
        while (true)
        {
            yield return DefaultCoroutines.MathfLerpUnclamped((v) => { _Image.color = new Color(_Image.color.r, _Image.color.g, _Image.color.b, v); }, _Image.color.a, 1f, time, m_animaitonCurve, done);
            yield return DefaultCoroutines.MathfLerpUnclamped((v) => { _Image.color = new Color(_Image.color.r, _Image.color.g, _Image.color.b, v); }, _Image.color.a, 0.3f, time, m_animaitonCurve, done);
        }
    }

    protected IEnumerator ColorTo(Color s, Color e, float time, UnityAction done = null)
    {
        yield return DefaultCoroutines.ColorLerpUnclamped((v) => { _Image.color = v; }, s, e, time, m_animaitonCurve, done);
    }
}