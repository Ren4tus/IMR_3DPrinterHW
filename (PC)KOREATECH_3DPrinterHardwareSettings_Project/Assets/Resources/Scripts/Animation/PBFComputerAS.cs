using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBFComputerAS : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SlicerOpen()
    {
        PlayButtonSound();
        animator.SetTrigger("SlicerOpen");
    }
    public void SlicerClose()
    {
        PlayButtonSound();
        animator.SetTrigger("SlicerClose");
    }

    // Sound
    public void PlayButtonSound()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.SelectClick);
    }
}
