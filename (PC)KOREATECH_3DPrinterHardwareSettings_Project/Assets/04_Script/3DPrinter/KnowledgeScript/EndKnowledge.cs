using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndKnowledge : MonoBehaviour
{
    private bool[] isClear = { false, false, false};

    public static EndKnowledge instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ClearChapter(int num)
    {
        isClear[num] = true;
    }

    public bool IsAllClear()
    {
        if(isClear[0] && isClear[1] && isClear[2])
            return true;

        return false;
    }
}