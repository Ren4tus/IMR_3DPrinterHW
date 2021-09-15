using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class KnowledgeTabController : MonoBehaviour
{
    public class TabsChild
    {
        public GameObject TabTitle = null;
        public GameObject Tab = null;

        public Text titleText = null;
        public AnimatedImage tabImage = null;

        public TabsChild(GameObject gameObject1, GameObject gameObject2, Text text, AnimatedImage image)
        {
            TabTitle = gameObject1;
            Tab = gameObject2;
            titleText = text;
            tabImage = image;
        }
    }

    [Header("UI")]
    public string TitleText;
    public string SubtitleText;

    public TitleIndicatorController TitleIndicator;
    public AnimatedUIController TopBar;
    public AnimatedUIController ContextPanel;
    public GameObject[] Tabs;
    public List<TabsChild> TabsInfo;

    public TextAsset scriptFile;
    public Text explanationText; // 설명이 출력될 Text 오브젝트

    private int m_MaxTab;
    private int m_CurrentTabID; // m_CurrentTabID, 0번부터 시작
    private string tempStr;

    [SerializeField]
    private float typingSpeed;

    private bool isTyping;

    private readonly string[] richTextSymbols = { "b", "i" };
    private readonly string[] richTextCloseSymbols = { "b", "i", "size", "color" };

    public bool IsTyping => isTyping;
    public bool IsStart = false;

    public Image ToolTipImg; // 탭의 타이핑이 끝나고 활성화될 툴팁 이미지

    private IEnumerator Effect;

    public static KnowledgeTabController instance;

    [Header("Tutorial Num")]
    public int tutorialNum;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        m_MaxTab = Tabs.Length;
        TabsInfo = new List<TabsChild>();

        for (int i=0;i< m_MaxTab; i++)
        {
            TabsChild tab = new TabsChild(Tabs[i].transform.Find("Title").gameObject,
                                       Tabs[i].transform.Find("Tab").gameObject,
                                       Tabs[i].transform.Find("Title").GetComponent<Text>(),
                                       Tabs[i].transform.Find("Tab").GetComponent<AnimatedImage>());
            TabsInfo.Add(tab);
        }
    }

    private void Start()
    {
        // CL_CommonFunctionManager.Instance.QuickMenuActive(true);

        if (CL_CommonFunctionManager.Instance.UseTutorial()) // 튜토리얼 사용
        {
            CL_CommonFunctionManager.Instance.TutorialPanelOpen(tutorialNum);
        }
        else // 미사용
        {
            StartKnowledge();
        }
    }
    public void OnDestroy()
    {
        TabsInfo = null;

        Resources.UnloadUnusedAssets();
    }

    public void OnEnable()
    {
        // RichTextTyping((string)CSVController.data[m_CurrentTabID][ConfigurationController.Language]);
    }

    public void StartKnowledge()
    {
        StartCoroutine(DelayedRun(2.5f));
    }

    public void QuickLoadTab()
    {
        StopCoroutine(Effect);

        explanationText.text = (string)CSVController.data[m_CurrentTabID][ConfigurationController.Language];

        AfterScriptCompleteProcess();
    }

    public void GoStruct(string sceneName)
    {
#if UNITY_WEBGL_EACH
        string popupTitle = "학습이 종료되었습니다.\n다시 학습하시겠습니까?";
#else
        string popupTitle = "지식 학습이 마무리되었습니다.\n구조로 이동하시겠습니까?";
#endif

        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp(popupTitle, CL_MessagePopUpController.DialogType.NOTICE,
                                                                    () => CL_CommonFunctionManager.Instance.LoadScene(sceneName),
                                                                    null);
    }

    public void MoveNextTab()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.ScriptButtonSound);

        //CommonFunctionManager.Instance.PlayUXSound(SoundManager.UXSound.KnowledgeTabButtonClick);
        if (m_CurrentTabID < m_MaxTab - 1)
        {
            m_CurrentTabID += 1;
            ChangeTabState(m_CurrentTabID);
        }
        else
        {
#if UNITY_WEBGL_EACH
            string chapter = CL_CommonFunctionManager.Instance.GetCurrentSceneName();
#else
            string chapter = CL_CommonFunctionManager.Instance.GetCurrentSceneName().Replace("Knowledge", "Structure");
#endif
            GoStruct(chapter);
        }
    }
    public void MovePrevTab()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.ScriptButtonSound);

        //CommonFunctionManager.Instance.PlayUXSound(SoundManager.UXSound.KnowledgeTabButtonClick);
        if (m_CurrentTabID > 0)
        {
            m_CurrentTabID -= 1;
            ChangeTabState(m_CurrentTabID);
        }
    }

    // 타이핑 전 활성화되어야 할 오브젝트를 활성화하는 함수
    // 탭 오브젝트의 하위(자식)에 위치한 오브젝트 중, AfterLoading 레이어에 포함되지 않은 오브젝트를 모두 활성화합니다.
    public void LoadObject(int TabID)
    {
        GameObject child = null;

        for (int i = 0; i < Tabs[TabID].transform.childCount; i++)
        {
            child = Tabs[TabID].transform.GetChild(i).gameObject;

            if (child.layer == LayerMask.NameToLayer("AfterLoading") ||
                child.layer == LayerMask.NameToLayer("NotInit"))
            {
                child.SetActive(false);
            }
            else
            {
                child.SetActive(true);
            }
        }
    }

    // 타이핑이 끝난 뒤 활성화되어야 할 오브젝트를 활성화하는 함수
    // 탭 오브젝트의 하위(자식)에 위치한 오브젝트 중, AfterLoading 레이어에 위치한 오브젝트를 모두 활성화합니다.
    public void LoadAfterObject(int TabID)
    {
        GameObject child = null;

        for (int i = 0; i < Tabs[TabID].transform.childCount; i++)
        {
            child = Tabs[TabID].transform.GetChild(i).gameObject;

            if (child.layer.Equals(LayerMask.NameToLayer("AfterLoading")))
                child.SetActive(true);
        }
    }

    public void LoadTab()
    {
        RichTextTyping((string)CSVController.data[m_CurrentTabID][ConfigurationController.Language]);
    }

    public void AfterScriptCompleteProcess()
    {
        // 효과음 재생
        //CommonFunctionManager.Instance.PlayUXSound(SoundManager.UXSound.TypingComplete);

        // 타이핑이 끝난 뒤 AfterLoading Layer 오브젝트들 활성화
        LoadAfterObject(m_CurrentTabID);

        // if(m_CurrentTabID < m_MaxTab - 1)
        ToolTipImg.gameObject.SetActive(true);

        isTyping = false;
    }

    private void RichTextTyping(string text)
    {
        ToolTipImg.gameObject.SetActive(false);

        if (isTyping)
        {
            StopAllCoroutines();
            isTyping = false;
        }

        isTyping = true;
        Effect = new RichTextTypingEffect().CoRichTextTyping(text, 0.03f, explanationText, AfterScriptCompleteProcess);
        StartCoroutine(Effect);
    }
    
    // 오브젝트를 비활성화하는 함수
    public void InactivateObjects(int TabID)
    {
        int i;

        for (i = 0; i < Tabs[TabID].transform.childCount; i++)
        {
            GameObject gameObject = Tabs[TabID].transform.GetChild(i).gameObject;

            if (!gameObject.name.Equals("Tab") && !gameObject.name.Equals("Title"))
                gameObject.SetActive(false);
        }
    }

    // 탭의 상태가 변경될 때 호출하는 함수
    public void ChangeTabState(int tab)
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.ScriptButtonSound);

        if (tab >= m_MaxTab || tab < 0)
            return;

        int i; 

        for (i=0;i<m_MaxTab;i++)
        {
            if (!i.Equals(tab))
            {
                TabsInfo[i].titleText.color = UIPresetData.TextColorInActive;

                TabsInfo[i].tabImage.BlinkStop();
                TabsInfo[i].tabImage.SetColor(UIPresetData.TextColorInActive);
                InactivateObjects(i);
            }
        }

        m_CurrentTabID = tab;
        TabsInfo[tab].titleText.color = CL_UIPresetData.ThemeColor;
        TabsInfo[tab].tabImage.SetColor(CL_UIPresetData.ThemeColor);
        TabsInfo[tab].tabImage.Blink();
        LoadObject(tab);

        RichTextTyping((string)CSVController.data[m_CurrentTabID][ConfigurationController.Language]);
        
        // Play Narration
        CL_CommonFunctionManager.Instance.PlayNarration(m_CurrentTabID);
    }

    public int GetCurrentTabID()
    {
        return m_CurrentTabID;
    }

    // 대기 코루틴
    IEnumerator DelayedRun(float time)
    {
        TopBar.Show();
        ContextPanel.FadeOut();

        TitleIndicator.ShowTitleSlideIn(TitleText, SubtitleText);
        yield return new WaitForSeconds(time);

        IsStart = true;

        // 시작될 때 0번 탭 활성화
        m_CurrentTabID = 0;
        ChangeTabState(0);

        ContextPanel.FadeIn();
    }
}