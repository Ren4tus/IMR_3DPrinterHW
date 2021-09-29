using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PracticeToolTipController : AnimatedUI
{
    public bool isShow = true;
    public Text TitleUI;
    public Text ContentUI;

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
        if (!isShow)
            return;

        SetPanelMousePosition();
        FadeIn(m_duration);
    }
    public void Hide()
    {
        FadeOut(m_duration);
    }

    public void ShowForDurtaion(float duration)
    {
        if (!isShow)
            return;

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
                TitleUI.text = "컴퓨터 프린터 연결 아이콘";
                ContentUI.text = "클릭하면 컴퓨터와 프린터를 연결할 수 있습니다.";
                break;

            case "UI_BtnPrinterStatus":
                TitleUI.text = "프린터 상태 보기 아이콘";
                ContentUI.text = "클릭하면 프린터 상태를 확인할 수 있는 인터페이스 창이 열립니다.";
                break;

            case "UI_BtnHeadClean":
                TitleUI.text = "헤드 청소 아이콘";
                ContentUI.text = "클릭하면 프린터 블록이 청소 위치로 이동하게 합니다.";
                break;

            case "UI_BtnSlicer":
                TitleUI.text = "슬라이서 아이콘";
                ContentUI.text = "클릭하면 슬라이서가 실행됩니다.";
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

            case "PowderBag":
                TitleUI.text = "파우더백";
                ContentUI.text = "모델 조형에 사용하는 분말 재료가 들어있습니다.";
                break;

            case "BafflePlug":
                TitleUI.text = "배플플러그";
                ContentUI.text = "레이저 윈도우를 제거한 뒤 임시로 오염을 막는 데 사용합니다.";
                break;

            case "Scrapper":
                TitleUI.text = "스크래퍼";
                ContentUI.text = "헤라라고도 하며 금속이나 플라스틱 재질로 긁거나 주걱과 같이 끌어올리는 용도로 사용합니다. 조형물을 빌드트레이에서 분리할 때 사용합니다.";
                break;

            case "MovingTray":
                TitleUI.text = "이동용 트레이";
                ContentUI.text = "프린팅이 완료된 재료를 후처리 공간으로 이동시 사용합니다.";
                break;

            case "CleaningSwab":
                TitleUI.text = "산업용 면봉";
                ContentUI.text = "헤드를 세척하는 용도로 사용하며, 보푸라기가 없는 섬유로 만들어져 있습니다.";
                break;

            case "EthanolCase":
                TitleUI.text = "에탄올";
                ContentUI.text = "에틸 알코올이라고도 하며 소독제, 세척제 등의 용도로 사용합니다.";
                break;

            case "CleanFabric":
                TitleUI.text = "세척용 천";
                ContentUI.text = "헤드를 세척하는 용도로 사용하는, 분진이 없는 섬유입니다.";
                break;

            case "Tissu":
                TitleUI.text = "티슈";
                ContentUI.text = "트레이 및 각종 주요 부품을 등을 청소할 때 사용합니다.";
                break;

            case "Mirror":
                TitleUI.text = "거울";
                ContentUI.text = "플라스틱으로 만들어져 깨지지 않습니다. 헤드 청소시 내부 관찰용으로 사용합니다.";
                break;

            case "HexagonWrench":
                TitleUI.text = "육각렌치";
                ContentUI.text = "레이저 윈도우를 분해하고 조립할 때 사용합니다.";
                break;

            case "SafetyGlasses":
                TitleUI.text = "보안경";
                ContentUI.text = "플라스틱으로 미분, 칩, 액체, 약품 등 기타 비산물로부터 눈을 보호합니다.";
                break;

            case "DustproofMask":
                TitleUI.text = "방진마스크";
                ContentUI.text = "1급 방진마스크, 기계적 분진등 발생 장소에서 사용합니다.";
                break;

            case "NitrilGloves":
                TitleUI.text = "니트릴장갑";
                ContentUI.text = "합성고무로 제작된 장갑으로, 찢김에 강하며 라텍스보다 강하여 내열, 내화용도에 사용합니다.";
                break;

            case "CleaningBrush":
                TitleUI.text = "청소용 브러쉬";
                ContentUI.text = "파우더를 털어내기 위해서 사용하는 솔입니다.";
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
            timer += Time.deltaTime / m_duration;
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
            timer += Time.deltaTime / m_duration;
            fadecolor = Mathf.Lerp(_canvasGroup.alpha, 0.0f, timer);
            _canvasGroup.alpha = fadecolor;

            yield return null;
        }

        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}