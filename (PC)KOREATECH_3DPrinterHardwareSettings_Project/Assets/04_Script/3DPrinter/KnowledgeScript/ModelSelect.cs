using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelect : MonoBehaviour
{
    public GameObject subPanel01;
    public GameObject subPanel02;
    public GameObject subPanel03;

    [Space]
    public GameObject subPanel;

    private void OnMouseDown()
    {
        switch(this.gameObject.name)
        {
            case "MaterialJettingPrinter":
                subPanel01.SetActive(true);
                subPanel.SetActive(false);
                break;
            case "MaterialJettingPrinterCabinet":
                subPanel02.SetActive(true);
                subPanel.SetActive(false);
                break;
            case "WaterJet":
                subPanel03.SetActive(true);
                subPanel.SetActive(false);
                break;
        }
    }
}
