using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class CL_TextUI : MonoBehaviour
{
    private Text _text;
    public int TextIndex = 0;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void UpdateText(string text)
    {
        _text.text = text;
    }
}