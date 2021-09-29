using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1MovingTray : EvaluationInteractiveObject
{
    public EvaluationCH1BuildTray BuildTray;
    private Animation _animation;
    public Animation sculpture;

    private void Awake()
    {
        base.Awake();

        _animation = GetComponent<Animation>();
        sculpture = sculpture.GetComponent<Animation>();
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        _animation.Play("MovingTray");
        sculpture.Play("WrenchSculpture");

        Invoke("DelayRun", 1.8f);
    }

    public void DelayRun()
    {
        BuildTray.IsInteractive = true;

        _animation.gameObject.SetActive(false);
        sculpture.gameObject.SetActive(false);
    }
}