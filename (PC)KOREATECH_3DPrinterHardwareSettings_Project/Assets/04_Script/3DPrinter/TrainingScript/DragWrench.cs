using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWrench : MonoBehaviour
{
    float distance = 10;

    void OnMouseDrag()
    {
        /*
        Vector3 mousePosition = new Vector3(Input.mousePosition.x,
        Input.mousePosition.y, 0);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;
        */

        /*
        Debug.Log("Drag!");

        Vector3 v3WorldPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 v3Offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, v3WorldPos.z));

        Vector3 v3CurScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, v3WorldPos.z);
        Vector3 v3CurObjPos = Camera.main.ScreenToWorldPoint(v3CurScreenPos) + v3Offset;
        transform.position = new Vector3(v3CurObjPos.x, transform.position.y, v3CurObjPos.z);
        Debug.Log(v3CurObjPos.x + ", " + transform.position.y + ", " + v3CurObjPos.z);
        */

        Vector3 mousePosition = new Vector3(Input.mousePosition.x,
        Input.mousePosition.y, 0);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;
    }

    /*
    private Collider _collide;

    private Vector3 _curPosition;

    private Vector3 _direction;

    private bool _isTrigger = false;

    void Start()
    {
        _collide = GetComponent<Collider>();
    }

    IEnumerator OnMouseDown()
    {

        Vector3 scrSpace = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z));

        while (Input.GetMouseButton(0))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z);

            _curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            yield return null;
        }
    }

    void OnMouseUp()
    {
        _isTrigger = true;
    }

    void Update()
    {
        if (_isTrigger)
        {
            float step = 5 * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, _curPosition, step);
        }

        if (transform.position == _curPosition)
        {
            _isTrigger = false;
        }
    }
    */
}