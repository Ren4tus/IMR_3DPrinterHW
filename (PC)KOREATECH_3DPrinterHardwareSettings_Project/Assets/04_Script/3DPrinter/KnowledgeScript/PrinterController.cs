using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrinterController : MonoBehaviour
{
    public GameObject[] chapter;
    public GameObject chapterClear;
    public Text chapterClearText;
    public int idx=0;

    private int max_idx=0;
    public static PrinterController instance;

    public void BlockBehindUI(bool b)
    {
        HoverObj.SetBlocking(b);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        foreach (GameObject i in chapter)
        {
            i.SetActive(false);
        }

        chapter[0].SetActive(true);
    }

    public void NextBtn()
    {
        if (idx >= chapter.Length-1)
        {
            return;
        }

        chapter[idx].SetActive(false);
        chapter[++idx].SetActive(true);

        //CommonFunctionManager.Instance.PlayNarration(idx);

        if (max_idx < idx)
            max_idx = idx;
    }

    public void PrevBtn()
    {
        if (idx <= 0)
            return;

        chapter[idx].SetActive(false);
        chapter[--idx].SetActive(true);

        //CommonFunctionManager.Instance.PlayNarration(idx);
    }
}