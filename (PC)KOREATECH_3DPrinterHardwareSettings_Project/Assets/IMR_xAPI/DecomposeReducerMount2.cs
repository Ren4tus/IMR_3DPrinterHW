//using System.Collections;
//using System.Collections.Generic;
//using StoryMaker;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;
//using IMR;

//public class DecomposeReducerMount2 : MonoBehaviour
//{
//    public float animationPlaySpeed = 1.0f;

//    public bool motorPractice;
//    public Evaluation evaluator;
//    public OutlineTarget outliner;
//    public GameObject resultPanel_S;
//    public GameObject resultPanel_F;
//    public ClickInventoryManager inventoryManager_tool;
//    public ClickInventoryManager inventoryManager_part;
//    public ClickInventoryManager inventoryManager_Newpart;
//    public ObjectDuplicator duplicator;
//    public GraphicRaycaster uiRaycaster;
//    public ObjectInformationManager objInfoManager;
//    public Transform motor_reducer_AssemblyObject;
//    public Vector3 motor_reducer_AssemblyObjectLocalPosition;

//    public string selectedToolName; //도구 json매핑 미구현, 테스트용
//    public string targetToolName;
//    int controlCount;

//    bool reverseFlag;

//    public Text processIndicator;
//    public Text timeText;

//    public GameObject selectedToolParent;

//    public Sprite testSprite;

//    List<int> ruleDecScore;

//    bool firstErrorFlag = false;
//    bool flashFlag = false;

//    private GameObject parentofToolBox;
//    private GameObject parentofPartsBox;

//    private string nextSceneStr = "";

//    private List<int> exceptionObjIdx;
//    private List<int> exceptionObjIdx_2;


//    public float waittime = 2.5f;

//    public Screwdirections[] screwAxisMapper;
//    public Transform longWrenchTransform;
//    public Animation longWrenchAni;
//    public Transform shortWrenchTransform;
//    public Animation shortWrenchAni;

//    //public Transform spannerTransform;
//    //public Animation spannerAni;

//    public Transform GhostPivotParent;
//    List<Transform> GhostPivots;

//    private int screwIdx = 0;
//    public int firstEndPivot = 12;

//    bool reverseScrewReadyFlag = false;

//    public enum Screwdirections
//    {
//        forward,
//        up,
//        right,
//        back,
//        down,
//        left
//    };

//    public GameObject messageBox;
//    private bool blockFlag = false;
//    private bool cameraMovableFlag = false;
//    private int studyCode;

//    private bool evaluationEnd = false;
//    private bool partsReadyFlag = false;


//    public Color32[] colors; // FFFFFF 4CFF60
//    public float[] alphas; // 0.0f 1.0f

//    private List<int> screwPosIdx;
//    private bool[] exruletest;
//    private bool coroutinePlayFlag = false;
//    private bool HighlightOnByItem = false;
//    List<GameObject> targetObjects;
//    int targetID_Prev = -1;
//    private bool useItemFlag = false;

//    private int tmpIdx = -1;
//    public Text shortMessager;

//    private string selectedToolItemName;

//    private void Awake()
//    {
//        //motor_reducer_AssemblyObjectPosition = motor_reducer_AssemblyObject.position;
//        //motor_reducer_AssemblyObject.position = new Vector3(100000.0f, 100000.0f, 100000.0f);
//    }

//    private void Start()
//    {
//        if (motorPractice)
//            studyCode = 11;
//        else
//            studyCode = 21;

//        Debug.Assert(evaluator != null);
//        Debug.Assert(outliner != null);

//        MessageListSet.Instance.ClearList();

//        GhostPivots = new List<Transform>();
//        for (int i = 0; i < GhostPivotParent.childCount; i++)
//            GhostPivots.Add(GhostPivotParent.GetChild(i));

//        MoveCamera(0);

//        outliner.multiRaycaster.uiRaycaster = uiRaycaster;

//        controlCount = 0;
//        reverseFlag = false;

//        processIndicator.text = "감속기 분해";

//        //test code
//        ruleDecScore = new List<int>();
//        ruleDecScore.Add(5);

//        parentofToolBox = inventoryManager_tool.transform.parent.parent.gameObject;
//        parentofPartsBox = inventoryManager_part.transform.parent.parent.parent.gameObject;

//        foreach (var i in evaluator.mapper.objectList)
//        {
//            if (evaluator.mapper.GetIDofObject(i) >= 1000)
//                i.tag = "Screw";
//            else if (evaluator.mapper.GetIDofObject(i) >= 0)
//                i.tag = "Parts";
//            else continue;

//            i.layer = LayerMask.NameToLayer("MultipleRaycastTarget");
//        }

//        duplicator.dupInit();
//        //duplicator.SetCollider_Box();
//        duplicator.RemoveCollider_All();


//        //evaluator.mapper.SetCollider_Box();
//        evaluator.mapper.SetLayer_forRaycast(outliner.raycastLayerName);

//        outliner.currentFindObject = evaluator.mapper.GetObject(0);

//        {
//            exceptionObjIdx = new List<int>();


//            exceptionObjIdx_2 = new List<int>();
//        }

//        foreach (int i in exceptionObjIdx_2)
//        {
//            evaluator.mapper.GetObject(i).transform.parent.gameObject.AddComponent<TransformFix>().fixScale = true;
//        }

//        foreach (GameObject onePart in evaluator.mapper.objectList)
//        {
//            if (onePart.GetComponent<MeshCollider>() == null)
//                onePart.AddComponent<MeshCollider>();
//            if (onePart.GetComponent<HighlightingSystem.Highlighter>() == null)
//            {
//                HighlightingSystem.Highlighter highlighter = onePart.AddComponent<HighlightingSystem.Highlighter>();
//                highlighter.overlay = true;
//                highlighter.tween = true;
//                highlighter.constant = false;
//                highlighter.forceRender = true;
//                highlighter.tweenDuration = 0.5f;

//                GradientColorKey[] GradColors =
//                    {new GradientColorKey(colors[0], 0.0f), new GradientColorKey(colors[1], 1.0f)};
//                GradientAlphaKey[] GradAlphas =
//                    {new GradientAlphaKey(alphas[0], 0.0f), new GradientAlphaKey(alphas[1], 1.0f)};
//                highlighter.tweenGradient.SetKeys(GradColors, GradAlphas);
//                highlighter.constantColor = colors[1];

