using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EvaluationCH2CloseUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject UICamera;
    public GameObject BlackBG_Camera;
    public GameObject UI_BlackBG_Clicks;
    
    private bool closeActive;

    private void Awake()
    {
        closeActive = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (closeActive && Input.GetMouseButtonDown(0))
        {
            UICamera.SetActive(false);
            // BlackBG_Camera.SetActive(false);
            UI_BlackBG_Clicks.SetActive(false);
            EvaluationSceneController.Instance.CameraControllOn();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        closeActive = true;
    }
}