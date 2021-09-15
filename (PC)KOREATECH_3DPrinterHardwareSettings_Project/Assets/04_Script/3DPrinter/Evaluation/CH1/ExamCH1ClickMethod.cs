using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using UnityEngine.EventSystems;

public class ExamCH1ClickMethod : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (ExamController.instance.isStart() && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (this.gameObject.name)
            {
                case "Cover":
                    MJPASController.Instance.CoverToggle();
                    break;
                case "Wrench03_Complete":
                    this.gameObject.SetActive(CH1MethodSet.instance.Step5_1_Check());
                    break;
                case "BuildTray":
                    CH1MethodSet.instance.Step5_2_Correct();
                    break;
                case "Glass":
                    CH1MethodSet.instance.Step6_Correct();
                    break;
                case "CabinetDoor":
                    MJPASController.Instance.CabinetDoorToggle();
                    break;
                case "ModelBox1_L":
                    CH1MethodSet.instance.Step8_1_Correct(0);
                    break;
                case "ModelBox1_R":
                    CH1MethodSet.instance.Step8_1_Correct(1);
                    break;
                case "ModelBox2_L":
                    CH1MethodSet.instance.Step8_1_Correct(2);
                    break;
                case "ModelBox2_R":
                    CH1MethodSet.instance.Step8_1_Correct(3);
                    break;
                case "ModelBox3_L":
                    CH1MethodSet.instance.Step8_1_Correct(4);
                    break;
                case "ModelBox3_R":
                    CH1MethodSet.instance.Step8_1_Correct(5);
                    break;
                case "SupportBox_L":
                    CH1MethodSet.instance.Step8_1_Correct(6);
                    break;
                case "SupportBox_R":
                    CH1MethodSet.instance.Step8_1_Correct(7);
                    break;
            }
        }
    }

    private void OnMouseEnter()
    {
        if(ExamController.instance.isStart())
            this.GetComponent<Highlighter>().tween = true;
    }

    private void OnMouseExit()
    {
        if(ExamController.instance.isStart())
            this.GetComponent<Highlighter>().tween = false;
    }
}