//                highlighter.enabled = false;
//            }
//        }

//        screwPosIdx = new List<int>();
//        for (int i = 0, j = 0; i < evaluator.mapper.objectList.Length; ++i)
//        {
//            if (evaluator.mapper.GetIDofObject(evaluator.mapper.objectList[i]) < 1000)
//                screwPosIdx.Add(-1);
//            else
//                screwPosIdx.Add(j++);
//        }

//        exruletest = new bool[2];
//        exruletest[0] = false;
//        exruletest[1] = false;

//        targetObjects = new List<GameObject>();
//    }

//    public void Test()
//    {
//        int decScore = 0;
//        GameObject curObj = evaluator.mapper.GetObject(controlCount);
//        decScore = ruleDecScore[0];

//        if (reverseFlag)
//            MessageListSet.Instance.AddList(evaluator.mapper.dataMap[curObj].partName + " 조립", decScore, 31);
//        else
//            MessageListSet.Instance.AddList(evaluator.mapper.dataMap[curObj].partName + " 분해", decScore, 31);

//        evaluator.addFailScore(1);

//        List<ClickInventoryCell> partsCells = inventoryManager_part.GetCells();
//        foreach (var i in partsCells)
//        {
//            if (i.GetItem() != null)
//            {
//                GameObject originObj = objInfoManager.GetOriginObject(i.GetItem().itemModel);
//                int id = evaluator.mapper.GetIDofObject(originObj);

//                if (id == evaluator.mapper.GetIDofObject(evaluator.mapper.GetObject(controlCount)))
//                {
//                    //i.UseItem();

//                    break;
//                }
//            }
//        }

//        int idx = 0;
//        if (!reverseFlag)
//        {
//            for (idx = 1; idx < evaluator.mapper.objectList.Length; idx++)
//            {
//                if (evaluator.mapper.objectList[idx].GetComponent<Renderer>().enabled == true)
//                    break;
//            }
//        }
//        else
//        {
//            idx = evaluator.mapper.objectList.Length - 1;
//            for (; idx >= 1; idx--)
//            {
//                if (evaluator.mapper.objectList[idx].GetComponent<Renderer>().enabled == false)
//                    break;
//            }
//        }

//        Success(idx, true);

//        firstErrorFlag = false;
//        partsReadyFlag = false;
//    }

//    private void Update()
//    {
//       // Debug.Log(SStoryManager.G.ActiveStoryline.Name);
//        if (Input.GetKeyDown(KeyCode.Space))
//            Test();
//        if (Input.GetKeyDown(KeyCode.B))
//            Success();
//        if (Input.GetKeyDown(KeyCode.N))
//        {
//            for (int i = 0; i < 50; i++)
//                Success();
//        }

//        if (!evaluationEnd)
//            timeText.text = evaluator.GetTimeText();

//        if (evaluator.limitTime <= 0)
//            EndStage();

//        if (blockFlag)
//        {
//            return;
//        }

//        if (coroutinePlayFlag)
//            return;
//        //if (evaluator.mapper.isAniamtionPlaying())
//        //    return;

//        //if (evaluator.mapper.targetAnimationPlayer != null && evaluator.mapper.targetAnimationPlayer.isPlaying)
//        //    return;
//        if (longWrenchAni != null && longWrenchAni.isPlaying)
//            return;

//        if (reverseFlag && controlCount <= firstEndPivot && !evaluationEnd)
//            EndStage();

//        SelectedPartsCheck();

//        int targetID =
//            evaluator.mapper.GetIDofObject(objInfoManager.GetOriginObject(inventoryManager_part.selectedItemModel));
//        if (targetID >= 0)
//        {
//            Debug.Log(targetID);
//            if (targetID_Prev != targetID)
//            {
//                foreach (var obj in targetObjects)
//                {
//                    HighlightingSystem.Highlighter h = obj.GetComponent<HighlightingSystem.Highlighter>();
//                    if (h != null) h.enabled = false;
//                }

//                targetObjects.Clear();
//            }

//            //targetObjects = evaluator.mapper.GetObjectsByID(targetID);
//            targetObjects.Clear();

//            foreach (var obj in evaluator.mapper.objectList)
//            {
//                if (targetID < 100)
//                {
//                    if (evaluator.mapper.GetIDofObject(obj) < 100)
//                        targetObjects.Add(obj);
//                }
//                else if (targetID < 200)
//                {
//                    if (evaluator.mapper.GetIDofObject(obj) < 200)
//                        targetObjects.Add(obj);
//                }
//                else if (targetID == 999)
//                {
//                    if (evaluator.mapper.GetIDofObject(obj) == 999)
//                        targetObjects.Add(obj);
//                }
//                else if (targetID >= 1000)
//                {
//                    if (evaluator.mapper.GetIDofObject(obj) >= 1000)
//                        targetObjects.Add(obj);
//                }
//            }

//            if (targetObjects.Count == 0)
//            {
//                targetObjects.Add(objInfoManager.GetOriginObject(inventoryManager_part.selectedItemModel));
//            }

//            foreach (var obj in targetObjects)
//            {
//                HighlightingSystem.Highlighter h = obj.GetComponent<HighlightingSystem.Highlighter>();
//                if (h != null) h.enabled = true;
//            }

//            targetID_Prev = targetID;

//            //HighlightOnByItem = true;
//        }
//        else if (targetID < 0 && targetObjects.Count > 0 || useItemFlag)
//        {
//            foreach (var obj in targetObjects)
//            {
//                HighlightingSystem.Highlighter h = obj.GetComponent<HighlightingSystem.Highlighter>();
//                if (h != null) h.enabled = false;
//            }

//            targetObjects.Clear();
//            targetID_Prev = targetID;

//            useItemFlag = false;
//            //HighlightOnByItem = false;
//        }

//        if (Input.GetKeyUp(KeyCode.Mouse0))
//        {
//            if (inventoryManager_tool.useFlag)
//            {
//                inventoryManager_tool.useFlag = false;
//                return;
//            }


