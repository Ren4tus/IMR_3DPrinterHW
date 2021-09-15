using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionData : MonoBehaviour
{
    /*
    public enum InputAction
    {
        Click
    }
    */

    // ObjectOn/Off는 Seq를 나누어 관리하자.
    // 여기에 enum값이랑 메소드를 추가해서 관리하자
    public enum OutputAction
    {
        GoNextSeq,
        GoForcedNextSeq,
        WaitGoNextSeq,
        WaitGoNextSeqOneS
    }

    //public InputAction[] inputAction;
    [Header("현재 클릭했을 경우만 관리")]
    public string[] ObjName;

    public OutputAction[] outputAction;

    private Camera[] cameras;
    private bool oneClick = false;

    private void OnEnable()
    {
        cameras = Camera.allCameras;
    }

    void Update()
    {
        if (ObjName.Length == 0)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            //RaycastHit hit = new RaycastHit();

            foreach (Camera ca in cameras)
            {
                Ray ray = ca.ScreenPointToRay(Input.mousePosition);

                RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction);

                //if (Physics.Raycast(ray.origin, ray.direction, out hits[0]))
                for(int n=0; n<hits.Length; n++)
                {
                    for (int i = 0; i < ObjName.Length; i++)
                    {
                        if (hits[n].transform.gameObject.name == ObjName[i] && !oneClick)
                        {
                            oneClick = true;

                            switch (outputAction[i])
                            {
                                case OutputAction.GoNextSeq:
                                    GoNextSeq();
                                    break;
                                case OutputAction.GoForcedNextSeq:
                                    GoForcedNextSeq();
                                    break;
                                case OutputAction.WaitGoNextSeq:
                                    StartCoroutine(WaitForSecondsAndGoNextSeq());
                                    break;
                                case OutputAction.WaitGoNextSeqOneS:
                                    StartCoroutine(WaitOneSeconds());
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }


            /*
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);

                for (int i = 0; i < ObjName.Length; i++)
                {
                    if (hit.transform.gameObject.name == ObjName[i])
                    {
                        switch(outputAction[i])
                        {
                            case OutputAction.GoNextSeq:
                                GoNextSeq();
                                break;
                            case OutputAction.GoForcedNextSeq:
                                GoForcedNextSeq();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            */
        }
    }

    IEnumerator WaitOneSeconds()
    {
        yield return new WaitForSeconds(1f);

        MainController.instance.isRightClick = true;
        MainController.instance.goNextSeq();
        MainController.instance.isRightClick = false;

        oneClick = false;
    }

    IEnumerator WaitForSecondsAndGoNextSeq()
    {
        yield return new WaitForSeconds(2f);

        MainController.instance.isRightClick = true;
        MainController.instance.goNextSeq();
        MainController.instance.isRightClick = false;

        oneClick = false;
    }

    private void GoNextSeq()
    {
        MainController.instance.isRightClick = true;
        MainController.instance.goNextSeq();
        MainController.instance.isRightClick = false;

        oneClick = false;
    }

    private void GoForcedNextSeq()
    {
        MainController.instance.LoadSeq(MainController.instance.GetCurrentSeq()+1, true);

        oneClick = false;
    }
}
