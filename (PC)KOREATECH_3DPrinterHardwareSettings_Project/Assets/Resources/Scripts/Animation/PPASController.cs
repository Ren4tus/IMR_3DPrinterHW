using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPASController : MonoBehaviour
{
    private static PPASController instance = null;

    public PPBuildChamberDoorUpperAS upperDoorAS;
    public PPBuildChamberDoorLowerAS lowerDoorAS;
    public PPUmbilicalCableAS umbilicalCableAS;
    public PPBuildPlatformAS buildPlatformAS;
    public CommonSwitchActionsAS powerSwitchAS;

    public PPGUIController pGUIController;

    public static PPASController Instance
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
    }
    
    public void UpperDoorOpen()
    {
        upperDoorAS.Open();
    }
    public void UpperDoorClose()
    {
        upperDoorAS.Close();
    }
    public void UpperDoorToggle()
    {
        if (upperDoorAS.IsOpen == true)
        {
            UpperDoorClose();
        }
        else
        {
            UpperDoorOpen();
        }
    }

    public void LowerDoorOpen()
    {
        lowerDoorAS.Open();
    }
    public void LowerDoorClose()
    {
        lowerDoorAS.Close();
    }
    public void LowerDoorToggle()
    {
        if (lowerDoorAS.IsOpen == true)
        {
            LowerDoorClose();
        }
        else
        {
            LowerDoorOpen();
        }
    }
    //

    public void ConnectCartCable()
    {
        umbilicalCableAS.Equip();
    }
    public void DisconnectCartCable()
    {
        umbilicalCableAS.Remove();
    }
    public void CartCableToggle()
    {
        if (umbilicalCableAS.IsEquiped == true)
        {
            umbilicalCableAS.Remove();
        }
        else
        {
            umbilicalCableAS.Equip();
        }
    }

    ///
    public void BuildPlatformEquip()
    {
        buildPlatformAS.EquipBuildPlatform();
    }
    public void BuildPlatformRemove()
    {
        buildPlatformAS.RemoveBuildPlatform();
    }
    public void BuildPlatformToggle()
    {
        if (buildPlatformAS.IsEqiup)
        {
            buildPlatformAS.RemoveBuildPlatform();
        }
        else
        {
            buildPlatformAS.EquipBuildPlatform();
        }
    }

    //
    public void PowerOn()
    {
        pGUIController.GUIOn();
        powerSwitchAS.SwitchOn();
    }
    public void PowerOff()
    {
        pGUIController.GUIOff();
        powerSwitchAS.SwitchOff();
    }
    public void PowerToggle()
    {
        if (powerSwitchAS.IsOn)
        {
            PowerOff();
        }
        else
        {
            PowerOn();
        }
    }

    //

    public void Printing()
    {
    }
}
