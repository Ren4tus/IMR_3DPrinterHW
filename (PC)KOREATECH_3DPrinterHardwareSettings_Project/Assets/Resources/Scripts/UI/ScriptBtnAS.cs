using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBtnAS : MonoBehaviour
{
    public Animator animator;

    public void OnNext()
    {
        animator.SetTrigger("OnNext");
    }
    public void Init()
    {
        animator.SetTrigger("Init");
    }
}
