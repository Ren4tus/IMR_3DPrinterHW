using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CL_AllMenuController : TweenableCanvas
{
    private void Awake()
    {
        base.Awake();
    }
    public void PanelOpen()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.PanelOpen);
        FadeIn();
    }

    public void PanelClose()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.PanelClose);
        FadeOut();
    }

    public void MakePopUpAndSceneMove(string sceneName)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<color='");
        sb.Append(CL_UIPresetData.RgbToHex(CL_UIPresetData.ThemeColor));
        sb.Append("'>");

        switch (sceneName)
        {
            case "KnowledgeCH1":
                sb.Append("[재료분사프린터 지식]");
                break;

            case "KnowledgeCH2":
                sb.Append("[광중합프린터 지식]");
                break;

            case "KnowledgeCH3":
                sb.Append("[분말적층용융결합프린터 지식]");
                break;
                
            case "StructureCH1":
                sb.Append("[재료분사프린터 구조]");
                break;

            case "StructureCH2":
                sb.Append("[광중합프린터 구조]");
                break;

            case "StructureCH3":
                sb.Append("[분말적층용융결합프린터 구조]");
                break;

            case "TrainingCH1":
                sb.Append("[재료분사프린터 실습]");
                break;

            case "TrainingCH2":
                sb.Append("[광중합프린터 실습]");
                break;

            case "TrainingCH3":
                sb.Append("[분말적층용융결합프린터 실습]");
                break;

            case "EvaluationCH1":
                sb.Append("[재료분사프린터 평가]");
                break;

            case "EvaluationCH2":
                sb.Append("[광중합프린터 평가]");
                break;

            case "EvaluationCH3":
                sb.Append("[분말적층용융결합프린터 평가]");
                break;

            default:
                break;
        }

        sb.Append("</color>");
        sb.Append(" 화면으로 이동하시겠습니까?");

        CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp(sb.ToString(),
                                                                   CL_MessagePopUpController.DialogType.NOTICE,
                                                                   () =>
                                                                   {
                                                                       CL_CommonFunctionManager.Instance.LoadScene(sceneName);
                                                                       PanelClose();
                                                                   },
                                                                   null);
    }
}
