using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPGUIController : MonoBehaviour
{
    private Animator animator;
    private bool isLoggedin = false;

    // IsLoggedIn 변수를 사용하여 프린터에 로그인이 되어있는지 다른 객체에서 확인이 가능합니다
    public bool IsLoggedIn => isLoggedin;

    public GameObject loggedinPanel;
    public GameObject logoutPanel;

    public Text loginStatusText;
    public InputField userName;
    public InputField userPasswd;

    public string correctName;
    public string correctPasswd;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        loginStatusText.text = "Printer Account";
    }
    
    public void LoginPopupOpen()
    {
        animator.SetTrigger("LoginPopup");
    }
    public void LoginPopupClose()
    {
        animator.SetTrigger("PopupClose");
    }

    public void Login()
    {
        if (userName.text == correctName && userPasswd.text == correctPasswd)
        {
            // 로그인 성공
            CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.TypingComplete);
            loginStatusText.text = correctName + " logged in";

            isLoggedin = true;
            loggedinPanel.SetActive(true);
            logoutPanel.SetActive(false);

            animator.SetTrigger("PopupClose");
        }
        else
        {
            // 로그인 실패
            CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.AccessDenied);
        }
    }
    public void Logout()
    {
        if (isLoggedin == true)
        {
            isLoggedin = false;
            loggedinPanel.SetActive(false);
            logoutPanel.SetActive(true);
        }
    }

    public void GUIOn()
    {
        animator.SetTrigger("GUIOn");
    }
    public void GUIOff()
    {
        animator.SetTrigger("GUIOff");
    }

    public void PlayGUIOnSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.PP_GUIOn);
    }
}
