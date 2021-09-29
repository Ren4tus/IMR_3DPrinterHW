using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSViewer : MonoBehaviour
{
    float deltaTime = 0.0f;

    GUIStyle style;
    Rect rect;
    float msec;
    float fps;
    float worstFps = 100f;
    string text;

    void Awake()
    {
        int w = Screen.width, h = Screen.height;

        rect = new Rect(0, 0, w, h * 4 / 100);

        style = new GUIStyle();
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 4 / 100;
        style.normal.textColor = Color.cyan;
    }
    
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {

        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime; 

        if (fps < worstFps)
            worstFps = fps; // 최소 프레임 구현은 해놓았으나 출력은 X

        text = msec.ToString("F1") + "ms (" + fps.ToString("F1") + ")";
        GUI.Label(rect, text, style);
    }
}
