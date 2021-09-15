using UnityEngine;
using UnityEngine.EventSystems;

public class ObjEventControll : MonoBehaviour
{
    public IntroMainNextControll controller;
    public int TargetNum=0;
    
    private void OnMouseEnter()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            controller.EnableHighlightTarget(TargetNum);
            controller.PlayTextAnimation(TargetNum);
        }                      
    }

    private void OnMouseDown()
    {
         if(!EventSystem.current.IsPointerOverGameObject())
            controller.SetTarget(TargetNum);
    }

    private void OnMouseExit()
    {
        controller.SetTarget(0);
        controller.StopAllTextAnimation();
        controller.DisableHighlightTarget();
    }
}
