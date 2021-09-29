using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class TextUtil
{
    public static bool GetWordRectInText(this Text textUI, out Rect rect, string word)
    {
        rect = new Rect();
        if (string.IsNullOrEmpty(textUI.text) || string.IsNullOrEmpty(word) || !textUI.text.Contains(word))
        {
            return false;
        }

        Canvas.ForceUpdateCanvases();

        TextGenerator txtGen = textUI.cachedTextGenerator;

        if (txtGen.characterCount == 0)
        {
            txtGen = textUI.cachedTextGeneratorForLayout;
        }

        if (txtGen.characterCount == 0 || txtGen.lineCount == 0)
        {
            return false;
        }

        List<UILineInfo> lines = txtGen.lines as List<UILineInfo>;
        List<UICharInfo> characters = txtGen.characters as List<UICharInfo>;

        int startIndex = textUI.text.IndexOf(word);
        UILineInfo lineInfo = new UILineInfo();

        for (int i = txtGen.lineCount - 1; i >= 0; i--)
        {
            if (lines != null && lines[i].startCharIdx <= startIndex)
            {
                lineInfo = lines[i];
                break;
            }
        }

        if (lines != null && characters != null)
        {
            var anchoredPosition = textUI.rectTransform.anchoredPosition;
            var screenRatio = 1080f / Screen.height; // 기준해상도 FHD(1080p)

            rect.x = characters[startIndex].cursorPos.x * screenRatio + anchoredPosition.x;
            rect.y = characters[startIndex].cursorPos.y * screenRatio + anchoredPosition.y;

            for (var index = startIndex; index < startIndex + word.Length; index++)
            {
                var info = characters[index];
                rect.width += info.charWidth * screenRatio;
            }
            
            rect.height = lineInfo.height * screenRatio; // Line
        }

        return true;
    }
}

