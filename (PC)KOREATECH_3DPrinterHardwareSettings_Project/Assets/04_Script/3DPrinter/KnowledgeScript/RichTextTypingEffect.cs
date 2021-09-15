using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RichTextTypingEffect
{
    public delegate void Callback();

    private readonly string[] richTextSymbols = { "b", "i" };
    private readonly string[] richTextCloseSymbols = { "b", "i", "size", "color" };

    // 리치텍스트 타이핑 효과
    public IEnumerator CoRichTextTyping(string text, float typingSpeed, Text textOut, Callback call)
    {
        //줄바꿈 처리
        text = text.Replace("\\n", "\n");

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        int typingIndex = 0;
        float nowWaitTime = 0f;
        char[] splitTypingTexts = text.ToCharArray();
        StringBuilder stringBuilder = new StringBuilder();

        bool bold = false;
        bool italic = false;
        bool size = false;
        bool color = false;
        int fontSize = 0;
        string colorCode = null;

        string tempStr = text;
        textOut.text = null;

        yield return new WaitForSeconds(1.2f);

        while (true)
        {
            yield return waitForEndOfFrame;
            nowWaitTime += Time.deltaTime;

            if (typingIndex == splitTypingTexts.Length)
            {
                break;
            }

            if (splitTypingTexts[typingIndex] == ' ')
            {
                stringBuilder.Append(splitTypingTexts[typingIndex]);
                typingIndex++;
                continue;
            }

            if (nowWaitTime >= typingSpeed)
            {
                if (textOut.supportRichText)
                {
                    bool symbolCatched = false;

                    for (int i = 0; i < richTextSymbols.Length; i++)
                    {
                        string symbol = string.Format("<{0}>", richTextSymbols[i]);
                        string closeSymbol = string.Format("</{0}>", richTextCloseSymbols[i]);

                        if (splitTypingTexts[typingIndex] == '<' && typingIndex + (1 + richTextSymbols[i].Length) < text.Length && text.Substring(typingIndex, 2 + richTextSymbols[i].Length).Equals(symbol))
                        {
                            switch (richTextSymbols[i])
                            {
                                case "b":
                                    typingIndex += symbol.Length;
                                    if (typingIndex + closeSymbol.Length + 3 <= text.Length)
                                    {
                                        if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                                        {
                                            bold = true;
                                            symbolCatched = true;
                                        }
                                    }
                                    break;
                                case "i":
                                    typingIndex += symbol.Length;
                                    if (typingIndex + closeSymbol.Length + 3 <= text.Length)
                                    {
                                        if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                                        {
                                            italic = true;
                                            symbolCatched = true;
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    if (splitTypingTexts[typingIndex] == '<' && typingIndex + 14 < text.Length && text.Substring(typingIndex, 8).Equals("<color=#") && splitTypingTexts[typingIndex + 14] == '>')
                    {
                        string closeSymbol = string.Format("</{0}>", "color");
                        string tempColorCode = text.Substring(typingIndex + 8, 6);

                        typingIndex += 15;
                        if (typingIndex + closeSymbol.Length <= text.Length)
                        {
                            if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                            {
                                color = true;
                                colorCode = tempColorCode;
                                symbolCatched = true;
                            }
                        }
                    }

                    if (splitTypingTexts[typingIndex] == '<' && typingIndex + 6 < text.Length && text.Substring(typingIndex, 6).Equals("<size="))
                    {
                        string closeSymbol = string.Format("</{0}>", "size");
                        string sizeSub = text.Substring(typingIndex + 6);
                        int symbolIndex = sizeSub.IndexOf('>');
                        string tempSize = sizeSub.Substring(0, symbolIndex);

                        typingIndex += 7 + tempSize.Length;
                        if (typingIndex + closeSymbol.Length <= text.Length)
                        {
                            if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                            {
                                size = true;
                                fontSize = int.Parse(tempSize);
                                symbolCatched = true;
                            }
                        }
                    }

                    bool closeSymbolCatched = false;

                    for (int i = 0; i < richTextCloseSymbols.Length; i++)
                    {
                        string closeSymbol = string.Format("</{0}>", richTextCloseSymbols[i]);

                        if (splitTypingTexts[typingIndex] == '<' && typingIndex + (1 + richTextCloseSymbols[i].Length) < text.Length && text.Substring(typingIndex, 3 + richTextCloseSymbols[i].Length).Equals(closeSymbol))
                        {
                            switch (richTextCloseSymbols[i])
                            {
                                case "b":
                                    typingIndex += closeSymbol.Length;
                                    bold = false;
                                    closeSymbolCatched = true;
                                    break;
                                case "i":
                                    typingIndex += closeSymbol.Length;
                                    italic = false;
                                    closeSymbolCatched = true;
                                    break;
                                case "size":
                                    typingIndex += closeSymbol.Length;
                                    size = false;
                                    fontSize = 0;
                                    closeSymbolCatched = true;
                                    break;
                                case "color":
                                    typingIndex += closeSymbol.Length;
                                    color = false;
                                    colorCode = null;
                                    closeSymbolCatched = true;
                                    break;
                            }
                        }

                        if (closeSymbolCatched)
                        {
                            break;
                        }
                    }

                    if (symbolCatched || closeSymbolCatched)
                    {
                        continue;
                    }

                    if (typingIndex < text.Length)
                    {
                        string convertedRichText = RichTextConvert(splitTypingTexts[typingIndex].ToString(), bold, italic, size, fontSize, color, colorCode);
                        stringBuilder.Append(convertedRichText);
                    }
                }
                else
                {
                    if (typingIndex < text.Length)
                    {
                        stringBuilder.Append(splitTypingTexts[typingIndex]);
                    }
                }

                textOut.text = stringBuilder.ToString();
                typingIndex++;
                nowWaitTime = 0f;

                //if (typingIndex % 3 == 0)
                    //CommonFunctionManager.Instance.PlayUXSound(SoundManager.UXSound.Typing);
            }
        }

        yield return new WaitForSeconds(0.3f);

        // 타이핑 종료후 지정해둔 콜백 실행
        call?.Invoke();
    }

    private string RichTextConvert(string text, bool bold, bool italic, bool size, int fontSize, bool color, string colorCode)
    {
        StringBuilder stringBuilder = new StringBuilder();

        if (bold)
        {
            stringBuilder.Append("<b>");
            stringBuilder.Append(text);
            stringBuilder.Append("</b>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        if (italic)
        {
            stringBuilder.Append("<i>");
            stringBuilder.Append(text);
            stringBuilder.Append("</i>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        if (color)
        {
            stringBuilder.Append("<color=#");
            stringBuilder.Append(colorCode);
            stringBuilder.Append(">");
            stringBuilder.Append(text);
            stringBuilder.Append("</color>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        if (size)
        {
            stringBuilder.Append("<size=");
            stringBuilder.Append(fontSize);
            stringBuilder.Append(">");
            stringBuilder.Append(text);
            stringBuilder.Append("</size>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        return text;
    }
}