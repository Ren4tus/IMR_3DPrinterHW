using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationScoreMessage : MonoBehaviour
{
    private StringBuilder _stringBuilder = null;
    private Animation _animation;

    public Text ScoreText;
    public Image Icon;
    public Sprite CorrectImg;
    public Sprite WrongImg;

    private void Awake()
    {
        _stringBuilder = new StringBuilder();
        _animation = GetComponent<Animation>();
    }

    public void GainScoreMsgPopup(string text, int score, bool isSkip = false)
    {
        if (!isSkip)
            Icon.sprite = CorrectImg;
        else
            Icon.sprite = WrongImg;

        _stringBuilder.Clear();
        _stringBuilder.Append(text);
        _stringBuilder.Append(" + ");
        _stringBuilder.Append(score);
        _stringBuilder.Append("점");
        ScoreText.text = _stringBuilder.ToString();

        _animation.Play();
    }
}
