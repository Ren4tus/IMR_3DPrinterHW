using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class LoginPopupBase : MonoBehaviour
{

    public enum PopupType { FindPassword, Error, Notify }

    public Transform m_Popup;
    public PopupType m_PopupType;

    [SerializeField, Range(0, 1)]
    protected float m_Duration;
    [SerializeField]
    protected Vector3 m_Destination;


    public virtual void PopupOpen(UnityAction done = null) { }
    public virtual void PopupClose() { }
    public virtual void SetText(string s) { }


    public IEnumerator Opening(Transform tr, UnityAction done)
    {
        float time = 0f;
        Vector3 scale = Vector3.zero;

        while (time < m_Duration)
        {
            time += Time.deltaTime;
            scale = Vector3.Lerp(tr.localScale, m_Destination, time / m_Duration);
            tr.localScale = scale;
            yield return null;
        }

        if (done != null) { done(); }

    }

    public IEnumerator Closing(Transform tr, UnityAction done)
    {

        float time = 0f;
        Vector3 scale = Vector3.zero;

        while (time < m_Duration)
        {
            time += Time.deltaTime;
            scale = Vector3.Lerp(tr.localScale, Vector3.zero, time / m_Duration);
            tr.localScale = scale;

            yield return null;
        }

        if (done != null) { done(); }

    }
}
