using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CL_ConfigurationController : AnimatedUI
{
    public static int refreshRate = 0;

    [Header("Sounds")]
    public Slider TotalSlider;
    public Slider EffectSlider;
    public Slider EquipSlider;
    public Slider NarrationSlider;

    public Text TotalPercentage;
    public Text EffectPercentage;
    public Text EquipPercentage;
    public Text NarrationPercentage;

    private float totalVolume = 1f;
    private float narrationVolume = 1f;
    private float effectVolume = 1f;
    private float equipVolume = 1f;

    public float TotalVolume => totalVolume;
    public float NarrationVolume => narrationVolume;
    public float EffectVolume => effectVolume;
    public float EquipVolume => equipVolume;
    
    private float typingSpeed = 0.03f; // 기본 타이핑 속도
    public float TypingSpeed => typingSpeed;

    [Header("Fullscreen Mode")]
    public Toggle FullScreenSwitch;
    public GameObject FullScreenObj;
    public GameObject Checkbox;

    [Header("Graphic Quality")]
    private int graphicQuality = 2;
    public int GraphicQuality => graphicQuality;

    [Header("Resolution")]
    private readonly int[] widthResolutions = { 1280, 1280, 1920, 2560 };
    private readonly int[] heightResolutions = { 720, 1024, 1080, 1440 };
    private int resolution = 2;
    public int Resolution => resolution;
    public Toggle[] resolutionBtns;
    public Text[] resolutionTexts;
    public Color disableColor;

    [Header("Tutorial Switch")]
    public Toggle TutorialSwitch;
    public bool isUseTutorial => TutorialSwitch.isOn;

    [Header("Script On/Off Switch")]
    public Toggle ScriptSwitch;
    public bool isUseScript => ScriptSwitch.isOn;

    private string language = "KR";
    public string Language => language;

    [Header("Languages")]
    public Toggle LanguageSwitchKR;
    public Toggle LanguageSwitchEN;

    public Image BlockPanel;
    public Material BlurMat;
    public Color black;
    public Color white;

#if UNITY_WEBGL
    private bool isCammandFullScreen;
    private bool initFullscreenBtn = false;
    private bool isClick = false;
#endif

    private void Awake()
    {
        base.Awake();
#if UNITY_WEBGL
        Destroy(FullScreenSwitch.GetComponent<Toggle>());
#else
        Destroy(FullScreenSwitch.GetComponent<EventTrigger>());
#endif
    }

    private void Start()
    {
        OnTotalSound();
        OnNarrationSound();
        OnEffectSound();
        OnEquipSound();

        // SetFullscreen();
#if !UNITY_WEBGL
        SetResolution(resolution);
        InitFullScreenToggle();
#endif
        ChangeTutorialMode();
#if UNITY_WEBGL
        isCammandFullScreen = false;
        DisableResolutionBtns();
        resolutionBtns[0].isOn = true;
#endif
    }

#if UNITY_WEBGL
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt)  || Input.GetKey(KeyCode.RightAlt))
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                ToggleFullScreen();
            }
        }

        if (Screen.fullScreen)
            Checkbox.SetActive(true);
        else
            Checkbox.SetActive(false);

        if(isClick)
        {
            Screen.fullScreen = true;
            isClick = false;
        }
    }

    private void WebGLFullScreenSetting()
    {
        // FullScreenObj.AddComponent<Button>().onClick.AddListener(() => SetFullscreen());
        FullScreenObj.AddComponent<Button>();
        FullScreenObj.GetComponent<Button>().onClick.AddListener(SetFullscreen);
        Checkbox.SetActive(false);
    }
#endif

    public void SetGraphicQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality, true);
        graphicQuality = quality;
    }

    public void SetAutoGraphicMode(bool on)
    {
        PlayerPrefs.SetInt("AutoGraphic", on ? 1 : 0);
    }

    public void InitFullScreenToggle()
    {
        bool fullState = Screen.fullScreen;
        FullScreenSwitch.isOn = fullState;
    }

#if UNITY_WEBGL
    public void ToggleFullScreen()
    {
        // isCammandFullScreen = true;
        SetFullscreen();
        /*
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        FullScreenSwitch.isOn = isFullScreen;
        */
    }
#endif

    public void SetFullscreen()
    {
#if UNITY_WEBGL
        if(Screen.fullScreen)
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        else
        {
            isClick = true;
        }        
#else
        Screen.fullScreen = FullScreenSwitch.isOn;
#endif
    }

    public void DisableResolutionBtns()
    {
        for(int i=0; i< resolutionBtns.Length; i++)
        {
            resolutionBtns[i].interactable = false;
        }
        for(int i=0; i<resolutionTexts.Length; i++)
        {
            resolutionTexts[i].color = disableColor;
        }
    }

    public void SetResolution(int resolitionLevel)
    {
        Screen.SetResolution(widthResolutions[resolitionLevel], heightResolutions[resolitionLevel], Screen.fullScreen);
        resolution = resolitionLevel;

        ResolutionSet();

        /*
        CanvasScaler[] canvas = GameObject.FindObjectsOfType<CanvasScaler>();
        for(int i=0; i<canvas.Length; i++)
        {
            canvas[i].referenceResolution = new Vector2(widthResolutions[resolitionLevel], heightResolutions[resolitionLevel]);
        }
        */
    }
    
    public void OnTotalSound()
    {
        TotalPercentage.text = (TotalSlider.value * 100f).ToString("N0");
        totalVolume = TotalSlider.value;
        CL_CommonFunctionManager.Instance.UpdateVolume();
    }

    public void OnNarrationSound()
    {
        NarrationPercentage.text = (NarrationSlider.value * 100f).ToString("N0");
        narrationVolume = NarrationSlider.value;
        CL_CommonFunctionManager.Instance.UpdateVolume();
    }

    public void OnEffectSound()
    {
        EffectPercentage.text = (EffectSlider.value * 100f).ToString("N0");
        effectVolume = EffectSlider.value;
        CL_CommonFunctionManager.Instance.UpdateVolume();
    }

    public void OnEquipSound()
    {
        EquipPercentage.text = (EquipSlider.value * 100f).ToString("N0");
        equipVolume = EquipSlider.value;
        CL_CommonFunctionManager.Instance.UpdateVolume();
    }

    public void ChangeTutorialMode()
    {
        // 추후 작성 필요
    }
    /* Add */
    public void PanelOpen()
    {
#if !UNITY_WEBGL
        ResolutionSet();
#else 
        if(!initFullscreenBtn)
        {
            initFullscreenBtn = true;
            // WebGLFullScreenSetting();
        }
#endif
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.PanelOpen);
        FadeIn();
    }
    public void PanelClose()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.PanelClose);

        FadeOut();
    }
    
    public void ResolutionSet()
    {
        if (Screen.width == 1280 && Screen.height == 1024)
        {
            BlockPanel.material = null;
            BlockPanel.color = black;
        }
        else
        {
            BlockPanel.material = BlurMat;
            BlockPanel.color = white;
        }
    }
}