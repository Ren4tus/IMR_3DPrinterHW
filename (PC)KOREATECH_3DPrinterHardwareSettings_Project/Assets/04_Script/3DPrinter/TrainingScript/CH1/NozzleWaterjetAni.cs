using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NozzleWaterjetAni : MonoBehaviour
{
    public Animation LeftHand;
    public Animation RightHand;

    private void OnEnable()
    {
        this.gameObject.transform.localPosition = new Vector3(-0.265f, 1.134f, 0.037f);
    }

    private void OnMouseDown()
    {
        RightHand.gameObject.SetActive(true);
        RightHand.Play("WaterJetRightHand");

        LeftHand.gameObject.SetActive(false);

        this.gameObject.transform.localPosition = new Vector3(-2f, 0, 0);
    }
}