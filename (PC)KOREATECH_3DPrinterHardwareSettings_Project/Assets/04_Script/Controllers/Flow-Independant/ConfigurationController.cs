using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationController : MonoBehaviour
{
    public MainController mainController; // 스크립트 언어 변경에 필요

    private static bool initialized = false;

    //[Header("AutoGraphic Mode")]
    //public Button[] autoGraphic_buttons;

    private static bool autoQualityMode = false;
    private int frameCounter = 0;
    private int targetQuality = 0;
    private float countingTimer = 0f;
    private float countingTime = 5f;
    private float maxRate = 0.9f;
    private float minRate = 0.6f;
    public static int refreshRate = 0;

    [Header("Sounds")]
    //public Button allMuteButton;
    //public Button narrationMuteButton;
    //public Button sfxMuteButton;
    //public Button BackgroundMuteButton;
    public Slider TotalSlider;
    public Slider EffectSlider;
    public Slider EquipSlider;
    public Slider NarrationSlider;

    public Text TotalPercentage;
    public Text EffectPercentage;
    public Text EquipPercentage;
    public Text NarrationPercentage;

    public AudioSource narration;
    public AudioSource[] sfx;
    public AudioSource backgroundSound;

    private static float totalVolume = 1f;
    private static float narrationVolume = 1f;
    private static float effectVolume = 1f;
    private static float equipVolume = 1f;

    public float TotalVolume => totalVolume;
    public float NarrationVolume => narrationVolume;
    public float EffectVolume => effectVolume;
    public float EquipVolume => equipVolume;

    [Header("Fullscreen Mode")]
    public Image FullScreenSwitch;
    private bool isFullScreen = false;

    [Header("Graphic Quality")]
    public Button[] graphic_buttons;
    public static int graphicQuality = 2;
    private int[] quality2Idx = { 0, 0, 1, 1, 2, 2 };
    private int[] idx2Quality = { 0, 2, 4 };

    [Header("Resolution")]
    public Button[] resolution_buttons;
    public static int[] widthResolutions = { 1280, 1280, 1920, 2560 };
    public static int[] heightResolutions = { 720, 1024, 1080, 1440 };
    private int resolutionTarget = 0;

    [Header("Script")]
    public Image ScriptSwitch;


    [Header("Languages")]
    public Image LanguageSwitch;
    public static string Language = "KR";

    [Header("Sprites")]
    public Sprite[] SwitchSprite;
    public Sprite[] CheckBoxSprite;

    [Header("Commons")]
    public Color normal_color;
    public Color selected_color;
    public Color disabled_normal_color;
    public Color disabled_selected_color;

    void Awake()
    {
        Language = "KR";
        /*
        ColorBlock tempColorBlock;

        for (int i = 0; i < graphic_buttons.Length; i++)
        {
            graphic_buttons[i].interactable = !autoQualityMode;

            tempColorBlock = graphic_buttons[i].colors;

            tempColorBlock.normalColor = (i == quality2Idx[graphicQuality]) ? selected_color : normal_color;
            tempColorBlock.disabledColor = (i == quality2Idx[graphicQuality]) ? disabled_selected_color : disabled_normal_color;

            graphic_buttons[i].colors = tempColorBlock;
        }

        tempColorBlock = autoGraphic_buttons[0].colors;
        tempColorBlock.normalColor = autoQualityMode ? selected_color : normal_color;
        autoGraphic_buttons[0].colors = tempColorBlock;

        tempColorBlock = autoGraphic_buttons[1].colors;
        tempColorBlock.normalColor = !autoQualityMode ? selected_color : normal_color;
        autoGraphic_buttons[1].colors = tempColorBlock;

        tempColorBlock = fullscreenMode_buttons[0].colors;
        tempColorBlock.normalColor = Screen.fullScreen ? selected_color : normal_color;
        fullscreenMode_buttons[0].colors = tempColorBlock;

        tempColorBlock = fullscreenMode_buttons[1].colors;
        tempColorBlock.normalColor = Screen.fullScreen ? normal_color : selected_color;
        fullscreenMode_buttons[1].colors = tempColorBlock;

#if UNITY_WEBGL
        for (int i = 0; i < resolution_buttons.Length; i++)
            resolution_buttons[i].interactable = false;
#else
        if (Screen.width == 1280)
        {
            if (Screen.height == 720)
            {
                tempColorBlock = resolution_buttons[0].colors;
                tempColorBlock.normalColor = selected_color;
                resolution_buttons[0].colors = tempColorBlock;
            }
            else if (Screen.width == 1024)
            {
                tempColorBlock = resolution_buttons[3].colors;
                tempColorBlock.normalColor = selected_color;
                resolution_buttons[3].colors = tempColorBlock;
            }

        }
        else if (Screen.width == 1920)
        {
            tempColorBlock = resolution_buttons[1].colors;
            tempColorBlock.normalColor = selected_color;
            resolution_buttons[1].colors = tempColorBlock;
        }
        else if (Screen.width == 2560)
        {
            tempColorBlock = resolution_buttons[2].colors;
            tempColorBlock.normalColor = selected_color;
            resolution_buttons[2].colors = tempColorBlock;
        }
#endif


        totalSlider.value = totalVolume;
        narrationSlider.value = narrationVolume;
        sfxSlider.value = sfxVolume;
        backgroundSlider.value = backgroundVolume;

        totalPercentage.text = (totalSlider.value * 100f).ToString("N0") + "%";
        narrationPercentage.text = (narrationSlider.value * 100f).ToString("N0") + "%";
        sfxPercentage.text = (sfxSlider.value * 100f).ToString("N0") + "%";
        backgroundPercentage.text = (backgroundSlider.value * 100f).ToString("N0") + "%";

        AudioListener.volume = totalSlider.value;
        narration.volume = narrationSlider.value;
        for (int i = 0; i < sfx.Length; i++)
            sfx[i].volume = sfxSlider.value;
        backgroundSound.volume = backgroundSlider.value;
        */
    }

    private void Update()
    {
        // Sound Slider Text
        OnTotalSound();
        OnEffectSound();
        OnEquipSound();
        OnNarrationSound();
        //CommonFunctionManager.Instance.UpdateVolume();
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetFullscreen(false);
        }

#if UNITY_WEBGL
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Return))
                SetFullscreen(true);
        }