//            if (reverseFlag)
//            {
//                GameObject hitObject = objInfoManager.GetOriginObject(outliner.alwayshitObject);
//                //GameObject hitObject = objInfoManager.GetOriginObject(outliner.onclickTarget);
//                GameObject selectedItemObject = objInfoManager.GetOriginObject(inventoryManager_part.selectedItemModel);
//                GameObject currentTargetObj = evaluator.mapper.GetObject(controlCount);

//                int hitObjectID = evaluator.mapper.GetIDofObject(hitObject);
//                int selectedItemObjectID = evaluator.mapper.GetIDofObject(selectedItemObject);
//                int currentTargetObjID = evaluator.mapper.GetIDofObject(currentTargetObj);

//                int hitObjectID2 = evaluator.mapper.GetID2ofObject(hitObject);
//                int currentTargetObjID2 = evaluator.mapper.GetID2ofObject(currentTargetObj);

//                //Destroy(inventoryManager_part.selectedItemModel);
//                //inventoryManager_part.selectedItemModel = null;

//                if (hitObject != null)
//                {
//                    //if (!partsReadyFlag)
//                    //{
//                    Debug.Log("Sel :" + selectedItemObjectID);
//                    Debug.Log("Cur :" + currentTargetObjID);
//                    Debug.Log("Hit Obj :" + currentTargetObjID);
//                    if ((selectedItemObjectID != currentTargetObjID) || (hitObjectID != selectedItemObjectID))
//                    {
//                        if (selectedItemObjectID != -1)
//                            Fail();
//                    }
//                    else
//                    {
//                        if (evaluator.mapper.dataMap[hitObject].id1 >= 1000)
//                        {
//                            string toolName = inventoryManager_tool.selectedItemCell == null
//                                ? ""
//                                : inventoryManager_tool.selectedItemCell.GetItem().name;

//                            //int toolNumber = controlCount <= firstEndPivot ? 0 : 1;
//                            int toolNumber = controlCount <= firstEndPivot ? 1 : 0;

//                            if (toolName == null ||
//                                !evaluator.ToolPartMatchCheck(toolName, toolNumber, outliner.onclickTarget))
//                            {
//                                Fail(true);

//                                return;
//                            }
//                        }

//                        if (evaluator.mapper.dataMap[hitObject].id1 == 999)
//                        {
//                            if (!exruletest[hitObjectID2])
//                            {
//                                Fail();
//                                return;
//                            }
//                        }

//                        if (evaluator.mapper.dataMap[hitObject].id1 == 1002)
//                        {
//                            exruletest[hitObjectID2] = true;
//                        }

//                        GameObject pickedObject = outliner.onclickTarget;
//                        int pickedObjectIndex = evaluator.mapper.GetIndexOfObjectByName(pickedObject);

//                        //evaluator.mapper.ActiveObject(controlCount);
//                        //duplicator.SetActive(controlCount, false);
//                        outliner.currentFindObject = evaluator.mapper.GetObject(controlCount);
//                        ////partsReadyFlag = true;

//                        //if(inventoryManager_part.GetSelectedCell() != null)
//                        //    inventoryManager_part.GetSelectedCell().UseItem();


//                        Success(pickedObjectIndex);
//                    }

//                    //}
//                    //else
//                    //{
//                    //if (outliner.lastRayTarget != null)
//                    //{
//                    //    if (currentTargetObj == hitObject)
//                    //    {
//                    //        if (evaluator.mapper.dataMap[hitObject].id1 >= 1000)
//                    //        {
//                    //            string toolName = inventoryManager_tool.selectedItemCell == null ? "" : inventoryManager_tool.selectedItemCell.GetItem().name;

//                    //            //int toolNumber = controlCount <= firstEndPivot ? 0 : 1;
//                    //            int toolNumber = controlCount <= firstEndPivot ? 1 : 0;

//                    //            if (toolName == null || !evaluator.ToolPartMatchCheck(toolName, toolNumber, outliner.onclickTarget))
//                    //                Fail();
//                    //            else
//                    //            {
//                    //                Success();

//                    //            }
//                    //        }
//                    //        else
//                    //        {
//                    //            Success();
//                    //        }
//                    //    }
//                    //    else
//                    //        Fail();
//                    //}
//                    //}
//                }
//            }

//            else if (outliner.onclickTarget != null && evaluator.mapper.dataMap.ContainsKey(outliner.onclickTarget))
//            {
//                GameObject currentFindObject = evaluator.mapper.GetObject(controlCount);

//                int currentTargetObjID = evaluator.mapper.GetIDofObject(currentFindObject);
//                int onClickTargetObjID = evaluator.mapper.GetIDofObject(outliner.onclickTarget);
//                Debug.Log(currentFindObject.name);
//                Debug.Log(currentFindObject);
//                Debug.Log(outliner.onclickTarget.name);
//                Debug.Log(outliner.onclickTarget);
//                Debug.Log(currentTargetObjID);
//                Debug.Log(onClickTargetObjID);

//                //if (evaluator.mapper.GetObject(controlCount) == outliner.onclickTarget)

//                if (onClickTargetObjID >= 0 && (currentTargetObjID == 1002 || currentTargetObjID == 999) &&
//                    (onClickTargetObjID == 1002 || onClickTargetObjID == 999))
//                {
//                    GameObject pickedObject = outliner.onclickTarget;
//                    int pickedObjectIndex = evaluator.mapper.GetIndexOfObjectByName(pickedObject);

//                    if (onClickTargetObjID == 1002)
//                    {
//                        string toolName = inventoryManager_tool.selectedItemCell == null
//                            ? ""
//                            : inventoryManager_tool.selectedItemCell.GetItem().name;

//                        //int toolNumber = controlCount <= firstEndPivot ? 0 : 1;
//                        int toolNumber = pickedObjectIndex <= firstEndPivot ? 1 : 0;

