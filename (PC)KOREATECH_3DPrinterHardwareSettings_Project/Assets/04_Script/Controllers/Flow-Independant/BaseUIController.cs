using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIController : MonoBehaviour
{
    [Header("Base Menu")]
    public bool baseMenuOn = false;
    public Animator baseMenuAnim;

    [Header("Base Menu Panel Control")]
    public GameObject HomePanel;
    public GameObject HelpPanel;
    public GameObject TotalMenuPanel;
    public GameObject ConfigurationPanel;
    public GameObject ExitPanel;

    [Header("Total Menu Control")]
    public LoadSceneController loadSceneController;
    public GameObject ChapMovePanel;
    public Text ChapMoveText;
    private string ChapterText;
    private string ChapterSceneName;

    public void ConfigPanelOnOff(bool b)
    {
        ConfigurationPanel.SetActive(b);
    }

    public void HomePanelOnOff(bool b)
    {
        HomePanel.SetActive(b);
    }

    public void HelpPanelOnOff(bool b)
    {
        HelpPanel.SetActive(b);
    }

    public void TotalMenuPanelOnOff(bool b)
    {
        TotalMenuPanel.SetActive(b);
    }

    public void ExitPanelOnOff(bool b)
    {
        ExitPanel.SetActive(b);
    }

    public void ExitButtonYes()
    {
        Application.Quit();
    }

    public void BaseMenuOnOffToggle()
    {
        baseMenuOn = !baseMenuOn;
        baseMenuAnim.SetBool("enable", baseMenuOn);
    }
    private void BaseMenuOnOff(bool on)
    {
        baseMenuOn = on;
        baseMenuAnim.SetBool("enable", on);
    }

    // 전체 메뉴 패널 수정 메소드
    public void OnChapMovePanel(string ChapName, string SceneName)
    {
        ChapterText = ChapName;
        ChapterSceneName = SceneName;
        ChapMoveText.text = "[<color=orange>" + ChapterText + "</color>]" + "\n" + "학습으로 이동하시겠습니까?";
        ChapMovePanel.SetActive(true);
    }

    public void ClickChapMoveYes()
    {
        loadSceneController.LoadScene(ChapterSceneName);
    }

    public void OffChapMovePanel()
    {
        ChapMovePanel.SetActive(false);
    }
}
