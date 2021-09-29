using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnimationCoroutines
{
    public static class DefaultCoroutines
    {
        public static IEnumerator ColorLerpUnclamped(UnityAction<Color> f, Color s, Color e, float d, AnimationCurve c = null, UnityAction done = null)
        {
            yield return Eval(d,
                              (x) =>
                              {
                                  f?.Invoke(new Color(Mathf.LerpUnclamped(s.r, e.r, c != null ? c.Evaluate(x) : x), 
                                                      Mathf.LerpUnclamped(s.g, e.g, c != null ? c.Evaluate(x) : x), 
                                                      Mathf.LerpUnclamped(s.b, e.b, c != null ? c.Evaluate(x) : x),
                                                      Mathf.LerpUnclamped(s.a, e.a, c != null ? c.Evaluate(x) : x)));
                              },
                              () => { f?.Invoke(e); done?.Invoke(); });
        }

        public static IEnumerator MathfLerpUnclamped(UnityAction<float> f, float s, float e, float d, AnimationCurve c = null, UnityAction done = null)
        {
            yield return Eval(d, (x) => { f?.Invoke(Mathf.LerpUnclamped(s, e, c != null ? c.Evaluate(x) : x)); }, () => { f?.Invoke(e); done?.Invoke(); });
        }

        public static IEnumerator Vector2LerpUnclamped(UnityAction<Vector2> v, Vector2 s, Vector2 e, float d, AnimationCurve c = null, UnityAction done = null)
        {
            yield return Eval(d, (x) => { v?.Invoke(Vector2.LerpUnclamped(s, e, c != null ? c.Evaluate(x) : x)); }, () => { v?.Invoke(e); done?.Invoke(); });
        }

        public static IEnumerator Eval(float d, UnityAction<float> onTime, UnityAction done = null)
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
}