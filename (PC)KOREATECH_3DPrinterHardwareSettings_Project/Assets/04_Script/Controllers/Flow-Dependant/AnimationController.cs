using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
public class AnimationController : MonoBehaviour
{
    public static AnimationController instance;
    private void Awake()
    {
        instance = this;
    }
    public void StopLoadingSeq()
    {
        MainController.instance.startByCont(Controllers.Animation);
    }
    public void StartLoadingSeq()
    {
        MainController.instance.finishedByCont(Controllers.Animation);
    }
}
