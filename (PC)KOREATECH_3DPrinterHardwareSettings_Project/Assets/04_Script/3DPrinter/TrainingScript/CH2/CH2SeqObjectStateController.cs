using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CH2SeqObjectStateController : MonoBehaviour
{
    public MainController maincontroller;

    [Header("ChamberDoorRatate")]
    public Animation ChamberDoorUpper;
    public Animation ChamberDoorLower;
    public int chamberDoorOpenSeqNum;
    public int chamberDoorCloseSeqNum;
    public int chamberDoorLowerOpenSeqNum;
    public int chamberDoorLowerCloseSeqNum;
    public int chamberDoorUpperOpenSeqNum;
    private bool onceOpen = true;

    [Space]
    [Header("MaterialCartIn")]
    public Animation MaterialCart;
    public int materialCartInSeqNum;
    private Vector3 materialCartPos = new Vector3(-7.17f, 0.093f, -4.15f);

    /*
    [Space]
    [Header("ChamberPin")]
    public Animation ChamberPin;
    public int chamberPinSeqNum;
    private Vector3 chamberPinPos = new Vector3(0f, -0.012f, -0.3635f);

    [Space]
    [Header("ConnectorIn")]
    public Animation Connector;
    public int connectorSeqNum;
    private Vector3 connectorPos = new Vector3(0f, 0.03f, 0f);
    */

    [Space]
    [Header("BuildPlatform")]
    public Animation BuildPlat;
    public int buildPlatSeqNum;
    public int buildPlatOutSeqNum; // 여기 49번 시퀀스
    private bool isBuildPlatPlay = true;
    private bool isBuildPlatOutPlay = true;

    [Space]
    [Header("PowerSwtich")]
    public Animation PowerSwitch;
    public int powerSwitchSeqNum;
    private bool powerSwitchOn = false;
    private Vector3 powerSwtichRot = new Vector3(-90f, 0f, 180f);

    [Space]
    [Header("MaterialHolder")]
    public Animation MaterialHolder;
    public int materialHolderOutSeqNum;
    public int materialHolderInSeqNum;
    private Vector3 materialHolderPos = new Vector3(0.1926369f, -0.03081208f, 0.2500948f);

    /*
    [Space]
    [Header("MaterialBottle")]
    public Animation MaterialBottle;
    public int materialBottleOutSeqNum;
    private bool materialBottleOut = false;
    private Vector3 materialBottlePos = new Vector3(-0.01106823f, -0.198715f, 0.1812082f);
    */

    [Space]
    [Header("ResinAwl")]
    public Animation ResinAwl;
    public int resinAwlSeqNum;

    [Space]
    [Header("Resin")]
    public Animation Resin;
    public int resinSeqNum;
    private Vector3 resinPos = new Vector3(-8.949f, 0.81f, -1.053f);

    [Space]
    [Header("Resin")]
    public Animation ResinIn;
    public int resinInSeqNum;
    private Vector3 resinInPosBefore = new Vector3(-0.011f, -0.2f, 0.17f);
    private Vector3 resinInPosAfter = new Vector3(-0.011f, -0.2f, -0.04f);

    [Space]
    [Header("MaterialEmpty")]
    public int materialEmptyAlertSeqNum;
    private bool alertPopup = true;

    [Space]
    [Header("USB")]
    public Animation USB;
    public int usbComputerInSeqNum;
    public int usbPrinterInSeqNum;
    private bool isInsert = true;

    [Header("Slicer")]
    public int slicerOpenSeqNum;
    public int slicerCloseSeqNum;
    private bool slicerOpen = false;
    public Button[] SlicerBtns;
    public int slicerMoveRotate;
    public int slicerViewCheck;
    public int slicerEstimateTime;
    public int slicerPrinting;
    private bool isSlicerPass = false;

    [Header("SlicerPopup")]
    public int slicerPopupOpenSeqNum;
    public int slicerPopupCloseSeqNum;
    private bool isSlicerPopupOpen = false;

    [Header("Scraper")]
    public Animation Scraper;
    public int scraperUseSeqNum;

    [Header("Clipper")]
    public Animation Clipper;
    public int clipperUseSeqNum;

    [Header("Grinder, HandFile, SandPaper")]
    public Animation GHS;
    public int ghsUseSeqNum;

    [Header("Brush")]
    public Animation Brush;
    public int brushUseSeqNum;
    /*
    [Header("Grinder")]
    public Animation Grinder;
    public int grinderUseSeqNum;

    [Header("SandPaper")]
    public Animation SandPaper;
    public int sandPaperUseSeqNum;

    [Header("HandFile")]
    public Animation HandFile;
    public int handFileUseSeqNum;
    */
    void Update()
    {
        // 챔버
        if (maincontroller.GetCurrentSeq() >= chamberDoorOpenSeqNum&&
            maincontroller.GetCurrentSeq() < chamberDoorCloseSeqNum)
        {
            if (ChamberDoorUpper.transform.localRotation.y <= 0f)
            {
                ChamberDoorUpper.Play("ChamberDoorOpen");
                ChamberDoorLower.Play("ChamberDoorOpen");
            }
            onceOpen = true;
        }
        else if(maincontroller.GetCurrentSeq() >= chamberDoorCloseSeqNum &&
            maincontroller.GetCurrentSeq() < chamberDoorLowerOpenSeqNum)
        {
            if (ChamberDoorUpper.transform.localRotation.y >= 0.6f)
                ChamberDoorUpper.Play("ChamberDoorClose");
            if (ChamberDoorLower.transform.localRotation.y >= 0.6f)
                ChamberDoorLower.Play("ChamberDoorClose");
            onceOpen = true;
        }
        else if(maincontroller.GetCurrentSeq() >= chamberDoorLowerOpenSeqNum && 
            maincontroller.GetCurrentSeq() < chamberDoorLowerCloseSeqNum)
        {
            /*
            if(ChamberDoorLower.transform.localRotation.y <= 0f)
            {
                if(maincontroller.GetCurrentSeq() == chamberDoorLowerCloseSeqNum-1)
                {
                    onceOpen = true;
                }
                else
                {

                }
                    ChamberDoorLower.Play("ChamberDoorOpen");
            }
            */
            if (onceOpen)
            {
                onceOpen = false;
                ChamberDoorLower.Play("ChamberDoorOpen");
            }
        }
        else if(maincontroller.GetCurrentSeq() >= chamberDoorLowerCloseSeqNum && 
            maincontroller.GetCurrentSeq() < chamberDoorUpperOpenSeqNum)
        {
            if (ChamberDoorUpper.transform.localRotation.y >= 0.6f)
                ChamberDoorUpper.Play("ChamberDoorClose");
            if (ChamberDoorLower.transform.localRotation.y >= 0.6f)
                ChamberDoorLower.Play("ChamberDoorClose");
            onceOpen = true;
        }
        else if(maincontroller.GetCurrentSeq() >= chamberDoorUpperOpenSeqNum)
        {
            if(ChamberDoorUpper.transform.localRotation.y <= 0f)
            {
                ChamberDoorUpper.Play("ChamberDoorOpen");
            }
            onceOpen = true;
        }
        else
        {
            if (ChamberDoorUpper.transform.localRotation.y >= 0.6f)
                ChamberDoorUpper.Play("ChamberDoorClose");
            if (ChamberDoorLower.transform.localRotation.y >= 0.6f)
                ChamberDoorLower.Play("ChamberDoorClose");
            onceOpen = true;
        }

        // 재료카트
        if(maincontroller.GetCurrentSeq() >= materialCartInSeqNum)
        {
            if(MaterialCart.transform.localPosition.x > -8f)
                MaterialCart.Play("MaterialCartIn");
        }
        else
        {
            MaterialCart.transform.localPosition = materialCartPos;
        }

        // 챔버핀
        /*
        if(maincontroller.GetCurrentSeq() >= chamberPinSeqNum)
        {
            if(ChamberPin.transform.localPosition.z > -0.38f)
            {
                ChamberPin.gameObject.SetActive(true);
                ChamberPin.Play("ChamberPin");
            }
        }
        else
        {
            ChamberPin.transform.localPosition = chamberPinPos;
            ChamberPin.gameObject.SetActive(false);
        }
        */

        // 커넥터
        /*
        if(maincontroller.GetCurrentSeq() >= connectorSeqNum)
        {
            if(Connector.transform.localPosition.y >= 0.025f)
            {
                Connector.Play("ConnectorIn");
            }
        }
        else
        {
            Connector.transform.localPosition = connectorPos;
        }
        */

        // 빌드플랫폼
        /*
        if (maincontroller.GetCurrentSeq() < buildPlatSeqNum)
        {
            isBuildPlatPlay = true;
            BuildPlat.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            BuildPlat.gameObject.transform.GetChild(0).position = new Vector3(0.085f, 1.051f, -0.06529331f);
        }
        else if (maincontroller.GetCurrentSeq() == buildPlatSeqNum)
        {
            if (isBuildPlatPlay)
           {
                isBuildPlatPlay = false;
                BuildPlat.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                BuildPlat.Play("BuildPlatform");
            }
        }
        */
        if(maincontroller.GetCurrentSeq() >= buildPlatSeqNum &&
            maincontroller.GetCurrentSeq() < buildPlatOutSeqNum)
        {
            isBuildPlatPlay = true;
            isBuildPlatOutPlay = true;
            BuildPlat.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            BuildPlat.gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0.085f, 1.051f, -0.06529f);
            BuildPlat.Play("BuildPlatformInit");
        }
        else if(maincontroller.GetCurrentSeq() >= buildPlatOutSeqNum)
        {
            if (isBuildPlatOutPlay)
            {
                isBuildPlatOutPlay = false;
                BuildPlat.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                BuildPlat.Play("BuildPlatformOut");
            }
        }
        /*
        else
        {
            BuildPlat.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        */

        // 파워스위치
        if(maincontroller.GetCurrentSeq() >= powerSwitchSeqNum)
        {
            if(!powerSwitchOn)
            {
                powerSwitchOn = true;
                PowerSwitch.Play("PowerSwitch");
            }
        }
        else
        {
            powerSwitchOn = false;
            PowerSwitch.transform.localRotation = Quaternion.Euler(powerSwtichRot);
        }

        // 재료홀더
        if(maincontroller.GetCurrentSeq() >= materialHolderOutSeqNum &&
            maincontroller.GetCurrentSeq() < materialHolderInSeqNum)
        {
            if(MaterialHolder.transform.localPosition.y >= -0.3f)
            {
                MaterialHolder.Play("MaterialHolderOut");
            }
        }
        else if(maincontroller.GetCurrentSeq() >= materialHolderInSeqNum)
        {
            if(MaterialHolder.transform.localPosition.y < -0.3f)
                MaterialHolder.Play("MaterialHolderIn");
        }
        else
        {
            MaterialHolder.transform.localPosition = materialHolderPos;
        }

        /*
        // 재료병
        if(maincontroller.GetCurrentSeq() == materialBottleOutSeqNum)
        {
            if(!materialBottleOut)
            {
                MaterialBottle.gameObject.SetActive(true);
                MaterialBottle.Play("MaterialBottleOut");
                materialBottleOut = true;
            }
        }
        else if(maincontroller.GetCurrentSeq() >= materialBottleOutSeqNum + 1)
        {
            MaterialBottle.gameObject.SetActive(false);
        }
        else
        {
            MaterialBottle.gameObject.SetActive(true);
            MaterialBottle.transform.localPosition = materialBottlePos;
            materialBottleOut = false;
        }
        */

        /*
        // Awl
        if (maincontroller.GetCurrentSeq() == resinAwlSeqNum)
        {
            ResinAwl.gameObject.transform.localPosition = new Vector3(-8.846f, 0.855f, -1.018f);
            ResinAwl.gameObject.transform.localRotation = Quaternion.Euler(-90.0f, 180.0f, 0.0f);
        }
        else if (maincontroller.GetCurrentSeq() == resinAwlSeqNum + 1)
        {
            if (Mathf.Abs(ResinAwl.transform.rotation.eulerAngles.x) < 1.0f &&
                ResinAwl.transform.localPosition.y < 1.16f)
            {
                ResinAwl.gameObject.SetActive(false);
            }
            else
            {
                ResinAwl.Play("ResinAwl");
            }
        }
        */

        // 레진
        /*
        if (maincontroller.GetCurrentSeq() >= resinSeqNum)
        {
            if(Resin.transform.localPosition.z >= -3f)
            {
                Resin.Play("ResinAnim");
            }
        }
        else
        {
            Resin.transform.localPosition = resinPos;
        }
        */

        // ResinIn
        if (maincontroller.GetCurrentSeq() == resinInSeqNum)
        {
            if (ResinIn.transform.localPosition.z >= -0.03f)
            {
                ResinIn.Play("ResinIn");
            }
        }
        else if (maincontroller.GetCurrentSeq() < resinInSeqNum)
        {
            ResinIn.transform.localPosition = resinInPosBefore;
        }
        else
        {
            ResinIn.transform.localPosition = resinInPosAfter;
        }

        // 재료부족 팝업
        if (maincontroller.GetCurrentSeq() == materialEmptyAlertSeqNum)
        {
            if(alertPopup)
            {
                alertPopup = false;
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("재료가 부족합니다.",CL_MessagePopUpController.DialogType.NOTICE, null);
            }
        }
        else
        {
            alertPopup = true;
        }

        // USB
        if(maincontroller.GetCurrentSeq() >= usbComputerInSeqNum &&
            maincontroller.GetCurrentSeq() < usbPrinterInSeqNum)
        {
            if(isInsert)
            {
                isInsert = false;
                // USB.Play("USB_Computer");
                USB.gameObject.transform.localPosition = Vector3.zero;
            }
        }
        else if(maincontroller.GetCurrentSeq() >= usbPrinterInSeqNum)
        {
            if(!isInsert)
            {
                isInsert = true;
                USB.Play("USB_PrinterIn");
            }
        }
        else
        {
            isInsert = true;
        }

        // 슬라이서
        if (maincontroller.GetCurrentSeq() >= slicerOpenSeqNum && !slicerOpen
            && maincontroller.GetCurrentSeq() < slicerCloseSeqNum)
        {
            N_SlicerController.instance.SlicerOpen();
            slicerOpen = true;

            if (isSlicerPass)
            {
                N_SlicerController.instance.SlicingStatusInit();
            }

            isSlicerPass = true;
        }
        else if (maincontroller.GetCurrentSeq() >= slicerCloseSeqNum && slicerOpen)
        {
            N_SlicerController.instance.SlicerClose();
            slicerOpen = false;
        }
        else if (maincontroller.GetCurrentSeq() < slicerOpenSeqNum && slicerOpen)
        {
            N_SlicerController.instance.SlicerClose();
            slicerOpen = false;
        }

        if (maincontroller.GetCurrentSeq() < slicerOpenSeqNum || maincontroller.GetCurrentSeq() >= slicerCloseSeqNum)
        {
            N_SlicerController.instance.SetCursorDefault();
        }

        // 슬라이서 팝업
        if (maincontroller.GetCurrentSeq() >= slicerPopupOpenSeqNum && !isSlicerPopupOpen
            && maincontroller.GetCurrentSeq() < slicerPopupCloseSeqNum)
        {
            N_SlicerController.instance.SlicerPopupOpen();
            isSlicerPopupOpen = true;
        }
        else if (maincontroller.GetCurrentSeq() >= slicerPopupCloseSeqNum && isSlicerPopupOpen)
        {
            N_SlicerController.instance.SlicerPopupClose();
            isSlicerPopupOpen = false;
        }
        else if(maincontroller.GetCurrentSeq() < slicerPopupOpenSeqNum && isSlicerPopupOpen)
        {
            N_SlicerController.instance.SlicerPopupClose();
            isSlicerPopupOpen = false;
        }

        // 슬라이서 버튼
        if (maincontroller.GetCurrentSeq() >= slicerMoveRotate)
        {
            SlicerBtns[0].interactable = true;
            SlicerBtns[1].interactable = true;
        }
        else
        {
            SlicerBtns[0].interactable = false;
            SlicerBtns[1].interactable = false;
        }

        if (maincontroller.GetCurrentSeq() >= slicerViewCheck)
        {
            SlicerBtns[6].interactable = true;
            SlicerBtns[7].interactable = true;
            SlicerBtns[8].interactable = true;
            SlicerBtns[9].interactable = true;
        }
        else
        {
            SlicerBtns[6].interactable = false;
            SlicerBtns[7].interactable = false;
            SlicerBtns[8].interactable = false;
            SlicerBtns[9].interactable = false;
        }

        if (maincontroller.GetCurrentSeq() >= slicerEstimateTime) // 38
        {
            SlicerBtns[3].interactable = true;
            SlicerBtns[4].interactable = true;
        }
        else if(maincontroller.GetCurrentSeq() < slicerEstimateTime &&
            N_SlicerController.instance.isCoLocate)
        {
            N_SlicerController.instance.SlicingStatusBeforeCheck();
            SlicerBtns[3].interactable = false;
            SlicerBtns[4].interactable = false;
        }
        else
        {
            SlicerBtns[3].interactable = false;
            SlicerBtns[4].interactable = false;
        }

        if (maincontroller.GetCurrentSeq() >= slicerPrinting)
        {
            SlicerBtns[5].interactable = true;
        }
        else
        {
            SlicerBtns[5].interactable = false;
        }

        // Scraper
        if (maincontroller.GetCurrentSeq() == scraperUseSeqNum)
        {
            Scraper.Play("scraper_moving_legacy");
        }

        // Clipper
        if (maincontroller.GetCurrentSeq() == clipperUseSeqNum)
        {
            Clipper.Play("use_clipper");
        }

        // Brush
        if (maincontroller.GetCurrentSeq() == brushUseSeqNum)
        {
            Brush.Play("brush");
        }

        // Grinder, Handfile, Sandpaper
        if (maincontroller.GetCurrentSeq() == ghsUseSeqNum)
        {
            GHS.Play("grin_hand_sand");
        }
        else
        {
            GHS.Stop("grin_hand_sand");
        }

        /*
        // 그라인더
        if (maincontroller.GetCurrentSeq() >= grinderUseSeqNum)
        {

        }

        // 사포
        if(maincontroller.GetCurrentSeq() >= sandPaperUseSeqNum)
        {

        }

        // 줄
        if(maincontroller.GetCurrentSeq() >= handFileUseSeqNum)
        {

        }
        */
    }
}