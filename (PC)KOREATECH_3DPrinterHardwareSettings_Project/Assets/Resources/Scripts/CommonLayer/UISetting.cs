/*
 * 퀵메뉴를 활성화/비활성화하고 싶은 씬이 있다면, 씬 내 아무 오브젝트에 이 스크립트를 붙이면 됩니다. (CommonLayer 외)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISetting : MonoBehaviour
{
    public bool useQuickMenu;

    void Start()
    {
        CL_CommonFunctionManager.Instance.QuickMenuActive(useQuickMenu);
        CL_CommonFunctionManager.Instance.FullScreenPanelInActvie();

#if !UNITY_WEBGL_PRINTER
        if(SceneManager.GetActiveScene().name.Contains("Intro"))
            CL_CommonFunctionManager.Instance.QuickMenuActive(false);
        else
            CL_CommonFunctionManager.Instance.QuickMenuActive(true);
#else
        CL_CommonFunctionManager.Instance.QuickMenuActive(false);
        CL_CommonFunctionManager.Instance.FullScreenPanelActive();
#endif
    }
}