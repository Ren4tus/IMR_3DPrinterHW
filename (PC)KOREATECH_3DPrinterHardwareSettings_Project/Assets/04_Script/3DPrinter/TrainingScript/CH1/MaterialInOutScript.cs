using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInOutScript : MonoBehaviour
{
    private bool materialIn = true;
    private bool canClick = true;

    private void OnMouseDown()
    {
        if (canClick)
        {
            if (materialIn)
            {
                canClick = false;
                MJPASController.Instance.MaterialOut(4);
                materialIn = false;
                StartCoroutine(CanClick());
            }
            else
            {
                canClick = false;
                MJPASController.Instance.MaterialIn(4);
                materialIn = true; 
                StartCoroutine(CanClick());
            }
        }
    }

    IEnumerator CanClick()
    {
        yield return new WaitForSeconds(1f);

        canClick = true;
    }
}
