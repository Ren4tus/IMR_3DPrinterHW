using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAutoRotate : MonoBehaviour
{
    public float RotateSpeed; // 회전 속도
    private float x, y, z;

    void Start()
    {
        x = this.transform.localEulerAngles.x;
        y = 0.0f;
        z = this.transform.localEulerAngles.z;
    }

    // Rotate
    void Update()
    {
        y += Time.deltaTime * RotateSpeed;
        this.transform.rotation = Quaternion.Euler(x, y, z);
    }
}