//                        if (toolName == null ||
//                            !evaluator.ToolPartMatchCheck(toolName, toolNumber, outliner.onclickTarget))
//                        {
//                            //Fail(true);
//                            if (Fail(true) && currentTargetObjID == 1002)
//                                exruletest[evaluator.mapper.GetID2ofObject(currentFindObject)] = true;
//                        }
//                        else
//                        {
//                            Success(pickedObjectIndex);
//                            exruletest[evaluator.mapper.GetID2ofObject(pickedObject)] = true;
//                        }
//                    }
//                    else
//                    {
//                        if (exruletest[evaluator.mapper.GetID2ofObject(pickedObject)])
//                            Success(pickedObjectIndex);
//                        //id == 999 special case
//                        else
//                        {
//                            //Fail(false);
//                            if (Fail(false) && currentTargetObjID == 1002)
//                                exruletest[evaluator.mapper.GetID2ofObject(currentFindObject)] = true;
//                        }
//                    }
//                }
//                else if (onClickTargetObjID >= 0 && currentTargetObjID == onClickTargetObjID)
//                {
//                    GameObject pickedObject = outliner.onclickTarget;
//                    int pickedObjectIndex = evaluator.mapper.GetIndexOfObjectByName(pickedObject);

//                    if (evaluator.mapper.dataMap[pickedObject].id1 >= 1000)
//                    {
//                        string toolName = inventoryManager_tool.selectedItemCell == null
//                            ? ""
//                            : inventoryManager_tool.selectedItemCell.GetItem().name;

//                        //int toolNumber = controlCount <= firstEndPivot ? 0 : 1;
//                        int toolNumber = pickedObjectIndex <= firstEndPivot ? 1 : 0;

//                        if (toolName == null ||
//                            !evaluator.ToolPartMatchCheck(toolName, toolNumber, outliner.onclickTarget))
//                            Fail(true);
//                        else Success(pickedObjectIndex);
//                    }
//                    else
//                        Success(pickedObjectIndex);
//                }
//                else
//                {
//                    if (Fail(false) && currentTargetObjID == 1002)
//                        exruletest[evaluator.mapper.GetID2ofObject(currentFindObject)] = true;
//                }

//                //    Fail();
//            }
//        }

//        UpdateMessage(controlCount, reverseFlag, false);
//    }

//    private void SelectedPartsCheck()
//    {
//        string sToolName;
//        if (inventoryManager_tool.selectedItemCell == null)
//            sToolName = null;
//        else
//            sToolName = inventoryManager_tool.selectedItemCell.GetItem().name;

//        if (selectedToolName != sToolName)
//        {
//            inventoryManager_part.UnSelectItem();
//            inventoryManager_part.windowToggler.CloseWindow();
//            selectedToolName = sToolName;
//        }
//    }

//    private void LateUpdate()
//    {
//        if (blockFlag)
//        {
//            if (messageBox == null)
//                blockFlag = false;

//            blockFlag = messageBox.activeSelf;
//        }

//        if (nextSceneStr != "")
//        {
//            if (waittime > 0.0f)
//                waittime -= Time.deltaTime;
//            else if (!evaluator.mapper.isAniamtionPlaying())
//                SceneManager.LoadScene(nextSceneStr);
//        }
//    }

//    void UpdateMessage(int index, bool isRev, bool left)
//    {
//        var script = isRev ? evaluator.mapper.scriptDatasReverse.ScriptData : evaluator.mapper.scriptDatas.ScriptData;


//        var shortScript = isRev
//            ? evaluator.mapper.shortScriptDatasReverse.ScriptData
//            : evaluator.mapper.shortScriptDatas.ScriptData;
//        for (int i = 0; i < shortScript.Length; i++)
//            if (index >= shortScript[i].start - 1 && index <= shortScript[i].end - 1)
//            {
//                shortMessager.text = shortScript[i].content;

//                if (i != tmpIdx)
//                    tmpIdx = i;
//            }
//    }

//    void MoveCamera(int transIdx)
//    {
//        //Vector3 translateVector = GhostPivots[transIdx].position - GCameraDirector.G.GhostTranslation;
//        //Vector3 rotateEulerVector = GhostPivots[transIdx].rotation.eulerAngles - GCameraDirector.G.GhostRotation;

//        //GCameraDirector.G.TranslateGhost(translateVector);
//        //GCameraDirector.G.RotateGhost(rotateEulerVector);

//        GCameraDirector.G.SetGhostCameraTransform(GhostPivots[transIdx]);
//    }

//    void ObjectOnWithHighlight()
//    {
//        int currentFindObjectID = evaluator.mapper.GetIDofObject(evaluator.mapper.GetObject(controlCount));
//        foreach (var obj in evaluator.mapper.objectList)
//        {
//            //if (evaluator.mapper.GetIDofObject(obj) != currentFindObjectID)
//            //    continue;

//            HighlightingSystem.Highlighter h = obj.GetComponent<HighlightingSystem.Highlighter>();

//            if (h == null)
//                continue;

//            obj.SetActive(true);
//            if (h != null) h.enabled = false;
//        }
//    }

//    void Success(int customIndex = -1, bool failure = false)
//    {
//        Debug.Log(SStoryManager.G.ActiveStoryline.Name);
//        Debug.Log("Success() Called....\ncontrolcount  " + controlCount.ToString() + "\tFirstErrorFlag  " +
//                  firstErrorFlag.ToString());
//        firstErrorFlag = false;
//        partsReadyFlag = false;

//        if (!failure)
//            evaluator.addSuccessScore(1);

//        if (!reverseFlag)
//        {
//            Debug.Log("called Success 1()");

//            //if (!exceptionObjIdx.Contains(controlCount))
//            AddItemToInventory(controlCount);

//            if (customIndex < 0)
//            {
//                StartCoroutine(PlayAni(evaluator.mapper.GetObject(controlCount).transform, controlCount++));
//                //++
//                if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
//                {
//                    XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(
//                        true
//                        , controlCount
//                        , evaluator.mapper.Length);
//                    XAPIApplication.current.SendMotorStatement("Choice");
//                }
//                else if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
//                {
//                    XAPIApplication.current.reducer_lesson_manager.SetChoiceStatementElements(
//                        true
//                        , controlCount
//                        , evaluator.mapper.Length);
//                    XAPIApplication.current.SendReducerStatement("Choice");
//                }
//            }
//            else
//            {
//                evaluator.mapper.objectList[customIndex].SetActive(false);
//                StartCoroutine(PlayAni(evaluator.mapper.GetObject(customIndex).transform, customIndex));
//                controlCount++;
//                if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
//                {
//                    XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(true
//                        , controlCount
//                        , evaluator.mapper.Length
//                    );
//                    XAPIApplication.current.SendMotorStatement("Choice");
//                }
//                else if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
//                {
//                    XAPIApplication.current.reducer_lesson_manager.SetChoiceStatementElements(
//                        true
//                        , controlCount
//                        , evaluator.mapper.Length
//                    );
//                    XAPIApplication.current.SendReducerStatement("Choice");
//                }
//            }

