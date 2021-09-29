using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH1AniamtionController : MonoBehaviour
{
    private static EvaluationCH1AniamtionController instance = null;

    public MJPCoverAS coverAS;
    public MJPWasteContainerAS wasteContainerAS;
    public EvaluationCH1Printer movingObjectsAS;
    public MJPMaterialCabinetDoorAS cabinetDoorAS;
    public MJPMaterialAS[] materialAs;

    public Animator Hand_CleanBuildTray;

    private int materialCount;
    public int MaterialCount => materialCount;
    private bool isprinting = false;

    public static EvaluationCH1AniamtionController Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        materialCount = materialAs.Length;
    }

    // MJP 본체
    public void CoverOpen()
    {
        coverAS.Open();
    }
    public void CoverClose()
    {
        coverAS.Close();
    }
    public void CoverToggle()
    {
        if (coverAS.IsOpen == true)
        {
            coverAS.Close();
        }
        else
        {
            coverAS.Open();
        }
    }
    public bool IsCoverOpen()
    {
        return coverAS.IsOpen;
    }

    public void TrayDownPos()
    {
        movingObjectsAS.TrayDownPos();
    }

    public void WastePanelOpen()
    {
        wasteContainerAS.Open();
    }
    public void WastePanelClose()
    {
        wasteContainerAS.Close();
    }
    public void WastePanelToggle()
    {
        if (wasteContainerAS.IsOpen == true)
        {
            wasteContainerAS.Close();
        }
        else
        {
            wasteContainerAS.Open();
        }
    }
    public bool IsWasteContainerOpen()
    {
        return wasteContainerAS.IsOpen;
    }

    public void Printing()
    {
        movingObjectsAS.PrintStart();
        isprinting = true;
    }
    public void PrintingStop()
    {
        movingObjectsAS.PrintStop();
        isprinting = false;
    }
    public bool isPrinting()
    {
        return isprinting;
    }
    public void HeadCleaning()
    {
        movingObjectsAS.CleanPosition();
    }
    public void ResetPrinterBlockPosition()
    {
        movingObjectsAS.PositionReset();
    }

    // 재료보관함
    public void CabinetDoorOpen()
    {
        cabinetDoorAS.Open();
    }
    public void CabinetDoorClose()
    {
        cabinetDoorAS.Close();
    }
    public void CabinetDoorToggle()
    {
        if (cabinetDoorAS.IsOpen == true)
        {
            cabinetDoorAS.Close();
        }
        else
        {
            cabinetDoorAS.Open();
        }
    }
    public bool IsCabinetCoverOpen()
    {
        return cabinetDoorAS.IsOpen;
    }

    public void MaterialIn(int idx)
    {
        if (idx > materialAs.Length || idx < 0)
            return;

        materialAs[idx].Equip();
    }
    public void MaterialOut(int idx)
    {
        if (idx > materialAs.Length || idx < 0)
            return;

        materialAs[idx].Remove();
    }
    public void MaterialToggle(int idx)
    {
        if (idx > materialAs.Length || idx < 0)
            return;

        if (materialAs[idx].IsEquiped == true)
        {
            MaterialOut(idx);
        }
        else
        {
            MaterialIn(idx);
        }
    }
}
