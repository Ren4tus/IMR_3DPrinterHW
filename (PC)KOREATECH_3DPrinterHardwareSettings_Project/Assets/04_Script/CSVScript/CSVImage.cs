using UnityEngine;
using UnityEngine.UI;

public class CSVImage : MonoBehaviour
{
    public int ImagePathIndex = 0;

    void Start()
    {
        UpdateImage();
    }

    private int FindIndex(int checkNum)
    {
        int _MAX = CSVController.data.Count - 1;
        int count = 0;

        for (int i = 0; i < _MAX; i++)
        {
            if ((int)CSVController.data[i]["INDEX"] == checkNum)
                break;
            count++;
        }

        if (count == _MAX)
        {
            Debug.Log("Cannot Find Index");
            return 0;
        }

        return count;
    }

    public void UpdateImage()
    {
        this.GetComponent<Image>().sprite = 
            Resources.Load<Sprite>((string)CSVController.data[FindIndex(ImagePathIndex)][CL_CommonFunctionManager.Instance.GetProgramLanguage()]);
    }
}
