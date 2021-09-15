using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ColliderController : MonoBehaviour
{
    public MainController mainController;
    public SequenceGroupData sequenceGroupData;

    [Space]
    public BoxCollider[] boxColliders;
    //public ColliderData[] ColliderDatabySeq;

    public void firstSceneInit()
    {
        for (int i = 0; i < boxColliders.Length; i++)
            boxColliders[i].enabled = false;
    }

    public void activateCollider(int prevSeq, int nextSeq)
    {
        if (sequenceGroupData.AllDataBySeq[prevSeq].GetComponent<ColliderData>().ColliderOnObjects == null ||
            sequenceGroupData.AllDataBySeq[nextSeq].GetComponent<ColliderData>().ColliderOnObjects == null)
            return;

        GameObject[] prevObjs = sequenceGroupData.AllDataBySeq[prevSeq].GetComponent<ColliderData>().ColliderOnObjects;
        GameObject[] nextObjs = sequenceGroupData.AllDataBySeq[nextSeq].GetComponent<ColliderData>().ColliderOnObjects;

        for (int i = 0; i < prevObjs.Length; i++)
            prevObjs[i].GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < nextObjs.Length; i++)
            nextObjs[i].GetComponent<BoxCollider>().enabled = true;

        mainController.finishedByCont(Controllers.Collider);
    }
}
