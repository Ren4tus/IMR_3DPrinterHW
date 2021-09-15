using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBottle : MonoBehaviour
{
    [Header("MaterialBottle")]
    public Animation MaterialBottleAni;
    public int materialBottleOutSeqNum; // 25
    private bool materialBottleOut = false;
    private Vector3 materialBottlePos = new Vector3(-0.01106823f, -0.198715f, 0.1812082f);
    private bool canPlay = true;
    private bool init = true;

    void Init()
    {
        MaterialBottleAni.gameObject.SetActive(true);
        MaterialBottleAni.transform.localPosition = materialBottlePos;
        canPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (MainController.instance.GetCurrentSeq() < materialBottleOutSeqNum)
        {
            Init();
        }
        else if (MainController.instance.GetCurrentSeq() == materialBottleOutSeqNum && init)
        {
            init = false;
            Init();
        }
        else if (MainController.instance.GetCurrentSeq() > materialBottleOutSeqNum)
        {
            init = true;
        }
    }

    private void OnMouseDown()
    {
        if (canPlay)
        {
            canPlay = false;
            MaterialBottleAni.gameObject.GetComponent<Highlighter>().ConstantOff();
            MaterialBottleAni.Play("MaterialBottleOut");
            StartCoroutine(DisableBottle());
        }
    }

    IEnumerator DisableBottle()
    {
        yield return new WaitForSeconds(1f);

        MaterialBottleAni.gameObject.SetActive(false);
    }
}
