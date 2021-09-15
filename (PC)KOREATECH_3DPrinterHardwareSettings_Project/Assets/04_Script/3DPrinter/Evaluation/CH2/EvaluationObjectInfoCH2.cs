using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationObjectInfoCH2 : TweenableCanvas
{
    public Text TitleUI;
    public Text ContentUI;

    public float Duration = 0.3f;

    private RectTransform _rectTransform;

    protected override void Awake()
    {
        base.Awake();

        _rectTransform = GetComponent<RectTransform>();
    }

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
            case "Login":
                TitleUI.text = "로그인 아이콘";
                ContentUI.text = "클릭하여 로그인 팝업을 열 수 있습니다.";
                break;

            default:
                break;
        }
    }
    public void SetContentsByName(string name)
    {
        switch (name)
        {
            case "ChamberDoorUpper":
                TitleUI.text = "상단 챔버 도어";
                ContentUI.text = "클릭해서 열고 닫을 수 있습니다.";
                break;

            case "ChamberDoorLower":
                TitleUI.text = "하단 챔버 도어";
                ContentUI.text = "클릭해서 열고 닫을 수 있습니다.";
                break;

            case "Scrapper":
                TitleUI.text = "스크래퍼";
                ContentUI.text = "조형물을 빌드트레이에서 분리할 때 사용합니다.";
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

            case "Sculpture":
                TitleUI.text = "조형물";
                ContentUI.text = "조형이 완료된 조형물입니다.";
                break;

            case "PrintedSculpure":
                TitleUI.text = "조형물";
                ContentUI.text = "조형이 완료된 조형물입니다.";
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
