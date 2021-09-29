using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBFEquipItem : MonoBehaviour
{
    private bool isEquip = false;
    public bool IsEquip => isEquip;

    public void Equip()
    {
        isEquip = true;
    }
}
