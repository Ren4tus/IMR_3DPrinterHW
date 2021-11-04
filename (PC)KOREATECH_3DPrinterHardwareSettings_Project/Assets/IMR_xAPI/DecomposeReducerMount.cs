//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;


//public class DecomposeReducerMount : MonoBehaviour {

//    //public Evaluation evaluator;
//    //public OutlineTarget outliner;
//    //public ClickInventoryManager inventoryManager_tool;
//    //public ClickInventoryManager inventoryManager_part;
//    //public ObjectDuplicator duplicator;
//    //public MouseOrbitImproved inputController;
//    //public GraphicRaycaster uiRaycaster;
//    //public ObjectInformationManager objInfoManager;

//    //public CursorChanger cursorChanger;
    
//    //int controlCount;

//    //bool reverseFlag;

//    //public Text processIndicator;
//    //public Text timeText;
//    //public GameObject alertPopUp;
//    //public GameObject resultPopUp;

//    //public GameObject selectedToolParent;

//    //public Sprite testSprite;

//    //List<int> ruleDecScore;

//    //bool firstErrorFlag = false;

//    //private GameObject parentofToolBox;
//    //private GameObject parentofPartsBox;

//    //private string nextSceneStr = "";

//    //private List<int> exceptionObjIdx;
//    //private List<int> exceptionObjIdx_2;

//    //public float waittime = 2.5f;

//    //public Screwdirections[] screwAxisMapper;
//    //public Transform wrenchTransform;
//    //public Animation wrenchAni;

//    //public Transform spannerTransform;
//    //public Animation spannerAni;

//    //private int screwIdx = 0;
//    //public int firstEndPivot = 12;

//    //public enum Screwdirections { forward, up, right, back, down, left };

//    //private void Start()
//    //{
//    //    Debug.Assert(evaluator != null);
//    //    Debug.Assert(outliner != null);

//    //    outliner.multiRaycaster.uiRaycaster = uiRaycaster;

//    //    controlCount = 0;
//    //    reverseFlag = false;

//    //    processIndicator.text = "모터 분해";

//    //    //test code
//    //    ruleDecScore = new List<int>();
//    //    ruleDecScore.Add(5);

//    //    parentofToolBox = inventoryManager_tool.transform.parent.parent.gameObject;
//    //    parentofPartsBox = inventoryManager_part.transform.parent.parent.parent.gameObject;

//    //    duplicator.dupInit();
//    //    duplicator.SetCollider_Box();

//    //    evaluator.mapper.SetCollider_Box();
//    //    evaluator.mapper.SetLayer_forRaycast(outliner.raycastLayerName);

//    //    outliner.currentFindObject = evaluator.mapper.objectList[0];

//    //    {
//    //        exceptionObjIdx = new List<int>();
//    //        exceptionObjIdx_2 = new List<int>();
//    //    }

//    //    foreach(int i in exceptionObjIdx_2)
//    //        evaluator.mapper.GetObject(i).transform.parent.gameObject.AddComponent<TransformFix>().fixScale = true;
//    //}

//    //private void Update()
//    //{

//    //    timeText.text = evaluator.GetTimeText();

//    //    if (evaluator.limitTime <= 0)
//    //        EndStage();

//    //    inputController.enabled = !(parentofToolBox.activeSelf || parentofPartsBox.activeSelf);

//    //    PointerEventData pointerEventData = new PointerEventData(null);
//    //    List<RaycastResult> results = new List<RaycastResult>();
//    //    pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
//    //    uiRaycaster.Raycast(pointerEventData, results);

//    //    if(outliner.changeCursorFlag_Select)
//    //    {
//    //        cursorChanger.changeTo_SelectCursor();

//    //        outliner.changeCursorFlag_Select = false;
//    //    }
//    //    if (outliner.changeCursorFlag_UnSelect)
//    //    {
//    //        cursorChanger.changeTo_OriginCursor();

//    //        outliner.changeCursorFlag_UnSelect = false;
//    //    }


//    //    if (evaluator.mapper.targetAnimationPlayer != null && evaluator.mapper.targetAnimationPlayer.isPlaying)
//    //        return;

//    //    if (Input.GetKeyDown(KeyCode.B))
//    //    {
//    //        //if (reverseFlag) evaluator.mapper.ActiveObject(controlCount);
//    //        //Success();
//    //    }
//    //    else if (results.Count <= 0 && outliner.isHit() && Input.GetKeyUp(KeyCode.Mouse0))
//    //    {

