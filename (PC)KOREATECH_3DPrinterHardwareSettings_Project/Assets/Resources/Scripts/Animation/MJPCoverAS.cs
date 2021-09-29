using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJPCoverAS : CommonDoorActionAS
{
    public override void PlayOpenSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_CoverOpen);
    }
    public override void PlayCloseSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_CoverClose);
    }
}
