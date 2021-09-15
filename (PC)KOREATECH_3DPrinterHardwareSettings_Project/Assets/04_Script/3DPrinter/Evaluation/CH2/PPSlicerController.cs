using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PPSlicerController : MonoBehaviour
{
    private enum ViewType
    {
        Isometric,
        Side,
        Front,
        Top
    }
    private enum InternalOptions
    {
        Empty,
        Light,
        Basic,
        Heavy
    }
    private string[] InternalOptionTexts = { "내부채움 :\n속빈(<color='#699CB0'>0%</color>)",
                                             "내부채움 :\n가볍게(<color='#699CB0'>25%</color>)",
                                             "내부채움 :\n보통(<color='#699CB0'>50%</color>)",
                                             "내부채움 :\n무겁게(<color='#699CB0'>100%</color>)"};

    [Header("Cursor")]
    public Texture2D cursorDefault;
    public Texture2D cursorMove;
    public Texture2D cursorRotate;
    public GameObject slicerPanel;
    public GameObject allMenuBG;
    public GameObject loadingBG;
    private int cursorState = 0; // 0 : Default, 1 : Move, 2 : Rotate
    List<RaycastResult> raycastResults;    

    [Header("Slicer")]
    public Animator animatorSlicer;
    public Animator animatorSlicerPopup;

    [Space]
    public Text InternalOptionText;
    public Text ModelViewText;
    public Button[] SlicerButtons;
    private Dictionary<string, Button> FindSlicerButtons;

    [Space]
    [Header("슬라이서 옵션을 설정합니다.")]
    [Tooltip("[0] EX1, [1] EX2, [2] EX1 & EX2")]
    public int ImportOption = 2; // 모델 타입
    public int InternalOption = 2; // 내부 채움
    public int estimateTimeEX1 = 10;
    public int estimateTimeEX2 = 15;

    public Collider slicerTrayRange;
    public Transform slicerObjects;
    public Transform[] printedSculptures;
    public PPSlicerObjectController[] slicerSculptures;

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
    private ViewType currentView = ViewType.Isometric;
    private InternalOptions currentInternalOption = InternalOptions.Basic;

    private bool isSlicerOpen = false;
    private bool isSlicerPopupOpen = false;
    private bool isModelImported = false;

    public static PPSlicerController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        if (FindSlicerButtons == null)
        {
            FindSlicerButtons = new Dictionary<string, Button>();

            for (int i = 0; i < SlicerButtons.Length; i++)
            {
                FindSlicerButtons.Add(SlicerButtons[i].name, SlicerButtons[i]);
            }
        }
    }

    private void Start()
    {
        InactiveSlicerObjects();

        SetCursorDefault();

        InternalOptionText.text = InternalOptionTexts[(int)currentInternalOption];

        InternalOption = (int)currentInternalOption;

        raycastResults = new List<RaycastResult>();
    }

    private void Update()
    {
        if (cursorState != 0) // Cursor is not default
        {
            if (CheckCursorIsInUI())
            {
                if (cursorState == 1)
                {
                    Cursor.SetCursor(cursorMove, new Vector2(cursorMove.width / 2, cursorMove.height / 2), CursorMode.Auto);
                }
                else if (cursorState == 2)
                {
                    Cursor.SetCursor(cursorRotate, new Vector2(cursorRotate.width / 2, cursorRotate.height / 2), CursorMode.Auto);
                }
            }
            else
            {
                Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    public bool CheckCursorIsInUI()
    {
        bool findSlicerPanel = false;

        // Check if cursor is in Slicer UI
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        raycastResults.Clear();

        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (RaycastResult raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.Equals(slicerPanel))
                findSlicerPanel = true;

            if (raycastResult.gameObject.Equals(allMenuBG))
                return false;

            if (raycastResult.gameObject.Equals(loadingBG))
                return false;
        }
        return findSlicerPanel;
    }

    public void SlicerOpen()
    {
        if (!isSlicerOpen)
        {
            isSlicerOpen = true;
            animatorSlicer.SetFloat("speed", 1.0f);
            animatorSlicer.Play("Open", -1, 0f);

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

            animatorSlicer.SetFloat("speed", -1.0f);
            animatorSlicer.Play("Open", -1, 0f);
        }
    }

    public void SlicerPopupOpen()
    {
        // RE-Initialization
        for (int i = 0; i < slicerSculptures.Length; i++)
            slicerSculptures[i].gameObject.SetActive(false);

        for (int i = 1; i < SlicerButtons.Length; i++)
            SlicerButtons[i].interactable = false;

        modelEX1.isOn = false;
        modelEX2.isOn = false;

        slicerObjects.gameObject.SetActive(false);

        isSlicerPopupOpen = true;

        animatorSlicerPopup.SetFloat("speed", 1.0f);
        animatorSlicerPopup.Play("Open", -1, 0f);
    }
    public void SlicerPopupClose()
    {
        if (isSlicerPopupOpen)
        {
            isSlicerPopupOpen = false;

            for (int i = 1; i < SlicerButtons.Length; i++)
                SlicerButtons[i].interactable = true;

            //animatorSlicerPopup.SetTrigger("Close");
            animatorSlicerPopup.SetFloat("speed", -1.0f);
            animatorSlicerPopup.Play("Open", -1, 0f);
        }
    }
    public void SlicerPopupModelImportBtn()
    {
        // 모델 버튼에 따른 모델 임포트 현재 모든 오브젝트를 불러옴
        if(modelEX1.isOn && modelEX2.isOn)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("[EX1.STL], [EX2.STL]\n모델을 불러오시겠습니까?",
                                                            CL_MessagePopUpController.DialogType.NOTICE,
                                                            ModelImport,
                                                            null);

            ImportOption = 2;
        }
        else if(modelEX1.isOn)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("[EX1.STL]모델을 불러오시겠습니까?",
                                                            CL_MessagePopUpController.DialogType.NOTICE,
                                                            ModelImport,
                                                            null);

            ImportOption = 0;
        }
        else if(modelEX2.isOn)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("[EX2.STL]모델을 불러오시겠습니까?",
                                                            CL_MessagePopUpController.DialogType.NOTICE,
                                                            ModelImport,
                                                            null);

            ImportOption = 1;
        }
        else
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("불러올 모델을 선택해주세요",
                                                CL_MessagePopUpController.DialogType.NOTICE,
                                                null);
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

        if(ImportOption >= 2)
        {
            for (int i = 0; i < slicerSculptures.Length; i++)
            {
                slicerSculptures[i].Init();
                slicerSculptures[i].ResetPosition();
                slicerSculptures[i].CollocatedOff();
                slicerSculptures[i].gameObject.SetActive(true);
            }
        }
        else
        {
            slicerSculptures[ImportOption].Init();
            slicerSculptures[ImportOption].ResetPosition();
            slicerSculptures[ImportOption].CollocatedOff();
            slicerSculptures[ImportOption].gameObject.SetActive(true);
        }

        slicerObjects.gameObject.SetActive(true);

        isModelImported = true;
        isPassCollocate = false;

        // SlicerButtons[0].interactable = false;
        SlicerButtons[0].GetComponentInChildren<Text>().text = "다시 불러오기";

        for(int i=1; i<SlicerButtons.Length; i++)
        {
            SlicerButtons[i].interactable = true;
        }
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
                    printedSculptures[1].rotation = slicerSculptures[1].gameObject.transform.rotation;
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
        {
            isPassCollocate = true;

            for (int i = 0; i < SlicerButtons.Length-2; i++)
                SlicerButtons[i].interactable = false;

            CH2MethodSet.instance.isAllCollocated = true;
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
        cursorState = 0;
    }
    public void SetCursorMove()
    {
        Cursor.SetCursor(cursorMove, new Vector2(cursorMove.width / 2, cursorMove.height / 2), CursorMode.Auto);
        cursorState = 1;
    }
    public void SetCursorRotate()
    {
        Cursor.SetCursor(cursorRotate, new Vector2(cursorRotate.width / 2, cursorRotate.height / 2), CursorMode.Auto);
        cursorState = 2;
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
        InternalOption = (int)currentInternalOption;
    }
}