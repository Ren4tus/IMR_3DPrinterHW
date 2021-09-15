using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour
{
    public MainController MainController;
    public int SeqNum;
    public Vector3 ToAngle;
    public float OpeningTime;
    bool Opened = false;
    void Update()
    {
        if(SeqNum == MainController.GetCurrentSeq() && !Opened)
        {
            Opened = true;
            StartCoroutine("Open");
        }
    }
    IEnumerator Open()
    {
        Vector3 v = (ToAngle - transform.localEulerAngles) / OpeningTime;
        Vector3 k = transform.eulerAngles;
        float T = 0;
        while (T < OpeningTime)
        {
            Quaternion Q = new Quaternion();
            k += v * Time.deltaTime;
            Q.eulerAngles = k;
            T += Time.deltaTime;
            transform.rotation = Q;
            yield return null;
        }
        yield return null;
    }
}
