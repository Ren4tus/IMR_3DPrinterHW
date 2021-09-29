using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using AnimationCoroutines;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class TweenableCanvas : MonoBehaviour
{
    protected float m_Duration = 0.5f;
    public AnimationCurve m_Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    protected IEnumerator _coroutine = null;
    protected Canvas _canvas = null;
    protected CanvasGroup _canvasGroup = null;

    protected virtual void Awake()
    {
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();

        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void FadeIn(float time = 0.5f)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _canvas.enabled = true;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        _coroutine = AlphaTo(_canvasGroup.alpha, 1f, time);
        StartCoroutine(_coroutine);
    }
    
    public virtual void FadeOut(float time = 0.5f)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _coroutine = AlphaTo(_canvasGroup.alpha, 0f, time, () => { _canvas.enabled = false; });

        StartCoroutine(_coroutine);
    }

    protected IEnumerator AlphaTo(float s, float e, float time, UnityAction done = null)
    {
        yield return DefaultCoroutines.MathfLerpUnclamped((v) => { _canvasGroup.alpha = v; }, s, e, time, m_Curve, done);
    }
}
