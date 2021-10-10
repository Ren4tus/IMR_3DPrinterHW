//using System;
//using System.IO;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class MessageListSet
//{
//    private static MessageListSet instance = null;
//    public static MessageListSet Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                instance = new MessageListSet();
//            }
//            return instance;
//        }
//    }

    
//    public List<string> partsName;
//    public List<int> score;
//    public List<int> theoryID;

//    private MessageListSet()
//    {
//        SetMessageListBuff();
//    }

//    public void SetMessageListBuff()
//    {
//        partsName = new List<string>();
//        score = new List<int>();
//        theoryID = new List<int>();

//    }

//    public void AddList(string partText, int scoreNum, int tid)
//    {
//        if (partsName.Contains(partText))
//            return;

//        partsName.Add(partText);
//        score.Add(scoreNum);
//        theoryID.Add(tid);
//    }

//    public void ClearList()
//    {
//        partsName.Clear();
//        score.Clear();
//        theoryID.Clear();
//    }
//}

//namespace MessageDataType
//{
//    [Serializable]
//    public class MessageData
//    {
//        public int tID;
//        public string tName;
//        public string tScene;
//    }

//    [Serializable]
//    public class MessageDataSet
//    {
//        public MessageData[] DataSet;
//    }
//}

//public class MessageList : MonoBehaviour
//{
//    public string dataJsonFileName;
//    public GameObject listBoard;
//    public List<GameObject> messageList;
//    public GameObject listElem;
//    public int messageListCount;

//    private MessageDataType.MessageDataSet dataSet;

//    public Image FilledImage;
//    public Text timeText;
//    public Text resultText;

//    private void OnEnable()
//    {
//        for (int i = 0; i < listBoard.transform.childCount; i++)
//            Destroy(listBoard.transform.GetChild(i).gameObject);

//        StartCoroutine(CalculateStar());
//        //if (StaticClassEvaulateResult.TotalScore == 0)
//        //    starSet.transform.GetChild(2).gameObject.SetActive(true);
//        //else if (StaticClassEvaulateResult.TotalScore < 15)
//        //    starSet.transform.GetChild(1).gameObject.SetActive(true);
//        //else
//        //    starSet.transform.GetChild(0).gameObject.SetActive(true);

//        float elapsedTime = StaticClassEvaulateResult.StartTime - StaticClassEvaulateResult.CurrentTime;
//        //float elapsedTime = StaticClassEvaulateResult.CurrentTime;
//        timeText.text = (string.Format("{0:D2}", (int)elapsedTime / 60).ToString()) + " : " +
//            (string.Format("{0:D2}", (int)elapsedTime % 60).ToString());

//        if (elapsedTime < StaticClassEvaulateResult.StartTime)
//            resultText.text += "성공";
//        else
//            resultText.text += "실패";

//    }

//    private void Start()
//    {
//        if(dataJsonFileName == null || dataJsonFileName =="")
//        {
//            dataJsonFileName = "EvaluationResultMessageData";
//        }

//        string jsonDataPath = Application.dataPath + "/StreamingAssets/" + dataJsonFileName + ".json";
//        string jsonDataString = File.ReadAllText(jsonDataPath);
//        dataSet = JsonUtility.FromJson<MessageDataType.MessageDataSet>(jsonDataString);

//        messageList = new List<GameObject>();
//        Debug.Assert(listBoard != null);
//        Debug.Assert(listElem != null);
//        messageListCount = 0;

//        for(int i=0; i< MessageListSet.Instance.partsName.Count; i++)
//        {
//            AddMissText(MessageListSet.Instance.partsName[i], 
//                MessageListSet.Instance.score[i], MessageListSet.Instance.theoryID[i]);
//        }

//        //scoreText.text = StaticClassEvaulateResult.TotalScore.ToString() + "점";

//    }

//    private string GetMessageData(int tid, int subidx)
//    {
//        if (tid < 0)
//            return null;
//        if (subidx <= 0)
//            return null;

//        string returnString = null;

//        int arridx = -1;
//        for (int i = 0; i < dataSet.DataSet.Length; i++)
//        {
//            if (dataSet.DataSet[i].tID == tid)
//            {
//                arridx = i;
//                break;
//            }
//        }

//        if (arridx < 0)
//            return null;

//        switch (subidx)
//        {
//            case 1:

//                returnString = dataSet.DataSet[arridx].tName;

//                break;

//            case 2:

//                returnString = dataSet.DataSet[arridx].tScene;

//                break;
//        }

//        return returnString;
//    }

//    public void AddMissText(string partsName, int score, int tid)
//    {
//        if (messageListCount >= messageList.Count)
//        {
//            GameObject newListElem = Instantiate(listElem);
//            newListElem.transform.SetParent(listBoard.transform);
//            newListElem.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
//            newListElem.SetActive(true);
//            messageList.Add(newListElem);
//        }

//        Transform tmpTransform = messageList[messageListCount].transform;

//        tmpTransform.GetChild(0).GetComponentInChildren<Text>().text = partsName;
//        //tmpTransform.GetChild(1).GetComponentInChildren<Text>().text = partsName;
//        //tmpTransform.GetChild(1).GetComponentInChildren<Text>().text = score.ToString();
//        //tmpTransform.GetChild(1).GetComponentInChildren<Text>().text = GetMessageData(tid, 1);
//        //string tSceneStr = GetMessageData(tid, 2);
//        Button tButton = tmpTransform.GetChild(0).GetComponent<Button>();

//        int mainStoryIndex = (tid / 10) -1;
//        int storyLineIndex = (tid % 10) + 1;

//        if(tButton != null)
//            tmpTransform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
//                GSceneLoader.G.StartSceneTransition(StoryType.EXERCISE, mainStoryIndex);
//                //StoryLineListControl.index = storyLineIndex;
//                ButtonClickFunctionControl.moveIndex = storyLineIndex;
//            });
//        messageListCount++;
//    }

//    private void OnDestroy()
//    {
//        Debug.Log("result message result delete");

//        foreach (var obj in messageList)
//            Destroy(obj);
//    }

//    public void GoToHome()
//    {
//        GSceneLoader.G.StartSceneTransition(EVScene.HOME);
//    }

//    public void GoToNext()
//    {
//        GMainStoryBrowser.G.OnNext();
//    }

//    IEnumerator CalculateStar()
//    {
//        // float time = 0.0f;
//        float Percentage = Evaluation.finalResult;
//        Debug.Log("Filled Percentage : " + Percentage);

//        // BGM 추가 필요

//        while (Percentage >= FilledImage.fillAmount)
//        {
//            FilledImage.fillAmount += Time.deltaTime * 0.3f;
//            yield return null;
//        }
//    }
//}