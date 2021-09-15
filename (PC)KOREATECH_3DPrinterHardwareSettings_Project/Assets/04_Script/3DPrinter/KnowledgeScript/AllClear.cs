using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllClear : MonoBehaviour
{
    public GameObject clearpopup;

    private void OnEnable()
    {
        if(EndKnowledge.instance.IsAllClear())
        {
            clearpopup.SetActive(true);
        }
    }
}
