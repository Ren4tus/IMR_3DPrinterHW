using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvaluationCore;

public class EvaluationRatingCanvasController : MonoBehaviour
{
    public float FadeTime = 0.5f;
    public TweenableCanvas FixedCanvas;
    public TweenableCanvas DynamicCanvas;

    public Transform ScoreBoard;
    public List<EvaluationScoreBoardItem> ScoreBoardItems;

    public GameObject ScoreItemPrefab;

    public int ChapterNumber = 1;
    public int[] trainingIndexs;

    public RectTransform mainBtn;
    public RectTransform replayAllBtn;

    private void Start()
    {
        CloseRatingPanel();
    }

    public void OpenRatingPanel()
    {
        // 평가 패널이 열리면 퀵메뉴 사용 중지
        CL_CommonFunctionManager.Instance.QuickMenuActive(false);

#if UNITY_WEBGL_EACH
        mainBtn.gameObject.SetActive(false);
        replayAllBtn.gameObject.SetActive(false);
        replayAllBtn.transform.localPosition = new Vector3(0.0f, -446.0f, 0.0f);
#elif UNITY_WEBGL_PRINTER
        mainBtn.gameObject.SetActive(false);
        replayAllBtn.gameObject.SetActive(true);
        replayAllBtn.transform.localPosition = new Vector3(0.0f, -446.0f, 0.0f);
#else
        mainBtn.gameObject.SetActive(true);
        replayAllBtn.gameObject.SetActive(true);
        mainBtn.transform.localPosition = new Vector3(-200.0f, -446.0f, 0.0f);
        replayAllBtn.transform.localPosition = new Vector3(200.0f, -446.0f, 0.0f);
#endif

        FixedCanvas.FadeIn(FadeTime);
        DynamicCanvas.FadeIn(FadeTime);
    }

    public void CloseRatingPanel()
    {
        FixedCanvas.FadeOut(0f);
        DynamicCanvas.FadeOut(0f);
    }

    public void MakeScoreBoard(EvaluationContainer container)
    { 
        ScoreBoardItems = new List<EvaluationScoreBoardItem>();

        StartCoroutine(ScoreBoardAnimation_co(container));
    }

    public int AddScoreItem(string context, string score = "")
    {
        GameObject gameObject = GameObject.Instantiate(ScoreItemPrefab);
        gameObject.transform.SetParent(ScoreBoard);
        gameObject.transform.localScale = Vector3.one;

        EvaluationScoreBoardItem scoreItem = gameObject.GetComponent<EvaluationScoreBoardItem>();

        ScoreBoardItems.Add(scoreItem);
        int index = ScoreBoardItems.Count - 1;

        ScoreBoardItems[index].SetContext(context);

        if (score.Length > 0)
            ScoreBoardItems[index].SetScore(score);

        // Debug.Log(trainingIndexs.Length);
#if !UNITY_WEBGL_EACH
        if (index < trainingIndexs.Length)
            ScoreBoardItems[index].AddOnclickAtButtonRelearn(ChapterNumber, trainingIndexs[index]);
        else
#endif
            ScoreBoardItems[index].ReplayBtn.gameObject.SetActive(false);

        return index;
    }

    IEnumerator ScoreBoardAnimation_co(EvaluationContainer container)
    {
        int i = 0;
        int index = 0;

        foreach (KeyValuePair<int, EvaluationSequence> item in container._sequenceList)
        {
            index = AddScoreItem(item.Value.Name);
            
            ScoreBoardItems[index].SetScore("0/" + item.Value.TotalScores().ToString());
            ScoreBoardItems[index].FadeIn(1f);
            yield return new WaitForSeconds(1);

            for (i=1; i<= item.Value.TotalGainScores(); i++)
            {
                if(!item.Value.IsSkip)
                    ScoreBoardItems[index].SetScore(i.ToString() + "/" + item.Value.TotalScores().ToString());
                else
                    ScoreBoardItems[index].SetScore("0/" + item.Value.TotalScores().ToString());
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        // 안전

        index = AddScoreItem("안전하게 작업할 수 있다.", (EvaluationSceneController.Instance.IsPassSafety) ? "PASS" : "FAIL");
        ScoreBoardItems[index].FadeIn(1f);
        yield return new WaitForSeconds(1);

        // 시간 내
        index = AddScoreItem("시간 내 작업할 수 있다.", (EvaluationSceneController.Instance.IsCompleteInTime) ? "PASS" : "FAIL");
        ScoreBoardItems[index].FadeIn(1f);
        yield return new WaitForSeconds(1);
    }
}