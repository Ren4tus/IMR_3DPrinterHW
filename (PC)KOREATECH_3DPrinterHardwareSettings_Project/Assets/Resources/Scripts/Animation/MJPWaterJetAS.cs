using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJPWaterJetAS : MonoBehaviour
{
    public void PlaySteamSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_WaterJetSteam);
    }
    public void PlaySteamSoundShort()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_WaterJetSteamShort);
    }
}
