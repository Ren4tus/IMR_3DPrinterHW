using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUI : MonoBehaviour
{
    public GameObject mainUI;
    public void Close()
    {
        this.gameObject.SetActive(false);
        mainUI.SetActive(true);
    }
    public void Open()
    {
        this.gameObject.SetActive(true);
        mainUI.SetActive(false);
    }
}
