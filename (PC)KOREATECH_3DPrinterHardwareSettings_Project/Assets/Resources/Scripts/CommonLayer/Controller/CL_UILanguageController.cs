/*
 * 다언어 대응을 위한 CSV 컨트롤러
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SBFrameworkCore;
using UnityEngine;

public class CL_UILanguageController
{
    private Dictionary<int, Dictionary<string, string>> UIData = null;

    public CL_UILanguageController(string filePath)
    {
        UIData = new Dictionary<int, Dictionary<string, string>>();
        ReadUIData(filePath);
    }
    ~CL_UILanguageController()
    {
        UIData.Clear();
        UIData = null;
    }

    public string GetUIData(int index)
    {
        if (!UIData.ContainsKey(index))
            return null;

        if (!UIData[index].ContainsKey(CL_CommonFunctionManager.Instance.GetProgramLanguage()))
            return null;

        return UIData[index][CL_CommonFunctionManager.Instance.GetProgramLanguage()];
    }
    
    public void ReadUIData(string filePath)
    {
        const string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        const char SPLIT_CHAR = '|';

        TextAsset data = Resources.Load(filePath) as TextAsset;
        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1)
            return;

        // 할당, [index][Language] = Text
        if (UIData == null)
        {
            UIData = new Dictionary<int, Dictionary<string, string>>();
        }
        else
        {
            UIData.Clear();
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(SPLIT_CHAR);

            if (values.Length == 0 || values[0] == "") continue;

            Dictionary<string, string> entry = new Dictionary<string, string>();

            // 0 = Index
            // 1 = KR
            // 2 = EN

            if (values[1].Contains("<n>"))
                values[1] = values[1].Replace("<n>", "\n");

            if (values[2].Contains("<n>"))
                values[2] = values[2].Replace("<n>", "\n");

            entry["KR"] = values[1];
            entry["EN"] = values[2];

            UIData.Add(int.Parse(values[0]), entry);
        }
    }

    public void UpdateLanguageChange(Canvas canvas)
    {
        CL_TextUI[] TextList = canvas.GetComponentsInChildren<CL_TextUI>();
        string language = CL_CommonFunctionManager.Instance.GetProgramLanguage();

        foreach (CL_TextUI UIText in TextList)
        {
            UIText.UpdateText(UIData[UIText.TextIndex][language]);
        }
    }
}