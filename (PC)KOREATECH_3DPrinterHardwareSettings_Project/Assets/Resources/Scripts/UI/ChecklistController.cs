using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using EvaluationCore;
using System.Text;
using AnimationCoroutines;

public class ChecklistController : TweenableCanvas
{
    private RectTransform rectTransform = null;
    private CanvasGroup canvasGroup = null;

    [Header("Layout")]
    public int PaddingRight = 10;
    public Color CompleteColor;
    public Color IncompleteColor;
    public Color SkipColor;

    public GameObject ContentContainer;
    public GameObject ItemPrefab;
    public GameObject ItemIndent2Prefab;
    public Text ToggleBtnTxt;

    public Dictionary<int, List<ChecklistItem>> evaluationChecklist;
    private List<ChecklistItem> checkList;
    private Vector2 checklistSize;
    private bool isShow = true;
    
    private Vector2 originPos;
    private Vector2 originSize;

    public AnimationCurve m_sizeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float m_sizeDuration = 0.3f;

    private new void Awake()
    {
        base.Awake();

        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        originPos = rectTransform.anchoredPosition;
        originSize = rectTransform.sizeDelta;
    }
    
    public void ActiveChecklist()
    {
        FadeIn();
    }

    public void InactiveChecklist()
    {
        FadeOut();
    }

    public void MakeCheckList(string[] items, bool isChecklist)
    {
        if (items.Length <= 0)
            return;
        
        checkList = new List<ChecklistItem>();
        
        for (int i=0; i<items.Length; i++)
        {
            GameObject gameObject = GameObject.Instantiate(ItemPrefab);
            gameObject.transform.SetParent(ContentContainer.transform);
            gameObject.transform.localScale = Vector3.one;

            ChecklistItem item = gameObject.GetComponent<ChecklistItem>();
            item.SetText(items[i]);
            item.IsCheckable = isChecklist;
            
            checkList.Add(item);
        }

        checklistSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        ShowChecklist();
    }

    public void MakeEvaluationCheckList(EvaluationContainer items)
    {
        if (items.TotalSequenceCount() <= 0)
            return;

        StringBuilder sb = new StringBuilder();
        int total = items.TotalSequenceCount();
        
        evaluationChecklist = new Dictionary<int, List<ChecklistItem>>();

        foreach (KeyValuePair<int, EvaluationSequence> item in items._sequenceList)
        {
            GameObject gameObject = GameObject.Instantiate(ItemPrefab);
            gameObject.transform.SetParent(ContentContainer.transform);
            gameObject.transform.localScale = Vector3.one;

            ChecklistItem checklistItem = gameObject.GetComponent<ChecklistItem>();
            checklistItem.SetText(item.Value.Name);
            checklistItem.SetBackgroundColor(IncompleteColor);
            checklistItem.IsCheckable = false;

            if (!evaluationChecklist.ContainsKey(item.Key))
                evaluationChecklist[item.Key] = new List<ChecklistItem>();

            evaluationChecklist[item.Key].Add(checklistItem);

            foreach (EvaluationSequenceItem seqitem in item.Value.scoreItems)
            {
                GameObject temp = GameObject.Instantiate(ItemIndent2Prefab);
                temp.transform.SetParent(gameObject.transform);
                temp.transform.localScale = Vector3.one;

                ChecklistItem item2 = temp.GetComponent<ChecklistItem>();
                item2.SetText(" - " + seqitem.Name);
                item2.SetBackgroundColor(IncompleteColor);
                item2.SetButtonEvent(seqitem.MovePosition);
                item2.IsCheckable = false;

                evaluationChecklist[item.Key].Add(item2);
            }
        }

        checklistSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

        ActiveChecklist();
        ShowChecklist();
    }

    public void HideChecklist(float time = 0.5f)
    {
        ToggleBtnTxt.text = "평가항목 열기";

        isShow = false;
        StopAllCoroutines();

        StartCoroutine(ScaleTo(rectTransform.anchoredPosition, new Vector2(originPos.x, rectTransform.anchoredPosition.y - rectTransform.sizeDelta.y + 32f), time));
    }
    public void ShowChecklist()
    {
        ToggleBtnTxt.text = "평가항목 닫기";

        isShow = true;
        StopAllCoroutines();

        StartCoroutine(ScaleTo(rectTransform.anchoredPosition, originPos));
    }

    public void ChecklistVisibleToggle()
    {
        if (!isShow)
        {
            ShowChecklist();
        }
        else
        {
            HideChecklist();
        }
    }

    IEnumerator ScaleTo(Vector2 s, Vector2 e, float time = 0.5f)
    {
        yield return DefaultCoroutines.Vector2LerpUnclamped((v) => { rectTransform.anchoredPosition = v; }, s, e, time, m_sizeCurve);
    }
}
