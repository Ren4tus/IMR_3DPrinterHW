using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PP_PopupLogin : MonoBehaviour
{
    public string CORRECT_ID;
    public string CORRECT_PASSWORD;

    public InputField ID;
    public InputField Password;

    private void OnMouseDown()
    {
        if(ID.text == CORRECT_ID && Password.text == CORRECT_PASSWORD)
        {
            MainController.instance.isRightClick = true;
            MainController.instance.goNextSeq();
            MainController.instance.isRightClick = false;
        }
    }
}
