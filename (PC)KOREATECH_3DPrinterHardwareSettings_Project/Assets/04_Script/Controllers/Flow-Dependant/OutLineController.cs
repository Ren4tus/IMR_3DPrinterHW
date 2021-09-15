using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class OutLineController : MonoBehaviour
{
    public MainController mainController;
    public SequenceGroupData sequenceGroupData;
    public Color EffectColor;
    public Vector2 EffectDistance;


    [Space]
    public Outline[] AllOutlines;

    public void firstSceneInit()
    {
        for (int i = 0; i < AllOutlines.Length; i++)
        {
            AllOutlines[i].enabled = false;
        }

        AllInit();
    }

    public void AllInit()
    {
        for (int i = 0; i < AllOutlines.Length; i++)
        {
            AllOutlines[i].effectColor = EffectColor;
            AllOutlines[i].effectDistance = EffectDistance;
        }
    }

    public void setOnOffBySeq(int prevSeq, int nextSeq)
    {
        int i = 0;

        if (sequenceGroupData.AllDataBySeq[prevSeq].GetComponent<OutLineData>().OutLineObj == null ||
            sequenceGroupData.AllDataBySeq[nextSeq].GetComponent<OutLineData>().OutLineObj == null)
            return;

        GameObject[] prevObjects = sequenceGroupData.AllDataBySeq[prevSeq].GetComponent<OutLineData>().OutLineObj;
        GameObject[] nextObjects = sequenceGroupData.AllDataBySeq[nextSeq].GetComponent<OutLineData>().OutLineObj;

        for (i = 0; i < prevObjects.Length; i++)
        {
            if (prevObjects[i].GetComponent<Outline>().enabled)
            {
                prevObjects[i].GetComponent<Outline>().enabled = false;
            }
        }

        for (i = 0; i < nextObjects.Length; i++)
        {
            if (!nextObjects[i].GetComponent<Outline>().enabled)
            {
                nextObjects[i].GetComponent<Outline>().enabled = true;
            }
        }

        mainController.finishedByCont(Controllers.OutLine);
    }
}