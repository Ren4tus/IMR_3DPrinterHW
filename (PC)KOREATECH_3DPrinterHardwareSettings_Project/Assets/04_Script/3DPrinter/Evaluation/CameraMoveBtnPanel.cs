using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMoveBtnPanel : MonoBehaviour
{
    private Animator ani;

    public Image ToggleBtn;
    public Sprite[] toggleImg;
    private bool isOpen = false;

    public Transform mainCamera;

    public Transform[] BtnTransform;

    void Start()
    {
        ani = this.GetComponent<Animator>();
    }

    public void ChangeToggleBtn()
    {
        if(isOpen)
        {
            ani.SetTrigger("PanelClose");
            ToggleBtn.sprite = toggleImg[0];
            isOpen = false;
        }
        else
        {
            ani.SetTrigger("PanelOpen");
            ToggleBtn.sprite = toggleImg[1];
            isOpen = true;
        }
    }

    public void MoveCamera(int n)
    {
        CameraController.instance.setCameraByTransform(mainCamera, BtnTransform[n]);
        ChangeToggleBtn();
    }

    public void JumpSeq(int num)
    {
        MainController.instance.GoJumpSeq(num);
        ChangeToggleBtn();
    }
}
