using UnityEngine;

public class EavluationCH2ChamberDoor : EvaluationInteractiveObject
{
    private bool isDoorOpen = false;

    public void OnMouseDown()
    {
        if (!IsInteractive || IsPointerOverUIObject())
            return;

        ToggleUpperDoor();
    }

    public void ToggleUpperDoor()
    {
        if(isDoorOpen)
        {
            this.GetComponent<Animation>().Play("ChamberDoorClose");
            isDoorOpen = false;
        }
        else
        {
            this.GetComponent<Animation>().Play("ChamberDoorOpen");
            isDoorOpen = true;
        }
    }
}