using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJPComputerAS : MonoBehaviour
{
    private Animator animator;
    private bool isGUITemperature = true;

    public GameObject GUI_Temperature;
    public GameObject GUI_Material;

    public static MJPComputerAS instance;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (instance == null)
            instance = this;
    }

    public void SlicerOpen()
    {
        PlayButtonSound();
        animator.SetTrigger("SlicerOpen");
    }
    public void SlicerClose()
    {
        PlayButtonSound();
        animator.SetTrigger("SlicerClose");
    }

    public void PrinterLink()
    {
        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("컴퓨터와 프린터가 연결되었습니다.", CL_MessagePopUpController.DialogType.NOTICE);
    }

    // 프린터 상태창
    public void GUIOpen()
    {
        PlayButtonSound();
        animator.SetTrigger("GUIOpen");
    }
    public void GUIClose()
    {
        animator.SetTrigger("GUIClose");
    }
    public void GUIToggle()
    {
        if (isGUITemperature)
        {
            isGUITemperature = false;
            GUI_Material.SetActive(true);
            GUI_Temperature.SetActive(false);
        }
        else
        {
            isGUITemperature = true;
            GUI_Material.SetActive(false);
            GUI_Temperature.SetActive(true);
        }

    }

    // Sound
    public void PlayButtonSound()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.SelectClick);
    }
}
