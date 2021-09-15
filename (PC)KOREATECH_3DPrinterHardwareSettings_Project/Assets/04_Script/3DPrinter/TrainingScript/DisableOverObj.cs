using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOverObj : MonoBehaviour
{
    public int Tobeon;
    void Update()
    {
        if(Tobeon != MainController.instance.GetCurrentSeq())
        {
            gameObject.SetActive(false);
        }
    }
}