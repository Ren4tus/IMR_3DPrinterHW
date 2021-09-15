using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicerBuildTrayRotateObj : MonoBehaviour
{
    // y축 0, 90, 180, 270 회전

    private int rotateIndex = 0;

    public void ChangeRotate()
    {
        rotateIndex++;

        if (rotateIndex >= 4)
            rotateIndex = 0;

        if (this.gameObject.name == "Impeller")
        {
            switch (rotateIndex)
            {
                case 0:
                    transform.localRotation = Quaternion.Euler(-263.797f, 67.108f, 95.257f);
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(-263.797f, 67.108f, 20f);
                    break;
                case 2:
                    transform.localRotation = Quaternion.Euler(-263.797f, 67.108f, 95f);
                    break;
                case 3:
                    transform.localRotation = Quaternion.Euler(-263.797f, 67.108f, 20f);
                    break;
                default:
                    Debug.Log("Setting Error");
                    break;
            }
        }
        else
        {
            switch (rotateIndex)
            {
                case 0:
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    break;
                case 2:
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    break;
                case 3:
                    transform.localRotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                    break;
                default:
                    Debug.Log("Setting Error");
                    break;
            }
        }
    }

    public int getRotateIndex()
    {
        return rotateIndex;
    }
}
