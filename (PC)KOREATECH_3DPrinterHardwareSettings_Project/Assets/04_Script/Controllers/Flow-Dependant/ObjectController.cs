using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class ObjectController : MonoBehaviour
{
    public MainController mainController;
    public SequenceGroupData sequenceGroupData;

    [Space]
    public GameObject[] AllObjects;

    [Space]
    public Button seqButtonUp;
    public Button seqButtonDown;

    public void firstSceneInit()
    {
        for (int i = 0; i < AllObjects.Length; i++)
        {
            AllObjects[i].SetActive(false);
        }
    }
    GameObject[] prevObjects;
    // 이전 시퀀스에서 ON 되어 있는 오브젝트를 모두 OFF 하고,
    // 다음 시퀀스에 필요한 오브젝트를 모두 ON 한다.
    // 처음 프로그램 시작 때 오브젝트는 모두 OFF 되어 있어야 한다. (추후 수정 예정)
    public void setOnOffBySeq(int prevSeq, int nextSeq)
    {
        int i = 0;

        if (sequenceGroupData.AllDataBySeq[nextSeq].GetComponent<ObjectData>() == null)
        {
            mainController.finishedByCont(Controllers.Object);
            return;
        }
        if(sequenceGroupData.AllDataBySeq[prevSeq].GetComponent<ObjectData>() != null)
        {
            prevObjects = sequenceGroupData.AllDataBySeq[prevSeq].GetComponent<ObjectData>().ObjectToBeOn;
        }
        GameObject[] nextObjects = sequenceGroupData.AllDataBySeq[nextSeq].GetComponent<ObjectData>().ObjectToBeOn;

        for (i = 0; i < prevObjects.Length; i++)
        {
            if(prevObjects[i].activeSelf)
                prevObjects[i].SetActive(false);
        }

        for (i = 0; i < nextObjects.Length; i++)
        {
            if (!nextObjects[i].activeSelf)
                nextObjects[i].SetActive(true);
        }
        prevObjects = nextObjects;
        mainController.finishedByCont(Controllers.Object);

        if (seqButtonUp != null) seqButtonUp.enabled = true;
        if (seqButtonDown != null) seqButtonDown.enabled = true;
    }
}
