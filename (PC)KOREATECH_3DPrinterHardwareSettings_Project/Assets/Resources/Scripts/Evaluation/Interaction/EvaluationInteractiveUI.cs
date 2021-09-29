using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Outline))]
public class EvaluationInteractiveUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int TargetSequence = -1;
    public int TargetStep = -1;
    public string ObjectName;

    protected Outline _outline;
    public bool IsInteractive = false; // 상호작용 가능 여부. 꺼져있을 시 하이라이트 활성화 x
    public bool IsClickCallCompleteStep = false;

    protected StringBuilder tempStr = null;

    protected void Awake()
    {
        if (_outline == null)
            _outline = GetComponent<Outline>();

        tempStr = new StringBuilder();
    }

    protected void Start()
    {
        _outline.enabled = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsInteractive)
            return;

        _outline.enabled = true;

        EvaluationSceneController.Instance.FollowGuideController.SetContentsByNameUI(ObjectName);
        EvaluationSceneController.Instance.FollowGuideController.Show();
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!IsInteractive)
            return;

        _outline.enabled = false;

        EvaluationSceneController.Instance.FollowGuideController.Hide();
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!IsClickCallCompleteStep)
            return;

        CompleteStep();
    }

    public virtual void CompleteStep()
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

            for (int i = 0; i < tempTarget.Length; i++)
            {
                tempStr.Append("<b>[");
                tempStr.Append(EvaluationSceneController.Instance.GetSequenceName(tempTarget[i]));
                tempStr.Append("]</b>");
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
            tempStr.Append("<b>[");
            tempStr.Append(EvaluationSceneController.Instance.GetStepName(TargetSequence, parentIdx));
            tempStr.Append("]</b>\n작업을 먼저 완료해야 합니다.");

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
