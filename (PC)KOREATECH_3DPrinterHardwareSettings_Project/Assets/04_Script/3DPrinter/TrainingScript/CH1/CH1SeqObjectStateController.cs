using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CH1SeqObjectStateController : MonoBehaviour
{
    public MainController mainController;

    [Header("Printing")]
    public int printingStartSeqNum;
    public int printingStopSeqNum;
    private bool isPrinting = true;

    [Header("Cover")]
    public Transform Cover;
    public int coverOpenSeqNum;
    public int coverCloseSeqNum;
    public int coverOpenSeqNum2;
    private bool isOpen = true;

    [Header("Tray")]
    public int trayMessageSeqNum;
    private bool viewMessage = true;
    public int trayDownSeqNum;
    public int trayUpSeqNum;
    private bool isDown = true;

    [Header("MaterialCabinet")]
    public int cabinetDoorOpenSeqNum;
    public int materialOutSeqNum;
    public int materialInSeqNum;
    private bool doorOpen = true;
    private bool materialOut = false;

    [Header("Slicer")]
    public int slicerOpenSeqNum;
    public int slicerCloseSeqNum;
    private bool slicerOpen = false;
    public Button[] SlicerBtns;
    public int slicerMoveRotate;
    public int slicerViewCheck;
    public int slicerEstimateTime;
    public int slicerPrinting;
    public int slicerLocateCorrectSeqNum;
    private bool isSlicerPass = false;

    [Header("SlicerPopup")]
    public int slicerPopupOpenSeqNum;
    public int slicerPopupCloseSeqNum;
    private bool isSlicerPopupOpen = false;

    [Header("Ethanol")]
    public int EthanolSeqNum;
    public GameObject Ethanol;
    public GameObject EthanolObj;

    [Header("CleanTowel")]
    public int CleanSeqNum;
    public GameObject CleanObj;

    [Header("ETC")]
    public CameraController cameraController;
    public Button DownBtn;

    [Header("Equip")]
    public int EquipSeqNum;
    private bool equipInit = true;

    IEnumerator PrintingWithWarmingUp()
    {
        yield return new WaitForSeconds(4f);

        MJPASController.Instance.Printing();

        yield return new WaitForSeconds(2f);

        MainController.instance.isRightClick = true;
        MainController.instance.goNextSeq();
        MainController.instance.isRightClick = false;
    }

    void Update()
    {
        
        if (mainController.GetCurrentSeq() == 38 || mainController.GetCurrentSeq() == 45
            || mainController.GetCurrentSeq() == 55 || mainController.GetCurrentSeq() == 66)
        {
            StopAllCoroutines();
            MJPASController.Instance.PrintingStop();
        }
        

        // 카메라 이동 후 약간의 딜레이 후 애니메이션 재생
        if (!cameraController.CamMoveComplete)
        {
            // 프린터블록
            if (mainController.GetCurrentSeq() >= printingStartSeqNum &&
                mainController.GetCurrentSeq() < printingStopSeqNum)
            {
                if (isPrinting)
                {
                    isPrinting = false;
                    MJPASController.Instance.movingObjectsAS.GetComponent<Animator>().StopPlayback();
                    // MJPASController.Instance.Printing();
                    StartCoroutine(PrintingWithWarmingUp());
                }
            }
            else if (mainController.GetCurrentSeq() >= printingStopSeqNum)
            {
                if (!isPrinting)
                {
                    isPrinting = true;
                    MJPASController.Instance.PrintingStop();
                }
            }
            else
            {
                if (!isPrinting)
                {
                    isPrinting = true;
                    MJPASController.Instance.PrintingStop();
                }
            }
        }

        // 트레이
        if (mainController.GetCurrentSeq() >= trayDownSeqNum &&
            mainController.GetCurrentSeq() < trayUpSeqNum)
        {
            if (isDown)
            {
                isDown = false;
                MJPASController.Instance.HeadCleaning();
            }
        }
        else if (mainController.GetCurrentSeq() >= trayUpSeqNum)
        {
            if (!isDown)
            {
                isDown = true;
                MJPASController.Instance.ResetPrinterBlockPosition();
            }
        }
        else
        {
            if (!isDown)
            {
                isDown = true;
                MJPASController.Instance.ResetPrinterBlockPosition();
            }
        }

        // 재료 보관함 materialOut = false
        if (mainController.GetCurrentSeq() >= cabinetDoorOpenSeqNum)
        {
            if (doorOpen)
            {
                doorOpen = false;
                MJPASController.Instance.CabinetDoorOpen();
                materialOut = true;
            }
        }
        /*
        else if (mainController.GetCurrentSeq() >= materialOutSeqNum &&
            mainController.GetCurrentSeq() < materialInSeqNum)
        {
            if (materialOut)
            {
                MJPASController.Instance.MaterialOut(4);
                materialOut = false;
            }
        }
        else if (mainController.GetCurrentSeq() >= materialInSeqNum)
        {
            if (!doorOpen)
            {
                doorOpen = true;
                MJPASController.Instance.MaterialIn(4);
                materialOut = true;
            }
        }*/
        else
        {
            if (!doorOpen)
            {
                MJPASController.Instance.MaterialIn(4);
                MJPASController.Instance.CabinetDoorClose();
            }
            doorOpen = true;
        }

        // 커버
        if (mainController.GetCurrentSeq() >= coverOpenSeqNum &&
            mainController.GetCurrentSeq() < coverCloseSeqNum)
        {
            if (isOpen)
            {
                isOpen = false;
                MJPASController.Instance.CoverOpen();
            }
        }
        else if (mainController.GetCurrentSeq() == coverCloseSeqNum)
        {
            if (!isOpen)
            {
                isOpen = true;
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("프린터 커버를 닫아주세요.", CL_MessagePopUpController.DialogType.NOTICE, CoverClose);
            }
        }
        else if (mainController.GetCurrentSeq() >= coverOpenSeqNum2)
        {
            if (isOpen)
            {
                isOpen = false;
                MJPASController.Instance.CoverOpen();
            }
        }
        else
        {
            if (!isOpen)
            {
                isOpen = true;
                MJPASController.Instance.CoverClose();
            }
        }

        // 트레이 메시지
        if (mainController.GetCurrentSeq() == trayMessageSeqNum)
        {
            if (viewMessage)
            {
                viewMessage = false;
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("트레이가 비어있는지 확인하세요.", CL_MessagePopUpController.DialogType.NOTICE, null);
            }
        }
        else
        {
            viewMessage = true;
        }

        // 슬라이서 및 장비들 일부 제외
        // 슬라이서
        if (mainController.GetCurrentSeq() >= slicerOpenSeqNum && !slicerOpen
            && mainController.GetCurrentSeq() < slicerCloseSeqNum)
        {
            N_SlicerController.instance.SlicerOpen();
            slicerOpen = true;

            if (isSlicerPass)
            {
                N_SlicerController.instance.SlicingStatusInit();
            }

            isSlicerPass = true;
        }
        else if (mainController.GetCurrentSeq() >= slicerCloseSeqNum && slicerOpen)
        {
            N_SlicerController.instance.SlicerClose();
            slicerOpen = false;
        }
        else if(mainController.GetCurrentSeq() < slicerOpenSeqNum && slicerOpen)
        {
            N_SlicerController.instance.SlicerClose();
            slicerOpen = false;
        }

        // 슬라이서 배치 상태
        if(mainController.GetCurrentSeq() == slicerLocateCorrectSeqNum)
        {
            if(N_SlicerController.instance.isCoLocate)
            {
                DownBtn.interactable = true;
            }
            else
            {
                DownBtn.interactable = false;
            }
        }


        // 슬라이서 팝업
        if (mainController.GetCurrentSeq() >= slicerPopupOpenSeqNum && !isSlicerPopupOpen
            && mainController.GetCurrentSeq() < slicerPopupCloseSeqNum)
        {
            N_SlicerController.instance.SlicerPopupOpen();
            isSlicerPopupOpen = true;
        }
        else if (mainController.GetCurrentSeq() >= slicerPopupCloseSeqNum && isSlicerPopupOpen)
        {
            N_SlicerController.instance.SlicerPopupClose();
            isSlicerPopupOpen = false;
        }

        // 슬라이서 버튼
        if (mainController.GetCurrentSeq() >= slicerMoveRotate &&
            N_SlicerController.instance.isCoLocate)
        {
            SlicerBtns[0].interactable = false;
            SlicerBtns[1].interactable = false;
        }
        else if(mainController.GetCurrentSeq() >= slicerMoveRotate)
        {
            SlicerBtns[0].interactable = true;
            SlicerBtns[1].interactable = true;
        }
        else
        {
            SlicerBtns[0].interactable = false;
            SlicerBtns[1].interactable = false;
            N_SlicerController.instance.isCoLocate = false;
        }

        if (mainController.GetCurrentSeq() >= slicerViewCheck)
        {
            SlicerBtns[2].interactable = true;
            SlicerBtns[3].interactable = true;
        }
        else
        {
            SlicerBtns[2].interactable = false;
            SlicerBtns[3].interactable = false;
        }

        if (mainController.GetCurrentSeq() >= slicerEstimateTime)
        {
            SlicerBtns[4].interactable = true;
        }
        else
        {
            SlicerBtns[4].interactable = false;
        }

        if (mainController.GetCurrentSeq() >= slicerPrinting)
        {
            SlicerBtns[5].interactable = true;
        }
        else
        {
            SlicerBtns[5].interactable = false;
        }

        // 장비 착용
        if (EquipList.instance.EquipCheck("Nitril_gloves") &&
            EquipList.instance.EquipCheck("Safety_glasses") &&
            EquipList.instance.EquipCheck("Dustproof_mask") &&
            mainController.GetCurrentSeq() == 40 && !equipInit)
        {
            mainController.goNextSeq();
            equipInit = true;
        }
        else if(mainController.GetCurrentSeq() == 40 && equipInit)
        {
            EquipList.instance.UnEquip();
            equipInit = false;
        }

        if (mainController.GetCurrentSeq() != EthanolSeqNum)
            Ethanol.transform.localPosition = Vector3.zero;
        else if (mainController.GetCurrentSeq() < EthanolSeqNum)
            EthanolObj.SetActive(true);

        if (mainController.GetCurrentSeq() != CleanSeqNum)
            CleanObj.transform.localPosition = Vector3.zero;


    }

    void CoverClose()
    {
        MJPASController.Instance.CoverClose();
    }
}