using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationObjectInfo : AnimatedUI
{
    public Text TitleUI;
    public Text ContentUI;

    public float Duration = 0.3f;

    private void Start()
    {
        Hide();
    }

    public void SetPanelMousePosition()
    {
        Vector3 newPosition = Input.mousePosition;
        transform.position = new Vector3(newPosition.x + _rectTransform.rect.width / 2, newPosition.y + _rectTransform.rect.height / 2, 0);
    }

    public void Show()
    {
        SetPanelMousePosition();
        FadeIn(Duration);
    }
    public void Hide()
    {
        FadeOut(Duration);
    }

    public void ShowForDurtaion(float duration)
    {
        StopAllCoroutines();
        SetPanelMousePosition();
        StartCoroutine(ShowForDuraion_Co(duration));
    }

    public void SetTitle(string title)
    {
        TitleUI.text = title;
    }

    public void SetContent(string content)
    {
        ContentUI.text = content;
    }

    public void SetContentsByNameUI(string name)
    {
        switch (name)
        {
            case "ModelMaterial1L":
                TitleUI.text = "모델 재료 1L";
                ContentUI.text = "모델 조형에 사용되는 재료입니다. (1L)";
                break;

            case "UI_BtnLink":
                TitleUI.text = "컴퓨터 프린터 연결 버튼";
                ContentUI.text = "클릭하면 컴퓨터와 프린터를 연결할 수 있습니다.";
                break;

            case "UI_BtnPrinterStatus":
                TitleUI.text = "프린터 상태 보기 버튼";
                ContentUI.text = "클릭하면 프린터 상태를 확인할 수 있는 인터페이스 창이 열립니다.";
                break;

            case "UI_BtnReacoter":
                TitleUI.text = "리코터 원점복귀 버튼";
                ContentUI.text = "클릭하면 리코터가 원점으로 복귀합니다.";
                break;

            case "UI_BtnHeadClean":
                TitleUI.text = "헤드 청소 버튼";
                ContentUI.text = "클릭하면 프린터 블록이 청소 위치로 이동하게 합니다.";
                break;

            case "UI_BtnSlicer":
                TitleUI.text = "슬라이서 버튼";
                ContentUI.text = "클릭하면 슬라이서가 실행됩니다.";
                break;

            case "UI_BtnPartExtract":
                TitleUI.text = "파트 추출 버튼";
                ContentUI.text = "프린팅이 완료된 이후, 조형물 회수 시에 사용합니다.";
                break;

            case "UI_BtnToggle_Temperature":
                TitleUI.text = "프린터 인터페이스 토글버튼";
                ContentUI.text = "클릭하면 프린터 재료 상태를 확인하실 수 있습니다.";
                break;

            case "UI_BtnToggle_Material":
                TitleUI.text = "프린터 인터페이스 토글버튼";
                ContentUI.text = "클릭하면 프린터의 온도 상태를 확인하실 수 있습니다.";
                break;

            case "UI_BtnPrinerStatusClose":
                TitleUI.text = "프린터 인터페이스 닫기 버튼";
                ContentUI.text = "클릭하면 프린터 인터페이스 창이 닫힙니다.";
                break;

            case "UI_HeadTemp":
                TitleUI.text = "헤드 온도";
                ContentUI.text = "헤드 온도가 적절하지 않으면 붉은 색으로 표시됩니다.";
                break;

            case "BtnLogin":
                TitleUI.text = "로그인 버튼";
                ContentUI.text = "프린터 시스템에 로그인 하는 버튼입니다.";
                break;

            case "PopupClose":
                TitleUI.text = "닫기 버튼";
                ContentUI.text = "팝업을 닫습니다.";
                break;

            case "BtnPopupLogin":
                TitleUI.text = "로그인 버튼";
                ContentUI.text = "ID와 Password가 일치하면 시스템에 로그인 됩니다.";
                break;

            case "BtnAdvanced":
                TitleUI.text = "어드밴스드 버튼";
                ContentUI.text = "빌드플랫폼 레벨링을 위한 메뉴로 이동할 수 있습니다.";
                break;

            case "BtnPrintStart":
                TitleUI.text = "프린팅 버튼";
                ContentUI.text = "클릭하면 프린팅을 시작합니다.";
                break;

            case "BtnAdvancedEL":
                TitleUI.text = "이동 버튼";
                ContentUI.text = "클릭하면 빌드플랫폼 레벨링 메뉴로 이동할 수 있습니다.";
                break;

            case "BtnAdvancedStart":
                TitleUI.text = "레벨링 시작 버튼";
                ContentUI.text = "클릭하면 빌드플랫폼 자동 레벨링이 실행됩니다.";
                break;



            default:
                break;
        }
    }
    public void SetContentsByName(string name)
    {
        switch (name)
        {
            case "PrinterBlock":
                TitleUI.text = "프린터 블록";
                ContentUI.text = "재료를 분사합니다.";
                break;

            case "PrinterCover":
                TitleUI.text = "프린터 커버";
                ContentUI.text = "클릭해서 열고 닫을 수 있습니다.";
                break;

            case "MaterialCabinetCover":
                TitleUI.text = "재료보관함 커버";
                ContentUI.text = "프린팅에 사용되는 재료들이 들어있습니다. 클릭해서 열고 닫을 수 있습니다. ";
                break;

            case "ModelMaterial1L":
                TitleUI.text = "모델 재료 1L";
                ContentUI.text = "모델 조형에 사용되는 재료입니다. (1L)";
                break;
            case "ModelMaterial1R":
                TitleUI.text = "모델 재료 1R";
                ContentUI.text = "모델 조형에 사용되는 재료입니다. (1R)";
                break;
            case "ModelMaterial2L":
                TitleUI.text = "모델 재료 2L";
                ContentUI.text = "모델 조형에 사용되는 재료입니다. (2L)";
                break;
            case "ModelMaterial2R":
                TitleUI.text = "모델 재료 2R";
                ContentUI.text = "모델 조형에 사용되는 재료입니다. (2R)";
                break;
            case "ModelMaterial3L":
                TitleUI.text = "모델 재료 3L";
                ContentUI.text = "모델 조형에 사용되는 재료입니다. (3L)";
                break;
            case "ModelMaterial3R":
                TitleUI.text = "모델 재료 3R";
                ContentUI.text = "모델 조형에 사용되는 재료입니다. (3R)";
                break;
            case "SupportMaterialL":
                TitleUI.text = "서포트 재료 L";
                ContentUI.text = "서포트 조형에 사용되는 재료입니다. (L)";
                break;
            case "SupportMaterialR":
                TitleUI.text = "서포트 재료 R";
                ContentUI.text = "서포트 조형에 사용되는 재료입니다. (R)";
                break;

            case "Scrapper":
                TitleUI.text = "스크래퍼";
                ContentUI.text = "조형물을 빌드트레이에서 분리할 때 사용합니다.";
                break;

            case "MovingTray":
                TitleUI.text = "이동용 트레이";
                ContentUI.text = "조형물을 옮길 때 사용합니다.";
                break;

            case "CleanFabric":
                TitleUI.text = "청소용 천";
                ContentUI.text = "헤드와 트레이를 청소할 때 사용합니다";
                break;

            case "SafetyGlasses":
                TitleUI.text = "보안경";
                ContentUI.text = "각종 화학 물질으로부터 눈을 보호합니다.";
                break;

            case "DustproofMask":
                TitleUI.text = "방진마스크";
                ContentUI.text = "각종 화학 물질으로부터 호흡기를 보호합니다.";
                break;

            case "NitrilGloves":
                TitleUI.text = "니트릴장갑";
                ContentUI.text = "열과 부상으로부터 손을 보호합니다.";
                break;

            case "BafflePlug":
                TitleUI.text = "배플플러그";
                ContentUI.text = "레이저윈도우를 탈거한 뒤 빈 공간에 장착하여 오염을 막습니다.";
                break;

            case "WaterJet":
                TitleUI.text = "워터젯";
                ContentUI.text = "조형이 완료된 조형물에 고압의 물을 분사하여 서포트를 제거할 수 있습니다.";
                break;

            case "Sculpture":
                TitleUI.text = "조형물";
                ContentUI.text = "조형이 완료된 조형물입니다.";
                break;

            case "BuildTray":
                TitleUI.text = "빌드트레이";
                ContentUI.text = "조형이 이루어지는 곳입니다.";
                break;

            case "PrintedSculpure":
                TitleUI.text = "조형물";
                ContentUI.text = "조형이 완료된 조형물입니다.";
                break;

            case "UpperBuildChamberDoor":
                TitleUI.text = "상단 빌드 챔버 도어";
                ContentUI.text = "어플리케이터 암, 빌드플랫폼을 확인하실 수 있습니다.";
                break;

            case "LowerBuildChamberDoor":
                TitleUI.text = "하단 빌드 챔버 도어";
                ContentUI.text = "재료카트를 확인하실 수 있습니다.";
                break;

            case "ExtractionCylinder":
                TitleUI.text = "파트 추출 실린더";
                ContentUI.text = "조형물을 회수할 때 사용합니다.";
                break;

            case "ChamberOutDoor":
                TitleUI.text = "외부 잠금 도어";
                ContentUI.text = "열면 내부 챔버 도어가 나옵니다.";
                break;

            case "ChamberInDoor":
                TitleUI.text = "내부 챔버 도어";
                ContentUI.text = "열어서 챔버 내부를 확인할 수 있습니다.";
                break;

            case "Recoater":
                TitleUI.text = "리코터";
                ContentUI.text = "파우더를 이송하는 부품입니다.";
                break;

            case "IRHeaters":
                TitleUI.text = "IR 히터";
                ContentUI.text = "적외선 히터로 워밍업에 사용됩니다.";
                break;

            case "ChamberLightSwitch":
                TitleUI.text = "챔버 라이트 스위치";
                ContentUI.text = "챔버 라이트를 켜거나 끄는데 사용합니다.";
                break;

            case "ShifterSwitch":
                TitleUI.text = "시프터 스위치";
                ContentUI.text = "재료공급장치의 시프터를 작동시킵니다.";
                break;

            case "SandBlasterDoor":
                TitleUI.text = "샌드블라스터 도어";
                ContentUI.text = "내부에 조형물을 투입할 수 있습니다.";
                break;

            case "DustCollectorNozzle":
                TitleUI.text = "집진기 노즐";
                ContentUI.text = "압축 공기를 분사합니다.";
                break;

            case "LaserWindow":
                TitleUI.text = "레이저윈도우";
                ContentUI.text = "스캔 미러 시스템으로 적외선을 원하는 곳에 조사합니다.";
                break;

            case "LaserWindowLens":
                TitleUI.text = "렌즈";
                ContentUI.text = "적외선을 투과하여 원하는 곳에 조사합니다.";
                break;

            case "PowderBag":
                TitleUI.text = "파우더백";
                ContentUI.text = "분말 재료가 들어있는 파우더 백입니다.";
                break;

            case "VaccumCleaner":
                TitleUI.text = "진공청소기";
                ContentUI.text = "챔버 내부의 잔여물을 흡입할 때 사용합니다.";
                break;

            case "MaterialCart":
                TitleUI.text = "재료 카트";
                ContentUI.text = "프린팅에 필요한 재료를 공급합니다.";
                break;

            case "PowerSwitch":
                TitleUI.text = "프린터 전원";
                ContentUI.text = "프린터의 전원을 키고 끌 수 있습니다.";
                break;

            case "BottleRelease":
                TitleUI.text = "탈거 버튼";
                ContentUI.text = "재료 교체 시 재료를 탈거하는 버튼입니다.";
                break;

            case "MaterialBottle":
                TitleUI.text = "기존 레진";
                ContentUI.text = "재료가 부족한 상태의 레진입니다.";
                break;

            case "Resin":
                TitleUI.text = "레진";
                ContentUI.text = "가득차 있는 레진입니다.";
                break;

            case "UVOven":
                TitleUI.text = "경화 장치";
                ContentUI.text = "회수한 조형물을 경화시키는 장치입니다.";
                break;

            case "BuildPlatform":
                TitleUI.text = "빌드플랫폼";
                ContentUI.text = "조형이 이루어지는 곳입니다.";
                break;

            case "PrinterUSBPort":
                TitleUI.text = "USB포트";
                ContentUI.text = "USB를 삽입하여 슬라이서를 실행할 수 있습니다.";
                break;

            case "Grinder":
                TitleUI.text = "그라인더";
                ContentUI.text = "회수한 조형물을 후처리할 때 사용합니다.";
                break;

            case "Sandpaper":
                TitleUI.text = "사포";
                ContentUI.text = "회수한 조형물을 후처리할 때 사용합니다.";
                break;

            case "HandFile":
                TitleUI.text = "줄";
                ContentUI.text = "회수한 조형물을 후처리할 때 사용합니다.";
                break;

            default:
                break;
        }
    }

    private IEnumerator ShowForDuraion_Co(float duration)
    {
        float timer = 0.0f;
        float fadecolor = _canvasGroup.alpha;

        while (fadecolor < 1f)
        {
            timer += Time.deltaTime / Duration;
            fadecolor = Mathf.Lerp(_canvasGroup.alpha, 1.0f, timer);
            _canvasGroup.alpha = fadecolor;

            yield return null;
        }

        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        timer = 0.0f;

        while (timer <= duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0.0f;

        while (fadecolor > 0f)
        {
            timer += Time.deltaTime / Duration;
            fadecolor = Mathf.Lerp(_canvasGroup.alpha, 0.0f, timer);
            _canvasGroup.alpha = fadecolor;

            yield return null;
        }

        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
