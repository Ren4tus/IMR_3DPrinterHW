using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using HighlightingSystem;

public class HoverObj : MonoBehaviour
{
    public GameObject ContextBox;
    public Text FText;

    [Space]
    public GameObject HelpMeBox;
    public GameObject HelpPanel;
    public Text PanelText;

    [Space]
    public GameObject subPanel1;
    public GameObject subPanel2;

    public bool isCoverOpen = true;
    public bool isDoorOpen = true;
    private static bool blocking = false;

    [Header("CSV Text Index")]
    public int TextIndex = 0;

    void OnMouseOver()
    {
        // 이거 쓸 모든 오브젝트의 태그를 하나로 통일하자.
        // 여긴 마우스 오버 시 패널에 텍스트 출력만 해주고
        // 무언가 액션이 필요하다면 스크립트를 따로 만들자.
        if(this.gameObject.tag == "HoverPanelObj")
        {

        }

        switch(this.gameObject.tag)
        {
            case "Cover":
                FText.text = "<b>프린터 커버</b>\n프린터 블록과 트레이, 조형물을 보호하는 역할";
                if(Input.GetMouseButtonUp(0) && isCoverOpen)
                {
                    this.gameObject.transform.localRotation = Quaternion.Euler(-150f, 0f, 0f);
                    
                    isCoverOpen = false;
                }
                break;
            case "Cabinet":
                FText.text = "<b>재료보관함</b>\n모델 재료 및 서포트 재료를 보관하는 역할";
                break;
            case "PrinterBlock":
                FText.text = "<b>프린터 블록</b>\n프린터 헤드 및 UV램프, 롤러등으로 구성, 가로, 세로로 움직여 프린터 헤드를 원하는 곳에 이동시키는 역할";
                if (Input.GetMouseButtonUp(0))
                {
                    subPanel1.SetActive(true);
                    subPanel2.SetActive(false);
                }
                break;
            case "PrinterHead":
                FText.text = "<b>프린터 헤드</b>\n모델 재료 및 서포트 재료를 트레이에 분사하는 역할\n";
                HighlighterOn();
                break;
            case "ModelMaterial":
                FText.text = "<b>모델 재료</b>\n프린팅 시 모델 조형에 사용되는 재료";
                break;
            case "SupportMaterial":
                FText.text = "<b>서포트 재료</b>\n프린팅 시 서포트 조형에 사용되는 재료";
                break;
            case "WaterJet":
                FText.text = "<b>워터젯</b>\n고압의 물을 분사하여 서포트를 제거하는 역할";
                break;
            case "WasteBottle":
                FText.text = "<b>폐기물 용기</b>\n재료 분사 중 초과 물질이 버려지면 보관";
                break;
            case "Wiper":
                FText.text = "<b>와이퍼</b>\n퍼지시퀀스 동안 프린트 헤드를 청소";
                break;
            case "BuildTray":
                FText.text = "<b>빌드 트레이</b>\n조형이 이루어지는 곳으로, 한 레이어가 완성될 때마다 하강하여 적층이 가능하도록 함";
                break;
            case "UV":
                FText.text = "<b>자외선램프</b>\n분사 된 재료를 큐어링하는 역할";
                HighlighterOn();
                break;
            case "Vaccum":
                FText.text = "<b>진공</b>\n헤드에서 재료가 떨어지지 않도록 하는 역할";
                HighlighterOn();
                break;
            case "Pump":
                FText.text = "<b>펌프</b>\n재료를 프린터 블록에 전달하는 역할";
                HighlighterOn();
                break;
            case "CabinetDoor":
                ContextBox.SetActive(false);
                if (Input.GetMouseButtonDown(0))
                {
                    this.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 150f);

                    isDoorOpen = false;
                }
                break;
            default:
                break;
        }
    }

    private void HighlighterOn()
    {
        if (this.GetComponent<Highlighter>())
            this.GetComponent<Highlighter>().tween = true;
    }

    private void OnMouseUp()
    {
        if (blocking) return;

        if(this.gameObject.name.Contains("HelpBox"))
        {
            ShowHelpMePanel((string)CSVController.data[FindIndex(TextIndex)][ConfigurationController.Language]);
        }

        /*
        switch (this.gameObject.name)
        {
            case "HelpBoxUV":
                ShowHelpMePanel("<b>자외선(Ultraviolet Ray)</b> : 파장 범위 10-280nm 빛으로서, 파장이 가시광보다 더 짧고 엑스선보다는 더 긴 전자기파의 총칭");
                break;
            case "HelpBoxPhoto":
                ShowHelpMePanel("<b>포토큐어링(Photo Curing)</b> : 고무, 수지 등을 가열하여 경화시키는 과정");
                break;
            case "HelpBoxThermal":
                ShowHelpMePanel("<b>열변형(Thermal Strain)</b> : 열에 의해 형태나 크기 등이 변형되는 과정");
                break;
            case "HelpBoxBuildTray":
                ShowHelpMePanel("<b>빌드트레이(Build Tray)</b> : 베드(Bed)와 같은 의미의 프린팅 영역");
                break;
            case "HelpBoxPurge":
                ShowHelpMePanel("<b>퍼지 시퀀스(Purge Sequence)</b> : 청소 단계");
                break;
            case "HelpBoxVaccum":
                ShowHelpMePanel("<b>진공(Vaccum)</b> : 물질을 빨아들이는 장치");
                break;
            case "HelpBoxSLA":
                ShowHelpMePanel("<b>SLP</b> : Stereo Lithography Apparatus");
                break;
            case "HelpBoxDLP":
                ShowHelpMePanel("<b>DLP</b> : Digital Light Processing");
                break;
            case "HelpBoxFlat":
                ShowHelpMePanel("<b>평탄화</b> : 스윕 암(sweep arm)이나 스위퍼(sweeper), 블레이드(blade) 등을 이용하여 수지가 고르게 퍼지도록 하는 작업");
                break;
            case "HelpBoxViscosity":
                ShowHelpMePanel("<b>점도</b> : 물질의 차지고 끈끈함의 정도");
                break;
            case "HelpBoxLightBleeding":
                ShowHelpMePanel("<b>빛 샘 현상(Light Bleeding)</b> : 빛이 새어나가서 의도치 않은 부분이 경화되거나 조형물이 지저분해지는 현상");
                break;
            case "HelpBoxSintering":
                ShowHelpMePanel("<b>소결(Sintering)</b> : 분말과 같은 비표면적이 넓은 입자들을 치밀한 덩어리로 만들어내기 위해 충분한 온도와 압력을 가하는 공정");
                break;
            case "HelpBoxSLS":
                ShowHelpMePanel("<b>SLS</b> : Selective Laser Sintering");
                break;
            case "HelpBoxDMLS":
                ShowHelpMePanel("<b>DMLS</b> : Direct Metal Laser Sintering");
                break;
            case "HelpBoxEBM":
                ShowHelpMePanel("<b>EBM</b> : Electron Beam Melting");
                break;
            case "HelpBoxBlending":
                ShowHelpMePanel("<b>혼합(Blending)</b> : 새로운 재료와 사용한 재료를 섞는 과정");
                break;
            case "HelpBoxSieving":
                ShowHelpMePanel("<b>여과(Sieving)</b> : 사용한 재료가 일부 경화되어 뭉쳐진 경우 이를 다시 사용할 수 있게끔 미립화하는 작업");
                break;
            default:
                break;
        }
        */
    }

    private void OnMouseEnter()
    {
        if(this.gameObject.name == "HelpBoxPurge" || this.gameObject.name == "HelpBoxVaccum")
        {
            HelpMeBox.SetActive(true);
            return;
        }

        if (blocking) return;

        if (HelpMeBox == null)
        {
            ContextBox.SetActive(true);
        }
        else
        {
            HelpMeBox.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (HelpMeBox != null)
            HelpMeBox.SetActive(false);

        ContextBox.SetActive(false);

        if(this.GetComponent<Highlighter>())
        {
            this.GetComponent<Highlighter>().tween = false;
        }
    }

    private void ShowHelpMePanel(string str)
    {
        PanelText.text = str;
        //HelpPanel.SetActive(true);
    }

    public bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    public static void SetBlocking(bool b)
    {
        blocking = b;
    }

    public static bool GetBlocking()
    {
        return blocking;
    }

    private int FindIndex(int checkNum)
    {
        int _MAX = CSVController.data.Count - 1;
        int count = 0;

        for (int i = 0; i <= _MAX; i++)
        {
            if ((int)CSVController.data[i]["INDEX"] == checkNum)
                break;
            count++;
        }

        if (count == _MAX && (int)CSVController.data[count]["INDEX"] != checkNum)
        {
            return 0;
        }
        return count;
    }
}
