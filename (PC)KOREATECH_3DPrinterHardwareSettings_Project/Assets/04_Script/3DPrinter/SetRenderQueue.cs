using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderQueue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.renderQueue = renderer.material.renderQueue + 1;
    }
}
