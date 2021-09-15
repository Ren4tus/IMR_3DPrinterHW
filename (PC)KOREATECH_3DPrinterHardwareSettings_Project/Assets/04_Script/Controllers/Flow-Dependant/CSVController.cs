using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using System.Text;

public class CSVController : MonoBehaviour
{
    public MainController mainController;

    [Space]
    public int[] ScriptIndex;
    public Text scriptWindow;
    public Text scriptCountText;
    private IEnumerator Effect;
    private bool isScriptPlay;
    public GameObject canvas;

    [Space]
    public string CSVFileName;
    public static List<Dictionary<string, object>> data;

    [Header("Step Progress")]
    public Text StepText;
    public Sprite stepOn;
    public Sprite stepOff;
    public Sprite stepComplete;
    public int[] stepSeq;
    public string[] stepText;
    private int stepCount = 0;
    public TBProgressBarGeneratorV2 TopBar;

    [SerializeField]
    private float typingSpeed = 0.03f;
    private readonly string[] richTextSymbols = { "b", "i" };
    private readonly string[] richTextCloseSymbols = { "b", "i", "size", "color" };
    private string tempstr;

    public Image ScriptNextBtn;
    public Image ScriptPrevBtn;
    public Color normalColor;

    public AudioClip[] narration;

    [Header("TitleIndicator")]
    public TitleIndicatorController TitleIndicator;
    public string TitleString;

    // 추후 실습에서 사용
    //public Button NextButton;
    //public Button NextSceneButton;

    private void Awake()
    {
        data = CSVReader.Read(CSVFileName);
    }

    public void Start()
    {
        isScriptPlay = false;

        if (narration.Length != 0)
            CL_CommonFunctionManager.Instance.SetNarrations(narration);
    }

    public void viewScriptBySeq(int nextSeq)
    {
        if (scriptCountText != null)
            RefreshScriptCounter();

        if (isScriptPlay)
        {
            StopCoroutine(Effect);
        }

        string tempStr = (string)data[ScriptIndex[nextSeq]][CL_CommonFunctionManager.Instance.GetProgramLanguage()];

        Effect = DialogueEffect(tempStr);
        StartCoroutine(Effect);

        // Start Narration
        CL_CommonFunctionManager.Instance.PlayNarration(nextSeq);
    }

    public void viewScriptBySeq(int prevSeq, int nextSeq)
    {
        if (scriptCountText != null)
            RefreshScriptCounter();

        if (ScriptIndex[prevSeq] == ScriptIndex[nextSeq])
        {   // 이전 시퀀스와 다음 시퀀스 인덱스가 같은 경우
            mainController.finishedByCont(Controllers.Script);
            return;
        }


        if (isScriptPlay)
        {
            StopCoroutine(Effect);
        }

        string tempStr = (string)data[ScriptIndex[nextSeq]][CL_CommonFunctionManager.Instance.GetProgramLanguage()];

        Effect = DialogueEffect(tempStr);
        StartCoroutine(Effect);

        // Start Narration
        CL_CommonFunctionManager.Instance.PlayNarration(nextSeq);
    }

    public void skipEffectBySeq(int currSeq)
    {
        if (scriptCountText != null)
            RefreshScriptCounter();

        StopCoroutine(Effect);

        string tempStr = (string)data[ScriptIndex[currSeq]][CL_CommonFunctionManager.Instance.GetProgramLanguage()];

        scriptWindow.text = tempStr;
        isScriptPlay = false;
        ScriptNextBtn.GetComponent<Animation>().Play();
        mainController.finishedByCont(Controllers.Script);
    }

