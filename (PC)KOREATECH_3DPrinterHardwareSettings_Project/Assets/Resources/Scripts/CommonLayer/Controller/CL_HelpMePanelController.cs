using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CL_HelpMePanelController : MonoBehaviour
{
    public Texture2D cursorChange;
    public Texture2D cursorDefault;
    public CursorMode cursorMode = CursorMode.Auto;
    public Text PanelText;

    public void PanelOpen()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.HilightingWordClick);
        GetComponent<Animator>().SetTrigger("HelpMePanelOpen");
        Cursor.SetCursor(cursorChange, Vector2.zero, cursorMode);
    }
    public void PanelClose()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.HilightingWordClick);
        GetComponent<Animator>().SetTrigger("HelpMePanelClose");
        Cursor.SetCursor(cursorDefault, Vector2.zero, cursorMode);
    }

    public void SetHelpText(string tags)
    {
        switch (tags)
        {
            case "자외선(UV)":
                PanelText.text = "<b>자외선(Ultraviolet Ray)</b> : 파장 범위 10-280nm 빛으로서, 파장이 가시광보다 더 짧고 엑스선보다는 더 긴 전자기파의 총칭";
                break;
            case "포토큐어링":
                PanelText.text ="<b>포토큐어링(Photo Curing)</b> : 고무, 수지 등을 가열하여 경화시키는 과정";
                break;
            case "열변형":
                PanelText.text ="<b>열변형(Thermal Strain)</b> : 열에 의해 형태나 크기 등이 변형되는 과정";
                break;
            case "빌드 트레이":
                PanelText.text ="<b>빌드트레이(Build Tray)</b> : 베드(Bed)와 같은 의미의 프린팅 영역";
                break;
            case "퍼지 시퀀스":
                PanelText.text ="<b>퍼지 시퀀스(Purge Sequence)</b> : 청소 단계";
                break;
            case "진공":
                PanelText.text ="<b>진공(Vaccum)</b> : 물질을 빨아들이는 장치";
                break;
            case "SLA":
                PanelText.text ="<b>SLP</b> : Stereo Lithography Apparatus";
                break;
            case "DLP":
                PanelText.text ="<b>DLP</b> : Digital Light Processing";
                break;
            case "평탄화":
                PanelText.text ="<b>평탄화</b> : 스윕 암(sweep arm)이나 스위퍼(sweeper), 블레이드(blade) 등을 이용하여 수지가 고르게 퍼지도록 하는 작업";
                break;
            case "점도":
                PanelText.text ="<b>점도</b> : 물질의 차지고 끈끈함의 정도";
                break;
            case "빛 샘 현상":
                PanelText.text ="<b>빛 샘 현상(Light Bleeding)</b> : 빛이 새어나가서 의도치 않은 부분이 경화되거나 조형물이 지저분해지는 현상";
                break;
            case "소결":
                PanelText.text ="<b>소결(Sintering)</b> : 분말과 같은 비표면적이 넓은 입자들을 치밀한 덩어리로 만들어내기 위해 충분한 온도와 압력을 가하는 공정";
                break;
            case "SLS":
                PanelText.text = "<b>SLS</b> : Selective Laser Sintering";
                break;
            case "DMLS":
                PanelText.text ="<b>DMLS</b> : Direct Metal Laser Sintering";
                break;
            case "EBM":
                PanelText.text ="<b>EBM</b> : Electron Beam Melting";
                break;
            case "혼합":
                PanelText.text ="<b>혼합(Blending)</b> : 새로운 재료와 사용한 재료를 섞는 과정";
                break;
            case "여과":
                PanelText.text ="<b>여과(Sieving)</b> : 사용한 재료가 일부 경화되어 뭉쳐진 경우 이를 다시 사용할 수 있게끔 미립화하는 작업";
                break;
            default:
                break;
        }

        PanelOpen();
    }
}
