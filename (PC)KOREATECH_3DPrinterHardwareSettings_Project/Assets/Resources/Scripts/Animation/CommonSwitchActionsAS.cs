using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSwitchActionsAS : MonoBehaviour
{
    private Animator _animator;
    private bool isOn = false;

    public Animator animator => _animator;
    public bool IsOn => isOn;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SwitchOn()
    {
        isOn = true;
        _animator.SetTrigger("On");
    }

    public void SwitchOff()
    {
        isOn = false;
        _animator.SetTrigger("Off");
    }

    public virtual void PlaySwitchOnSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.PP_PowerSwitch);
    }

    public virtual void PlaySwitchOffSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.PP_PowerSwitch);
    }
}
