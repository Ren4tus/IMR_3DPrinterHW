using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIWebGLQuickMenuInit : MonoBehaviour
{
    public Button homeButton;
    public Button listButton;
    public Button exitButton;

    public GameObject homeTipBG;
    public GameObject listTipBG;
    public GameObject exitTipBG;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL_ALL
        exitButton.interactable = false;

        EventTrigger eventTrigger;
        eventTrigger = exitButton.GetComponent<EventTrigger>();
        eventTrigger.enabled = false;

        exitTipBG.SetActive(false);
#elif UNITY_WEBGL_EACH
        homeButton.interactable = false;
        listButton.interactable = false;
        exitButton.interactable = false;

        EventTrigger eventTrigger;
        eventTrigger = homeButton.GetComponent<EventTrigger>();
        eventTrigger.enabled = false;
        eventTrigger = listButton.GetComponent<EventTrigger>();
        eventTrigger.enabled = false;
        eventTrigger = exitButton.GetComponent<EventTrigger>();
        eventTrigger.enabled = false;

        homeTipBG.SetActive(false);
        listTipBG.SetActive(false);
        exitTipBG.SetActive(false);
#elif UNITY_WEBGL_PRINTER
        homeButton.interactable = false;
        listButton.interactable = false;
        exitButton.interactable = false;

        EventTrigger eventTrigger;
        eventTrigger = homeButton.GetComponent<EventTrigger>();
        eventTrigger.enabled = false;
        eventTrigger = listButton.GetComponent<EventTrigger>();
        eventTrigger.enabled = false;
        eventTrigger = exitButton.GetComponent<EventTrigger>();
        eventTrigger.enabled = false;

        homeTipBG.SetActive(false);
        listTipBG.SetActive(false);
        exitTipBG.SetActive(false);
#endif
    }
}
