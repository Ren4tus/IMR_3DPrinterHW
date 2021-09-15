using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public MainController mainController;
    public SequenceGroupData sequenceGroupData;

    public void firstSceneInit()
    {
        for(int i=0; i<sequenceGroupData.AllDataBySeq.Length; i++)
        {
            sequenceGroupData.AllDataBySeq[i].GetComponent<ActionData>().enabled = false;
        }
    }

    public void setOnOffBySeq(int prevSeq, int nextSeq)
    {
        int i = 0;

        if (sequenceGroupData.AllDataBySeq[prevSeq].GetComponent<ActionData>().ObjName == null ||
            sequenceGroupData.AllDataBySeq[nextSeq].GetComponent<ActionData>().ObjName == null)
            return;

        sequenceGroupData.AllDataBySeq[prevSeq].GetComponent<ActionData>().enabled = false;
        sequenceGroupData.AllDataBySeq[nextSeq].GetComponent<ActionData>().enabled = true;
    }
}