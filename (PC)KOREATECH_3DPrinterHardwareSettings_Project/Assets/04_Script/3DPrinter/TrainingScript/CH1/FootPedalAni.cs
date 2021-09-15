using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPedalAni : MonoBehaviour
{
    private Animation _ani;

    // Start is called before the first frame update
    void Start()
    {
        _ani = GetComponent<Animation>();
    }

    private void OnMouseDown()
    {
        _ani.Play("Footpedal");
    }
}
