using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlink : MonoBehaviour
{
    private Color InitialColor;
    private bool IsWhite;

    public void Awake()
    {
        InitialColor = GetComponent<Image>().color;
    }

    public void OnEnable()
    {
        StartCoroutine(Blink());
    }

    public void OnDisable()
    {
        StopCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while(true)
        {
            if (IsWhite)
            {
                GetComponent<Image>().color = InitialColor;
                IsWhite = false;
                
            }                
            else
            {
                GetComponent<Image>().color = Color.white;
                IsWhite = true;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
