/*
 * 기본 GUI 기능 통합 관리 스크립트
 */

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class LoadSceneParams
{
    public static int sequenceNumber { get; set; }
}

public class CL_CommonFunctionManager : MonoBehaviour
{
    private const int CL_UITEXT_POPUP_EXIT = 501;
    private const int CL_UITEXT_POPUP_HOME = 502;

    private static CL_CommonFunctionManager instance = null;
    
    // 컨트롤러는 하나씩만 존재하며, Awake 시에 자동으로 얻어옴
    private CL_LoadSceneController sceneController;
    private CL_SoundManager soundManager;
    private CL_QuickMenuController quickMenu;
    private CL_MessagePopUpController popUpPanel;
    private CL_HelpMePanelController helpmePanel;
    private CL_HelpPanelController helpPanel;
    private CL_ConfigurationController configPanel;
    private CL_AllMenuController allMenuPanel;
    private CL_TutorialPanelController tutorialController;
    private CL_FullScreenPanelController fullscreenController;

    private CL_UILanguageController uiController;

    private string currentSceneName;
    public string CurrentSceneName => currentSceneName;
    public string UIDataCSVPath = "CSVData/UI/CommonLayer";

    [Header("Cursor")]
    public Texture2D cursorDefault;

    // Singleton
    public static CL_CommonFunctionManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Init()
    {
        sceneController = FindObjectOfType(typeof(CL_LoadSceneController)) as CL_LoadSceneController;
        uiController = new CL_UILanguageController(UIDataCSVPath);
        soundManager = FindObjectOfType(typeof(CL_SoundManager)) as CL_SoundManager;
        quickMenu = FindObjectOfType(typeof(CL_QuickMenuController)) as CL_QuickMenuController;
        popUpPanel = FindObjectOfType(typeof(CL_MessagePopUpController)) as CL_MessagePopUpController;
        helpmePanel = FindObjectOfType(typeof(CL_HelpMePanelController)) as CL_HelpMePanelController;
        helpPanel = FindObjectOfType(typeof(CL_HelpPanelController)) as CL_HelpPanelController;
        configPanel = FindObjectOfType(typeof(CL_ConfigurationController)) as CL_ConfigurationController;
        allMenuPanel = FindObjectOfType(typeof(CL_AllMenuController)) as CL_AllMenuController;
        tutorialController = FindObjectOfType(typeof(CL_TutorialPanelController)) as CL_TutorialPanelController;
        fullscreenController = FindObjectOfType(typeof(CL_FullScreenPanelController)) as CL_FullScreenPanelController;

        // Scene Status
        currentSceneName = sceneController.GetSceneName();

        // Sound Initialize
        UpdateVolume();

        // Initialize Static Parameter
        LoadSceneParams.sequenceNumber = -1;
    }

    private void OnDestroy()
    {
        sceneController = null;
        uiController = null;
        soundManager = null;
        quickMenu = null;
        popUpPanel = null;
        helpmePanel = null;
        helpPanel = null;
        configPanel = null;
        allMenuPanel = null;
        tutorialController = null;
        fullscreenController = null;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // 각 컨트롤러는 차일드에 하나만 존재
            Init();

            //씬 전환이 되더라도 파괴되지 않게
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // 만약 씬 이동이 되었다면 새로운 씬의 CL_CommonFunctionManager를 삭제
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SetCursorDefault();
    }

    public string GetCurrentSceneName()
    {
        return sceneController.GetSceneName();
    }

    // 이하 기본 기능, 어떤 씬에서도 Instance로 접근 가능
    public void QuickMenuActive(bool state)
    {
        if (state == true)
        {
            quickMenu.QuickMenuActive();
        }
        else
        {
            quickMenu.QuickMenuInactive();
        }
    }

    // 커서 초기화
    public void SetCursorDefault()
    {
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
    }

    // 풀화면 패널 관련
    public void FullScreenPanelActive()
    {
        fullscreenController.FullScreenPanelActive();
    }
    public void FullScreenPanelInActvie()
    {
        fullscreenController.FullScreenPanelInActive();
    }

    // -------------------------------------------------------
    // 오디오 관련 기능
    // -------------------------------------------------------
    public float TotalVolume()
    {
        return configPanel.TotalVolume;
    }
    public float EffectVolume()
    {
        return configPanel.EffectVolume;
    }
    public float EquipVolume()
    {
        return configPanel.EquipVolume;
    }

    public void SetNarrations(AudioClip[] clips)
    {
        if (clips == null || clips.Length <= 0 || soundManager == null)
            return;
        
        soundManager.narration = clips;
    }

