using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CL_HelpPanelController : TweenableCanvas
{
    private Animator _animator = null;

    public int ContentID = 0;
    public Button NextBtn;
    public Button PrevBtn;

    public RectTransform FullLayout;
    public RectTransform ContentsLayout;
    public Image[] Contents; // 튜토리얼 콘텐츠들

    [Header("Bottom Toggles")]
    public Transform BottomLayout;
    public GameObject pageTogglePrefab;

    private Vector2 goalPos;
    private bool[] pageTabs;
    private GameObject[] pageToggles;

    private IEnumerator transition = null;

    private void Awake()
    {
        base.Awake();

        if (_animator == null)
            _animator = GetComponent<Animator>();

        pageTabs = new bool[Contents.Length];
        pageToggles = new GameObject[Contents.Length];

        for (int i = 0; i < Contents.Length; i++)
        {
            pageTabs[i] = false;
            pageToggles[i] = GameObject.Instantiate(pageTogglePrefab);
            pageToggles[i].transform.SetParent(BottomLayout);
        }

        ContentID = 0;
        pageTabs[0] = true;
    }

    private void Start()
    {
        TabActivateCheck();
    }

    public void PanelOpen()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.PanelOpen);
        InitPage();
        FadeIn();
    }

    public void PanelClose()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.PanelClose);
        FadeOut();

        if (MainController.instance != null && MainController.instance.IsStart())
            MainController.instance.StartInit();

        if (KnowledgeTabController.instance != null && !KnowledgeTabController.instance.IsStart)
            KnowledgeTabController.instance.StartKnowledge();

        if (EvaluationSceneController.Instance != null && EvaluationSceneController.Instance.startSeq)
            EvaluationSceneController.Instance.StartInit();

        if (PracticeSceneController.Instance != null && PracticeSceneController.Instance.startSeq)
            PracticeSceneController.Instance.StartInit();
    }

    public void JumpTab(int num)
    {
        pageTabs[0] = false;

        ContentID = num;
        pageTabs[ContentID] = true;

        goalPos = new Vector2(ContentsLayout.anchoredPosition.x + (FullLayout.rect.width * -num), 0);

        ContentsLayout.anchoredPosition = goalPos;

        TabActivateCheck();
    }

    // 모든 탭을 왼쪽으로 민다
    public void OnNextTab()
    {
        if (ContentID >= Contents.Length - 1)
        {
            NextBtn.interactable = false;
            return;
        }

        pageTabs[ContentID] = false;
        ContentID++;

        if (transition != null)
        {
            StopCoroutine(transition);
            ContentsLayout.anchoredPosition = goalPos;
        }

        transition = MovePage(-1);
        StartCoroutine(transition);

        TabActivateCheck();
    }

    // 현재 탭은 오른쪽으로 민다
    public void OnPrevTab()
    {
        if (ContentID <= 0)
        {
            PrevBtn.interactable = false;
            return;
        }

        pageTabs[ContentID] = false;
        ContentID--;

        if (transition != null)
        {
            StopCoroutine(transition);
            ContentsLayout.anchoredPosition = goalPos;
        }

        transition = MovePage(1);
        StartCoroutine(transition);

        TabActivateCheck();
    }

    public void TabActivateCheck()
    {
        PrevBtn.interactable = (ContentID > 0) ? true : false;
        NextBtn.interactable = (ContentID < Contents.Length - 1) ? true : false;

        for (int i=0; i<Contents.Length; i++)
            pageToggles[i].GetComponent<Image>().color = (pageTabs[i]) ? CL_UIPresetData.ThemeColor : CL_UIPresetData.TextColorInActive;
    }

    // Coroutine
    IEnumerator MovePage(int dir)
    {
        pageTabs[ContentID] = true;
        
        goalPos = new Vector2(ContentsLayout.anchoredPosition.x + (FullLayout.rect.width * dir), 0);

        while (Vector2.Distance(goalPos, ContentsLayout.anchoredPosition) > 1.5f)
        {
            ContentsLayout.anchoredPosition = Vector2.Lerp(ContentsLayout.anchoredPosition,
                                                           goalPos,
                                                           Time.deltaTime * 6);
            yield return new WaitForEndOfFrame();
        }

        ContentsLayout.anchoredPosition = goalPos;
    }

    void InitPage()
    {        
        pageTabs[ContentID] = false;

        ContentID = 0;
        pageTabs[0] = true;

        goalPos = new Vector2(0, 0);
        ContentsLayout.anchoredPosition = goalPos;

        TabActivateCheck();
    }
}
