using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CL_TutorialPanelController : MonoBehaviour
{
    private Animation _animation;

    public GameObject[] TutorialList;
    public GameObject[] TutorialList_WEB;

    public Text BottomText;

    private void Awake()
    {
        _animation = GetComponent<Animation>();

        for (int i = 0; i < TutorialList.Length; i++)
        {
            TutorialList[i].SetActive(false);
            TutorialList_WEB[i].SetActive(false);
        }
    }

    private void Start()
    {
        BottomText.text = "아무곳이나 클릭하면 학습을 진행할 수 있습니다.";

#if !UNITY_WEBGL
        BottomText.text += "\n현재 창은 빠른 메뉴의 도움말을 통해 다시 볼 수 있습니다.";
#endif
    }

    public void PanelOpen(int num)
    {
#if UNITY_WEBGL_PRINTER
        for (int i = 0; i < TutorialList_WEB.Length; i++)
            TutorialList_WEB[i].SetActive(false);

        TutorialList_WEB[num].SetActive(true);
#else
        for (int i = 0; i < TutorialList.Length; i++)
            TutorialList[i].SetActive(false);

        TutorialList[num].SetActive(true);
#endif
        _animation.Play("TutorialPanel");
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.PanelOpen);
    }

    public void PanelClose()
    {
        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("튜토리얼 화면을 종료하고 다음으로 진행하시겠습니까?", CL_MessagePopUpController.DialogType.NOTICE, () => Invoke("CloseProcess", 0.5f), null);
    }

    public void CloseProcess()
    {
        _animation.Play("TutorialPanelClose");
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.PanelClose);

        if (MainController.instance != null && MainController.instance.IsStart())
            MainController.instance.StartInit();

        if (KnowledgeTabController.instance != null && !KnowledgeTabController.instance.IsStart)
            KnowledgeTabController.instance.StartKnowledge();

        if (EvaluationSceneController.Instance != null && EvaluationSceneController.Instance.startSeq)
            EvaluationSceneController.Instance.StartInit();

        if (PracticeSceneController.Instance != null && PracticeSceneController.Instance.startSeq)
            PracticeSceneController.Instance.StartInit();
    }

    // 
    IEnumerator AnimationDelayStart()
    {
        yield return new WaitForSeconds(0.5f);

        CloseProcess();
    }
}
