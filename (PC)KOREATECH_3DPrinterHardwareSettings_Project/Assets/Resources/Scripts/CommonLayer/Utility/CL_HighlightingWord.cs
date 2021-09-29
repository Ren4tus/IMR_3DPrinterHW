using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CL_HighlightingWord : AnimatedImage, IPointerEnterHandler, IPointerExitHandler
{
    [Header("하이라이팅 할 단어가 들어있는 Text UI")]
    public Text text;

    [Header("하이라이팅 할 단어")]
    public string HighlightWord;
    public Color DefaultColor;
    public Color HighlightColor;

    public KnowledgeHelpMeController HelpMeController;

    void Awake()
    {
        base.Awake();

        if (HelpMeController == null)
            HelpMeController = (KnowledgeHelpMeController)FindObjectOfType(typeof(KnowledgeHelpMeController));

        FadeOut(0f);
    }

    void OnEnable()
    {
        Hilighting();
    }

    public void Hilighting()
    {
        if (text.GetWordRectInText(out var rect, HighlightWord))
        {
            _Image.rectTransform.anchorMin = text.rectTransform.anchorMin;
            _Image.rectTransform.anchorMax = text.rectTransform.anchorMax;
            _Image.rectTransform.pivot = new Vector2(0f, 1f);

            _Image.rectTransform.anchoredPosition = new Vector2(rect.x, rect.y);
            _Image.rectTransform.sizeDelta = new Vector2(rect.width, rect.height);

            SetColor(DefaultColor);
            FadeIn(m_duration);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        ColorChange(HighlightColor, m_duration);
        HelpMeController.SetHighlightWordContents(HighlightWord);
        HelpMeController.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ColorChange(DefaultColor, m_duration);
        HelpMeController.Hide();
    }
}
