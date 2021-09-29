using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlicerController : MonoBehaviour
{
    protected enum ViewType
    {
        Isometric,
        Side,
        Front,
        Top
    }
    protected enum InternalOptions
    {
        Empty,
        Light,
        Basic,
        Heavy
    }
    protected string[] InternalOptionTexts = { "내부채움 :\n속빈(<color='#699CB0'>0%</color>)",
                                             "내부채움 :\n가볍게(<color='#699CB0'>25%</color>)",
                                             "내부채움 :\n보통(<color='#699CB0'>50%</color>)",
                                             "내부채움 :\n무겁게(<color='#699CB0'>100%</color>)"};

    [Header("Cursor")]
    public Texture2D cursorDefault;
    public Texture2D cursorMove;
    public Texture2D cursorRotate;

    [Header("Slicer")]
    public AnimatedUI SlicerPanel;
    public AnimatedUI SlicerPopupPanel;

    [Space]
    public Text InternalOptionText;
    public Text ModelViewText;
    public Button[] SlicerButtons;
    protected Dictionary<string, Button> FindSlicerButtons;

    [Space]
    [Header("슬라이서 옵션을 설정합니다.")]
    [Tooltip("[0] EX1, [1] EX2, [2] EX1 & EX2")]
    public int ImportOption = 2;
    public int estimateTimeEX1 = 10;
    public int estimateTimeEX2 = 15;

    public Collider slicerTrayRange;
    public Transform slicerObjects;
    public Transform[] printedSculptures;
    public SlicerObjectController[] slicerSculptures;

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

    protected bool isPassCollocate = false;
    protected ViewType currentView = ViewType.Isometric;
    protected InternalOptions currentInternalOption = InternalOptions.Basic;

    protected bool isSlicerOpen = false;
    protected bool isSlicerPopupOpen = false;
    protected bool isModelImported = false;

    protected void Awake()
    {
        if (FindSlicerButtons == null)
        {
            FindSlicerButtons = new Dictionary<string, Button>();

            for (int i = 0; i < SlicerButtons.Length; i++)
            {
                FindSlicerButtons.Add(SlicerButtons[i].name, SlicerButtons[i]);
            }
        }
    }

    protected void Start()
    {
        InactiveSlicerObjects();

        SetCursorDefault();

        InternalOptionText.text = InternalOptionTexts[(int)currentInternalOption];
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

            SetCursorDefault();
            InactiveSlicerObjects();

            SlicerPanel.FadeOut();
        }
    }

    public void SlicerPopupOpen()
    {
        if (!isSlicerPopupOpen)
        {
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
        switch (ImportOption)
        {
            case 0:
                if (modelEX1.isOn)
                {
                    CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("[EX1.STL]모델을 불러오시겠습니까?",
                                                                                CL_MessagePopUpController.DialogType.NOTICE,
                                                                                ModelImport,
                                                                                null);
                }
                break;
            case 1:
                if (modelEX2.isOn)
                {
                    CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("[EX2.STL]모델을 불러오시겠습니까?",
                                                                                CL_MessagePopUpController.DialogType.NOTICE,
                                                                                ModelImport,
                                                                                null);
                }
                break;
            case 2:
                if (modelEX1.isOn && modelEX2)
                {
                    CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("[EX1.STL], [EX2.STL]\n모델을 불러오시겠습니까?",
                                                                            CL_MessagePopUpController.DialogType.NOTICE,
                                                                            ModelImport,
                                                                            null);
                }
                break;
        }
    }

    public void ModelImport()
    {
        SlicerPopupClose();

        // 초기 뷰 : 등각
        currentView = ViewType.Front;
        ChangeView();

        // 초기 내부채움 : 기본(50%)
        currentInternalOption = InternalOptions.Basic;

        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            slicerSculptures[i].ResetPosition();
            slicerSculptures[i].CollocatedOff();
            slicerSculptures[i].gameObject.SetActive(true);
        }

        slicerObjects.gameObject.SetActive(true);

        isModelImported = true;
        isPassCollocate = false;
    }

    public void CheckSlicerCollocate()
    {
        bool isAllCollocated = true;
        
        switch (ImportOption)
        {
            case 0:
                if (SlicerCollocateCheck.IsTargetInsideRange(slicerTrayRange, slicerSculptures[0].GetCollider()))
                {
                    slicerSculptures[0].CollocatedOn();
                    printedSculptures[0].localPosition = slicerSculptures[0].gameObject.transform.localPosition;
                    printedSculptures[0].rotation = slicerSculptures[0].gameObject.transform.rotation;
                }
                else
                {
                    isAllCollocated = false;
                    slicerSculptures[0].CollocatedOff();
                }
                break;
            case 1:
                if (SlicerCollocateCheck.IsTargetInsideRange(slicerTrayRange, slicerSculptures[1].GetCollider()))
                {
                    slicerSculptures[1].CollocatedOn();
                    printedSculptures[1].localPosition = slicerSculptures[1].gameObject.transform.localPosition;
                    printedSculptures[1].rotation = slicerSculptures[11].gameObject.transform.rotation;
                }
                else
                {
                    isAllCollocated = false;
                    slicerSculptures[1].CollocatedOff();
                }
                break;
            case 2:
                for (int i = 0; i < slicerSculptures.Length; i++)
                {
                    if (SlicerCollocateCheck.IsTargetInsideRange(slicerTrayRange, slicerSculptures[i].GetCollider()))
                    {
                        slicerSculptures[i].CollocatedOn();
                        printedSculptures[i].localPosition = slicerSculptures[i].gameObject.transform.localPosition;
                        printedSculptures[i].rotation = slicerSculptures[i].gameObject.transform.rotation;
                    }
                    else
                    {
                        isAllCollocated = false;
                        slicerSculptures[i].CollocatedOff();
                    }
                }
                break;
        }

        // 모두 정렬된 상태
        if (isAllCollocated)
            isPassCollocate = true;
    }

    public void SlicingStatusInit()
    {
        for (int i = 0; i < slicerSculptures.Length; i++)
        {
            slicerSculptures[i].ResetPosition();
            slicerSculptures[i].CollocatedOff();
            slicerSculptures[i].gameObject.SetActive(false);
        }

        slicerObjects.gameObject.SetActive(false);

        isModelImported = false;
        isPassCollocate = false;
    }

    public void EstimateTimePopup()
    {
        switch (ImportOption)
        {
            case 0:
                if (isModelImported)
                    CL_CommonFunctionManager.Instance.MakePopUp().PopUp("예상 프린팅 시간 " + estimateTimeEX1 + ":00:00", CL_MessagePopUpController.DialogType.NOTICE);
                break;
            case 1:
                if (isModelImported)
                    CL_CommonFunctionManager.Instance.MakePopUp().PopUp("예상 프린팅 시간 " + estimateTimeEX1 + ":00:00", CL_MessagePopUpController.DialogType.NOTICE);
                break;
            case 2:
                if (isModelImported)
                    CL_CommonFunctionManager.Instance.MakePopUp().PopUp("예상 프린팅 시간 " + (estimateTimeEX1 + estimateTimeEX2) + ":00:00", CL_MessagePopUpController.DialogType.NOTICE);
                break;
        }

    }
    
    // Cursor
    public void ExitEditMode()
    {
        SetCursorDefault();

        for (int i = 0; i < slicerSculptures.Length; i++)
            slicerSculptures[i].EditModeExit();
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
        Cursor.SetCursor(cursorRotate, new Vector2(cursorRotate.width / 2, cursorRotate.height / 2), CursorMode.Auto);
    }

    public void ActiveSlicerObjects()
    {
        if (slicerObjects != null)
            slicerObjects.gameObject.SetActive(true);
    }
    public void InactiveSlicerObjects()
    {
        if (slicerObjects != null)
            slicerObjects.gameObject.SetActive(false);
    }

    public void ActiveSlicerSculpture()
    {
        for (int i = 0; i < slicerSculptures.Length; i++)
            slicerSculptures[i].gameObject.SetActive(true);
    }
    public void InactiveSlicerSculpture()
    {
        for (int i = 0; i < slicerSculptures.Length; i++)
            slicerSculptures[i].gameObject.SetActive(false);
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
    public void ChangeInternalOption()
    {
        currentInternalOption = (InternalOptions)(((int)currentInternalOption + 1) % 4);
        InternalOptionText.text = InternalOptionTexts[(int)currentInternalOption];
    }
}