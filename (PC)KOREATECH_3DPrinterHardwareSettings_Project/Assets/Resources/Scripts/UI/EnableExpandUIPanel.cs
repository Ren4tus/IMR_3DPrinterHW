using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableExpandUIPanel : MonoBehaviour
{
    public GameObject UICamera;
    public GameObject ExpandUIPanel;
    public GameObject BlackBG_Camera;
    public GameObject BlackBG_Button;

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UICamera.SetActive(true);
            ExpandUIPanel.SetActive(false);
            BlackBG_Camera.SetActive(true);
            BlackBG_Button.SetActive(true);
        }
    }    
}
