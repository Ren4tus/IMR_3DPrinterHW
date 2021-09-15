using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveListScript : MonoBehaviour
{
    public GameObject[] TrainingList;
    public GameObject ListPopup;
    public GameObject MoveStepPopup;
    public Text StepText;

    public static int a=0;

    public void OpenPopup(int idx)
    {
        MoveStepPopup.SetActive(true);

        foreach(GameObject list in TrainingList)
        {
            list.SetActive(false);
        }

        TrainingList[idx-1].SetActive(true);
        ListPopup.SetActive(false);

        Debug.Log(this.gameObject.name);
    }

    public void MoveList(int idx)
    {
        a = idx-1;
    }

    public void YesBtn()
    {
        foreach (GameObject list in TrainingList)
        {
            list.SetActive(false);
        }

        TrainingList[a].SetActive(true);
        ListPopup.SetActive(false);
        MoveStepPopup.SetActive(false);

        PrinterController.instance.idx = a;
        PrinterController.instance.BlockBehindUI(false);
        //Debug.Log("idx의 현재 값은 : "+PrinterController.instance.idx);
    }

    public void SetStepText(string str)
    {
        StepText.text = str;
        MoveStepPopup.SetActive(true);
    }
}