//            if (controlCount == evaluator.mapper.Length)
//            {
//                reverseFlag = true;
//                cameraMovableFlag = true;
//                --controlCount;
//                //duplicator.SetActiveAll(true);
//                if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
//                {
//                    XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(
//                        true
//                        , controlCount
//                        , evaluator.mapper.Length
//                    );
//                    XAPIApplication.current.SendMotorStatement("Choice");
//                }
//                else if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
//                {
//                    XAPIApplication.current.reducer_lesson_manager.SetChoiceStatementElements(true, controlCount
//                        , evaluator.mapper.Length
//                    );
//                    XAPIApplication.current.SendReducerStatement("Choice");
//                }

//                exruletest[0] = false;
//                exruletest[1] = false;

//                ObjectOnWithHighlight();

//                outliner.outlineOn = false;

//                //duplicator.SetLayer_forRaycast(outliner.raycastLayerName);
//                //outliner.currentFindObject = duplicator.duplicatedObject[controlCount];
//                outliner.currentFindObject = evaluator.mapper.GetObject(controlCount);
//            }
//            else
//                outliner.currentFindObject = evaluator.mapper.GetObject(controlCount);
//        }
//        else
//        {
//            Debug.Log("called Success 2()");

//            if (customIndex < 0)
//            {
//                StartCoroutine(PlayReverse(evaluator.mapper.GetObject(controlCount).transform, controlCount--));
//                evaluator.mapper.GetObject(controlCount).layer = 0;

//                if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
//                {
//                    XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(true
//                        , controlCount
//                        , evaluator.mapper.Length
//                        );
//                    XAPIApplication.current.SendMotorStatement("Choice");
//                }
//                else if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
//                {
//                    XAPIApplication.current.reducer_lesson_manager.SetChoiceStatementElements(true
//                        , controlCount
//                        , evaluator.mapper.Length
//                        );
//                    XAPIApplication.current.SendReducerStatement("Choice");
//                }
//            }
//            else
//            {
//                StartCoroutine(PlayReverse(evaluator.mapper.GetObject(customIndex).transform, customIndex));
//                evaluator.mapper.GetObject(customIndex).layer = 0;
//                controlCount--;

//                if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
//                {
//                    XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(
//                        true
//                        , controlCount
//                        , evaluator.mapper.Length
//                        );
//                    XAPIApplication.current.SendMotorStatement("Choice");
//                }
//                else if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
//                {
//                    XAPIApplication.current.reducer_lesson_manager.SetChoiceStatementElements(
//                        true
//                        , controlCount
//                        , evaluator.mapper.Length
//                        );
//                    XAPIApplication.current.SendReducerStatement("Choice");
//                }
//            }

//            int itemFindIndex = customIndex < 0 ? controlCount : customIndex;

//            Destroy(evaluator.mapper.GetObject(itemFindIndex).GetComponent<HighlightingSystem.Highlighter>());

//            GameObject obj = evaluator.mapper.GetObject(itemFindIndex);
//            foreach (var i in inventoryManager_part.GetCells())
//            {
//                if (i.GetItem() == null)
//                    continue;
//                GameObject tObj = i.GetItem().itemModel;
//                int id = evaluator.mapper.GetIDofObject(objInfoManager.GetOriginObject(tObj));
//                if (id == evaluator.mapper.GetIDofObject(obj))
//                {
//                    i.UseItem();
//                    inventoryManager_part.CheckItemExist(i);
//                    break;
//                }
//            }

//            foreach (var i in inventoryManager_Newpart.GetCells())
//            {
//                if (i.GetItem() == null)
//                    continue;
//                GameObject tObj = i.GetItem().itemModel;
//                int id = evaluator.mapper.GetIDofObject(objInfoManager.GetOriginObject(tObj));
//                if (id == evaluator.mapper.GetIDofObject(obj))
//                {
//                    i.UseItem();
//                    inventoryManager_Newpart.CheckItemExist(i);
//                    break;
//                }
//            }

//            useItemFlag = true;
//            if (controlCount >= 0) outliner.currentFindObject = evaluator.mapper.GetObject(controlCount);

//            ObjectOnWithHighlight();
//            //outliner.currentFindObject = duplicator.duplicatedObject[controlCount];
//        }
//    }

//    public void OnEmptyCallback()
//    {
//    }

//    bool Fail(bool toolError = false)
//    {
//        Debug.Log("Fail() Called....\ncontrolcount  " + controlCount.ToString() + "\tFirstErrorFlag  " +
//                  firstErrorFlag.ToString());

//        if (toolError)
//        {
//            if (inventoryManager_tool.selectedItemCell == null)
//            {
//                UserConfirmPopupControl.G.RequestConfirm(
//                    "공구를 선택해주세요."
//                    , null);
//            }
//            else
//            {
//                UserConfirmPopupControl.G.RequestConfirm(
//                    "다른 공구를 선택해주세요."
//                    , null);
//            }

//            return false;
//        }

//        if (!firstErrorFlag)
//        {
//            if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
//            {
//                XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(false, controlCount,
//                    evaluator.mapper.Length);
//                XAPIApplication.current.SendMotorStatement("Choice");
//            }
//            else if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
//            {
//                XAPIApplication.current.reducer_lesson_manager.SetChoiceStatementElements(false, controlCount,
//                    evaluator.mapper.Length);
//                XAPIApplication.current.SendReducerStatement("Choice");
//            }

//            UserConfirmPopupControl.G.RequestConfirm(
//                "다시 선택해주세요!"
//                , null);

