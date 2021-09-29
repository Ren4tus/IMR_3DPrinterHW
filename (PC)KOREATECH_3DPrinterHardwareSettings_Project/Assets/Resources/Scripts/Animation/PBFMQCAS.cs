using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBFMQCAS : MonoBehaviour
{
    public void PlayOpenSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_WastePanelOpen);
    }
    public void PlayCloseSound()
    {
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_WastePanelClose);
    }
}
