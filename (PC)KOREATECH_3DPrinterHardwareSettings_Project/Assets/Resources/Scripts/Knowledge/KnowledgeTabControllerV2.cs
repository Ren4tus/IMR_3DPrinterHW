using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class KnowledgeTabControllerV2 : MonoBehaviour
{
    public class TabsChild
    {
        public GameObject TabTitle;
        public GameObject Tab;

        public Text titleText;
        public Image tabImage;
        public Animator tabAnimator;

        public TabsChild()
        {
            TabTitle = null;
            Tab = null;

            titleText = null;
            tabImage = null;
            tabAnimator = null;
        }
        public TabsChild(GameObject gameObject1, GameObject gameObject2, Text text, Image image, Animator animator)
        {
            TabTitle = gameObject1;
            Tab = gameObject2;
            titleText = text;
            tabImage = image;
            tabAnimator = animator;
        }
    }

    [Header("스크립트")]
    public Text explanationText; // 스크립트가 출력될 Text 오브젝트

    private int MaxTab = 4;
    public GameObject[] Tabs;
    private TabsChild[] TabsInfo;

    private int tabID; // tabID, 0번부터 시작
    private string tempStr;

    [SerializeField]
    private float typingSpeed;

    [SerializeField]
    private bool enableTyping; // 타이핑이 가능한 상태인지

    private bool isTyping;

    private readonly string[] richTextSymbols = { "b", "i" };
    private readonly string[] richTextCloseSymbols = { "b", "i", "size", "color" };

    public bool IsTyping => isTyping;

    public Image ToolTipImg; // 탭의 타이핑이 끝나고 활성화될 툴팁 이미지
    public Sprite TabOffSprite;
    public Sprite TabOnSprite;

    private void Initialize()
    {
        int i;

        MaxTab = Tabs.Length;
        tabID = 0;

        TabsInfo = new TabsChild[MaxTab];

        for (i=0;i< MaxTab; i++)
        {
            TabsInfo[i] = new TabsChild(Tabs[i].transform.Find("Title").gameObject, Tabs[i].transform.Find("Tab").gameObject, Tabs[i].transform.Find("Title").GetComponent<Text>(), Tabs[i].transform.Find("Tab").GetComponent<Image>(), Tabs[i].transform.Find("Tab").GetComponent<Animator>());
        }

        // 시작될 때 0번 탭 활성화
        ChangeTabState(0);
    }

    private void Start()
    {
        Initialize();
    }

    public void OnEnable()
    {
        enableTyping = true;
        RichTextTyping((string)CSVController.data[tabID][CL_CommonFunctionManager.Instance.GetProgramLanguage()]);
    }

    public void OnDisable()
    {
        enableTyping = true;
    }

    public void MoveNextTab()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.KnowledgeTabButtonClick);

        if (tabID < MaxTab - 1)
        {
            tabID += 1;
            ChangeTabState(tabID);
        }
    }
    public void MovePrevTab()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.KnowledgeTabButtonClick);

        if (tabID > 0)
        {
            tabID -= 1;
            ChangeTabState(tabID);
        }
    }

    // 타이핑 전 활성화되어야 할 오브젝트를 활성화하는 함수
    // 탭 오브젝트의 하위(자식)에 위치한 오브젝트 중, AfterLoading 레이어에 포함되지 않은 오브젝트를 모두 활성화합니다.
    public void LoadObject(int tabID)
    {
        GameObject child = null;

        for (int i = 0; i < Tabs[tabID].transform.childCount; i++)
        {
            child = Tabs[tabID].transform.GetChild(i).gameObject;

            if (child.layer == LayerMask.NameToLayer("AfterLoading"))
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
    public void LoadAfterObject(int tabID)
    {
        GameObject child = null;

        for (int i = 0; i < Tabs[tabID].transform.childCount; i++)
        {
            child = Tabs[tabID].transform.GetChild(i).gameObject;

            if (child.layer == LayerMask.NameToLayer("AfterLoading"))
            { 
                child.SetActive(true);
            }
        }
    }

    public void LoadTab()
    {
        RichTextTyping((string)CSVController.data[tabID][CL_CommonFunctionManager.Instance.GetProgramLanguage()]);
    }

    public void AfterScriptCompleteProcess()
    {
        // 효과음 재생
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.TypingComplete);

        // 타이핑이 끝난 뒤 AfterLoading Layer 오브젝트들 활성화
        LoadAfterObject(tabID);
        ToolTipImg.gameObject.SetActive(true);
        isTyping = false;
        enableTyping = true;
    }

    private void RichTextTyping(string text)
    {
        enableTyping = false;
        ToolTipImg.gameObject.SetActive(false);

        if (isTyping)
        {
            StopAllCoroutines();
            isTyping = false;
        }

        isTyping = true;
        //CL_CommonFunctionManager.Instance.TypingText(text, explanationText, AfterScriptCompleteProcess);
    }


    // 오브젝트를 비활성화하는 함수
    void InactivateObjects(int tabID)
    {
        for (int i = 0; i < Tabs[tabID].transform.childCount; i++)
        {
            Tabs[tabID].transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // 탭의 상태가 변경될 때 호출하는 함수
    private void ChangeTabState(int tabID)
    {
        for (int i=0;i<MaxTab;i++)
        {
            if (i != tabID)
            {
                TabsInfo[i].titleText.color = UIPresetData.TextColorInActive;
                TabsInfo[i].tabImage.sprite = TabOffSprite;
                TabsInfo[i].tabImage.color = new Color32(255,255,255,255); // 투명도 초기화
                TabsInfo[i].tabAnimator.enabled = false;

                InactivateObjects(i);
            }

            // 탭 라인과 타이틀은 보이게 설정
            TabsInfo[i].TabTitle.SetActive(true); 
            TabsInfo[i].Tab.SetActive(true);
        }
        //CL_CommonFunctionManager.Instance.PlayNarration(tabID);

        TabsInfo[tabID].titleText.color = UIPresetData.ThemeColor;
        TabsInfo[tabID].tabImage.sprite = TabOnSprite;
        TabsInfo[tabID].tabAnimator.enabled = true;
        LoadObject(tabID);

        RichTextTyping((string)CSVController.data[tabID][CL_CommonFunctionManager.Instance.GetProgramLanguage()]);
    }
}