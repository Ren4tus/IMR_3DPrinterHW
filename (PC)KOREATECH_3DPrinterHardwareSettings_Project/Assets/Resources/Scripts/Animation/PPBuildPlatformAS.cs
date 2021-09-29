using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPBuildPlatformAS : CommonDoorActionAS
{
    private bool isEquip = false;
    public bool IsEqiup => isEquip;

    public void EquipBuildPlatform()
    {
        isEquip = true;
        _animator.SetTrigger("Equip");
    }
    public void RemoveBuildPlatform()
    {
        isEquip = false;
        _animator.SetTrigger("Remove");
    }
    public void PlayEquipSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.PP_BuildPlatformEqiup);
    }
    public void PlayRemoveSound()
    {

    }
}
