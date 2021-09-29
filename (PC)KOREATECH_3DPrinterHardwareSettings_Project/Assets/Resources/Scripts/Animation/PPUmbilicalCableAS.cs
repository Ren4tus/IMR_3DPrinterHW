using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPUmbilicalCableAS : CommonEquipActionAS
{
    public override void PlayEquipSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_MaterialPut);
    }
    public override void PlayRemoveSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_MaterialRemove);
    }
}
