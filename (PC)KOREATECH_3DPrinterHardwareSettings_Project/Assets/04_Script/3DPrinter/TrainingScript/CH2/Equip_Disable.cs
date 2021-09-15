using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip_Disable : MonoBehaviour
{
    private void OnMouseDown()
    {
        if(ExamController.instance == null)
        {
            this.gameObject.SetActive(false);
        }
        else if (ExamController.instance.isStart())
        {
            this.gameObject.SetActive(false);
        }
    }
}
