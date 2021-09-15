using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceChangeController : MonoBehaviour
{
    //현재 2단원만 구현 상태, 나머지 단원도 똑같은 방식으로 구현 필요
    private int initialSeq = 0;

    public CameraController cameraController;
    public MaterialController materialController;
    public ObjectController objectController;
    //public PopupController popupController;
    public CSVController scriptController;
    public MainController mainController;
    public ColliderController colliderController;

    [Space]
    public Button[] button;

    public void firstSceneInit()
    {
        //구현된 단원만 버튼 활성화하고 나머지는 비활성화, 이후 구현을 통해 더 활성화할 것
        /*
        for(int i = 0; i < button.Length; i++)
        {
            if (i == 1)
                button[i].enabled = true;
            else
                button[i].enabled = false;
        }
        */
    }

    public void ChangeSeq(int number)
    {
        button[number].onClick.AddListener(MovetoSeq);
    }

    public void MovetoSeq()
    {
        cameraController.setCameraBySeq(mainController.GetCurrentSeq(), initialSeq);
        materialController.setOnOffBySeq(mainController.GetCurrentSeq(), initialSeq);
        materialController.hideComponent(initialSeq);
        objectController.setOnOffBySeq(mainController.GetCurrentSeq(), initialSeq);
        //popupController.viewPopupButton(initialSeq);
        //popupController.viewPopupBySeq(initialSeq);
        scriptController.viewScriptBySeq(initialSeq);
        colliderController.activateCollider(mainController.GetCurrentSeq(), initialSeq);
        //popupController.popupPanel2.SetActive(false);
        mainController.SetCurrentSeq(initialSeq);
    }
}
