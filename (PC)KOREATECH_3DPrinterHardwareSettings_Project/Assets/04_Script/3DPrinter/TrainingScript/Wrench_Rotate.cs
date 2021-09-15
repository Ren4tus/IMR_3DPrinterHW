using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrench_Rotate : MonoBehaviour
{
    private float speed = 15f;

    void Update()
    {
        this.gameObject.transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f), Space.World);
    }
}