//    //        if (reverseFlag)
//    //        {
//    //            GameObject hitObject = objInfoManager.GetOriginObject(outliner.alwayshitObject);
//    //            GameObject selectedItemObject = objInfoManager.GetOriginObject(inventoryManager_part.selectedItemModel);
//    //            GameObject currentTargetObj = evaluator.mapper.objectList[controlCount];

//    //            int hitObjectID = evaluator.mapper.GetIDofObject(hitObject);
//    //            int selectedItemObjectID = evaluator.mapper.GetIDofObject(selectedItemObject);
//    //            int currentTargetObjID = evaluator.mapper.GetIDofObject(currentTargetObj);

//    //            Destroy(inventoryManager_part.selectedItemModel);
//    //            inventoryManager_part.selectedItemModel = null;

//    //            if (hitObject != null)
//    //            {
//    //                if (inventoryManager_part.useFlag)
//    //                {
//    //                    inventoryManager_part.useFlag = false;

//    //                    if (selectedItemObjectID != currentTargetObjID)
//    //                        Fail();
//    //                    else
//    //                    {
//    //                        evaluator.mapper.ActiveObject(controlCount);
//    //                        outliner.currentFindObject = evaluator.mapper.objectList[controlCount];
//    //                        //GameObject targetOriginObj = evaluator.mapper.GetObject(controlCount);
//    //                        //TransformFix currentTargetPositionScript = outliner.alwayshitObject.GetComponent<TransformFix>();
//    //                        //if (currentTargetPositionScript != null && currentTargetPositionScript.fixPosition)
//    //                        //    currentTargetPositionScript.position = targetOriginObj.transform.position;
//    //                        //outliner.alwayshitObject.transform.position = targetOriginObj.transform.position;

//    //                    }

//    //                }
//    //                else
//    //                {
//    //                    if (outliner.lastRayTarget != null)
//    //                    {
//    //                        if (currentTargetObj == hitObject)
//    //                        {
//    //                            if (evaluator.mapper.dataMap[hitObject].id1 >= 1000)
//    //                            {
//    //                                string toolName = inventoryManager_tool.selectedItemCell == null ? "" : inventoryManager_tool.selectedItemCell.GetItem().name;

//    //                                int toolNumber = controlCount <= firstEndPivot ? 0 : 1;

//    //                                if (toolName == null || !evaluator.ToolPartMatchCheck(toolName, toolNumber, outliner.onclickTarget))
//    //                                    Fail();
//    //                                else
//    //                                    Success();
//    //                            }
//    //                            else
//    //                                Success();
//    //                        }
//    //                        else
//    //                            Fail();
//    //                    }
//    //                }
//    //            }
//    //        }

//    //        //상기 내용은 모두 evaluation 스크립트 내의 함수로 변경해야함
//    //        //if문 너무 많음. 추후 수정할것. ##DP1
//    //        //분해 룰 코드
//    //        else if (outliner.onclickTarget != null && evaluator.mapper.dataMap.ContainsKey(outliner.onclickTarget))
//    //        {
//    //            if (evaluator.mapper.objectList[controlCount] == outliner.onclickTarget)
//    //            {
//    //                if (evaluator.mapper.dataMap[outliner.onclickTarget].id1 >= 1000)
//    //                {
//    //                    string toolName = inventoryManager_tool.selectedItemCell == null ? "" : inventoryManager_tool.selectedItemCell.GetItem().name;

//    //                    int toolNumber = controlCount <= firstEndPivot ? 0 : 1;

//    //                    if (toolName == null || !evaluator.ToolPartMatchCheck(toolName, toolNumber, outliner.onclickTarget))
//    //                        Fail();
//    //                    else Success();
//    //                }
//    //                else
//    //                    Success();
//    //            }
//    //            else
//    //                Fail();
//    //        }


//    //    }
        

//    //}

//    //private void LateUpdate()
//    //{
//    //    if (nextSceneStr != "")
//    //    {
//    //        if (waittime > 0.0f)
//    //            waittime -= Time.deltaTime;
//    //        else if (!evaluator.mapper.isAniamtionPlaying())
//    //            SceneManager.LoadScene(nextSceneStr);
//    //    }
//    //}

//    //void Success()
//    //{
//    //    if(!reverseFlag)
//    //    {
//    //        Debug.Log("called Success 1()");

