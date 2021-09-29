using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using UnityEngine.EventSystems;
using System.Text;

[RequireComponent(typeof(Highlighter))]
public class EvaluationInteractiveObject : MonoBehaviour
{
    public int TargetSequence = -1;
    public int TargetStep = -1;
    public string ObjectName;

    protected Highlighter _highlighter = null;
    public bool IsInteractive = false; // 상호작용 가능 여부. 꺼져있을 시 클릭 x

    protected StringBuilder tempStr;

    protected void Awake()
    {
        if (_highlighter == null)
            _highlighter = GetComponent<Highlighter>();

        tempStr = new StringBuilder();
    }

    public virtual void OnMouseEnter()
    {
        if (IsPointerOverUIObject())
            return;

        if (!IsInteractive)
            return;

        _highlighter.ConstantOn();

        EvaluationSceneController.Instance.FollowGuideController.SetContentsByName(ObjectName);
        EvaluationSceneController.Instance.FollowGuideController.Show();
    }
    public virtual void OnMouseOver()
    {
        if (IsPointerOverUIObject())
            return;

        EvaluationSceneController.Instance.FollowGuideController.SetPanelMousePosition();
    }
    public virtual void OnMouseExit()
    {
        _highlighter.Off();

        EvaluationSceneController.Instance.FollowGuideController.Hide();
    }

    public virtual void OnMouseDown()
    {
        if (!IsInteractive || IsPointerOverUIObject())
            return;
        
        if (TargetSequence.Equals(-1) || TargetStep.Equals(-1))
            return;

        if (EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            return;

        if (!EvaluationSceneController.Instance.IsPostSequenceComplete(TargetSequence))
        {
            // 이전 시퀀스를 완료하지 않았을 경우
            int[] tempTarget = EvaluationSceneController.Instance.GetParentSequenceIndex(TargetSequence);

            tempStr.Clear();

            for (int i=0;i<tempTarget.Length;i++)
            {
                tempStr.Append("[");
                tempStr.Append(EvaluationSceneController.Instance.GetSequenceName(tempTarget[i]));
                tempStr.Append("]");
            }

            tempStr.Append("\n작업을 먼저 완료해야 합니다.");
            CL_CommonFunctionManager.Instance.MakePopUp().PopUp(tempStr.ToString(),
                                                                CL_MessagePopUpController.DialogType.CAUTION);
            return;
        }

        if (!EvaluationSceneController.Instance.IsTargetStepParentComplete(TargetSequence, TargetStep))
        {
            // 이전 단계를 완료하지 않았을 경우
            int parentIdx = EvaluationSceneController.Instance.GetParentStepIndex(TargetSequence, TargetStep);

            tempStr.Clear();
            tempStr.Append("[");
            tempStr.Append(EvaluationSceneController.Instance.GetStepName(TargetSequence, parentIdx));
            tempStr.Append("]\n작업을 먼저 완료해야 합니다.");

            CL_CommonFunctionManager.Instance.MakePopUp().PopUp(tempStr.ToString(), CL_MessagePopUpController.DialogType.CAUTION);
            return;
        }

        EvaluationSceneController.Instance.FollowGuideController.Hide();
        EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
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
