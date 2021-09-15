using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIComponentAdder : MonoBehaviour
{
    public GameObject canvas;
    public bool x = false;
    // Update is called once per frame
    void Update()
    {
        if (!x)
        {
            return;
        }
        x = false;
        //        search(canvas, 1500);
        DeleteT();
    }
    int search(GameObject g, int i)
    {
        if(g.GetComponent<Text>() != null)
        {
            CSVText t = g.AddComponent<CSVText>();
            t.TextIndex = i;
            i++;
        }
        for(int a =0; a < g.transform.childCount; a++)
        {
            i = search(g.transform.GetChild(a).gameObject, i);
        }
        return i;
    }
    void DeleteT()
    {
        CSVText[] cs = FindObjectsOfType<CSVText>();
        for(int a = 0; a < cs.Length; a++)
        {
            DestroyImmediate(cs[a]);
        }
    }
}