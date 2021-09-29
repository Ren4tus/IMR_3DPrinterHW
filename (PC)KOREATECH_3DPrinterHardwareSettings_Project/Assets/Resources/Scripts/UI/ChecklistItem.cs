using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistItem : MonoBehaviour
{
    public bool IsCheckable = true; // false일때는 체크박스 없는 아이템이 됩니다
    public GameObject Checkbox;
    public GameObject Checkmark;
    public Text Text;

    private Image _image;

    private bool checkValue = false;
    private Button _button;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        Checkbox.SetActive(IsCheckable);
        SetCheck(checkValue);
    }

    public void SetText(string text)
    {
        Text.text = text;
    }

    public void SetCheck(bool value)
    {
        Checkmark.SetActive(value);
        checkValue = value;
    }

    public void SetBackgroundColor(Color color)
    {
        _image.color = color;
    }

    public void SetButtonEvent(Transform target)
    {
        EvaluationSceneController.Instance.StopAllCoroutines();

        _button.onClick.AddListener(() => StartCoroutine(EvaluationSceneController.Instance.MoveToTarget(target)));
    }
}