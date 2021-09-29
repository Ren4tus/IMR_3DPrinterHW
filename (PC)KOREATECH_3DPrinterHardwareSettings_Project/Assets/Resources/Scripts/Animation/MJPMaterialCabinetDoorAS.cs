using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJPMaterialCabinetDoorAS : CommonDoorActionAS
{
    public override void PlayOpenSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_WastePanelOpen);
    }
    public override void PlayCloseSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_WastePanelClose);
    }
}
