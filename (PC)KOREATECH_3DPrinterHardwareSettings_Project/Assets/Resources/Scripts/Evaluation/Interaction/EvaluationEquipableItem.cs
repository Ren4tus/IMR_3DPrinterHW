using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvaluationEquipableItem : MonoBehaviour
{
    public bool IsEquip = false;
    public bool IsInteractive = false; // 상호작용 가능 여부. 꺼져있을 시 클릭 x
    public string ObjectName;

    protected Highlighter _highlighter = null;

    protected void Awake()
    {
        if (_highlighter == null)
            _highlighter = GetComponent<Highlighter>();
    }

    public void OnMouseDown()
    {
        EquipToggle();
        EvaluationSceneController.Instance.FollowGuideController.Hide();
    }

    public void OnMouseEnter()
    {
        if (IsPointerOverUIObject())
            return;

        if (!IsInteractive)
            return;

        _highlighter.ConstantOn();

        EvaluationSceneController.Instance.FollowGuideController.SetContentsByName(ObjectName);
        EvaluationSceneController.Instance.FollowGuideController.Show();
    }
    public void OnMouseOver()
    {
        if (IsPointerOverUIObject())
            return;

        EvaluationSceneController.Instance.FollowGuideController.SetPanelMousePosition();
    }
    public void OnMouseExit()
    {
        _highlighter.Off();

        EvaluationSceneController.Instance.FollowGuideController.Hide();
    }

    public void EquipToggle()
    {
        if (!IsEquip)
        {
            // 장착
            gameObject.SetActive(false);
            IsEquip = true;
        }
        else
        {
            // 해제
            gameObject.SetActive(true);
            IsEquip = false;
        }
    }

    // 오브젝트를 클릭할때 UI 레이어를 함께 클릭했는지 검사하는 함수
    protected bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer.Equals(LayerMask.NameToLayer("UI")))
                return true;
        }

        return false;
    }
}
