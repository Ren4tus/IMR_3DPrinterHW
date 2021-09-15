using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CSVText : MonoBehaviour
{
    public int TextIndex = 0;

    void Start()
    {
        UpdateText();
    }

    private int FindIndex(int checkNum)
    {
        int _MAX = CSVController.data.Count - 1;
        int count = 0;

        for (int i = 0; i <= _MAX; i++)
        {
            if ((int)CSVController.data[i]["INDEX"] == checkNum)
                break;
            count++;
        }

        if (count == _MAX && (int)CSVController.data[count]["INDEX"] != checkNum)
        {
            return 0;
        }
        return count;
    }

    public void UpdateText()
    {
        this.GetComponent<Text>().text = (string)CSVController.data[FindIndex(TextIndex)][CL_CommonFunctionManager.Instance.GetProgramLanguage()];
    }
}
