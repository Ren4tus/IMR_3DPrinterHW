using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrinterInterfaceProgress : MonoBehaviour
{
    public Image[] GreenImage;

    public Text[] TempText;

    public int ClickSeqNum = 14;

    private bool isClick = false;

    private void OnEnable()
    {
        GreenImage[0].fillAmount = 0f;
        TempText[0].text = "49";

        GreenImage[1].fillAmount = 0f;
        TempText[1].text = "49";

        isClick = false;
    }

    private void OnMouseDown()
    {
        if (MainController.instance.GetCurrentSeq() == ClickSeqNum && !isClick)
        {
            isClick = true;
            StopAllCoroutines();
            StartCoroutine(FillTemp(1.5f));
        }
    }

    IEnumerator FillTemp(float time)
    {
        float t = 0f;

        while(t <= time)
        {
            t += Time.deltaTime;
            GreenImage[0].fillAmount = (t / 1f) / time;
            TempText[0].text = (GreenImage[0].fillAmount * 23f + 49).ToString("N0");

            GreenImage[1].fillAmount = (t / 1f) / time;
            TempText[1].text = (GreenImage[1].fillAmount * 23f + 49).ToString("N0");
            yield return new WaitForFixedUpdate();
        }
    }
}