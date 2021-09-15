using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Check : MonoBehaviour
{
    public int StartSeq;
    public int EndSeq;
    Text text;
    public Color color;
    string content;
    int befSeq = -1;
    private void Awake()
    {
        text = this.GetComponent<Text>();
        content = text.text.Substring(2);
        text.text = "○" + content;
    }
    private void Update()
    {
        int seq = MainController.instance.GetCurrentSeq();
        if (befSeq == seq)
        {
            return;
        }
        if (seq >= StartSeq && seq <= EndSeq)
        {
            text.color = color;
        }
        else
        {
            text.color = Color.white;
            if (seq > EndSeq)
            {
                text.text = "●" + content;
            }
            else
            {
                text.text = "○" + content;
            }
        }
        befSeq = seq;
    }
    private void OnDisable()
    {
        int seq = MainController.instance.GetCurrentSeq();
        if (befSeq == seq)
        {
            return;
        }
        if (seq >= StartSeq && seq <= EndSeq)
        {
            text.color = color;
        }
        else
        {
            text.color = Color.white;
            if (seq > EndSeq)
            {
                text.text = "●" + content;
            }
        }
        befSeq = seq;
    }
    public void GotoSeq()
    {
        MainController.instance.LoadSeq(StartSeq, true);
    }
}