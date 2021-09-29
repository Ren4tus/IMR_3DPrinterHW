using UnityEngine;

public class EvaluationCH2ExpandUI : MonoBehaviour
{
    public GameObject UICamera;
    public GameObject BlackBG_Camera;
    public GameObject UI_BlackBG_Clicks;

    public int TargetSequence;
    public int TargetStep;

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UICamera.SetActive(true);
            // BlackBG_Camera.SetActive(true);
            UI_BlackBG_Clicks.SetActive(true);
            EvaluationSceneController.Instance.CameraControllOff();

            if (!EvaluationSceneController.Instance.IsTargetStepComplete(TargetSequence, TargetStep))
            {
                EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
            }
        }
    }
}