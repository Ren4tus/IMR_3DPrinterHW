using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBGTutorial : MonoBehaviour
{
    public Image blackBG;
    Color color;
    float c=0f;
    public GameObject toturial;

    // Update is called once per frame
    void Update()
    {
        color.a += c+Time.deltaTime;
        blackBG.color = color;

        if(color.a >= 1f)
        {
            toturial.SetActive(true);
        }
    }
}
