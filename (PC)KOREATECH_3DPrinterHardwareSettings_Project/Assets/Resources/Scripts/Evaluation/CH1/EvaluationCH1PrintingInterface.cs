using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationCH1PrintingInterface : MonoBehaviour
{
    private float Fullsize;

    public Image[] Materials;
    public Text PrinterState;
    public Text PercentageText;
    public Text TimerText;

    public EvaluationCH1PrintedSclupture Sculputre;

    public int PrintingTime;
    public int RemainingTime = 0;

    public static EvaluationCH1PrintingInterface instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PrintStart()
    {
        if (!EvaluationSceneController.Instance.IsPrintingComplete)
        {
            PrinterState.text = "Printing";
            PercentageText.text = "0%";

            if (EvaluationSceneController.Instance.AssignmentModel.Equals(1))
            {
                PrintingTime = EvaluationSceneController.Instance.SlicerController.estimateTimeEX1;
            }
            else if (EvaluationSceneController.Instance.AssignmentModel.Equals(2))
            {
                PrintingTime = EvaluationSceneController.Instance.SlicerController.estimateTimeEX2;
            }

            TimerText.text = PrintingTime.ToString() + "00:00";
            StartCoroutine(QuickPrinting_Co(10f));
        }
    }

    public void PrintEnd()
    {
        EvaluationCH1AniamtionController.Instance.movingObjectsAS.PrintStop();

        PrinterState.text = "Complete";
        PercentageText.text = "100%";
        TimerText.text = "00:00:00";
    }

    IEnumerator QuickPrinting_Co(float time)
    {
        StringBuilder builder = new StringBuilder();
        int totalTicks = PrintingTime * 3600;

        int hour, minute, second;
        int percentage;
        float percentageSmall;

        float timer = 0.0f;

        Fullsize = Materials[0].rectTransform.sizeDelta.y;
        RemainingTime = 0;

        Sculputre.IsInteractive = false;
        EvaluationCH1AniamtionController.Instance.movingObjectsAS.FasterPrinting();
        EvaluationCH1AniamtionController.Instance.movingObjectsAS.PrintStart();

        while (timer < time)
        {
            timer += Time.deltaTime;
            RemainingTime = totalTicks - (int)((float)totalTicks * timer / time);

            percentage = (int)((float)(totalTicks - RemainingTime) / totalTicks * 100);
            percentageSmall = (float)percentage / 100.0f;

            Materials[0].rectTransform.sizeDelta = new Vector2(Materials[0].rectTransform.sizeDelta.x, Fullsize * (1.0f - percentageSmall));

            if (percentage <= 75)
            {
                Materials[1].rectTransform.sizeDelta = new Vector2(Materials[1].rectTransform.sizeDelta.x, Fullsize * (1.0f - percentageSmall));
                Materials[2].rectTransform.sizeDelta = new Vector2(Materials[2].rectTransform.sizeDelta.x, Fullsize * (1.0f - percentageSmall));
                Materials[3].rectTransform.sizeDelta = new Vector2(Materials[3].rectTransform.sizeDelta.x, Fullsize * (1.0f - percentageSmall));
                Materials[4].rectTransform.sizeDelta = new Vector2(Materials[4].rectTransform.sizeDelta.x, Fullsize * (1.0f - percentageSmall));
                Materials[5].rectTransform.sizeDelta = new Vector2(Materials[5].rectTransform.sizeDelta.x, Fullsize * (1.0f - percentageSmall));
                Materials[6].rectTransform.sizeDelta = new Vector2(Materials[6].rectTransform.sizeDelta.x, Fullsize * (1.0f - percentageSmall));
                Materials[7].rectTransform.sizeDelta = new Vector2(Materials[7].rectTransform.sizeDelta.x, Fullsize * (1.0f - percentageSmall));
            }

            PercentageText.text = percentage.ToString() + "%";

            hour = RemainingTime / 3600;
            minute = RemainingTime % 3600 / 60;
            second = RemainingTime % 3600 % 60;

            builder.Append(hour.ToString("D2"));
            builder.Append(":");
            builder.Append(minute.ToString("D2"));
            builder.Append(":");
            builder.Append(second.ToString("D2"));

            TimerText.text = builder.ToString();
            builder.Clear();

            yield return null;
        }

        EvaluationCH1AniamtionController.Instance.movingObjectsAS.PrintStop();
        Sculputre.IsInteractive = true;

        PrinterState.text = "Complete";
        PercentageText.text = "100%";
        TimerText.text = "00:00:00";
    }
}