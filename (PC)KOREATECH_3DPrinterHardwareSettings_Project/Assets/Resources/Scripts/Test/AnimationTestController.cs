using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTestController : MonoBehaviour
{
    public Text textUI;
    private int idx = 0;

    private string originalStr;
    private string tempStr;

    void Start()
    {
        originalStr = textUI.text;
    }
    
    void Update()
    {
        switch(idx)
        {
            case 0:
                tempStr = "모델재료1 L\n";
                break;
            case 1:
                tempStr = "모델재료1 R\n";
                break;
            case 2:
                tempStr = "모델재료2 L\n";
                break;
            case 3:
                tempStr = "모델재료2 R\n";
                break;
            case 4:
                tempStr = "모델재료3 L\n";
                break;
            case 5:
                tempStr = "모델재료3 R\n";
                break;
            case 6:
                tempStr = "서포트재료 L\n";
                break;
            case 7:
                tempStr = "서포트재료 R\n";
                break;
            default:
                tempStr = "해당 없음\n";
                break;
        }
        tempStr = tempStr + originalStr;
        textUI.text = tempStr;

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (MJPASController.Instance.MaterialCount > idx)
                idx++;
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (idx > 0)
                idx--;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            MJPASController.Instance.MaterialToggle(idx);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            MJPASController.Instance.WastePanelToggle();
        }
    }
}