//            firstErrorFlag = true;
//        }
//        else
//        {
//            int decScore = 0;
//            GameObject curObj = evaluator.mapper.GetObject(controlCount);
//            decScore = ruleDecScore[0];

//            if (reverseFlag)
//                MessageListSet.Instance.AddList(evaluator.mapper.dataMap[curObj].partName + " 조립", decScore, studyCode);
//            else
//                MessageListSet.Instance.AddList(evaluator.mapper.dataMap[curObj].partName + " 분해", decScore, studyCode);

//            evaluator.addFailScore(1);
//            if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
//            {
//                XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(false, controlCount
//                    , evaluator.mapper.Length
//                );
//                XAPIApplication.current.SendMotorStatement("Choice");
//            }
//            else if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
//            {
//                XAPIApplication.current.reducer_lesson_manager.SetChoiceStatementElements(false, controlCount
//                    , evaluator.mapper.Length
//                );
//                XAPIApplication.current.SendReducerStatement("Choice");
//            }

//            UserConfirmPopupControl.G.RequestConfirm(
//                "틀렸습니다. 다음 절차로 넘어갑니다."
//                , null);
//            //hc
//            // if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
//            // {
//            //     XAPIApplication.current.motor_lesson_manager.SetChoiceStatementElements(false);
//            //     XAPIApplication.current.SendMotorStatement("Time");
//            //     XAPIApplication.current.motor_lesson_manager.SetResultStatementExtention(false);
//            //     XAPIApplication.current.SendMotorStatement("Result");
//            // }
//            // else if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
//            // {
//            //     XAPIApplication.current.reducer_lesson_manager.SetLimitStatementResult(false);
//            //     XAPIApplication.current.SendReducerStatement("Time");
//            //     XAPIApplication.current.reducer_lesson_manager.SetResultStatementExtention(false);
//            //     XAPIApplication.current.SendReducerStatement("Result");
//            // }

//            if (!evaluator.mapper.GetObject(controlCount).activeSelf)
//            {
//                evaluator.mapper.ActiveObject(controlCount);
//                duplicator.SetActive(controlCount, false);
//            }

//            List<ClickInventoryCell> partsCells = inventoryManager_part.GetCells();
//            foreach (var i in partsCells)
//            {
//                if (i.GetItem() != null)
//                {
//                    GameObject originObj = objInfoManager.GetOriginObject(i.GetItem().itemModel);
//                    int id = evaluator.mapper.GetIDofObject(originObj);

//                    if (id == evaluator.mapper.GetIDofObject(evaluator.mapper.GetObject(controlCount)))
//                    {
//                        //i.UseItem();

//                        break;
//                    }
//                }
//            }

//            int idx = 0;
//            if (!reverseFlag)
//            {
//                for (; idx < evaluator.mapper.objectList.Length; idx++)
//                {
//                    if (evaluator.mapper.objectList[idx].GetComponent<Renderer>().enabled == true)
//                        break;
//                }
//            }
//            else
//            {
//                idx = evaluator.mapper.objectList.Length - 1;
//                for (; idx >= 0; idx--)
//                {
//                    if (evaluator.mapper.objectList[idx].GetComponent<Renderer>().enabled == false)
//                        break;
//                }
//            }

//            Success(idx, true);

//            firstErrorFlag = false;
//            partsReadyFlag = false;
//        }

//        //firstErrorFlag = !firstErrorFlag;
//        blockFlag = true;

//        //if ((evaluator.scoreDecreaseAndTest(0) || evaluator.limitTime <= 0.0f))
//        //    EndStage();

//        return !firstErrorFlag;
//    }

//    void GoToResult()
//    {
//        resultPanel_F.SetActive(true);
//    }

//    void EndStage()
//    {
//        Debug.Log("EndStage() Called....\ncontrolcount  " + controlCount.ToString() + "\tFirstErrorFlag  " +
//                  firstErrorFlag.ToString());

//        evaluationEnd = true;
//        evaluator.calculateResult();
//        //processIndicator.text = "모터 분해/결합 완료";

//        //resultPopUp.SetActive(true);

//        StaticClassEvaulateResult.CurrentTime = evaluator.limitTime;
//        Debug.Log(evaluator.limitTime);
//        //if (evaluator.limitTime > 0.0f)
//        evaluator.saveScoreInformaion();

//        //if (evaluator.scoreDecreaseAndTest(0) || evaluator.limitTime <= 0.0f)
//        //    nextSceneStr = "실습평가_결과";
//        ////SceneManager.LoadScene("실습평가_결과");
//        //else
//        //    nextSceneStr = "실습평가_점검_배터리";
//        //SceneManager.LoadScene("실습평가_점검_모터");

//        if (evaluator.limitTime <= 0.0f)
//        {
//            Debug.Log("timeout");
//            if (!SStoryManager.Exist)
//            {
//                return;
//            }

//            if (SStoryManager.G.HasNextStoryOutline)
//            {
//                if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
//                {
//                    XAPIApplication.current.motor_lesson_manager.SetLimitStatementResult(false);
//                    XAPIApplication.current.SendMotorStatement("Time");
//                    XAPIApplication.current.motor_lesson_manager.SetResultStatementExtention(false);
//                    XAPIApplication.current.SendMotorStatement("Result");
//                    EventManager.TriggerEvent("End");
//                }
//                else if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
//                {
//                    XAPIApplication.current.reducer_lesson_manager.SetLimitStatementResult(false);
//                    XAPIApplication.current.SendReducerStatement("Time");
//                    XAPIApplication.current.reducer_lesson_manager.SetResultStatementExtention(false);
//                    XAPIApplication.current.SendReducerStatement("Result");
//                    EventManager.TriggerEvent("End");
//                }

//                UserConfirmPopupControl.G.RequestConfirm("제한 시간을 초과하였습니다.\n결과를 확인하세요.", GoToResult, true);
//                this.gameObject.SetActive(false);
//                // IMRStatement.SetActor();
//                // IMRStatement.SetVerb("failed");  
//                // IMRStatement.SetActivity(SStoryManager.G.ActiveStoryline.Name);
//                // IMRStatement.SetDefinition("Time-out "+SStoryManager.G.ActiveStoryline.Title);
//                // StatementSender.SendStatement();
//                //GMainStoryBrowser.G.OnNext();

