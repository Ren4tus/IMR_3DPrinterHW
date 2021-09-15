using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipCheckScript : MonoBehaviour
{
    public GameObject gloves;
    public GameObject glasses;
    public GameObject mask;

    private bool isCheck = true;

    private void Update()
    {
        if(!gloves.activeSelf && !glasses.activeSelf && !mask.activeSelf && isCheck)
        {
            isCheck = false;

            MainController.instance.isRightClick = true;
            MainController.instance.goNextSeq();
            MainController.instance.isRightClick = false;
        }
    }
}