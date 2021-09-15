using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterInterfaceFollowingObj : MonoBehaviour
{
    public RectTransform PrinterInterfaceHoverObj;

    private void Start()
    {
        PrinterInterfaceHoverObj = PrinterInterfaceHoverObj.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 newPosition = Input.mousePosition;
        PrinterInterfaceHoverObj.position = new Vector3(newPosition.x + PrinterInterfaceHoverObj.rect.width / 2, newPosition.y + PrinterInterfaceHoverObj.rect.height / 2, 0);
    }
}
