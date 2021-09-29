using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NextStepButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator animator = null;
    private Button button = null;
    private Color32 color;

    // Hover 관련
    private Vector3 m_DefaultScale = Vector3.zero;
    [Range(0, 1.5f)] public float m_ScaleTo = 0.8f;
    public AnimationCurve m_Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Range(0, 0.5f)] public float m_Duration = 0.3f;

    private void Awake()
    {
        if (m_DefaultScale.Equals(Vector3.zero))
            m_DefaultScale = transform.localScale;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (button == null)
            button = GetComponent<Button>();

        color = button.image.color;
    }

    private void Start()
    {
        Init();
    }

    public bool IsBlocking()
    {
        return button.IsInteractable();
    }

    public void Blocking()
    {
        Init();
        button.interactable = false;
    }
    public void BlockingRelease()
    {
        button.interactable = true;
    }

    public void OnNext()
    {
        StopAllCoroutines();
        StartCoroutine(StartBlink(m_DefaultScale, Vector3.one * 0.8f));
    }
    public void Init()
    {
        StopAllCoroutines();
        transform.localScale = m_DefaultScale;
    }

    // Event Trigger
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        Init();
        StartCoroutine(ScaleTo(transform.localScale, Vector3.one * m_ScaleTo));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(transform.localScale, m_DefaultScale));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.ScriptButtonSound);
    }
    // Coroutines
    IEnumerator StartBlink(Vector3 s, Vector3 e)
    {
        while (true)
        {
            yield return VLerpUnclamped((v) => { transform.localScale = v; }, s, e, 0.3f, m_Curve);
            yield return VLerpUnclamped((v) => { transform.localScale = v; }, e, s, 0.3f, m_Curve);
        }
    }
    IEnumerator ScaleTo(Vector3 s, Vector3 e)
    {
        yield return VLerpUnclamped((v) => { transform.localScale = v; }, s, e, m_Duration, m_Curve);
    }
    IEnumerator TransparentTo(Vector3 s, Vector3 e)
    {
        yield return VLerpAndTransparentUnclamped((v) => { transform.localScale = v; }, (col) => { button.image.color = col; }, s, e, m_Duration, m_Curve);
    }

    static public IEnumerator VLerpUnclamped(UnityAction<Vector3> v, Vector3 s, Vector3 e, float d, AnimationCurve c = null, UnityAction done = null)
    {
        yield return Eval(d, (x) => { v?.Invoke(Vector3.LerpUnclamped(s, e, c != null ? c.Evaluate(x) : x)); }, () => { v?.Invoke(e); done?.Invoke(); });
    }

    static public IEnumerator VLerpAndTransparentUnclamped(UnityAction<Vector3> v, UnityAction<Color32> col, Vector3 s, Vector3 e, float d, AnimationCurve c = null, UnityAction done = null)
    {
        yield return Eval(d, 
                         (x) => {
                             v?.Invoke(Vector3.LerpUnclamped(s, e, c != null ? c.Evaluate(x) : x));
                         }, 
                         () => {
                             v?.Invoke(e); done?.Invoke();
                         });
    }

    static public IEnumerator Eval(float d, UnityAction<float> onTime, UnityAction done = null)
    {
        float elapsed = 0;
        onTime?.Invoke(0);
        while (elapsed < d)
        {
            elapsed += Time.deltaTime;
            onTime?.Invoke(elapsed / d);
            yield return null;
        }
        onTime?.Invoke(1);
        done?.Invoke();
    }
}
