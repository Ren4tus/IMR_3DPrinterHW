using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnowledgeHelpMeController : AnimatedUI
{
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
        SetPanelMousePosition();
        FadeIn(m_duration);
    }
    public void Hide()
    {
        FadeOut(m_duration);
    }

    public void ShowForDurtaion(float m_Duration)
    {
        StopAllCoroutines();
        SetPanelMousePosition();
        StartCoroutine(ShowForDuraion_Co(m_Duration));
    }

    public void SetTitle(string title)
    {
        TitleUI.text = title;
    }

    public void SetContent(string content)
    {
        ContentUI.text = content;
    }

    public void SetContentsByTag(string tag)
    {
        switch (tag)
        {
            case "PrinterBlock":
                TitleUI.text = "프린터블록";
                ContentUI.text = "프린터 헤드 및 UV램프, 롤러등으로 구성, 가로, 세로로 움직여 프린터 헤드를 원하는 곳에 이동시키는 역할";
                break;

            case "PrinterHead":
                TitleUI.text = "프린터헤드";
                ContentUI.text = "모델 재료 및 서포트 재료를 트레이에 분사하는 역할";
                break;

            case "ModelMaterial":
                TitleUI.text = "모델 재료";
                ContentUI.text = "프린팅 시 모델 조형에 사용되는 재료";
                break;

            case "SupportMaterial":
                TitleUI.text = "서포트 재료";
                ContentUI.text = "프린팅 시 서포트 조형에 사용되는 재료";
                break;

            case "WaterJet":
                TitleUI.text = "워터젯";
                ContentUI.text = "고압의 물을 분사하여 서포트를 제거하는 역할";
                break;

            case "WasteBottle":
                TitleUI.text = "폐기물 용기";
                ContentUI.text = "재료 분사 중 초과 물질이 버려지면 보관";
                break;

            case "Wiper":
                TitleUI.text = "와이퍼";
                ContentUI.text = "퍼지시퀀스 동안 프린트 헤드를 청소";
                break;

            case "BuildTray":
                TitleUI.text = "빌드 트레이";
                ContentUI.text = "조형이 이루어지는 곳으로, 한 레이어가 완성될 때마다 하강하여 적층이 가능하도록 함";
                break;
                
            case "UV":
                TitleUI.text = "자외선램프";
                ContentUI.text = "분사된 재료를 큐어링하는 역할";
                break;

            case "Vaccum":
                TitleUI.text = "진공";
                ContentUI.text = "헤드에서 재료가 떨어지지 않도록 하는 역할";
                break;

            case "Pump":
                TitleUI.text = "펌프";
                ContentUI.text = "재료를 프린터 블록에 전달하는 역할";
                break;

            default:
                break;
        }
    }

    public void SetHighlightWordContents(string word)
    {
        switch (word)
        {
            case "자외선(UV)":
                TitleUI.text = "자외선(Ultraviolet Ray)";
                ContentUI.text = "파장 범위 10-280nm 빛으로서, 파장이 가시광보다 더 짧고 엑스선보다는 더 긴 전자기파의 총칭";
                break;
            case "포토큐어링":
                TitleUI.text = "포토큐어링(Photo Curing)";
                ContentUI.text = "고무, 수지 등을 가열하여 경화시키는 과정";
                break;
            case "열변형":
                TitleUI.text = "열변형(Thermal Strain)";
                ContentUI.text = "열에 의해 형태나 크기 등이 변형되는 과정";
                break;
            case "빌드 트레이":
                TitleUI.text = "빌드트레이(Build Tray)";
                ContentUI.text = "베드(Bed)와 같은 의미의 프린팅 영역";
                break;
            case "퍼지 시퀀스":
                TitleUI.text = "퍼지 시퀀스(Purge Sequence)";
                ContentUI.text = "청소 단계";
                break;
            case "진공":
                TitleUI.text = "진공(Vaccum)";
                ContentUI.text = "물질을 빨아들이는 장치";
                break;
            case "SLA":
                TitleUI.text = "SLP";
                ContentUI.text = "Stereo Lithography Apparatus";
                break;
            case "DLP":
                TitleUI.text = "DLP";
                ContentUI.text = "Digital Light Processing";
                break;
            case "평탄화":
                TitleUI.text = "평탄화";
                ContentUI.text = "스윕 암(sweep arm)이나 스위퍼(sweeper), 블레이드(blade) 등을 이용하여 수지가 고르게 퍼지도록 하는 작업";
                break;
            case "점도":
                TitleUI.text = "점도";
                ContentUI.text = "물질의 차지고 끈끈함의 정도";
                break;
            case "빛 샘 현상":
                TitleUI.text = "빛 샘 현상(Light Bleeding)";
                ContentUI.text = "빛이 새어나가서 의도치 않은 부분이 경화되거나 조형물이 지저분해지는 현상";
                break;
            case "소결":
                TitleUI.text = "소결(Sintering)";
                ContentUI.text = "분말과 같은 비표면적이 넓은 입자들을 치밀한 덩어리로 만들어내기 위해 충분한 온도와 압력을 가하는 공정";
                break;
            case "SLS":
                TitleUI.text = "SLS";
                ContentUI.text = "Selective Laser Sintering";
                break;
            case "DMLS":
                TitleUI.text = "DMLS";
                ContentUI.text = "Direct Metal Laser Sintering";
                break;
            case "EBM":
                TitleUI.text = "EBM";
                ContentUI.text = "Electron Beam Melting";
                break;
            case "혼합":
                TitleUI.text = "혼합(Blending)";
                ContentUI.text = "새로운 재료와 사용한 재료를 섞는 과정";
                break;
            case "여과":
                TitleUI.text = "여과(Sieving)";
                ContentUI.text = "사용한 재료가 일부 경화되어 뭉쳐진 경우 이를 다시 사용할 수 있게끔 미립화하는 작업";
                break;
            default:
                break;
        }
    }

    private IEnumerator ShowForDuraion_Co(float floatingTime)
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

        while (timer <= floatingTime)
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
