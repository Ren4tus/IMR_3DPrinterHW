using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PBFUIController : MonoBehaviour
{
    private enum ViewType
    {
        Isometric,
        Side,
        Front,
        Top
    }

    [Header("3D Contents")]
    public Transform Scultures;

    [Header("Tooltip")]
    public PracticeToolTipController ToolTipController;
    public HoverHighlightController[] HoverObjects;

    [Header("Cursor")]
    public Texture2D cursorDefault;
    public Texture2D cursorMove;
    public Texture2D cursorRotate;

    [Header("GUI")]
    public Button recoaterBtn;
    public Button statusBtn;
    public Button slicerBtn;
    public Button fastFowardBtn;
    public Button partExtractBtn;

    [Header("Slicer")]
    public AnimatedUI StatusPanel;
    public AnimatedUI SlicerPanel;
    public AnimatedUI SlicerPopupPanel;
    [Space]
    public Text MaterialStatusText;
    public Text ModelViewText;
    public Button[] SlicerButtons;
    public Button[] SlicerViewButtons;
    private Dictionary<string, Button> FindSlicerButtons;

    [Space]
    public int estimateTimeEX1 = 10;
    public int estimateTimeEX2 = 15;

    [Header("Printing Status")]
    public AnimatedUI PrintingStautsPanel;
    public Slider progressSlider;
    public Text textCurrentStatus;
    public Text textProgress;
    public Text textElapsedTime;
    public Text textCurrentZ;
    public Text textTotalZ;

    private IEnumerator coroutineTimer = null;
    private float progressPercent = 0.0f;
    private int elapsedTime = 1;
    private float currentZ = 0.0f;
    private float totalZ = 0.0f;

    public Transform slicerObjects;
    public Collider slicerTrayRange;
    public SlicerObjectController[] slicerSculptures;
    public Transform[] printedSculptures;

    [Space]
    public Camera IsometricCamera;
    public Camera TopCamera;
    public Camera SideCamera;
    public Camera FrontCamera;

    [Space]
    [Header("Slicer Popup")]
    public Button loadModelBtn;
    public Toggle modelEX1;
    public Toggle modelEX2;

    private bool isPassCollocate = false;
    private int estimateTime = 0;
    private int warmingUpTime = 1;
    private ViewType currentView = ViewType.Isometric;

    private bool isStatusOpen = false;
    private bool isSlicerOpen = false;
    private bool isSlicerPopupOpen = false;
    private bool isModelImported = false;
    private bool isPrintingStatusOpen = false;

    public bool canMove = false;

    public static PBFUIController instance;

    private void Awake()
    {
        FindSlicerButtons = new Dictionary<string, Button>();

        for (int i = 0; i < SlicerButtons.Length; i++)
        {
            FindSlicerButtons.Add(SlicerButtons[i].name, SlicerButtons[i]);
        }

        for(int i=0; i<SlicerViewButtons.Length; i++)
        {
            FindSlicerButtons.Add(SlicerViewButtons[i].name, SlicerViewButtons[i]);
        }

        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        InactiveSlicerObjects();
        InactiveSlicerBtn();
        InactiveSlicerBtns();

        InactivePrinterStatusBtn();
        InactiveLoadModelBtn();
        InactiveRecoaterBtn();
        InactiveFastFowardBtn();
        InactivePartExtractBtn();

        SetCursorDefault();
    }

    public void ToolTipOn()
    {
        ToolTipController.isShow = true;
        HighlighterOn(HoverObjects);
    }
    public void ToolTipOff()
    {
        ToolTipController.isShow = false;
        HighlighterOff(HoverObjects);
    }

    // 컴퓨터 GUI
    public void ActivePartExtractBtn()
    {
        partExtractBtn.interactable = true;
    }
    public void InactivePartExtractBtn()
    {
        partExtractBtn.interactable = false;
    }
    public void StatusOpen()
    {
        if (!isStatusOpen)
        {
            isStatusOpen = true;
            StatusPanel.FadeIn();
        }
    }
    public void StatusClose()
    {
        if (isStatusOpen)
        {
            isStatusOpen = false;
            StatusPanel.FadeOut();
        }
    }

    public void StatusChangeNeedMaterial()
    {
        MaterialStatusText.text = "부족";
        MaterialStatusText.color = Color.red;
    }
    public void StatusChangepFullMaterial()
    {
        MaterialStatusText.text = "정상";
        MaterialStatusText.color = new Color32(94, 171, 93, 255);
    }

    public void SlicerOpen()
    {
        if (!isSlicerOpen)
        {
            isSlicerOpen = true;
            SlicerPanel.FadeIn();

            if (isModelImported)
                ActiveSlicerObjects();
        }
    }
    public void SlicerClose()
    {
        if (isSlicerOpen)
        {
            isSlicerOpen = false;

            InactiveSlicerObjects();
            SetCursorDefault();
            SlicerPopupClose();

            SlicerPanel.FadeOut();
        }
    }

    public void SlicerPopupOpen()
    {
        if (!isSlicerPopupOpen)
        {
            if (isModelImported)
            {
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("이미 모델을 불러왔습니다.", CL_MessagePopUpController.DialogType.NOTICE);
                return;
            }

            isSlicerPopupOpen = true;
            SlicerPopupPanel.FadeIn();
        }
    }
    public void SlicerPopupClose()
    {
        if (isSlicerPopupOpen)
        {
            isSlicerPopupOpen = false;
            SlicerPopupPanel.FadeOut();
        }
    }
    public void SlicerPopupModelImportBtn()
    {
        if (!modelEX1.isOn || !modelEX2.isOn)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("모든 모델을 선택해야 합니다.", CL_MessagePopUpController.DialogType.NOTICE);
            return;
        }

        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("[EX1.STL], [EX2.STL]\n모델을 불러오시겠습니까?",
                                                                    CL_MessagePopUpController.DialogType.NOTICE,
                                                                    ModelImportAndNext,
                                                                    null);
    }

    public void ModelImportUndo()
    {

        isModelImported = false;
        isPassCollocate = false;

        currentView = ViewType.Front;
        ChangeView();

        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            slicerSculptures[i].ResetPosition();
            slicerSculptures[i].gameObject.SetActive(false);
        }

        slicerObjects.gameObject.SetActive(false);

        // 초기 뷰 : 등각

        SlicerPopupOpen();
    }

    public void ModelImport()
    {
        if (!isModelImported)
        {
            SlicerPopupClose();

            estimateTime = estimateTimeEX1 + estimateTimeEX2;

            // 초기 뷰 : 등각
            currentView = ViewType.Front;
            ChangeView();

            for (int i = 0; i < slicerSculptures.Length; i++)
            {
                slicerSculptures[i].ResetPosition();
                slicerSculptures[i].CollocatedOff();
                slicerSculptures[i].gameObject.SetActive(true);
                slicerSculptures[i].rotateCount = 0;
            }

            slicerObjects.gameObject.SetActive(true);

            isModelImported = true;
            isPassCollocate = false;
        }
    }

    public void ModelImportAndNext()
    {
        ModelImport();
        PracticeSceneController.Instance.NextStep();
    }

    public void CollocatePassInit()
    {
        isPassCollocate = false;
    }
    public void CheckSlicerCollocate()
    {
        bool isAllCollocated = true;

        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            if (SlicerCollocateCheck.IsTargetInsideRange(slicerTrayRange, slicerSculptures[i].GetCollider()))
            {
                // 정상적으로 배치된 상태
                slicerSculptures[i].CollocatedOn();
            }
            else
            {
                isAllCollocated = false;
                slicerSculptures[i].CollocatedOff();
            }
        }

        if (isAllCollocated && !isPassCollocate)
        {
            isPassCollocate = true;
            PrintedSculpturePositionSet();
            PracticeSceneController.Instance.NextStep();
        }
    }

    public void PrintedSculpturePositionSet()
    {
        printedSculptures[0].localPosition = slicerSculptures[0].gameObject.transform.localPosition;
        printedSculptures[0].rotation = slicerSculptures[0].gameObject.transform.rotation;

        printedSculptures[1].localPosition = slicerSculptures[1].gameObject.transform.localPosition;
        printedSculptures[1].rotation = slicerSculptures[0].gameObject.transform.rotation;
    }

    public void CheckSlicingComplte()
    {
        // 학습자가 슬라이싱을 하지 않고 단계이동으로 넘어갔을 경우, 미리 지정해둔 위치로 이동(자동 배치)
        bool isAllCollocated = true;

        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            if (SlicerCollocateCheck.IsTargetInsideRange(slicerTrayRange, slicerSculptures[i].GetCollider()))
            {
                // 정상적으로 배치된 상태
                slicerSculptures[i].CollocatedOn();
            }
            else
            {
                isAllCollocated = false;
                slicerSculptures[i].CollocatedOff();
            }
        }
        
        if (isAllCollocated)
        {
            isPassCollocate = true;
            PrintedSculpturePositionSet();
        }

        if (!isPassCollocate)
        {
            slicerSculptures[0].gameObject.transform.localPosition = new Vector3(0.0496f, 0.0304f, 0.0351f);
            slicerSculptures[0].gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            slicerSculptures[0].CollocatedOn();

            slicerSculptures[1].gameObject.transform.localPosition = new Vector3(-0.00474f, 0.052f, 0.0361f);
            slicerSculptures[1].gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            slicerSculptures[1].CollocatedOn();

            PrintedSculpturePositionSet();
            isPassCollocate = true;
        }
    }
    public void SlicingStatusInit()
    {
        isPassCollocate = false;
    }

    public void EstimateTimePopup()
    {
        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("예상 프린팅 시간 " + estimateTime + ":00:00", CL_MessagePopUpController.DialogType.NOTICE);
    }

    public void ActiveSlicerMoveBtn()
    {
        FindSlicerButtons["BtnMove"].interactable = true;
    }
    public void InactiveSlicerMoveBtn()
    {
        FindSlicerButtons["BtnMove"].interactable = false;
    }

    public void ActiveSlicerRotateBtn()
    {
        FindSlicerButtons["BtnRotate"].interactable = true;
    }
    public void InactiveSlicerRotateBtn()
    {
        FindSlicerButtons["BtnRotate"].interactable = false;
    }

    public void ActiveSlicerViewBtn()
    {
        // FindSlicerButtons["BtnView"].interactable = true;

        for(int i=0; i<SlicerViewButtons.Length; i++)
        {
            FindSlicerButtons[SlicerViewButtons[i].name].interactable = true;
        }
    }
    public void InactiveSlicerViewBtn()
    {
        // FindSlicerButtons["BtnView"].interactable = false;

        for (int i = 0; i < SlicerViewButtons.Length; i++)
        {
            FindSlicerButtons[SlicerViewButtons[i].name].interactable = false;
        }
    }

    // 내부 채움 옵션
    public void ActiveSlicerInternalBtn()
    {
        //FindSlicerButtons["BtnInternalFill"].interactable = true;
    }
    public void InactiveSlicerInternalBtn()
    {
        //FindSlicerButtons["BtnInternalFill"].interactable = false;
    }

    // 배치 확인
    public void ActiveSlicerCheckBtn()
    {
        FindSlicerButtons["BtnCheck"].interactable = true;
    }
    public void InactiveSlicerCheckBtn()
    {
        FindSlicerButtons["BtnCheck"].interactable = false;
    }

    // 프린팅 시간 계산
    public void ActiveSlicerEstimateBtn()
    {
        FindSlicerButtons["BtnEstimateTime"].interactable = true;
    }
    public void InactiveSlicerEstimateBtn()
    {
        FindSlicerButtons["BtnEstimateTime"].interactable = false;
    }

    // 프린팅
    public void ActiveSlicerPrintingBtn()
    {
        FindSlicerButtons["BtnPrinting"].interactable = true;
    }
    public void InactiveSlicerPrintingBtn()
    {
        FindSlicerButtons["BtnPrinting"].interactable = false;
    }

    public void InactiveSlicerBtns()
    {
        for (int i = 0; i < SlicerButtons.Length; i++)
            SlicerButtons[i].interactable = false;

        for (int i = 0; i < SlicerViewButtons.Length; i++)
        {
            FindSlicerButtons[SlicerViewButtons[i].name].interactable = false;
        }
    }

    // Printing Status
    public void ActivePrinterStatusBtn()
    {
        statusBtn.interactable = true;
    }
    public void InactivePrinterStatusBtn()
    {
        statusBtn.interactable = false;
    }

    public void PrintingStatusOpen()
    {
        if (!isPrintingStatusOpen)
        {
            isPrintingStatusOpen = true;
            PrintingStautsPanel.FadeIn();
        }
    }
    public void PrintingStatusClose()
    {
        if (isPrintingStatusOpen)
        {
            isPrintingStatusOpen = false;
            PrintingStautsPanel.FadeOut();
        }
    }

    public void SetStatusWarmingUp()
    {
        StopAllCoroutines();

        textCurrentStatus.text = "Warming Up";
        textCurrentStatus.color = new Color32(247, 150, 71, 255);
        textCurrentZ.text = "";
        textTotalZ.text= "";

        // 워밍업은 1시간
        warmingUpTime = 1;
        elapsedTime = 0;

        ProgressUpdate(true);
        ElapsedTimeUpdate();
    }
    public void SetStatusPrinting()
    {
        StopAllCoroutines();

        textCurrentStatus.text = "Printing";
        textCurrentStatus.color = new Color32(71, 175, 247, 255);

        elapsedTime = 0;

        ProgressUpdate(false);
        ElapsedTimeUpdate();
    }
    public void SetStatusComplete()
    {
        StopAllCoroutines();

        textCurrentStatus.text = "Complete";
        textCurrentStatus.color = new Color32(94, 171, 93, 255);

        elapsedTime = estimateTime * 3600;

        ProgressUpdate(false);
        ElapsedTimeUpdate();
    }

    public void WarmingUpStart()
    {
        SetStatusWarmingUp();
        coroutineTimer = Co_WarmingUpNormal();
        StartCoroutine(coroutineTimer);
    }
    public void WarmingUpQuick()
    {
        StopAllCoroutines();
        coroutineTimer = Co_WarmingUpQuick(5f);
        StartCoroutine(coroutineTimer);
    }

    public void PrintingStart()
    {
        SetStatusPrinting();
        coroutineTimer = Co_PrintingNormal();
        StartCoroutine(coroutineTimer);
    }
    public void PrintingQuick()
    {
        StopAllCoroutines();
        coroutineTimer = Co_PrintingQuick(10f);
        StartCoroutine(coroutineTimer);
    }

    public void ProgressUpdate(bool isWarmingUp)
    {
        if (elapsedTime <= 0)
        {
            textProgress.text = "0%";
            progressSlider.value = 0;
            return;
        }

        int percentage = elapsedTime / estimateTime / 36;

        if (isWarmingUp)
            percentage = elapsedTime / warmingUpTime / 36;

        textProgress.text = percentage.ToString() + "%";
        progressSlider.value = percentage;
    }

    public void ElapsedTimeUpdate()
    {
        if (elapsedTime <= 0)
        {
            textElapsedTime.text = "00:00:00";
            return;
        }

        StringBuilder builder = new StringBuilder();

        int hour = elapsedTime / 3600;
        int minute = elapsedTime % 3600 / 60;
        int second = elapsedTime % 3600 % 60;

        builder.Append(hour.ToString("D2"));
        builder.Append(":");
        builder.Append(minute.ToString("D2"));
        builder.Append(":");
        builder.Append(second.ToString("D2"));

        textElapsedTime.text = builder.ToString();
        builder.Clear();
    }

    IEnumerator Co_WarmingUpNormal()
    {
        int TotalTicks = warmingUpTime * 3600;

        elapsedTime = 0;

        while (TotalTicks > elapsedTime)
        {
            elapsedTime += 1;
            ElapsedTimeUpdate();
            ProgressUpdate(true);
            yield return new WaitForSeconds(1);
        }
        
        SetStatusPrinting();
    }
    IEnumerator Co_WarmingUpQuick(float time)
    {
        int totalTicks = warmingUpTime * 3600;
        float timer = 0.0f;

        elapsedTime = 0;

        while (timer < time)
        {
            timer += Time.deltaTime;
            elapsedTime = (int)((float)totalTicks * timer / time);

            ElapsedTimeUpdate();
            ProgressUpdate(true);

            yield return null;
        }
        
        PracticeSceneController.Instance.NextStep();
    }

    IEnumerator Co_PrintingNormal()
    {
        int TotalTicks = estimateTime * 3600;

        elapsedTime = 0;

        while (TotalTicks > elapsedTime)
        {
            elapsedTime += 1;
            ElapsedTimeUpdate();
            ProgressUpdate(false);
            yield return new WaitForSeconds(1f);
        }
        
        PracticeSceneController.Instance.NextStep();
    }
    IEnumerator Co_PrintingQuick(float time)
    {
        int totalTicks = estimateTime * 3600;
        float timer = 0.0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            elapsedTime = (int)((float)totalTicks * timer / time);

            ElapsedTimeUpdate();
            ProgressUpdate(false);

            yield return null;
        }
        
        PracticeSceneController.Instance.NextStep();
    }

    public void ActiveLoadModelBtn()
    {
        loadModelBtn.interactable = true;
    }
    public void InactiveLoadModelBtn()
    {
        loadModelBtn.interactable = false;
    }

    public void ActiveRecoaterBtn()
    {
        recoaterBtn.interactable = true;
    }
    public void InactiveRecoaterBtn()
    {
        recoaterBtn.interactable = false;
    }

    public void ActiveSlicerBtn()
    {
        slicerBtn.interactable = true;
    }
    public void InactiveSlicerBtn()
    {
        slicerBtn.interactable = false;
    }
    public void ActiveFastFowardBtn()
    {
        fastFowardBtn.GetComponent<Collider>().enabled = true;
        fastFowardBtn.interactable = true;
    }
    public void InactiveFastFowardBtn()
    {
        fastFowardBtn.GetComponent<Collider>().enabled = false;
        fastFowardBtn.interactable = false;
    }

    // Cursor
    public void ExitEditMode()
    {
        SetCursorDefault();

        canMove = false;

        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            slicerSculptures[i].EditModeExit();
        }
    }
    public void SetCursorDefault()
    {
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
    }
    public void SetCursorMove()
    {
        Cursor.SetCursor(cursorMove, new Vector2(cursorMove.width / 2, cursorMove.height / 2), CursorMode.Auto);
    }
    public void SetCursorRotate()
    {
        canMove = false;

        Cursor.SetCursor(cursorRotate, new Vector2(cursorRotate.width / 2, cursorRotate.height / 2), CursorMode.Auto);
    }
    public void SetMove()
    {
        SetCursorMove();
        canMove = true;
    }
    // 3D Viewer
    public void ActiveSculptureViewer()
    {
        Scultures.gameObject.SetActive(true);
    }
    public void InactiveSculptureViewer()
    {
        Scultures.gameObject.SetActive(false);
    }

    public void ActiveSlicerObjects()
    {
        slicerObjects.gameObject.SetActive(true);
    }
    public void InactiveSlicerObjects()
    {
        slicerObjects.gameObject.SetActive(false);
    }

    public void ActiveSlicerSculpture()
    {
        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            slicerSculptures[i].gameObject.SetActive(true);
        }
    }
    public void InactiveSlicerSculpture()
    {
        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            slicerSculptures[i].gameObject.SetActive(false);
        }
    }

    public void ActiveIsometricView()
    {
        IsometricCamera.gameObject.SetActive(true);
    }
    public void InactiveIsometricView()
    {
        IsometricCamera.gameObject.SetActive(false);
    }

    public void ActiveTopView()
    {
        TopCamera.gameObject.SetActive(true);
    }
    public void InactiveTopView()
    {
        TopCamera.gameObject.SetActive(false);
    }

    public void ActiveSideView()
    {
        SideCamera.gameObject.SetActive(true);
    }
    public void InactiveSideView()
    {
        SideCamera.gameObject.SetActive(false);
    }
    public void ActiveFrontView()
    {
        FrontCamera.gameObject.SetActive(true);
    }
    public void InactiveFrontView()
    {
        FrontCamera.gameObject.SetActive(false);
    }


    // Prop
    public void HighlighterOff(HoverHighlightController[] hoverHighlights)
    {
        for (int i = 0; i < hoverHighlights.Length; i++)
        {
            hoverHighlights[i].HighlighterInit();
            hoverHighlights[i].IsInteractive = false;
        }
    }
    public void HighlighterOn(HoverHighlightController[] hoverHighlights)
    {
        for (int i = 0; i < hoverHighlights.Length; i++)
            hoverHighlights[i].IsInteractive = true;
    }

    public void BtnIsometicView()
    {
        currentView = ViewType.Isometric;

        ModelViewText.text = "모델뷰 : 등각";

        ActiveIsometricView();
        InactiveTopView();
        InactiveSideView();
        InactiveFrontView();

        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            slicerSculptures[i].SetCamera(IsometricCamera);
            slicerSculptures[i].SetViewNum(0);
        }
    }

    public void BtnFrontView()
    {
        currentView = ViewType.Front;

        ModelViewText.text = "모델뷰 : 정면";

        InactiveIsometricView();
        InactiveTopView();
        InactiveSideView();
        ActiveFrontView();

        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            slicerSculptures[i].SetCamera(FrontCamera);
            slicerSculptures[i].SetViewNum(1);
        }
    }

    public void BtnSideView()
    {
        currentView = ViewType.Side;

        ModelViewText.text = "모델뷰 : 측면";

        InactiveIsometricView();
        InactiveTopView();
        ActiveSideView();
        InactiveFrontView();

        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            slicerSculptures[i].SetCamera(SideCamera);
            slicerSculptures[i].SetViewNum(2);
        }
    }

    public void BtnTopView()
    {
        currentView = ViewType.Top;

        ModelViewText.text = "모델뷰 : 평면";

        InactiveIsometricView();
        ActiveTopView();
        InactiveSideView();
        InactiveFrontView();

        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            slicerSculptures[i].SetCamera(TopCamera);
            slicerSculptures[i].SetViewNum(3);
        }
    }

    public void ChangeView()
    {
        int i;

        switch (currentView)
        {
            case ViewType.Isometric:
                currentView = ViewType.Top;

                ModelViewText.text = "모델뷰 : 평면";

                InactiveIsometricView();
                ActiveTopView();
                InactiveSideView();
                InactiveFrontView();

                for (i = 0; i < slicerSculptures.Length; i++)
                    slicerSculptures[i].SetCamera(TopCamera);
                break;

            case ViewType.Top:
                currentView = ViewType.Side;

                ModelViewText.text = "모델뷰 : 측면";

                InactiveIsometricView();
                InactiveTopView();
                ActiveSideView();
                InactiveFrontView();

                for (i = 0; i < slicerSculptures.Length; i++)
                    slicerSculptures[i].SetCamera(SideCamera);
                break;

            case ViewType.Side:
                currentView = ViewType.Front;

                ModelViewText.text = "모델뷰 : 정면";

                InactiveIsometricView();
                InactiveTopView();
                InactiveSideView();
                ActiveFrontView();

                for (i = 0; i < slicerSculptures.Length; i++)
                    slicerSculptures[i].SetCamera(FrontCamera);
                break;

            case ViewType.Front:
                currentView = ViewType.Isometric;

                ModelViewText.text = "모델뷰 : 등각";

                ActiveIsometricView();
                InactiveTopView();
                InactiveSideView();
                InactiveFrontView();

                for (i = 0; i < slicerSculptures.Length; i++)
                    slicerSculptures[i].SetCamera(IsometricCamera);
                break;
        }
    }
}
