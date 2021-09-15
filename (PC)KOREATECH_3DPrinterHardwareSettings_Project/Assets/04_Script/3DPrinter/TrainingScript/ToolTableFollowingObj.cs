using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ToolTableFollowingObj : MonoBehaviour
{
    public RectTransform toolTableHoverObj;
    
    private void Start()
    {
        toolTableHoverObj = toolTableHoverObj.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 newPosition = Input.mousePosition;
        toolTableHoverObj.position = new Vector3(newPosition.x + toolTableHoverObj.rect.width / 2, newPosition.y + toolTableHoverObj.rect.height / 2, 0);
    }
}
