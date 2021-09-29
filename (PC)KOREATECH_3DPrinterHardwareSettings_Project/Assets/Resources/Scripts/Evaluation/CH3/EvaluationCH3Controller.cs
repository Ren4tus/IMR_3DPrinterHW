using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationCH3Controller : MonoBehaviour
{
    private static EvaluationCH3Controller instance = null;

    public static EvaluationCH3Controller Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

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
    
    private int estimateTime = 0;
    private int warmingUpTime = 1;
    
    private bool isPrintingStatusOpen = false;

    [Header("Doors")]
    public CommonDoorActionAS OutDoorActionScript;
    public CommonDoorActionAS InDoorActionScript;

    [Header("Status")]
    public bool IsStatusPanelOpen = false;
    public AnimatedUI StatusPanel;
    public Text MaterialStatusText;

    [Header("Slicer")]
    public bool IsSlicerPanelOpen = false;
    public EvaluationSlicerController Slicer;
    public EvaluationCH3BtnRecoater BtnRecoater;
    
    public bool IsWarmingUpComplete = false;
    public bool IsPrinting = false;
    public bool IsPrintingComplete = false;

    [Header("Recoater")]
    public bool IsRecoaterOrigin = false;
    public Animator RecoaterAnimator;

    [Header("Heaters")]
    public bool IsHeaterDown = false;
    public Animator IRHeaterAnimator;

    [Header("Extract")]
    public bool IsExtractionCylinderInChamber = false;
    public bool IsPartExtractComplete = false;
    public bool IsExtracting = false;
    public int CartPosition = 0; // 0은 기본 위치, 1은 프린터 앞, 2는 재료공급장치(MQC) 앞
    public Animator ExtractionAnimator;

    [Header("Break")]
    public bool IsBreaking = false;
    public bool IsBreakingCakeComplete = false;
    public bool IsTakeSculpture = false;
    public EvaluationCH3Sculpture Sculpture;

    [Header("Post-Processing")]
    public EvaluationCH3SandBlasterDoor SBDoor;
    public Animator SBNozzleAnimator;
    public bool IsUsingSandBalster = false;
    public bool IsUsingDustCollector = false;
    
    [Header("LaserWindwow")]
    public Animator LaserWindowAnimator;
    public EvaluationCH3LaserWindow LaserWindow;
    public bool IsCleaningLaserWindow = false;

    [Header("ETC")]
    public bool IsChamberLightOn = false;
    public GameObject ChamberLight;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // 컴퓨터 상태창 : Status
    public void StatusPanelToggle()
    {
        if (IsStatusPanelOpen)
        {
            StatusPanelOff();
        }
        else
        {
            StatusPanelOn();
        }
    }

    public void StatusPanelOn()
    {
        IsStatusPanelOpen = true;
        StatusPanel.FadeIn();
    }

    public void StatusPanelOff()
    {
        IsStatusPanelOpen = false;
        StatusPanel.FadeOut();
    }

    public void InDoorToggle()
    {
        if (IsExtracting)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("실린더 회수 중에는 챔버 도어를 닫을 수 없습니다.", CL_MessagePopUpController.DialogType.WARNING);
            return;
        }

        if (!OutDoorActionScript.IsOpen)
            return;

        if (IsPrinting)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("프린팅이 진행되고 있을 때는\n챔버 도어를 열 수 없습니다.", CL_MessagePopUpController.DialogType.WARNING);
            return;
        }

        InDoorActionScript.DoorToggle();
    }
    public void OutDoorToggle()
    {
        if (IsExtracting)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("실린더 회수 중에는 챔버 도어를 닫을 수 없습니다.", CL_MessagePopUpController.DialogType.WARNING);
            return;
        }

        if (InDoorActionScript.IsOpen)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("내부 챔버 도어를 닫아주세요.", CL_MessagePopUpController.DialogType.CAUTION);
            return;
        }

        OutDoorActionScript.DoorToggle();
    }
    // 재료 부족
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

    public void SlicerPanelToggle()
    {
        if (IsPrinting)
            return;

        if (InDoorActionScript.IsOpen)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("내부 챔버 도어를 닫아주세요.", CL_MessagePopUpController.DialogType.CAUTION);
            return;
        }

        if (IsSlicerPanelOpen)
        {
            SlicerPanelOff();
        }
        else
        {
            SlicerPanelOn();
        }
    }

    public void SlicerPanelOn()
    {
        if (IsRecoaterOrigin)
        {
            if (IsPrintingComplete || IsPrinting)
            {
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("이미 슬라이싱을 완료했습니다.", CL_MessagePopUpController.DialogType.CAUTION);
                return;
            }

            IsSlicerPanelOpen = true;
            Slicer.SlicerOpen();
        }
        else
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("먼저 리코터를 원점으로 이동시켜주세요.", CL_MessagePopUpController.DialogType.CAUTION);
        }
    }

    // Heaters
    public void HeaterToggle()
    {
        if (IsExtracting)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("실린더 회수 중에는 사용하실 수 없습니다.", CL_MessagePopUpController.DialogType.WARNING);
            return;
        }

        if (IsHeaterDown)
        {
            HeaterUp();
        }
        else
        {
            if (!IsExtractionCylinderInChamber)
            {
                HeaterDown();
            }
            else
            {
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("먼저 파트 추출 실린더를 제거해 주세요.", CL_MessagePopUpController.DialogType.CAUTION);
            }
        }
    }
    public void HeaterDown()
    {
        IsHeaterDown = true;
        IRHeaterAnimator.Play("Down");
    }
    public void HeaterUp()
    {
        IsHeaterDown = false;
        IRHeaterAnimator.Play("Up");
    }
    public void HeaterOn()
    {
        IsHeaterDown = true;
        IRHeaterAnimator.Play("On");
    }
    public void HeaterOff()
    {
        IsHeaterDown = true;
        IRHeaterAnimator.Play("Off");
    }

    public void SlicerPanelOff()
    {
        IsSlicerPanelOpen = false;
        Slicer.SlicerClose();
    }
    
    public void EstimateTimePopup()
    {
        estimateTime = Slicer.estimateTimeEX2;
        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("예상 프린팅 시간 " + estimateTime + ":00:00", CL_MessagePopUpController.DialogType.NOTICE, null);
    }
    
    public void RecoaterToOrigin()
    {
        if (IsRecoaterOrigin)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("리코터가 원점으로 복귀할 필요가 없습니다.",
                                                                CL_MessagePopUpController.DialogType.NOTICE, null);
            return;
        }

        IsRecoaterOrigin = true;
        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("리코터가 원점으로 복귀합니다.",
                                                            CL_MessagePopUpController.DialogType.NOTICE,
                                                            () =>
                                                            {
                                                                RecoaterAnimator.Play("Origin");
                                                            });
    }

    public void ExtractCylinder()
    {
        if (!IsPrintingComplete)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("프린팅 완료 후 사용해주세요.",
                                                                CL_MessagePopUpController.DialogType.NOTICE, null);
            return;
        }

        // 프린팅이 완료된 이후, 조형물 회수 시퀀스일 때
        if (!EvaluationSceneController.Instance.IsTargetStepComplete(3, 0))
        {
            if (!InDoorActionScript.IsOpen)
            {
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("내부 챔버 도어를 열어주세요.",
                                                                    CL_MessagePopUpController.DialogType.NOTICE, null);
                return;
            }

            if (IsHeaterDown)
            {
                CL_CommonFunctionManager.Instance.MakePopUp().PopUp("히터를 올려주세요.",
                                                                    CL_MessagePopUpController.DialogType.NOTICE, null);
                return;
            }

            IsExtractionCylinderInChamber = true;
            ExtractionAnimator.Play("PutCylinderIntoChamber");
            EvaluationSceneController.Instance.CompleteStep(3, 0);

            return;
        }

        if (!IsPartExtractComplete)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("파트 추출 명령을 실행해야 합니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
            return;
        }

        // 트레이에서 가져갈 때
        if (CartPosition.Equals(0))
        {
            StartCoroutine(Extraction_Co());
        }
        else if (CartPosition.Equals(2))
        {
            // MQC 앞
            if (IsExtracting)
                return;

            if (!IsBreakingCakeComplete && !IsBreaking)
            {
                IsBreaking = true;
                StartCoroutine(CakeBreak_Co());
            }
        }
    }

    public void Extract()
    {
        if (EvaluationSceneController.Instance.IsTargetStepComplete(3, 1))
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("이미 파트 추출을 완료했습니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
            return;
        }

        if (InDoorActionScript.IsOpen)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("내부 챔버 도어를 닫아주세요.",
                                                                CL_MessagePopUpController.DialogType.CAUTION, null);
            return;
        }

        ExtractionAnimator.Play("Extract");
        IsPartExtractComplete = true;

        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("파트 추출이 완료되었습니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
        EvaluationSceneController.Instance.IsTargetStepComplete(3, 0);
    }



    public void CleanLaserWindow()
    {
        if (IsCleaningLaserWindow)
            return;

        if (EvaluationSceneController.Instance.IsTargetStepComplete(3, 1))
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("이미 파트 추출을 완료했습니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
            return;
        }

        if (InDoorActionScript.IsOpen)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("내부 챔버 도어를 닫아주세요.",
                                                                CL_MessagePopUpController.DialogType.CAUTION);
            return;
        }

        ExtractionAnimator.Play("Extract");
        IsPartExtractComplete = true;

        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("파트 추출이 완료되었습니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
        EvaluationSceneController.Instance.IsTargetStepComplete(3, 0);
    }

    public void SculptureProcessing()
    {
        if (!IsBreakingCakeComplete) // 케이크 해체가 완료되지 않았을 때
            return;

        if (!IsTakeSculpture)
        {
            IsTakeSculpture = true;
            ExtractionAnimator.Play("TakeSculpture");

            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("조형물을 회수하였습니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
            EvaluationSceneController.Instance.CompleteStep(4, 0);

            Sculpture.gameObject.SetActive(false);
            return;
        }

        // 조형물 회수 이후, 후처리
        if (!IsUsingSandBalster)
        {
            IsTakeSculpture = true;
            ExtractionAnimator.Play("TakeSculpture");

            EvaluationSceneController.Instance.CompleteStep(4, 0);
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("조형물을 회수하였습니다.", CL_MessagePopUpController.DialogType.NOTICE, null);
            return;
        }
    }
    public void SandBlasterProcessing()
    {
        if (!IsBreakingCakeComplete || !IsTakeSculpture) // 케이크 해체가 완료되지 않았을 때
            return;

        if (IsUsingSandBalster)
            return;

        ExtractionAnimator.Play("PutInSandBlaster");
        Sculpture.gameObject.SetActive(true);

        SBNozzleAnimator.Play("Move");

        IsUsingSandBalster = true;
    }

    public void ShifterOn()
    {
        if (!IsTakeSculpture && ! EvaluationSceneController.Instance.IsTargetStepParentComplete(4, 1))
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("먼저 조형물을 회수해 주세요.", CL_MessagePopUpController.DialogType.WARNING, null);
            return;
        }

        ExtractionAnimator.Play("ShifterOn");
        EvaluationSceneController.Instance.CompleteStep(4, 1);
    }

    // ---------------------------------------------------------------------------------------------------
    // Post-Processing
    // ---------------------------------------------------------------------------------------------------

    public void ChamberLightOn()
    {
        IsChamberLightOn = true;
        ChamberLight.SetActive(true);
    }
    public void ChamberLightOff()
    {
        IsChamberLightOn = false;
        ChamberLight.SetActive(false);
    }
    public void ChamberLightToggle()
    {
        if (IsChamberLightOn)
        {
            ChamberLightOff();
        }
        else
        {
            ChamberLightOn();
        }
    }

    // Printing Status
    public void PrintingStatusOpen()
    {
        isPrintingStatusOpen = true;
        PrintingStautsPanel.FadeIn();
    }
    public void PrintingStatusClose()
    {
        isPrintingStatusOpen = false;
        PrintingStautsPanel.FadeOut();
    }
    
    public void SetStatusComplete()
    {
        textCurrentStatus.text = "Complete";
        textCurrentStatus.color = new Color32(94, 171, 93, 255);

        textProgress.text = "100%";
        progressSlider.value = 100f;

        elapsedTime = estimateTime * 3600;
        ElapsedTimeUpdate();
    }

    public void WarmingUpStart()
    {
        if (Slicer.isPassCollocate)
        {
            estimateTime = 1;
            StartCoroutine(Co_WarmingUpQuick(5f));
        }
    }

    public void PrintingStart()
    {
        estimateTime = Slicer.estimateTimeEX2;
        StartCoroutine(Co_PrintingQuick(estimateTime));
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
    
    // Coroutines

    IEnumerator Co_WarmingUpQuick(float time)
    {
        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("프린팅 전 1시간의 워밍업이 진행됩니다.",
                                                            CL_MessagePopUpController.DialogType.NOTICE, null);

        IsPrinting = true; // 워밍업도 프린팅 과정의 일부로 간주

        textCurrentStatus.text = "Warming Up";
        textCurrentStatus.color = new Color32(247, 150, 71, 255);
        textCurrentZ.text = "";
        textTotalZ.text = "";
        textProgress.text = "0%";
        progressSlider.value = 0;

        PrintingStatusOpen();

        if (!IsHeaterDown)
        {
            HeaterDown();
            yield return new WaitForSeconds(2);
        }

        HeaterOn();

        // 워밍업은 1시간
        warmingUpTime = 1;

        float timer = 0.0f;
        int totalTicks = warmingUpTime * 3600;
        int percentage = elapsedTime / warmingUpTime / 36;

        IsWarmingUpComplete = false;

        while (timer < time)
        {
            timer += Time.deltaTime;
            elapsedTime = (int)((float)totalTicks * timer / time);

            percentage = elapsedTime / warmingUpTime / 36;
            textProgress.text = percentage.ToString() + "%";
            progressSlider.value = percentage;
            ElapsedTimeUpdate();

            yield return new WaitForFixedUpdate();
        }

        IsWarmingUpComplete = true;
        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("워밍업이 완료되었습니다. 이제 프린팅이 시작됩니다.",
                                                            CL_MessagePopUpController.DialogType.NOTICE, null);

        RecoaterAnimator.Play("PrintStart");
        yield return new WaitForSeconds(2);

        PrintingStart();
    }

    IEnumerator Co_PrintingQuick(float time)
    {
        RecoaterAnimator.Play("Printing");

        IsPrinting = true;

        textCurrentStatus.text = "Printing";
        textCurrentStatus.color = new Color32(71, 175, 247, 255);
        textProgress.text = "0%";
        progressSlider.value = 0;
        textCurrentZ.text = "";
        textTotalZ.text = "";
        ElapsedTimeUpdate();

        PrintingStatusOpen();

        float timer = 0.0f;
        int totalTicks = estimateTime * 3600;
        int percentage = elapsedTime / estimateTime / 36;

        while (timer < time)
        {
            timer += Time.deltaTime;
            elapsedTime = (int)((float)totalTicks * timer / time);

            percentage = elapsedTime / estimateTime / 36;
            textProgress.text = percentage.ToString() + "%";
            progressSlider.value = percentage;
            ElapsedTimeUpdate();

            yield return new WaitForFixedUpdate();
        }

        IsPrinting = false;
        IsPrintingComplete = true;

        HeaterOff();
        RecoaterAnimator.Play("PrintEnd");
        StatusChangeNeedMaterial();

        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("프린팅이 완료되었습니다.",
                                                            CL_MessagePopUpController.DialogType.NOTICE, 
                                                            () => { EvaluationSceneController.Instance.CompleteStep(2, 0); });

        PrintingStatusClose();
    }

    IEnumerator Extraction_Co()
    {
        IsExtracting = true;

        ExtractionAnimator.Play("CartMoveToPrinter");
        CartPosition = 1;
        yield return new WaitForSeconds(3.5f);

        ExtractionAnimator.Play("TrayInChamber");
        yield return new WaitForSeconds(2f);

        ExtractionAnimator.Play("RemoveFromChamber");
        yield return new WaitForSeconds(2f);

        IsExtractionCylinderInChamber = false;

        ExtractionAnimator.Play("MoveToMQC");
        yield return new WaitForSeconds(4f);

        CartPosition = 2;
        IsExtracting = false;

        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("조형물 회수를 완료했습니다.",
                                                            CL_MessagePopUpController.DialogType.NOTICE, null);

        EvaluationSceneController.Instance.CompleteStep(3, 1);
    }

    IEnumerator CakeBreak_Co()
    {
        ExtractionAnimator.Play("CylinderMoveToMQC");
        yield return new WaitForSeconds(1f);

        ExtractionAnimator.Play("CylinderRemoveMQC");
        yield return new WaitForSeconds(3.5f);

        ExtractionAnimator.Play("BreakCake");
        yield return new WaitForSeconds(2f);
        

        CL_CommonFunctionManager.Instance.MakePopUp().PopUp("조형물 해체를 완료했습니다.\n조형물을 회수해 주세요.",
                                                            CL_MessagePopUpController.DialogType.NOTICE, null);

        IsBreakingCakeComplete = true;
        IsBreaking = false;
    }

    // Skip
    public void Seq3Skip()
    {
        if (IsPrinting)
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("프린팅이 완료된 후 스킵을 사용할 수 있습니다.",
                                                                CL_MessagePopUpController.DialogType.NOTICE, null);
            return;
        }

        IsPartExtractComplete = true;

        StartCoroutine(Seq3Skip_Co());
    }

    IEnumerator Seq3Skip_Co()
    {
        ExtractionAnimator.Play("MoveToMQC");
        yield return new WaitForSeconds(4f);

        CartPosition = 2;
        IsExtracting = false;

        EvaluationSceneController.Instance.CompleteStep(3, 0);
        EvaluationSceneController.Instance.CompleteStep(3, 1);

        EvaluationSceneController.Instance.isSkip = false;
    }

    public void Seq4Skip()
    {
        if (!CartPosition.Equals(2))
        {
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp("트레이 이동이 완료된 후 스킵을 사용할 수 있습니다.",
                                                                CL_MessagePopUpController.DialogType.NOTICE, null);
            return;
        }

        IsBreakingCakeComplete = true;
        IsBreaking = false;
        IsTakeSculpture = true;
        Sculpture.gameObject.SetActive(false);
        
        ExtractionAnimator.Play("ShifterOn"); 
        EvaluationSceneController.Instance.CompleteStep(4, 0);
        EvaluationSceneController.Instance.CompleteStep(4, 1);

        EvaluationSceneController.Instance.isSkip = false;
    }

    public void Seq5Skip()
    {
        ExtractionAnimator.Play("PutInSandBlaster");
        Sculpture.gameObject.SetActive(true);

        SBNozzleAnimator.Play("Move");

        IsUsingSandBalster = true;

        EvaluationSceneController.Instance.CompleteStep(5, 0);
        EvaluationSceneController.Instance.CompleteStep(5, 1);

        EvaluationSceneController.Instance.isSkip = false;
    }

    public void Seq7Skip()
    {
        EvaluationSceneController.Instance.CompleteStep(7, 0);
        EvaluationSceneController.Instance.CompleteStep(7, 1);
        EvaluationSceneController.Instance.CompleteStep(7, 2);

        EvaluationSceneController.Instance.isSkip = false;
    }
}