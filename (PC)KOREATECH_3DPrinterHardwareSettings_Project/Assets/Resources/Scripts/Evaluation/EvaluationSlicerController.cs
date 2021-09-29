using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationSlicerController : MonoBehaviour
{
    public enum ViewType
    {
        Isometric,
        Side,
        Front,
        Top
    }
    public enum InternalOptions
    {
        Empty,
        Light,
        Basic,
        Heavy
    }

    protected string[] InternalOptionTexts = { "내부채움 :\n속빈(<color='#699CB0'>0%</color>)",
                                             "내부채움 :\n가볍게(<color='#699CB0'>25%</color>)",
                                             "내부채움 :\n기본(<color='#699CB0'>50%</color>)",
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

    [Space]
    public Button BtnModelImport;
    public Button BtnMove;
    public Button BtnRotate;
    public Button BtnInternalOption;
    public Button BtnView;
    public Button BtnCollocateCheck;
    public Button BtnEstimate;
    public Button BtnPrinting;
    public Button[] BtnViews;

    [Space]
    // 스패너, 임펠러 중 하나 선택됨
    public int SlicerSequence;
    public int SlicerSteps;

    public int SlicerSequence2;
    public int SlicerSteps2;

    public int PrintingSequence;
    public int PrintingSteps;

    [Space]
    public int ImportOption;
    public bool UseInternalOption = true;
    public int InternalOption = 2;
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

    public bool isPassCollocate = false;
    public ViewType currentView = ViewType.Isometric;
    public InternalOptions currentInternalOption = InternalOptions.Basic;

    public bool isSlicerOpen = false;
    public bool isSlicerPopupOpen = false;
    public bool isModelImported = false;

    public static EvaluationSlicerController instance;

    public bool canMove = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    protected void Start()
    {
        ImportOption = EvaluationSceneController.Instance.AssignmentModel;
        // InternalOption = EvaluationSceneController.Instance.AssignmentInternalOption;

        BlockAllBtn();
        BtnModelImport.interactable = true;

        InactiveSlicerObjects();
        SetCursorDefault();

        if (UseInternalOption)
        {
            InternalOptionText.text = InternalOptionTexts[(int)currentInternalOption];
        }
        else
        {
            BtnInternalOption.gameObject.SetActive(false);
        }
    }

    public void BlockAllBtn()
    {
        BtnModelImport.interactable = false;
        BtnMove.interactable = false;
        BtnRotate.interactable = false;
        BtnInternalOption.interactable = false;
        BtnView.interactable = false;
        BtnCollocateCheck.interactable = false;
        BtnEstimate.interactable = false;
        BtnPrinting.interactable = false;

        foreach (Button btn in BtnViews)
            btn.interactable = false;
    }
    public void ReleaseAllBtn()
    {
        BtnModelImport.interactable = true;
        BtnMove.interactable = true;
        BtnRotate.interactable = true;
        BtnInternalOption.interactable = true;
        BtnView.interactable = true;
        BtnCollocateCheck.interactable = true;
        BtnEstimate.interactable = true;
        BtnPrinting.interactable = true;

        foreach (Button btn in BtnViews)
            btn.interactable = true;
    }

    public void SlicerOpen()
    {
        if (!isSlicerOpen)
        {
            isSlicerOpen = true;
            SlicerPanel.FadeIn();

            if (isModelImported)
                ActiveSlicerObjects();

            EvaluationSceneController.Instance.CameraControllOff();
        }
    }
    public void SlicerClose()
    {
        if (isSlicerOpen)
        {
            isSlicerOpen = false;

            SetCursorDefault();
            InactiveSlicerObjects();

            SlicerPopupClose();
            SlicerPanel.FadeOut();

            EvaluationSceneController.Instance.CameraControllOn();
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
        if (modelEX1.isOn)
        {
            if (ImportOption.Equals(1))
            {
                ModelImport();
            }
            else
            {
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("선택된 모델이 평가 과제 모델과 다릅니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
            }
        }
        else if (modelEX2.isOn)
        {
            if (ImportOption.Equals(2))
            {
                ModelImport();
            }
            else
            {
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("선택된 모델이 평가 과제 모델과 다릅니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
            }
        }
        else
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("불러올 모델을 선택해 주세요.", CL_MessagePopUpController.DialogType.NOTICE, null);
        }
    }

    public void ModelImport()
    {
        SlicerPopupClose();
        BtnModelImport.interactable = false;

        BtnMove.interactable = true;
        BtnRotate.interactable = true;
        if (UseInternalOption)
            BtnInternalOption.interactable = true;
        BtnView.interactable = true;
        BtnCollocateCheck.interactable = true;
        BtnEstimate.interactable = true;
        BtnPrinting.interactable = false;

        foreach (Button btn in BtnViews)
            btn.interactable = true;

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

        ActiveSlicerObjects();

        isModelImported = true;
        isPassCollocate = false;

        //Screen Space - Camera모드를 사용하면 Plane Distance 값을 지정하는데 이때
        //var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100.0f); // z값을 Plane Distance 값을 줘야 합니다!! transform.position = uiCamera.ScreenToWorldPoint(screenPoint); // 그리고 좌표 변환을 하면 끝!
    }

    public void CheckSlicerCollocate()
    {
        bool isAllCollocated = true;

        switch (ImportOption)
        {
            default:
                if (SlicerCollocateCheck.IsTargetInsideRange(slicerTrayRange, slicerSculptures[0].GetCollider()))
                {
                    slicerSculptures[0].CollocatedOn();
                    // printedSculptures[0].localPosition = slicerSculptures[0].gameObject.transform.localPosition;
                    // printedSculptures[0].rotation = slicerSculptures[0].gameObject.transform.rotation;
                }
                else
                {
                    isAllCollocated = false;
                    slicerSculptures[0].CollocatedOff();
                }
                break;

                /*
            case 2:
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
                break;*/
        }

        // 모두 정렬된 상태
        if (isAllCollocated)
        {
            isPassCollocate = true;

            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("배치가 완료되었습니다.", 
                                                                CL_MessagePopUpController.DialogType.NOTICE, 
                                                                ()=> {
                                                                    EvaluationSceneController.Instance.CompleteStep(SlicerSequence, SlicerSteps);
                                                                });

            BtnMove.interactable = false;
            BtnRotate.interactable = false;
            if (UseInternalOption)
                BtnInternalOption.interactable = true;

            BtnView.interactable = true;
            BtnCollocateCheck.interactable = false;
            BtnEstimate.interactable = true;
            BtnPrinting.interactable = true;
        }
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

    public void SlicerSkip()
    {
        SlicerOpen();
        ModelImport();

        slicerSculptures[0].transform.localPosition = new Vector3(0f, 0f, 0.035f);
        InternalOption = EvaluationSceneController.Instance.AssignmentInternalOption;
        InternalOptionText.text = InternalOptionTexts[(int)EvaluationSceneController.Instance.AssignmentInternalOption];

        slicerSculptures[0].CollocatedOn();
        EvaluationSceneController.Instance.CompleteStep(SlicerSequence, SlicerSteps);

        BtnMove.interactable = false;
        BtnRotate.interactable = false;
        if (UseInternalOption)
            BtnInternalOption.interactable = false;
        BtnView.interactable = false;
        BtnCollocateCheck.interactable = false;
        BtnEstimate.interactable = false;
        BtnPrinting.interactable = true;

        isPassCollocate = true;

        // CheckSlicerCollocate();        

        EstimateTimePopup();
    }

    public void PrintingSkip()
    {
        SlicerClose();
        BlockAllBtn();

        EvaluationSceneController.Instance.CompleteStep(PrintingSequence, PrintingSteps);

        EvaluationSceneController.Instance.isSkip = false;

        if (EvaluationCH1PrintingInterface.instance != null)
            EvaluationCH1PrintingInterface.instance.PrintStart();

        if (EvaluationCH3Controller.Instance != null)
            EvaluationCH3Controller.Instance.WarmingUpStart();
    }

    public void EstimateTimePopup()
    {
        if(EvaluationSceneController.Instance.AssignmentInternalOption != InternalOption)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("평가 과제의 내부채움 조건을 확인해 주세요.", CL_MessagePopUpController.DialogType.NOTICE, null);
            return;
        }

        switch (ImportOption)
        {
            case 1:
                if (isPassCollocate)
                {
                    CL_CommonFunctionManager.Instance.MakePopUp().PopUp("예상 프린팅 시간 " + estimateTimeEX1 + ":00:00",
                                                                        CL_MessagePopUpController.DialogType.NOTICE,
                                                                        () =>
                                                                        {
                                                                            EvaluationSceneController.Instance.CompleteStep(SlicerSequence2, SlicerSteps2);

                                                                            EvaluationSceneController.Instance.isSkip = false;
                                                                        });
                }
                else
                {
                    CL_CommonFunctionManager.Instance.MakePopUp().PopUp("먼저 모델을 배치해주세요.", CL_MessagePopUpController.DialogType.NOTICE);
                }
                break;
            case 2:
                if (isPassCollocate)
                {
                    CL_CommonFunctionManager.Instance.MakePopUp().PopUp("예상 프린팅 시간 " + estimateTimeEX2 + ":00:00",
                                                                        CL_MessagePopUpController.DialogType.NOTICE,
                                                                        () =>
                                                                        {
                                                                            EvaluationSceneController.Instance.CompleteStep(SlicerSequence2, SlicerSteps2);

                                                                            EvaluationSceneController.Instance.isSkip = false;
                                                                        });
                }
                else
                {
                    CL_CommonFunctionManager.Instance.MakePopUp().PopUp("먼저 모델을 배치해주세요.", CL_MessagePopUpController.DialogType.NOTICE);
                }
                break;
            /*
            case 2:
                if (isModelImported)
                    CL_CommonFunctionManager.Instance.MakePopUp().PopUp("예상 프린팅 시간 " + (estimateTimeEX1 + estimateTimeEX2) + ":00:00", CL_MessagePopUpController.DialogType.NOTICE);
                break;
           */
        }

    }

    public void Printing()
    {
        if (isPassCollocate)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("프린팅을 시작합니다.",
                                                                CL_MessagePopUpController.DialogType.NOTICE,
                                                                () => {
                                                                    EvaluationSceneController.Instance.CompleteStep(PrintingSequence, PrintingSteps);

                                                                    EvaluationSceneController.Instance.isSkip = false;
                                                                });
            BlockAllBtn();
            SlicerClose();
        }
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

    public void ChangeInternalOption()
    {
        currentInternalOption = (InternalOptions)(((int)currentInternalOption + 1) % 4);
        InternalOptionText.text = InternalOptionTexts[(int)currentInternalOption];

        InternalOption = (InternalOption + 1) % 4;
    }
}
