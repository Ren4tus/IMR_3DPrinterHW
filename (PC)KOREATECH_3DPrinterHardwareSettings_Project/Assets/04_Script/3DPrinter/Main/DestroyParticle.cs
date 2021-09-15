using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    void OnEnable()
    {
        Invoke("DisableParticle", 0.6f);
    }

    private void DisableParticle()
    {
        this.gameObject.SetActive(false);
    }
}
