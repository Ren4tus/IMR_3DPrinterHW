using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraResolution : MonoBehaviour
{
    private Camera camera;
    private Rect rect;

    private void Awake()
    {
        ResolutionSet2();
    }

    private void Update()
    {
        ResolutionSet2();
    }

    public void ResolutionSet()
    {
        camera = GetComponent<Camera>();
        rect = camera.rect;

        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }

    public void ResolutionSet2()
    {
        camera = GetComponent<Camera>();
        rect = camera.rect;

        if(Screen.width >= 1920)
        {
            rect.width = 1;
            rect.height = 1;
            rect.x = 0;
            rect.y = 0;
        }
        else
        {
            if(Screen.height >= 1024)
            {
                ResolutionSet();
            }
            else
            {
                rect.width = 1;
                rect.height = 1;
                rect.x = 0;
                rect.y = 0;
            }
        }

        camera.rect = rect;
    }
}
