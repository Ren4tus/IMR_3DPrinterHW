using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AnimationCoroutines;

[RequireComponent(typeof(RectTransform))]
public class AnimatedUI : MonoBehaviour
{
    public RectTransform _rectTransform;
    public bool IsInitialized = false;

    protected IEnumerator _transitionScale;
    protected IEnumerator _transitionPosition;
    protected IEnumerator _transitionAlpha = null;
    
    protected CanvasGroup _canvasGroup = null;
    
    protected Vector2 originPosition;
    protected Vector2 originSizeDelta;
    protected Vector2 originScale;

    [Header("Animation")]
    public AnimationCurve m_animaitonCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float m_duration = 0.3f;

    protected void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

        SetOrigin();
        IsInitialized = true;
    }

    public void SetOrigin()
    {
        originPosition = _rectTransform.anchoredPosition;
        originScale = _rectTransform.localScale;
        originSizeDelta = _rectTransform.sizeDelta;
    }

    public virtual void FadeIn(float time = 0.5f)
    {
        if (_transitionAlpha != null)
            StopCoroutine(_transitionAlpha);

        if (!IsInitialized)
            Initialize();

        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        _transitionAlpha = AlphaTo(_canvasGroup.alpha, 1f, time);
        StartCoroutine(_transitionAlpha);
    }

    public virtual void FadeOut(float time = 0.5f)
    {
        if (_transitionAlpha != null)
            StopCoroutine(_transitionAlpha);
        
        if (!IsInitialized)
            Initialize();

        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _transitionAlpha = AlphaTo(_canvasGroup.alpha, 0f, time);
        StartCoroutine(_transitionAlpha);
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
        yield return DefaultCoroutines.MathfLerpUnclamped((v) => { _canvasGroup.alpha = v; }, s, e, time, m_animaitonCurve, done);
    }
}
