using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsViewerController : MonoBehaviour
{
    public Dictionary<string, Transform> contentsList;

    private void Awake()
    {
        contentsList = new Dictionary<string, Transform>();
        
        for (int i=0; i< this.transform.childCount; i++)
        {
            Transform temp = this.transform.GetChild(i);
            contentsList.Add(temp.name, temp);
        }
    }
}
