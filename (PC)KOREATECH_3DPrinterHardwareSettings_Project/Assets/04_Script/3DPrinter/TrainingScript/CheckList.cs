using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
    public Toggle[] lists;

    private int count=0;

    private void OnEnable()
    {
        // Initialize all toggle buttons
        foreach (Toggle t in lists)
            t.isOn = false;
    }

    public void OnValueChangeCheckList()
    {
        foreach(Toggle t in lists)
        {
            if(t.isOn)
                count++;
        }

        if(count >= lists.Length)
        {
            MainController.instance.isRightClick = true;
            MainController.instance.goNextSeq();
            MainController.instance.isRightClick = false;
        }

        count = 0;
    }
}