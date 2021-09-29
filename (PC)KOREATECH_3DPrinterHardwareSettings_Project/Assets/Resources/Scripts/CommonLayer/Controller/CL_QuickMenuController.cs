/*
 * QuickMenuController
 * 
 * 퀵메뉴 작동을 담당하는 스크립트
 * 오브젝트 종속성을 정리
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CL_QuickMenuController : MonoBehaviour
{
    public Animator quickMenuAnim;

    private bool bIsMenuOpen;

    void Start()
    {
        bIsMenuOpen = false;
    }

    // 로고 버튼
    public void ToggleQuickMenu()
    {
        if (bIsMenuOpen)
        {
            quickMenuAnim.SetTrigger("MenuOpen");
        }
        else
        {
            quickMenuAnim.SetTrigger("MenuClose");
        }

        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.QuickMenuToggle);
        bIsMenuOpen = !bIsMenuOpen;
    }

    public void HoverBtn()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.QuickMenuButtonHover);
    }

    // 환경설정
    public void ConfigBtn()
    {
        CL_CommonFunctionManager.Instance.ConfigurationPanelOpen();
    }

    // 도움말
    public void HelpBtn()
    {
        CL_CommonFunctionManager.Instance.HelpPanelOpen();
    }

    // 전체목차
    public void ListBtn()
    {
        CL_CommonFunctionManager.Instance.AllMenuPanelOpen();
    }

    // 메인화면
    public void HomeBtn()
    {
        CL_CommonFunctionManager.Instance.ReturnHome();
    }

    // 종료
    public void ExitBtn()
    {
        CL_CommonFunctionManager.Instance.ExitCommand();
    }


    public void QuickMenuActive()
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    public void QuickMenuInactive()
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
}