#endif

        if (Screen.fullScreen != isFullScreen)
        {
            SetFullscreenButton(Screen.fullScreen);
            isFullScreen = Screen.fullScreen;
        }

        if (!autoQualityMode) return;

        if (countingTimer > countingTime)
        {
            countingTimer = 0;

            if (targetQuality > 0)
            {
                if (frameCounter < refreshRate * minRate * countingTime)
                    SetGraphicQuality(idx2Quality[--targetQuality]);
            }

            if (targetQuality < 2)
            {
                if (frameCounter > refreshRate * maxRate * countingTime)
                    SetGraphicQuality(idx2Quality[++targetQuality]);
            }

            frameCounter = 0;
        }

        countingTimer += Time.deltaTime;

        frameCounter++;
        */
    }

    public void SetGraphicQuality(int quality)
    {
        graphicQuality = quality;
        QualitySettings.SetQualityLevel(quality, true);

        ColorBlock tempColorBlock;

        for (int i = 0; i < graphic_buttons.Length; i++)
        {
            tempColorBlock = graphic_buttons[i].colors;

            tempColorBlock.normalColor = (i == quality2Idx[quality]) ? selected_color : normal_color;
            tempColorBlock.disabledColor = (i == quality2Idx[quality]) ? disabled_selected_color : disabled_normal_color;

            graphic_buttons[i].colors = tempColorBlock;
        }
    }


    public void SetAutoGraphicMode(bool on)
    {
        PlayerPrefs.SetInt("AutoGraphic", on ? 1 : 0);
        autoQualityMode = on;

        /*
        ColorBlock tempColorBlock;

        tempColorBlock = autoGraphic_buttons[0].colors;
        tempColorBlock.normalColor = autoQualityMode ? selected_color : normal_color;
        autoGraphic_buttons[0].colors = tempColorBlock;

        tempColorBlock = autoGraphic_buttons[1].colors;
        tempColorBlock.normalColor = !autoQualityMode ? selected_color : normal_color;
        autoGraphic_buttons[1].colors = tempColorBlock;
        
        for (int i = 0; i < graphic_buttons.Length; i++)
        {
            graphic_buttons[i].interactable = !on;
        }
        */
    }


    public void SetFullscreen()
    {
        isFullScreen = isFullScreen ? false : true;

        FullScreenSwitch.sprite = isFullScreen ? SwitchSprite[0] : SwitchSprite[1];

        Screen.fullScreen = isFullScreen;
    }

    /*
    private void SetFullscreenButton(bool on)
    {
        ColorBlock tempColorBlock;

        tempColorBlock = fullscreenMode_buttons[0].colors;
        tempColorBlock.normalColor = on ? selected_color : normal_color;
        fullscreenMode_buttons[0].colors = tempColorBlock;

        tempColorBlock = fullscreenMode_buttons[1].colors;
        tempColorBlock.normalColor = !on ? selected_color : normal_color;
        fullscreenMode_buttons[1].colors = tempColorBlock;
    }
    */


    public void SetResolution(int resolitionLevel)
    {
        Screen.SetResolution(widthResolutions[resolitionLevel], heightResolutions[resolitionLevel], Screen.fullScreen);

        ColorBlock tempColorBlock;

        for (int i = 0; i < resolution_buttons.Length; i++)
        {
            tempColorBlock = resolution_buttons[i].colors;

            tempColorBlock.normalColor = (i == resolitionLevel) ? selected_color : normal_color;

            resolution_buttons[i].colors = tempColorBlock;
        }
    }

    // 사운드. 아직 사용하지 않음
    public void OnTotalSound()
    {
        TotalPercentage.text = (TotalSlider.value * 100f).ToString("N0");
        totalVolume = TotalSlider.value;
    }

    public void OnNarrationSound()
    {
        NarrationPercentage.text = (NarrationSlider.value * 100f).ToString("N0");
        narrationVolume = NarrationSlider.value;
    }


    public void OnEffectSound()
    {
        EffectPercentage.text = (EffectSlider.value * 100f).ToString("N0");
        effectVolume = EffectSlider.value;
    }

    public void OnEquipSound()
    {
        EquipPercentage.text = (EquipSlider.value * 100f).ToString("N0");
        equipVolume = EquipSlider.value;
    }

    public void SwitchLanguage()
    {
        if(Language == "KR")
            ChangeLanguageEN();
        else
            ChangeLanguageKR();

        if (mainController != null) // 실습
            mainController.scriptController.FindObject();
        else { // 지식
            GameObject.Find("CSVController").GetComponent<CSVController>().FindObject();
            GameObject.Find("Controller").GetComponent<KnowledgeTabController>().LoadTab();
        }
    }

    private void ChangeLanguageKR()
    {
        Language = "KR";
        LanguageSwitch.sprite = SwitchSprite[0];
        //mainController.scriptController.skipEffectBySeq(mainController.GetCurrentSeq());
    }

    private void ChangeLanguageEN()
    {
        Language = "EN";
        LanguageSwitch.sprite = SwitchSprite[1];
        //mainController.scriptController.skipEffectBySeq(mainController.GetCurrentSeq());
    }

    /* Add */
    public void PanelOpen()
    {
        //CommonFunctionManager.Instance.PlayUXSound(SoundManager.UXSound.PanelOpen);
        GetComponent<Animator>().SetTrigger("ConfigOpen");
    }
    public void PanelClose()
    {
        //CommonFunctionManager.Instance.PlayUXSound(SoundManager.UXSound.PanelClose);
        GetComponent<Animator>().SetTrigger("ConfigClose");
    }
}