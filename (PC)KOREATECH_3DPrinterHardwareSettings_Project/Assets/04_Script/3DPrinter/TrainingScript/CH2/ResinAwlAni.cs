using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResinAwlAni : MonoBehaviour
{
    public Animation ResinAwl;
    public int resinAwlSeqNum;

    private bool init = true;

    private void OnMouseDown()
    {
        if (MainController.instance.GetCurrentSeq() == resinAwlSeqNum)
        {
            StartCoroutine(ResinAni());
        }
    }

    private void Init()
    {
        ResinAwl.gameObject.transform.localPosition = new Vector3(-8.846f, 0.855f, -1.018f);
        ResinAwl.gameObject.transform.localRotation = Quaternion.Euler(-90.0f, 180.0f, 0.0f);

        init = false;
    }

    private void Update()
    {
        if (MainController.instance.GetCurrentSeq() == resinAwlSeqNum)
        {
            if (init)
            {
                Init();
            }
        }
    }

    IEnumerator ResinAni()
    {
        ResinAwl.Play("ResinAwl");

        yield return new WaitForSeconds(1.0f);

        init = true;
    }
}