//                //SStoryManager.G.ActiveStoryOutlineIndex += 2;
//                //StoryLineListControl.S.OnSelectStoryLine();
//                return;
//            }

//            // if (SStoryManager.G.HasNextMainStory) { SStoryManager.G.MainStoryTo(1); return; }
//            //Debug.LogWarning("다음 스토리를 찾을 수 없습니다.");
//        }
//        //SceneManager.LoadScene("실습평가_결과");
//        //else if (evaluator.scoreDecreaseAndTest(0))
//        //{
//        //    UserConfirmPopupControl.G.RequestConfirm("분해조립 평가에 불합격 하였습니다.\n확인을 눌러 결과를 확인하세요.", OnEmptyCallback, true);
//        //    //resultPanel_F.SetActive(true);
//        //    GMainStoryBrowser.G.OnNext();
//        //    //StoryLineListControl.S.OnSelectStoryLine();
//        //}
//        else
//        {
//            // if (SStoryManager.G.ActiveStoryline.Name == "MotorPractice")
//            // {
//            //     XAPIApplication.current.motor_lesson_manager.SetLimitStatementResult(true);
//            //     XAPIApplication.current.SendMotorStatement("Time");
//            //     XAPIApplication.current.motor_lesson_manager.SetResultStatementExtention(true);
//            //     XAPIApplication.current.SendMotorStatement("Result");
//            //     EventManager.TriggerEvent("End");
//            //
//            // }
//            if (SStoryManager.G.ActiveStoryline.Name == "ReducerPractice")
//            {
//                XAPIApplication.current.reducer_lesson_manager.SetLimitStatementResult(true);
//                XAPIApplication.current.SendReducerStatement("Time");
//                XAPIApplication.current.reducer_lesson_manager.SetResultStatementExtention(true);
//                XAPIApplication.current.SendReducerStatement("Result");
//                EventManager.TriggerEvent("End");
//            }

//            Debug.Log("end ");
//            //hc
//            // IMRStatement.SetActor();
//            // IMRStatement.SetVerb("passed");
//            // IMRStatement.SetActivity(SStoryManager.G.ActiveStoryline.Name);
//            // IMRStatement.SetDefinition(SStoryManager.G.ActiveStoryline.Title);
//            // StatementSender.SendStatement();
//            //UserConfirmPopupControl.G.RequestConfirm("분해조립 평가를 완료하였습니다.\n결과를 확인하세요.", OnEmptyCallback, true);
//            //UserConfirmPopupControl.G.RequestConfirm("분해조립 평가를 완료하였습니다.\n점검기 평가를 시작합니다.", OnEmptyCallback, true);
//            if (motorPractice)
//                GMainStoryBrowser.G.OnNext();
//            else
//            {
//                resultPanel_F.SetActive(true);
//                this.gameObject.SetActive(false);
//            }

//            //resultPanel_F.SetActive(true);
//            //GMainStoryBrowser.G.OnNext();
//        }
//    }

//    void AddItemToInventory(int targetObjIdx)
//    {
//        if (targetObjIdx == evaluator.mapper.objectList.Length - 1)
//            return;

//        bool findFlag = false;
//        GameObject decomposedObj = evaluator.mapper.GetObject(targetObjIdx);

//        foreach (var cell in inventoryManager_part.GetCells())
//        {
//            Item itemInCell = cell.GetItem();

//            if (itemInCell == null) continue;

//            if (evaluator.mapper.dataMap[objInfoManager.GetOriginObject(itemInCell.itemModel)].id1 ==
//                evaluator.mapper.dataMap[decomposedObj].id1)
//            {
//                //itemInCell.itemModel = objInfoManager.DuplicateObject(decomposedObj);

//                cell.itemCount++;

//                //cell.inputStack.Push(decomposedObj);

//                findFlag = true;
//                break;
//            }
//        }

//        if (!findFlag)
//        {
//            Sprite newImage = Resources.Load(decomposedObj.name, typeof(Sprite)) as Sprite;

//            Item newItem = new Item(1);

//            if (newImage != null)
//                newItem.sprite = newImage;
//            else
//                newItem.sprite = testSprite;

//            newItem.itemModel = objInfoManager.DuplicateObject(decomposedObj);
//            //newItem.itemModel.name = decomposedObj.name;
//            newItem.itemModel.transform.position = new Vector3(1000000.0f, 1000000.0f, 1000000.0f);
//            newItem.itemModel.SetActive(true);

//            Debug.Assert(newItem.itemModel != null, "error debug point detected");

//            inventoryManager_part.GetEmptyCell().SetItem(newItem);
//            //ClickInventoryCell gcell = inventoryManager_part.GetEmptyCell();
//            //gcell.SetItem(newItem);
//            //gcell.inputStack.Push(decomposedObj);
//        }
//    }


//    private Vector3 GetScrewDirction(Transform screw, int index)
//    {
//        float amountOfTransform = 0.17f;
//        if (screwAxisMapper[index] == Screwdirections.forward)
//            return -screw.forward * amountOfTransform;
//        else if (screwAxisMapper[index] == Screwdirections.up)
//            return -screw.up * amountOfTransform;
//        else if (screwAxisMapper[index] == Screwdirections.right)
//            return -screw.right * amountOfTransform;
//        else if (screwAxisMapper[index] == Screwdirections.back)
//            return screw.forward * amountOfTransform;
//        else if (screwAxisMapper[index] == Screwdirections.left)
//            return screw.right * amountOfTransform;
//        else
//            return screw.up * amountOfTransform;
//    }

//    private IEnumerator PlayReverse(Transform screwTransform, int partNAniIdx)
//    {
//        coroutinePlayFlag = true;
//        evaluator.mapper.ActiveObject(partNAniIdx);
//        evaluator.mapper.PlayAnimation(partNAniIdx, 1, -animationPlaySpeed);
//        yield return new WaitForSeconds(evaluator.mapper.animList[partNAniIdx].length / animationPlaySpeed);
//        evaluator.mapper.DisableObject(partNAniIdx, true);
//        evaluator.mapper.EnableRenderer(partNAniIdx);

