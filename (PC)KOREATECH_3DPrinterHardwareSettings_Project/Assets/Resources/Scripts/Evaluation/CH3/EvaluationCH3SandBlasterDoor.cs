using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH3SandBlasterDoor : EvaluationInteractiveObject
{
    public bool IsOpen = false;
    private Animator _animator;

    protected void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

    public override void OnMouseDown()
    {
        if (!IsInteractive || IsPointerOverUIObject())
            return;

        EvaluationSceneController.Instance.FollowGuideController.Hide();
        Toggle();
    }

    public void Open()
    {
        IsOpen = true;
        _animator.Play("Open");
    }
    public void Close()
    {
        IsOpen = false;
        _animator.Play("Close");
    }
    public void Toggle()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