//    //        if (!exceptionObjIdx.Contains(controlCount))
//    //            AddItemToInventory(controlCount);


//    //        StartCoroutine(PlayAni(evaluator.mapper.objectList[controlCount].transform, controlCount++));
            

//    //        if (controlCount == evaluator.mapper.Length)
//    //        {
//    //            reverseFlag = true;

//    //            --controlCount;
//    //            duplicator.SetActiveAll(true);
                
//    //            duplicator.SetLayer_forRaycast(outliner.raycastLayerName);
//    //            outliner.currentFindObject = duplicator.duplicatedObject[controlCount];

//    //            processIndicator.text = "모터 결합";
                
//    //        }
//    //        else
//    //            outliner.currentFindObject = evaluator.mapper.objectList[controlCount];

//    //    }
//    //    else
//    //    {
//    //        Debug.Log("called Success 2()");

//    //        duplicator.SetActive(controlCount, false);
//    //        evaluator.mapper.objectList[controlCount].layer = 0;
//    //        StartCoroutine(PlayReverse(evaluator.mapper.objectList[controlCount].transform, controlCount--));
            
//    //        if (controlCount < 0)
//    //            EndStage();
//    //        else
//    //            outliner.currentFindObject = duplicator.duplicatedObject[controlCount];

//    //    }
//    //}

//    //void Fail()
//    //{
//    //    Debug.Log("called Fail()");

//    //    GameObject curObj = evaluator.mapper.objectList[controlCount];
//    //    int decScore = 0;
//    //    if (firstErrorFlag)
//    //        decScore = ruleDecScore[0];
//    //    MessageListSet.Instance.AddList(evaluator.mapper.dataMap[curObj].partName, decScore, 11);
//    //    if (firstErrorFlag && evaluator.scoreDecreaseAndTest(ruleDecScore[0]))
//    //        EndStage();

//    //    firstErrorFlag = true;

//    //    alertPopUp.SetActive(true);
//    //}

//    //void EndStage()
//    //{

//    //    //processIndicator.text = "모터 분해/결합 완료";

//    //    //resultPopUp.SetActive(true);

//    //    StaticClassEvaulateResult.CurrentTime = evaluator.limitTime;
//    //    StaticClassEvaulateResult.TotalScore += evaluator.maxScore;

//    //    cursorChanger.changeTo_OriginCursor();

//    //    if (evaluator.scoreDecreaseAndTest(0) || evaluator.limitTime <= 0.0f)
//    //        nextSceneStr = "실습평가_결과";
//    //    else
//    //        nextSceneStr = "실습평가_점검_모터";

//    //}

//    //void AddItemToInventory(int targetObjIdx)
//    //{
//    //    bool findFlag = false;
//    //    GameObject decomposedObj = evaluator.mapper.objectList[targetObjIdx];

//    //    foreach (var cell in inventoryManager_part.GetCells())
//    //    {
//    //        Item itemInCell = cell.GetItem();

//    //        if (itemInCell == null) continue;

//    //        if (evaluator.mapper.dataMap[objInfoManager.GetOriginObject(itemInCell.itemModel)].id1 == evaluator.mapper.dataMap[decomposedObj].id1)
//    //        {
//    //            cell.itemCount++;

//    //            findFlag = true;
//    //            break;
//    //        }

//    //    }
        
//    //    if (!findFlag)
//    //    {
//    //        Sprite newImage = Resources.Load(decomposedObj.name, typeof(Sprite)) as Sprite;

//    //        Item newItem = new Item(1);

//    //        if (newImage != null)
//    //            newItem.sprite = newImage;
//    //        else
//    //            newItem.sprite = testSprite;
            
//    //        newItem.itemModel = objInfoManager.DuplicateObject(decomposedObj);
//    //        newItem.itemModel.transform.position = new Vector3(1000000.0f, 1000000.0f, 1000000.0f);
//    //        newItem.itemModel.SetActive(true);

//    //        Debug.Assert(newItem.itemModel != null, "error debug point detected");

//    //        inventoryManager_part.GetEmptyCell().SetItem(newItem);
//    //    }
//    //}

