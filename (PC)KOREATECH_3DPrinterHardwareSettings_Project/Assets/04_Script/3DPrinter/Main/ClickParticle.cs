using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticle : MonoBehaviour
{
    public GameObject[] clickParticles;

    private RectTransform[] particlePos;

    private void Start()
    {
        particlePos = new RectTransform[clickParticles.Length];

        for(int i=0; i<clickParticles.Length; i++)
        {
            particlePos[i] = clickParticles[i].GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 newPosition = Input.mousePosition;

            particlePos[ParticleActive()].transform.position = newPosition;
            clickParticles[ParticleActive()].SetActive(true);
        }
    }


    private int ParticleActive()
    {
        int num = 0;

        for(int i=0; i<clickParticles.Length; i++)
        {
            if(!clickParticles[i].activeSelf)
            {
                num = i;
                break;
            }
        }

        return num;
    }
}
