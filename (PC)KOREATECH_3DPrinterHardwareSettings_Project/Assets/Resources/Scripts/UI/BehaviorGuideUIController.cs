using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BehaviorGuideUIController : TweenableCanvas
{
    public Text TextUI;
    private StringBuilder tempStr;
    private IEnumerator _coroutinePopup;
    
    private void Awake()
    {
        base.Awake();

        if (tempStr == null)
            tempStr = new StringBuilder();
    }

    public void Popup(float duration)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutinePopup);

        _coroutinePopup = showForDuration(duration);
        StartCoroutine(_coroutinePopup);
    }

    public void SetMessage(string text)
    {
        tempStr.Clear();
        tempStr.Append("<b>[팁]</b>");
        tempStr.Append("\n");
        tempStr.Append(text);

        TextUI.text = tempStr.ToString();
    }

    private IEnumerator showForDuration(float duration)
    {
        FadeIn(0.2f);
        yield return new WaitForSeconds(duration);
        FadeOut(0.4f);
    }
}
