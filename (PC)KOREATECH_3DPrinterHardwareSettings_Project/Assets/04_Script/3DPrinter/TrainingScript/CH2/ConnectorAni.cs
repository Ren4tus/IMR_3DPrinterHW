using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorAni : MonoBehaviour
{
    [Header("ConnectorIn")]
    public Animation Connector;
    public int connectorSeqNum; // 9
    private Vector3 connectorPos = new Vector3(0f, 0.03f, 0f);
    private bool init = true;

    void Init()
    {
        Connector.transform.localPosition = connectorPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (MainController.instance.GetCurrentSeq() < connectorSeqNum)
        {
            Init();
        }
        else if (MainController.instance.GetCurrentSeq() == connectorSeqNum && init)
        {
            init = false;
            Init();
        }
        else if (MainController.instance.GetCurrentSeq() > connectorSeqNum)
        {
            init = true;
        }

    }

    private void OnMouseDown()
    {
        if (Connector.transform.localPosition.y >= 0.025f)
        {
            this.GetComponent<Highlighter>().ConstantOff();
            Connector.Play("ConnectorIn");
        }
    }
}
