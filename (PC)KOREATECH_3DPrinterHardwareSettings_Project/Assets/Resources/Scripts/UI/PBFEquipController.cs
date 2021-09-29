using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBFEquipController : MonoBehaviour
{
    private const int ID_HEXAGON_WRENCH = 4;
    private const int ID_NITRIL_GLOVES = 5;
    private const int ID_DUSTPROOF_MASK = 6;
    private const int ID_TISSU_FOLD = 8;

    private const int ID_EXTRACTION_CYLINDER = 11;
    private const int ID_SCULPTURE_EX1 = 13;
    private const int ID_SCULPTURE_EX2 = 14;

    // 장착 가능한 아이템 리스트
    public List<Transform> EquipableItems;
    private bool[] isEquip = null;

    private void Awake()
    {
        if (EquipableItems.Count > 0 && isEquip == null)
        {
            isEquip = new bool[EquipableItems.Count];

            for (int i = 0; i < EquipableItems.Count; i++)
                isEquip[i] = false;
        }
    }

    public void Equip(int index)
    {
        if (index < 0 || index >= EquipableItems.Count)
            return;

        if (!isEquip[index])
        {
            // 장착
            EquipableItems[index].gameObject.SetActive(false);
            isEquip[index] = true;
        }
        else
        {
            // 해제
            EquipableItems[index].gameObject.SetActive(true);
            isEquip[index] = false;
        }
    }

    // PBF
    public void Equip_Hexagon_Wrench()
    {
        Equip(ID_HEXAGON_WRENCH);
    }
    public void Equip_NitrilGloves()
    {
        Equip(ID_NITRIL_GLOVES);
    }
    public void Equip_DustproofMask()
    {
        Equip(ID_DUSTPROOF_MASK);
    }
    
    public void Equip_TissuFold()
    {
        Equip(ID_TISSU_FOLD);
    }

    public void Equip_Extraction_Cylinder()
    {
        Equip(ID_EXTRACTION_CYLINDER);
    }
    
}