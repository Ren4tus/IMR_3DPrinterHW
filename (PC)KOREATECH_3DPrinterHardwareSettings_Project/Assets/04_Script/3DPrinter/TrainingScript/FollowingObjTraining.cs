using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObjTraining : MonoBehaviour
{
    public RectTransform HoverImageBG;

    private void Start()
    {
        HoverImageBG = HoverImageBG.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 newPosition = Input.mousePosition;
        HoverImageBG.position = new Vector3(newPosition.x + HoverImageBG.rect.width / 2 + 10, newPosition.y - HoverImageBG.rect.height / 2 - 10, 0);
    }
}
