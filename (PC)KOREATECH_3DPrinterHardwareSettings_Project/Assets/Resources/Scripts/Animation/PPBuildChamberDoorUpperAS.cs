using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPBuildChamberDoorUpperAS : CommonDoorActionAS
{
    public override void PlayOpenSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.PP_DoorOpen);
    }
    public override void PlayCloseSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.PP_DoorClose);
    }
}