    // 스크립트 텍스트 이펙트
    private IEnumerator DialogueEffect(string text)
    {
        scriptWindow.text = "";
        ScriptNextBtn.GetComponent<Animation>().Stop();
        ScriptNextBtn.color = normalColor;

        //줄바꿈 처리
        text = text.Replace("\\n", "\n");

        isScriptPlay = true;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        int typingIndex = 0;
        float nowWaitTime = 0f;
        char[] splitTypingTexts = text.ToCharArray();
        StringBuilder stringBuilder = new StringBuilder();

        bool bold = false;
        bool italic = false;
        bool size = false;
        bool color = false;
        int fontSize = 0;
        string colorCode = null;

        tempstr = text;
        scriptWindow.text = null;

        while (true)
        {
            yield return waitForEndOfFrame;
            nowWaitTime += Time.deltaTime;

            if (typingIndex == splitTypingTexts.Length)
            {
                break;
            }

            if (splitTypingTexts[typingIndex] == ' ')
            {
                stringBuilder.Append(splitTypingTexts[typingIndex]);
                typingIndex++;
                continue;
            }

            if (nowWaitTime >= typingSpeed)
            {
                if (scriptWindow.supportRichText)
                {
                    bool symbolCatched = false;

                    for (int i = 0; i < richTextSymbols.Length; i++)
                    {
                        string symbol = string.Format("<{0}>", richTextSymbols[i]);
                        string closeSymbol = string.Format("</{0}>", richTextCloseSymbols[i]);

                        if (splitTypingTexts[typingIndex] == '<' && typingIndex + (1 + richTextSymbols[i].Length) < text.Length && text.Substring(typingIndex, 2 + richTextSymbols[i].Length).Equals(symbol))
                        {
                            switch (richTextSymbols[i])
                            {
                                case "b":
                                    typingIndex += symbol.Length;
                                    if (typingIndex + closeSymbol.Length + 3 <= text.Length)
                                    {
                                        if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                                        {
                                            bold = true;
                                            symbolCatched = true;
                                        }
                                    }
                                    break;
                                case "i":
                                    typingIndex += symbol.Length;
                                    if (typingIndex + closeSymbol.Length + 3 <= text.Length)
                                    {
                                        if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                                        {
                                            italic = true;
                                            symbolCatched = true;
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    if (splitTypingTexts[typingIndex] == '<' && typingIndex + 14 < text.Length && text.Substring(typingIndex, 8).Equals("<color=#") && splitTypingTexts[typingIndex + 14] == '>')
                    {
                        string closeSymbol = string.Format("</{0}>", "color");
                        string tempColorCode = text.Substring(typingIndex + 8, 6);

                        typingIndex += 15;
                        if (typingIndex + closeSymbol.Length <= text.Length)
                        {
                            if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                            {
                                color = true;
                                colorCode = tempColorCode;
                                symbolCatched = true;
                            }
                        }
                    }

                    if (splitTypingTexts[typingIndex] == '<' && typingIndex + 6 < text.Length && text.Substring(typingIndex, 6).Equals("<size="))
                    {
                        string closeSymbol = string.Format("</{0}>", "size");
                        string sizeSub = text.Substring(typingIndex + 6);
                        int symbolIndex = sizeSub.IndexOf('>');
                        string tempSize = sizeSub.Substring(0, symbolIndex);

                        typingIndex += 7 + tempSize.Length;
                        if (typingIndex + closeSymbol.Length <= text.Length)
                        {
                            if (text.Substring(typingIndex, text.Length - typingIndex).Contains(closeSymbol))
                            {
                                size = true;
                                fontSize = int.Parse(tempSize);
                                symbolCatched = true;
                            }
                        }
                    }

                    bool closeSymbolCatched = false;

                    for (int i = 0; i < richTextCloseSymbols.Length; i++)
                    {
                        string closeSymbol = string.Format("</{0}>", richTextCloseSymbols[i]);

                        if (splitTypingTexts[typingIndex] == '<' && typingIndex + (1 + richTextCloseSymbols[i].Length) < text.Length && text.Substring(typingIndex, 3 + richTextCloseSymbols[i].Length).Equals(closeSymbol))
                        {
                            switch (richTextCloseSymbols[i])
                            {
                                case "b":
                                    typingIndex += closeSymbol.Length;
                                    bold = false;
                                    closeSymbolCatched = true;
                                    break;
                                case "i":
                                    typingIndex += closeSymbol.Length;
                                    italic = false;
                                    closeSymbolCatched = true;
                                    break;
                                case "size":
                                    typingIndex += closeSymbol.Length;
                                    size = false;
                                    fontSize = 0;
                                    closeSymbolCatched = true;
                                    break;
                                case "color":
                                    typingIndex += closeSymbol.Length;
                                    color = false;
                                    colorCode = null;
                                    closeSymbolCatched = true;
                                    break;
                            }
                        }

                        if (closeSymbolCatched)
                        {
                            break;
                        }
                    }

                    if (symbolCatched || closeSymbolCatched)
                    {
                        continue;
                    }

                    if (typingIndex < text.Length)
                    {
                        string convertedRichText = RichTextConvert(splitTypingTexts[typingIndex].ToString(), bold, italic, size, fontSize, color, colorCode);
                        stringBuilder.Append(convertedRichText);
                    }
                }
                else
                {
                    if (typingIndex < text.Length)
                    {
                        stringBuilder.Append(splitTypingTexts[typingIndex]);
                    }
                }

                scriptWindow.text = stringBuilder.ToString();
                typingIndex++;
                nowWaitTime = 0f;
            }
        }
        isScriptPlay = false;

        /*
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
        */
        ScriptNextBtn.GetComponent<Animation>().Play();
        mainController.finishedByCont(Controllers.Script);
    }

    private string RichTextConvert(string text, bool bold, bool italic, bool size, int fontSize, bool color, string colorCode)
    {
        StringBuilder stringBuilder = new StringBuilder();

        if (bold)
        {
            stringBuilder.Append("<b>");
            stringBuilder.Append(text);
            stringBuilder.Append("</b>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        if (italic)
        {
            stringBuilder.Append("<i>");
            stringBuilder.Append(text);
            stringBuilder.Append("</i>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        if (color)
        {
            stringBuilder.Append("<color=#");
            stringBuilder.Append(colorCode);
            stringBuilder.Append(">");
            stringBuilder.Append(text);
            stringBuilder.Append("</color>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        if (size)
        {
            stringBuilder.Append("<size=");
            stringBuilder.Append(fontSize);
            stringBuilder.Append(">");
            stringBuilder.Append(text);
            stringBuilder.Append("</size>");
            text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        return text;
    }

    public void FindObject()
    {
        CSVText[] TextList = canvas.GetComponentsInChildren<CSVText>();

        foreach (CSVText UIText in TextList)
        {
            UIText.UpdateText();
        }

        CSVImage[] ImageList = canvas.GetComponentsInChildren<CSVImage>();

        foreach (CSVImage UIImage in ImageList)
        {
            UIImage.UpdateImage();
        }
    }

    public void ViewScriptByChoice_Exam(bool correct)
    {
        StopCoroutine(Effect);

        string tempStr = "";
        if (ConfigurationController.Language == "KR")
        {
            if (correct)
                tempStr = "정답입니다. 다음 버튼을 눌러 계속 진행하세요.";
            else
                tempStr = "오답입니다. 다음 버튼을 눌러 계속 진행하세요.";
        }
        else if (ConfigurationController.Language == "EN")
        {
            if (correct)
                tempStr = "Correct Answer. Click Next Button to progress.";
            else
                tempStr = "Wrong Answer.";
        }
        else
        {
            Debug.Log("script Error");
        }

        Effect = DialogueEffect(tempStr);
        StartCoroutine(Effect);
    }

    public void SkipScriptByChoice_Exam(bool correct)
    {
        StopCoroutine(Effect);

        string tempStr = "";
        if (CL_CommonFunctionManager.Instance.GetProgramLanguage() == "KR")
        {
            if (correct)
                tempStr = "정답입니다. 다음 버튼을 눌러 계속 진행하세요.";
            else
                tempStr = "오답입니다. 다음 버튼을 눌러 계속 진행하세요.";
        }
        else if (CL_CommonFunctionManager.Instance.GetProgramLanguage() == "EN")
        {
            if (correct)
                tempStr = "Correct Answer. Click Next Button to progress.";
            else
                tempStr = "Wrong Answer.";
        }
        else
        {
            Debug.Log("script Error");
        }

        scriptWindow.text = tempStr;
        isScriptPlay = false;
        mainController.finishedByCont(Controllers.Script);
    }

    public void ShowTitleIndicator(int num)
    {
        // TitleIndicator.ShowTitleSlideIn(TitleString, stepText[num]);
        TitleIndicator.ShowTitleSlideIn(stepText[num], TitleString);
    }

    public int SyncTitleIndicator()
    {
        if (scriptCountText == null)
            return -1;

        int scriptIndex = 0;
        int currentCount = 1;

        for (int i = 0; i < stepSeq.Length; i++)
        {
            if (mainController.GetCurrentSeq() < stepSeq[i])
            {
                scriptIndex = i - 1;
                break;
            }
        }

        currentCount = mainController.GetCurrentSeq() - stepSeq[scriptIndex] + 1;

        return currentCount;
    }

    public void RefreshScriptCounter()
    {
        if (scriptCountText == null)
            return;

        int scriptIndex = 0;
        int currentCount = 1;
        int maxCount = 0;

        for(int i=0; i<stepSeq.Length; i++)
        {
            if(mainController.GetCurrentSeq() < stepSeq[i])
            {
                scriptIndex = i - 1;
                break;
            }
        }

        // 마지막 시퀀스 일경우
        if (mainController.GetCurrentSeq() >= stepSeq[stepSeq.Length - 1])
        {
            scriptIndex = stepSeq.Length - 1;
            maxCount = ScriptIndex.Length - stepSeq[scriptIndex];
            currentCount = mainController.GetCurrentSeq() - stepSeq[scriptIndex] + 1;
        }
        else
        {
            maxCount = stepSeq[scriptIndex + 1] - stepSeq[scriptIndex];
            currentCount = mainController.GetCurrentSeq() - stepSeq[scriptIndex] + 1;
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<color='#436d88'>");
        sb.Append(currentCount);
        sb.Append("</color><color='#858585'>/");
        sb.Append(maxCount);
        sb.Append("</color>");
        scriptCountText.text = sb.ToString();

        sb.Clear();
    }

    public void ShowTitleIndicatorStart(int currentSeq)
    {
        if (scriptCountText == null)
            return;

        int scriptIndex = 0;

        for (int i = 0; i < stepSeq.Length; i++)
        {
            if (currentSeq < stepSeq[i])
            {
                scriptIndex = i - 1;
                break;
            }
        }

        // 마지막 시퀀스 일경우
        if (currentSeq >= stepSeq[stepSeq.Length - 1])
        {
            scriptIndex = stepSeq.Length - 1;
        }

        ShowTitleIndicator(scriptIndex);
    }

    public void StepProgressBar()
    {
        if (StepText == null)
            return;

        for (int i=0; i<stepSeq.Length; i++)
        {
            if(mainController.GetCurrentSeq() < stepSeq[i])
            {
                stepCount = i-1;
                break;
            }
        }

        if (mainController.GetCurrentSeq() == stepSeq[stepSeq.Length-1])
            stepCount = stepSeq.Length - 1;

        if (stepCount > 0 && stepCount == TopBar.progressSteps.Length)
            stepCount--;

        for(int i=0; i<stepCount; i++)
        {
            TopBar.progressSteps[i].GetComponent<Image>().sprite = stepComplete;
        }

        TopBar.progressSteps[stepCount].GetComponent<Image>().sprite = stepOn;
        StepText.text = "<i>" + stepText[stepCount] + "</i>";

        for(int i=stepCount+1; i<stepSeq.Length; i++)
        {
            TopBar.progressSteps[i].GetComponent<Image>().sprite = stepOff;
        }


        /*
        for (int i=0; i<stepSeq.Length; i++)
        {
            if(mainController.GetCurrentSeq() == stepSeq[i])
            {
                for(int j=0; j<i; j++)
                {
                    StepProgress[j].sprite = stepComplete;
                }

                StepProgress[i].sprite = stepOn;
                StepText.text = stepText[i];
                stepCount = i;

                if((i+1) <= stepSeq.Length)
                {
                    for (int j = i + 1; j < stepSeq.Length; j++)
                    {
                        StepProgress[j].sprite = stepOff;
                    }
                }
            }
            else
            {
                
            }
        }
        */
    }
}