using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationTutorialController : TweenableCanvas
{
    public void BlockMouseInput()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReleaseMouseInput()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
