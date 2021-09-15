using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSlicePanelButtons : MonoBehaviour
{
    public MainController mainController;

    public int enableStartSeqNum;
    public int enableEndSeqNum;

    public Button btnModelImport;
    public Button[] buttons;
    public Button btnPrinting;

    public DragGameObject[] dragGameObjects;

    // Update is called once per frame
    void Update()
    {
        int curSeqNum = mainController.GetCurrentSeq();

        if (curSeqNum < enableEndSeqNum && curSeqNum > enableStartSeqNum)
        {
            btnModelImport.enabled = false;
            foreach (Button button in buttons)
                button.enabled = true;

            if (btnPrinting != null)
                btnPrinting.enabled = false;
        }
        else if (curSeqNum <= enableStartSeqNum)
        {
            btnModelImport.enabled = true;
            foreach (Button button in buttons)
                button.enabled = false;

            if (btnPrinting != null)
                btnPrinting.enabled = false;

            foreach (DragGameObject dgo in dragGameObjects)
                dgo.CanMove(false);
        }            
        else if (curSeqNum >= enableEndSeqNum)
        {
            btnModelImport.enabled = false;
            foreach (Button button in buttons)
                button.enabled = false;

            if (btnPrinting != null)
                btnPrinting.enabled = false;

            foreach (DragGameObject dgo in dragGameObjects)
                dgo.CanMove(false);
        }
    }
}
