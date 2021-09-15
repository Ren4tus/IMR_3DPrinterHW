using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnowledgeScriptControl : MonoBehaviour
{
    public static KnowledgeScriptControl instance;

    public TextAsset[] scriptsInFile;
    public Text scriptWindow;

    [Space]
    public GameObject UpBtn;
    public GameObject DownBtn;

    [Space]
    public Sprite[] InactiveImage;
    public Sprite[] ActiveImage;

    private bool isScriptPlay;

    int idx = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        string tempStr = scriptsInFile[0].text;

        int startIdx = tempStr.LastIndexOf("<KR>") + "<KR>".Length;
        int lastIdx = tempStr.IndexOf("</KR>");
        tempStr = tempStr.Substring(startIdx, lastIdx - startIdx);

        scriptWindow.text = tempStr;
    }

    private void Update()
    {
        if(idx-1 < 0)
        {
            UpBtn.GetComponent<Image>().sprite = InactiveImage[0];
            UpBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            UpBtn.GetComponent<Image>().sprite = ActiveImage[0];
            UpBtn.GetComponent<Button>().interactable = true;
        }


        if(idx+1 >= scriptsInFile.Length)
        {
            DownBtn.GetComponent<Image>().sprite = InactiveImage[1];
            DownBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            DownBtn.GetComponent<Image>().sprite = ActiveImage[1];
            DownBtn.GetComponent<Button>().interactable = true;
        }
    }

    public void NextScript()
    {
        string tempStr = scriptsInFile[++idx].text;

        int startIdx = tempStr.LastIndexOf("<KR>") + "<KR>".Length;
        int lastIdx = tempStr.IndexOf("</KR>");
        tempStr = tempStr.Substring(startIdx, lastIdx - startIdx);

        scriptWindow.text = tempStr;
    }
    public void PrevScript()
    {
        string tempStr = scriptsInFile[--idx].text;

        int startIdx = tempStr.LastIndexOf("<KR>") + "<KR>".Length;
        int lastIdx = tempStr.IndexOf("</KR>");
        tempStr = tempStr.Substring(startIdx, lastIdx - startIdx);

        scriptWindow.text = tempStr;
    }

    private IEnumerator DialogueEffect(string str)
    {
        scriptWindow.text = "";

        isScriptPlay = true;
        int charIdx = 0;
        while (str.Length > charIdx)
        {
            scriptWindow.text += str[charIdx];
            charIdx++;
            yield return new WaitForSeconds(0.05f);
        }
        scriptWindow.text = str;
        isScriptPlay = false;
    }
}