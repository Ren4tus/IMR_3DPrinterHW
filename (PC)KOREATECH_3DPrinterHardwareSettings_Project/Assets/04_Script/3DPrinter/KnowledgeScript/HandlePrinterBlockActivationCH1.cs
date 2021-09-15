using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePrinterBlockActivationCH1 : MonoBehaviour
{
    public KnowledgeTabController controller;

    public void SetActivation()
    {
        int tabID = controller.GetCurrentTabID();

        if (tabID <= 0)
            this.gameObject.SetActive(true);
        else
            this.gameObject.SetActive(false);
    }
}