//    //private Vector3 GetScrewDirction(Transform screw)
//    //{
//    //    if (screwAxisMapper[screwIdx] == Screwdirections.forward)
//    //        return -screw.forward * 0.04f;
//    //    else if (screwAxisMapper[screwIdx] == Screwdirections.up)
//    //        return -screw.up * 0.04f;
//    //    else if (screwAxisMapper[screwIdx] == Screwdirections.right)
//    //        return -screw.right * 0.04f;
//    //    else if (screwAxisMapper[screwIdx] == Screwdirections.back)
//    //        return screw.forward * 0.04f;
//    //    else if (screwAxisMapper[screwIdx] == Screwdirections.left)
//    //        return screw.right * 0.04f;
//    //    else
//    //        return screw.up * 0.04f;
//    //}

//    //private IEnumerator PlayReverse(Transform screwTransform, int partNAniIdx)
//    //{
//    //    evaluator.mapper.PlayAnimation(partNAniIdx, 1, -1);
//    //    yield return new WaitForSeconds(evaluator.mapper.animList[partNAniIdx].length);

//    //    Transform toolTransform = partNAniIdx <= 12 ? wrenchTransform : spannerTransform;
//    //    Animation toolAnimation = partNAniIdx <= 12 ? wrenchAni : spannerAni;

//    //    if (evaluator.mapper.dataMap[evaluator.mapper.objectList[partNAniIdx]].id1 >= 1000
//    //    && evaluator.mapper.dataMap[evaluator.mapper.objectList[partNAniIdx]].id1 != 1003)
//    //    {
//    //        toolTransform.position = screwTransform.position;
//    //        screwIdx--;
//    //        toolTransform.position += partNAniIdx <= 12 ? GetScrewDirction(screwTransform) : 0.1f * GetScrewDirction(screwTransform);
//    //        toolTransform.LookAt(screwTransform, Vector3.up);

//    //        toolAnimation[partNAniIdx <= firstEndPivot ? "Wrench" : "spanner"].normalizedTime = 1;
//    //        toolAnimation[partNAniIdx <= firstEndPivot ? "Wrench" : "spanner"].speed = -1;

//    //        //int soundNumber = partNAniIdx <= firstEndPivot ? 90002 : 90001;
//    //        //if (!StaticClassVolume.mute)
//    //        //    gameObject.AddComponent<SoundPlayer>().Play(soundNumber);

//    //        toolAnimation.Play();
//    //        yield return new WaitWhile(() => toolAnimation.isPlaying);
//    //    }

//    //    if (partNAniIdx > 0)
//    //    {
//    //        evaluator.mapper.PlayAnimation(partNAniIdx - 1, 1, -1);
//    //        yield return new WaitForSeconds(0.02f);

//    //        string clipName = evaluator.mapper.animList[partNAniIdx - 1].name;

//    //        evaluator.mapper.GetAnimationOfClip(clipName)[clipName].speed = 0.0f;
            
//    //    }

//    //    if (controlCount == firstEndPivot)
//    //        inputController.MayChangeTarget(0, true, true);
//    //}

//    //private IEnumerator PlayAni(Transform screwTransform, int partNAniIdx)
//    //{
//    //    // yield return null;

//    //    Transform toolTransform = partNAniIdx <= firstEndPivot ? wrenchTransform : spannerTransform;
//    //    Animation toolAnimation = partNAniIdx <= firstEndPivot ? wrenchAni : spannerAni;

//    //    if (evaluator.mapper.dataMap[evaluator.mapper.objectList[partNAniIdx]].id1 >= 1000
//    //    && evaluator.mapper.dataMap[evaluator.mapper.objectList[partNAniIdx]].id1 != 1003)
//    //    {
//    //        toolTransform.position = screwTransform.position;
//    //        toolTransform.position += partNAniIdx <= 12 ? GetScrewDirction(screwTransform) : 0.1f * GetScrewDirction(screwTransform);

//    //        screwIdx++;
//    //        toolTransform.LookAt(screwTransform, Vector3.up);

//    //        //int soundNumber = partNAniIdx <= firstEndPivot ? 90002 : 90001;
//    //        //if (!StaticClassVolume.mute)
//    //        //    gameObject.AddComponent<SoundPlayer>().Play(soundNumber);

//    //        toolAnimation.Play();
//    //        yield return new WaitWhile(() => toolAnimation.isPlaying);
//    //    }

//    //    evaluator.mapper.PlayAnimation(partNAniIdx, 0, 1);
//    //    evaluator.mapper.DisableObject(partNAniIdx, true);

//    //    if (controlCount == firstEndPivot + 1)
//    //        inputController.MayChangeTarget(1, false, false);
//    //}

//    //private void testFunc1()
//    //{

//    //}
//}