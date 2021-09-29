using UnityEngine;
using UnityEngine.UI;

public class EvaluationCH2BtnLoginPopup : EvaluationInteractiveUI
{
    public AnimatedUI CurrentUI;
    public AnimatedUI SequenceStatusCheck;

    public InputField UserID;
    public InputField UserPasssword;

    public string CORRECT_USER_ID = "guest";
    public string CORRECT_USER_PASSWORD = "0";

    public override void CompleteStep()
    {
        if (UserCheck())
        {
            base.CompleteStep();

            CurrentUI.FadeOut(0f);
            SequenceStatusCheck.FadeIn();

            return;
        }

        if(EvaluationSceneController.Instance.isSkip)
        {
            base.CompleteStep();

            CurrentUI.FadeOut(0f);
            SequenceStatusCheck.FadeIn();
        }
    }

    private bool UserCheck()
    {
        if (UserID.text == CORRECT_USER_ID && UserPasssword.text == CORRECT_USER_PASSWORD)
        {
            return true;
        }

        return false;
    }
}