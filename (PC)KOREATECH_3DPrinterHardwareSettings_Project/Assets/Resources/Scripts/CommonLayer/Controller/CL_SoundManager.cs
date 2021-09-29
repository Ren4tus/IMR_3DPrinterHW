using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CL_SoundManager : MonoBehaviour
{
    public enum UXSound {
        QuickMenuToggle = 0,
        QuickMenuButtonHover,
        Typing,
        TypingComplete,
        KnowledgeTabButtonClick,
        MessagePopUp,
        MessagePopUpClose,
        HilightingWordClick,
        PanelOpen,
        PanelClose,
        TopbarDown,
        AccessDenied,
        SelectClick,
        IndicatorStart,
        IndicatorEnd,
        ScriptButtonSound
    };

    // MJP = 재료분사 프린터(Material Jetting Printer)
    // PP  = 광중합 프린터(Photopolimerization Printer)
    // PBF = 분말적층용융 프린터(Powder Bed Fusion Printer)
    public enum EquipSound
    {
        MJP_CoverOpen = 0,
        MJP_CoverClose,
        MJP_MoveBlock1,
        MJP_MoveBlock2,
        MJP_MoveBlockCrank,
        MJP_MoveBlockReturn,
        MJP_WastePanelOpen,
        MJP_WastePanelClose,
        MJP_MaterialPut,
        MJP_MaterialRemove,
        PP_DoorOpen,
        PP_DoorClose,
        PP_GUIOn,
        PP_BuildPlatformEqiup,
        PP_PowerSwitch,
        MJP_WaterJetSteam,
        MJP_WaterJetSteamShort
    };

    public AudioSource audioNarration;
    public AudioSource audioEffect;
    public AudioSource audioEquip;
    
    public AudioClip[] narration;
    public AudioClip[] ux;
    public AudioClip[] equipment;

    public void SetNarrationVolume(float volume)
    {
        audioNarration.volume = volume;
    }
    public void SetEffectVolume(float volume)
    {
        audioEffect.volume = volume;
    }
    public void SetEquipVolume(float volume)
    {
        audioEquip.volume = volume;
    }

    public bool IsPlayingNarration()
    {
        return audioNarration.isPlaying;
    }
    public void PlayNarration(int idx)
    {
        if (idx >= narration.Length)
            return;

        audioNarration.clip = narration[idx];
        audioNarration.Play();
    }

    public void PlayUXSound(UXSound type)
    {
        if ((int)type >= ux.Length)
            return;

        audioEffect.PlayOneShot(ux[(int)type]);
    }

    public void PlayEquipSound(EquipSound type)
    {
        if ((int)type >= equipment.Length)
            return;

        audioEquip.PlayOneShot(equipment[(int)type]);
    }

    public void StopNarration()
    {
        audioNarration.Stop();
    }
    public void StopUXSound()
    {
        audioEffect.Stop();
    }
    public void StopEquipSound()
    {
        audioEquip.Stop();
    }

    public void StopAllSound()
    {
        StopUXSound();
        StopNarration();
        StopEquipSound();
    }

    // Audio/Training/CH3/분말적층용융프린터_실습_1
    public void LoadSound(string filePath)
    {
        if (narration != null)
            narration = null;

        narration = Resources.LoadAll<AudioClip>(filePath);
        Array.Sort(narration, (a, b) => {
            if (a.name.Length < b.name.Length)
                return -1;
            else if (a.name.Length > b.name.Length)
                return 1;
            else //a.Length == b.Length
                return String.Compare(a.name, b.name);
        });
    }
}
