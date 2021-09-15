using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSubPanel : MonoBehaviour
{
    public GameObject SubPanel;
    public GameObject MainPanel;

    private void OnMouseUp()
    {
        SubPanel.SetActive(true);
        MainPanel.SetActive(false);
    }
}
