using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationEquipController : MonoBehaviour
{
    // 장착 가능한 아이템 리스트
    public List<EvaluationEquipableItem> EquipableItems;

    public bool IsAllEquipSafetyEquipment()
    {
        foreach (EvaluationEquipableItem item in EquipableItems)
        {
            if (!item.IsEquip)
                return false;
        }

        return true;
    }
}
