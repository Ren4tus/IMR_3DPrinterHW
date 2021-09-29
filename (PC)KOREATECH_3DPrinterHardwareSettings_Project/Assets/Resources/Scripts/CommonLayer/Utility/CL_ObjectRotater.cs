using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CL_ObjectRotater : MonoBehaviour
{
    private float speed = 10.0f;
    private Vector3 rot;

    public HoverObj boolCover;
    public GameObject Cover;
    public GameObject Door;
    
    void Update()
    {
        if (Input.GetMouseButton(0) && !HoverObj.GetBlocking())
        {
            transform.Rotate(0f, -Input.GetAxis("Mouse X") * speed, 0f, Space.World);
            transform.Rotate(-Input.GetAxis("Mouse Y") * speed, 0f, 0f);
        }
    }

    public void ResetBtn()
    {
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        boolCover.isCoverOpen = true;
        Cover.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    public void ResetBtn_Cabinet()
    {
        transform.rotation = Quaternion.Euler(0f, 190f, 0f);
        boolCover.isDoorOpen = true;
        Door.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void ResetBtn_WaterJet()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}