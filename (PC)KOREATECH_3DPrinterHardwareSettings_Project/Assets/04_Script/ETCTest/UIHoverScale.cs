using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 m_DefaultScale = Vector3.zero;
    [Range(0, 1.5f)] public float m_ScaleTo = 1.1f;
    public AnimationCurve m_Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Range(0, 0.5f)] public float m_Duration = 0.3f;

    private void Awake()
    {
        if (m_DefaultScale.Equals(Vector3.zero))
            m_DefaultScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localScale = m_DefaultScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
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

    IEnumerator ScaleTo(Vector3 s, Vector3 e)
    {
        yield return VLerpUnclamped((v) => { transform.localScale = v; }, s, e, m_Duration, m_Curve);
    }

    static public IEnumerator VLerpUnclamped(UnityAction<Vector3> v, Vector3 s, Vector3 e, float d, AnimationCurve c = null, UnityAction done = null)
    {
        yield return Eval(d, (x) => { v?.Invoke(Vector3.LerpUnclamped(s, e, c != null ? c.Evaluate(x) : x)); }, () => { v?.Invoke(e); done?.Invoke(); });
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