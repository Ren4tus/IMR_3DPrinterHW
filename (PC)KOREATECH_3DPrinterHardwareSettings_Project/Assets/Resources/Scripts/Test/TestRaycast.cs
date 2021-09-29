using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestRaycast : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            Debug.Log(EventSystem.current.gameObject.name);

        RaycastHit hit = new RaycastHit();
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Debug.Log("Hit : " + hit.transform.gameObject.name);
        }
    }
}
