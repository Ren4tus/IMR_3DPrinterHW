using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPopup : MonoBehaviour
{
    public List<LoginPopupBase> m_Popups;

    public LoginPopupBase FindPopup(LoginPopupBase.PopupType type)
    {
        foreach (var v in m_Popups)
        {
            if (v.m_PopupType == type) { return v; }
        }

        return null;
    }

    public void ActiveRegisterCourse()
    {
        foreach(var v in m_Popups)
        {
            if(v.m_PopupType == LoginPopupBase.PopupType.Error) 
            {
                ErrorPopup errorPopup = v.GetComponent<ErrorPopup>();
                errorPopup.m_RegisterCourse.gameObject.SetActive(true);
            }
        }
    }

}
