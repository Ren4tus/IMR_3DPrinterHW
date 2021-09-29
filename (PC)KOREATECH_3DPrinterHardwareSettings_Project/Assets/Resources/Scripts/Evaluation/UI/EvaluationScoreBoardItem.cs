using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EvaluationScoreBoardItem : AnimatedUI
{
    public Text Context;
    public Text Score;
    public Button ReplayBtn;

    public void SetContext(string text)
    {
        Context.text = text;
    }
    public void SetScore(string text)
    {
        Score.text = text;
    }
    public void SetButton(UnityEvent action)
    {
        ReplayBtn.onClick = (Button.ButtonClickedEvent)action;
    }
    
    public void AddOnclickAtButtonRelearn(int chapter, int index)
    {
        switch (chapter)
        {
            case 1:
                ReplayBtn.onClick.AddListener(
                    () => CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("다시 학습을 진행할까요?",
                                                                           CL_MessagePopUpController.DialogType.NOTICE,
                                                                           () =>
                                                                           {
                                                                               CL_CommonFunctionManager.Instance.LoadTrain1SceneWithParameter(index);
                                                                           },
                                                                           null)
                    );
                break;
            case 2:
                ReplayBtn.onClick.AddListener(
                    () => CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("다시 학습을 진행할까요?",
                                                                           CL_MessagePopUpController.DialogType.NOTICE,
                                                                           () =>
                                                                           {
                                                                               CL_CommonFunctionManager.Instance.LoadTrain2SceneWithParameter(index);
                                                                           },
                                                                           null)
                    );
                break;
            case 3:
                ReplayBtn.onClick.AddListener(
                    () => CL_CommonFunctionManager.Instance.MakePopUp().YesOrNoPopUp("다시 학습을 진행할까요?",
                                                                           CL_MessagePopUpController.DialogType.NOTICE,
                                                                           () =>
                                                                           {
                                                                               CL_CommonFunctionManager.Instance.LoadTrain3SceneWithParameter(index);
                                                                           },
                                                                           null)
                    );
                break;
        }
    }
}