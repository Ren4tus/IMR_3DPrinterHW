using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DragGameObject : MonoBehaviour
{
    public SlicerBuildTrayRotateObj trayRotate;

    public Button btnMove;

    public Color deactiveColor;
    public Color activeColor;
    
    bool canMove = false;

    IEnumerator OnMouseDown()
    {
        Vector3 scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z));
       
        if (canMove)
        {
            while (Input.GetMouseButton(0))
            {
                int rotateIndex = trayRotate.getRotateIndex();

                Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
                
                Vector3 pos = new Vector3(curPosition.x, curPosition.y -offset.y, curPosition.z);
                transform.position = pos;

                Vector3 localPos = transform.localPosition;
                transform.localPosition = new Vector3(localPos.x, 0.0f, localPos.y + localPos.z);

                // Clamping to Z boundary
                localPos = transform.localPosition;
                float zMin = rotateIndex % 2 == 0 ? -0.11f : -0.01f;
                float zMax = rotateIndex % 2 == 0 ? 0.27f : 0.18f;
                if (localPos.z < zMin)
                    transform.localPosition = new Vector3(localPos.x, 0.0f, zMin);
                else if (localPos.z > zMax)
                    transform.localPosition = new Vector3(localPos.x, 0.0f, zMax);

                // Clamping to X boundary
                localPos = transform.localPosition;
                float xMin = rotateIndex % 2 == 0 ? -0.15f : -0.23f;
                float xMax = rotateIndex % 2 == 0 ? 0.13f : 0.25f;
                if (localPos.x < xMin)
                    transform.localPosition = new Vector3(xMin, 0.0f, localPos.z);
                else if (localPos.x > xMax)
                    transform.localPosition = new Vector3(xMax, 0.0f, localPos.z);

                yield return null;
            }
        }
    }

    public void CanMove()
    {
        btnMove.image.color = canMove ? deactiveColor : activeColor;
        canMove = canMove ? false : true;
    }

    public void CanMove(bool value)
    {
        btnMove.image.color = value ? activeColor : deactiveColor;
        canMove = value ? true : false;
    }

    /*
    IEnumerator OnMouseDown()
    {
        Vector3 scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z));

        while (Input.GetMouseButton(0))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            transform.position = curPosition;
            yield return null;
        }
    }
    */
}