using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipList : MonoBehaviour
{
    public GameObject[] Equipments;
    public RectTransform[] EquipImages;
    public Vector2[] pos;

    int count = 0;
    private bool[] checks;

    private string[] equipList;

    public static EquipList instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        checks = new bool[Equipments.Length];
        equipList = new string[Equipments.Length];
        for(int i=0; i<checks.Length; i++)
        {
            checks[i] = true;
            equipList[i] = "";
        }
    }

    void Update()
    {
        /*
        if (count >= Equipments.Length)
            return;
            */

        for(int i=0; i<Equipments.Length; i++)
        {
            if(!Equipments[i].activeSelf && checks[i])
            {
                EquipImages[i].gameObject.SetActive(true);
                EquipImages[i].anchoredPosition = pos[count];
                count++;
                checks[i] = false;
                equipList[i] = Equipments[i].name;
            }
        }

        for(int i=0; i<Equipments.Length; i++)
        {
            if((!EquipImages[i].gameObject.activeSelf || Equipments[i].activeSelf) && !checks[i])
            {
                Equipments[i].SetActive(true);
                EquipImages[i].gameObject.SetActive(false);
                count--;
                checks[i] = true;
                equipList[i] = "";
            }
        }
    }
    public bool EquipCheck(string equipName)
    {
        foreach(string s in equipList)
        {
            if(s.Equals(equipName))
            {
                return true;
            }
        }

        return false;
    }

    public void UnEquip()
    {
        Equipments[0].SetActive(true);
        Equipments[1].SetActive(true);
        Equipments[2].SetActive(true);
    }
}