//        Transform toolTransform = partNAniIdx <= firstEndPivot ? shortWrenchTransform : longWrenchTransform;
//        Animation toolAnimation = partNAniIdx <= firstEndPivot ? shortWrenchAni : longWrenchAni;

//        if (evaluator.mapper.dataMap[evaluator.mapper.GetObject(partNAniIdx)].id1 >= 1000
//            && evaluator.mapper.dataMap[evaluator.mapper.GetObject(partNAniIdx)].id1 != 1003)
//        {
//            screwIdx--;

//            toolTransform.position = evaluator.mapper.objectList[partNAniIdx].transform.position;
//            toolTransform.position += partNAniIdx <= 12
//                ? 0.27f * GetScrewDirction(evaluator.mapper.objectList[partNAniIdx].transform, screwPosIdx[partNAniIdx])
//                : 0.84f * GetScrewDirction(evaluator.mapper.objectList[partNAniIdx].transform,
//                    screwPosIdx[partNAniIdx]);

//            toolTransform.LookAt(screwTransform, Vector3.up);

//            toolAnimation["Wrench"].normalizedTime = 1;
//            toolAnimation["Wrench"].speed = -animationPlaySpeed;

//            toolTransform.LookAt(evaluator.mapper.objectList[partNAniIdx].transform, Vector3.up);
//            //fittingTool(screwIdx, toolTransform);
//            fittingTool(screwPosIdx[partNAniIdx], toolTransform);
//            toolAnimation.Play();
//            yield return new WaitWhile(() => toolAnimation.isPlaying);
//        }

//        if (partNAniIdx > 0)
//        {
//            evaluator.mapper.PlayAnimation(partNAniIdx - 1, 1, -animationPlaySpeed);
//            yield return new WaitForSeconds(0.02f);

//            string clipName = evaluator.mapper.animList[partNAniIdx - 1].name;

//            evaluator.mapper.GetAnimationOfClip(clipName)[clipName].speed = 0.0f;

//            yield return new WaitWhile(() => evaluator.mapper.GetAnimationOfClip(clipName).isPlaying);
//        }

//        coroutinePlayFlag = false;


//        if ((controlCount == firstEndPivot))
//        {
//            //yield return new WaitForSeconds(1.0f);
//            //MoveCamera(2);
//            cameraMovableFlag = false;
//        }
//    }

//    private IEnumerator PlayAni(Transform screwTransform, int partNAniIdx)
//    {
//        coroutinePlayFlag = true;
//        // yield return null;

//        Transform toolTransform = partNAniIdx <= firstEndPivot ? shortWrenchTransform : longWrenchTransform;
//        Animation toolAnimation = partNAniIdx <= firstEndPivot ? shortWrenchAni : longWrenchAni;

//        if (evaluator.mapper.dataMap[evaluator.mapper.GetObject(partNAniIdx)].id1 >= 1000
//            && evaluator.mapper.dataMap[evaluator.mapper.GetObject(partNAniIdx)].id1 != 1003)
//        {
//            toolTransform.position = evaluator.mapper.objectList[partNAniIdx].transform.position;
//            toolTransform.position += partNAniIdx <= 12
//                ? 0.27f * GetScrewDirction(evaluator.mapper.objectList[partNAniIdx].transform, screwPosIdx[partNAniIdx])
//                : 0.84f * GetScrewDirction(evaluator.mapper.objectList[partNAniIdx].transform,
//                    screwPosIdx[partNAniIdx]);

//            screwIdx++;
//            toolTransform.LookAt(screwTransform, Vector3.up);

//            toolAnimation["Wrench"].normalizedTime = 0;
//            toolAnimation["Wrench"].speed = animationPlaySpeed;

//            toolTransform.LookAt(evaluator.mapper.objectList[partNAniIdx].transform, Vector3.up);
//            //fittingTool(screwIdx - 1, toolTransform);
//            fittingTool(screwPosIdx[partNAniIdx], toolTransform);
//            toolAnimation.Play();
//            yield return new WaitWhile(() => toolAnimation.isPlaying);
//        }

//        evaluator.mapper.PlayAnimation(partNAniIdx, 0, animationPlaySpeed);
//        evaluator.mapper.DisableRenderer(partNAniIdx);
//        evaluator.mapper.DisableObject(partNAniIdx, false);
//        yield return new WaitForSeconds(evaluator.mapper.animList[partNAniIdx].length / animationPlaySpeed);
//        coroutinePlayFlag = false;

//        //yield return new WaitWhile(() => evaluator.mapper.GetAnimationOfClip(partNAniIdx).isPlaying);

//        if ((controlCount == firstEndPivot + 1))
//        {
//            motor_reducer_AssemblyObject.localPosition = motor_reducer_AssemblyObjectLocalPosition;
//            MoveCamera(1);
//            cameraMovableFlag = false;
//        }
//    }

//    private void fittingTool(int scrIdx, Transform toolTransform)
//    {
//        if (scrIdx < 3)
//            toolTransform.rotation *= Quaternion.Euler(0, 0, 180);
//        else if (scrIdx == 5)
//            toolTransform.rotation *= Quaternion.Euler(0, 0, 30);
//        else if (21 <= scrIdx && scrIdx <= 23)
//            toolTransform.rotation *= Quaternion.Euler(0, 0, 165);
//        else if (scrIdx == 24)
//            toolTransform.rotation *= Quaternion.Euler(0, 0, 180);
//        else if (scrIdx == 25)
//            toolTransform.rotation *= Quaternion.Euler(0, 0, 150);
//        else if (scrIdx == 26)
//            toolTransform.rotation *= Quaternion.Euler(0, 0, 45);
//        else if (scrIdx == 28)
//            toolTransform.rotation *= Quaternion.Euler(0, 0, 20);
//        else if (scrIdx == 29)
//            toolTransform.rotation *= Quaternion.Euler(0, 0, 20);
//        else if (scrIdx == 30)
//            toolTransform.rotation *= Quaternion.Euler(0, 0, 165);
//        else if (scrIdx == 31)
//            toolTransform.rotation *= Quaternion.Euler(0, 0, 200);
//    }
//}