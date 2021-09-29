using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CL_ScriptBoxController : AnimatedUI
{
    private IEnumerator enumerator;
    private StringBuilder tempStr;

    private string themeColor;
    private string inactiveTextColor;

    [Header("타이핑 텍스트가 나타날 Text UI")]
    public Text textBox;
    [Header("스크립트 수를 표시할 Text UI")]
    public Text textCounter;

    [Header("스크립트 창 확장 버튼 패널")]
    public AnimatedUI ExtensionPanel;
    private Vector2 scriptBoxExtendPos;
    private Vector2 scriptBoxReducePos;

    private IEnumerator _scriptBoxControll;

    private new void Awake()
    {
        base.Awake();

        tempStr = new StringBuilder();

        themeColor = CL_UIPresetData.RgbToHex(CL_UIPresetData.ThemeColor);
        inactiveTextColor = CL_UIPresetData.RgbToHex(CL_UIPresetData.TextColorInActive);
    }

    private void Start()
    {
        scriptBoxExtendPos = _rectTransform.anchoredPosition;
        scriptBoxReducePos = new Vector2(_rectTransform.anchoredPosition.x - _rectTransform.sizeDelta.x + ExtensionPanel._rectTransform.sizeDelta.x, _rectTransform.anchoredPosition.y);

        if (CL_CommonFunctionManager.Instance.UseScript())
        {
            ScriptBoxExtend();
        }
        else
        {
            ScriptBoxReduce();
        }
    }

    public string GetCurrentText()
    {
        return textBox.text;
    }

    public void SetText(string text)
    {
        textBox.text = text;
    }

    public void SetCounter(int currentCount, int totalCount)
    {
        tempStr.Clear();

        tempStr.Append("<color='");
        tempStr.Append(themeColor);
        tempStr.Append("'>");
        tempStr.Append(currentCount);
        tempStr.Append("</color>/<color='");
        tempStr.Append(inactiveTextColor);
        tempStr.Append("'>");
        tempStr.Append(totalCount);
        tempStr.Append("</color>");

        textCounter.text = tempStr.ToString();
    }

    // View
    public void ScriptBoxInactive()
    {
        FadeOut();
    }

    public void ScriptBoxActive()
    {
        FadeIn();
    }

    public void ScriptBoxReduce()
    {
        if (_scriptBoxControll != null)
            StopCoroutine(_scriptBoxControll);

        _scriptBoxControll = ScriptBoxReduce_Co();
        StartCoroutine(_scriptBoxControll);
    }
    public void ScriptBoxExtend()
    {
        if (_scriptBoxControll != null)
            StopCoroutine(_scriptBoxControll);

        _scriptBoxControll = ScriptBoxExtend_Co();
        StartCoroutine(_scriptBoxControll);
    }

    IEnumerator ScriptBoxReduce_Co()
    {
        // 스크립트 박스 숨기기
        this.Move(_rectTransform.anchoredPosition, scriptBoxReducePos, 0.5f);
        yield return new WaitForSeconds(0.5f);

        // 확장 아이콘 열기
        ExtensionPanel.Resize(ExtensionPanel._rectTransform.localScale, new Vector2(ExtensionPanel._rectTransform.localScale.x, 1f), 0.3f);
    }

    IEnumerator ScriptBoxExtend_Co()
    {
        // 확장 아이콘 숨기기
        ExtensionPanel.Resize(ExtensionPanel._rectTransform.localScale, new Vector2(ExtensionPanel._rectTransform.localScale.x, 0f), 0.3f);
        yield return new WaitForSeconds(0.3f);

        // 스크립트 박스 보이기
        this.Move(_rectTransform.anchoredPosition, scriptBoxExtendPos, 0.5f);
    }
}
