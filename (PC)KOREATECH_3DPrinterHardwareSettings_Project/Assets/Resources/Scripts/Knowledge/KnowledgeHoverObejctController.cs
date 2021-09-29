using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HighlightingSystem;

public class KnowledgeHoverObejctController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform PopupPanel;
    private Canvas _canvas;
    public Text TagShowText;
    public Text ContextShowText;
    
    private Highlighter hg;
    private GameObject targetObject;

    private void Awake()
    {
        _canvas = PopupPanel.GetComponent<Canvas>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject == null)
        {
            _canvas.enabled = false;
            return;
        }

        if (!eventData.pointerEnter.layer.Equals(LayerMask.NameToLayer("SelectableObject")))
        {
            _canvas.enabled = false;
            return;
        }

        targetObject = eventData.pointerEnter.gameObject;

        switch (targetObject.tag)
        {
            case "PrinterBlock":
                TagShowText.text = "프린터블록";
                ContextShowText.text = "프린터 헤드 및 UV램프, 롤러등으로 구성, 가로, 세로로 움직여 프린터 헤드를 원하는 곳에 이동시키는 역할";
                Highlighting(targetObject);
                break;

            case "PrinterHead":
                TagShowText.text = "프린터헤드";
                ContextShowText.text = "모델 재료 및 서포트 재료를 트레이에 분사하는 역할";
                Highlighting(targetObject);
                break;

            case "ModelMaterial":
                TagShowText.text = "모델 재료";
                ContextShowText.text = "프린팅 시 모델 조형에 사용되는 재료";
                Highlighting(targetObject);
                break;

            case "SupportMaterial":
                TagShowText.text = "서포트 재료";
                ContextShowText.text = "프린팅 시 서포트 조형에 사용되는 재료";
                Highlighting(targetObject);
                break;

            case "WaterJet":
                TagShowText.text = "워터젯";
                ContextShowText.text = "고압의 물을 분사하여 서포트를 제거하는 역할";
                Highlighting(targetObject);
                break;

            case "WasteBottle":
                TagShowText.text = "폐기물 용기";
                ContextShowText.text = "재료 분사 중 초과 물질이 버려지면 보관";
                Highlighting(targetObject);
                break;

            case "Wiper":
                TagShowText.text = "와이퍼";
                ContextShowText.text = "퍼지시퀀스 동안 프린트 헤드를 청소";
                Highlighting(targetObject);
                break;

            case "BuildTray":
                TagShowText.text = "빌드 트레이";
                ContextShowText.text = "조형이 이루어지는 곳으로, 한 레이어가 완성될 때마다 하강하여 적층이 가능하도록 함";
                Highlighting(targetObject);
                break;

            case "UV":
                TagShowText.text = "자외선램프";
                ContextShowText.text = "분사된 재료를 큐어링하는 역할";
                Highlighting("UV");
                break;

            case "Vaccum":
                TagShowText.text = "진공";
                ContextShowText.text = "헤드에서 재료가 떨어지지 않도록 하는 역할";
                Highlighting(targetObject);
                break;

            case "Pump":
                TagShowText.text = "펌프";
                ContextShowText.text = "재료를 프린터 블록에 전달하는 역할";
                Highlighting(targetObject);
                break;

            default:
                break;
        }

        if (targetObject.layer == 8) // SelectableObject
            _canvas.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != targetObject)
        {
            _canvas.enabled = false;

            if (hg != null)
                hg.Off();
        }
    }

    public void HighlightObject(string targetTag)
    {
        Highlighting(targetTag);
    }

    private void Update()
    {
        Vector3 newPosition = Input.mousePosition;
        PopupPanel.position = new Vector3(newPosition.x + PopupPanel.rect.width / 2, newPosition.y + PopupPanel.rect.height / 2, 0);
    }

    private void Highlighting(GameObject target)
    {
        if (hg != null)
        {
            hg.Off();
        }

        hg = target.GetComponent<Highlighter>();

        if (hg == null)
            return;
        
        hg.ConstantOn();
    }

    private void Highlighting(string targetTag)
    {
        targetObject = GameObject.FindGameObjectWithTag(targetTag);

        if (targetObject == null)
            return;

        if (hg != null)
        {
            hg.Off();
        }

        hg = targetObject.GetComponent<Highlighter>();

        if (hg == null)
            return;

        hg.ConstantOn();
    }
}
