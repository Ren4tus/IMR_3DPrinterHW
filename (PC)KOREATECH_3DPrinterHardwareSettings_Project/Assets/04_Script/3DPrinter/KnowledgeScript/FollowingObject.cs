using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObject : MonoBehaviour
{
    public RectTransform ContextBox;

    private void Start()
    {
        ContextBox = ContextBox.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 newPosition = Input.mousePosition;
        ContextBox.position = new Vector3(newPosition.x+ ContextBox.rect.width/2, newPosition.y+ ContextBox.rect.height/2, 0);
    }
}