    public float NarrationVolume()
    {
        return configPanel.NarrationVolume;
    }

    public void SetNarrationVolume(float volume)
    {
        soundManager.SetNarrationVolume(volume);
    }
    public void SetEffectVolume(float volume)
    {
        soundManager.SetEffectVolume(volume);
    }
    public void SetEquipVolume(float volume)
    {
        soundManager.SetEquipVolume(volume);
    }
    public void UpdateVolume()
    {
        SetNarrationVolume(NarrationVolume() * TotalVolume());
        SetEffectVolume(EffectVolume() * TotalVolume());
        SetEquipVolume(EquipVolume() * TotalVolume());
    }

    public void StopAllSound()
    {
        soundManager.StopAllSound();
    }

    public void PlayNarration(int idx)
    {
        soundManager.PlayNarration(idx);
    }

    // SBFramework 전용
    public void LoadNarrationFile(string path)
    {
        soundManager.LoadSound(path);
    }
    public void StopNarration()
    {
        soundManager.StopNarration();
    }

    public bool IsPlayingNarration()
    {
        return soundManager.IsPlayingNarration();
    }

    public void PlayUXSound(CL_SoundManager.UXSound type)
    {
        soundManager.PlayUXSound(type);
    }
    public void StopUXSound()
    {
        soundManager.StopUXSound();
    }

    public void PlayEquipSound(CL_SoundManager.EquipSound type)
    {
        soundManager.PlayEquipSound(type);
    }
    public void StopEquipSound()
    {
        soundManager.StopEquipSound();
    }

    public float TypingSpeed()
    {
        return configPanel.TypingSpeed;
    }

    // 메세지 팝업 기능
    public CL_MessagePopUpController MakePopUp()
    {
        return popUpPanel;
    }

    // 씬 이동
    public void LoadScene(string sceneName)
    {
        StopNarration();
        sceneController.LoadScene(sceneName);
    }

    // 씬 이동 with parameter
    public void LoadTrain1SceneWithParameter(int param)
    {
        StopNarration();
        LoadSceneParams.sequenceNumber = param;
        sceneController.LoadScene("TrainingCH1");
    }

    public void LoadTrain2SceneWithParameter(int param)
    {
        StopNarration();
        LoadSceneParams.sequenceNumber = param;
        sceneController.LoadScene("TrainingCH2");
    }

    public void LoadTrain3SceneWithParameter(int param)
    {
        StopNarration();
        LoadSceneParams.sequenceNumber = param;
        sceneController.LoadScene("TrainingCH3");
    }

    // 씬 이동 AllMenuPanel 사용
    public void LoadSceneWithPopup(string sceneName)
    {
        allMenuPanel.MakePopUpAndSceneMove(sceneName);
    }

    // 환경설정
    public void ConfigurationPanelOpen()
    {
        configPanel.PanelOpen();
    }
    public string GetProgramLanguage()
    {
        return configPanel.Language;
    }
    public bool UseTutorial()
    {
        return configPanel.isUseTutorial;
    }
    public bool UseScript()
    {
        return configPanel.isUseScript;
    }

    public string GetUIText(int index)
    {
        return uiController.GetUIData(index);
    }
    public void SetResolution()
    {
        configPanel.ResolutionSet();
    }
    public void SetFullScreen()
    {
        configPanel.SetFullscreen();
    }

    // 도움말
    public void HelpPanelOpen()
    {
        helpPanel.PanelOpen();
    }
    public void HelpPanelTutorialOpen(int num)
    {
        helpPanel.PanelOpen();
        helpPanel.JumpTab(num);
    }

    // 튜토리얼
    public void TutorialPanelOpen(int num)
    {
        tutorialController.PanelOpen(num);
    }

    // HelpMe 패널, string으로 단어를 식별합니다
    public void HelpMePanelOpen(string word)
    {
        helpmePanel.SetHelpText(word);
    }

    // 전체목차
    public void AllMenuPanelOpen()
    {
        allMenuPanel.PanelOpen();
    }

    // 메인화면
    public void ReturnHome()
    {
        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp(uiController.GetUIData(CL_UITEXT_POPUP_HOME),
                                                                CL_MessagePopUpController.DialogType.NOTICE,
                                                                () => {
                                                                    sceneController.LoadScene("IntroMain_Next");
                                                                },
                                                                null);
    }

    // 종료
    public void ExitCommand()
    {
        popUpPanel.YesOrNoPopUp(uiController.GetUIData(CL_UITEXT_POPUP_EXIT), CL_MessagePopUpController.DialogType.NOTICE, EndProgram, null);
    }
    
    //Callbacks
    public void EndProgram()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}