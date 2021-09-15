using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckList_Ch : MonoBehaviour
{
    public GameObject checklist;
    public int startseq;
    public int endseq;
    int befSeq = -1;

    private void Update()
    {
        int seq = MainController.instance.GetCurrentSeq();
        if (befSeq == seq)
        {
            return;
        }
        if (seq <= endseq && seq >= startseq)
        {
            checklist.SetActive(true);
        }
        else
        {
            checklist.SetActive(false);
        }
        befSeq = seq;
    }
}
