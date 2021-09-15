using UnityEngine;

public class PrinterAnimationControll : MonoBehaviour
{
    public MainController mainController;

    private void OnEnable()
    {
        int seqNum = mainController.GetCurrentSeq()+1;
        Debug.Log(seqNum);

        switch(seqNum)
        {
            case 35:
                MJPASController.Instance.Printing();
                break;
            case 37:
                MJPASController.Instance.Printing();
                break;
            case 39:
                MJPASController.Instance.PrintingStop();
                MJPASController.Instance.TrayDownPos();
                break;
            case 40:
                if (MJPASController.Instance.IsCoverOpen())
                    MJPASController.Instance.CoverClose();
                else
                    MJPASController.Instance.CoverOpen();
                break;
            default:
                //AllStop();
                break;
        }
    }

    void AllStop()
    {
        /*
        MJPASController.Instance.PrintingStop();
        MJPASController.Instance.ResetPrinterBlockPosition();

        if(MJPASController.Instance.IsCoverOpen())
        {
            MJPASController.Instance.CoverClose();
        }
        */
    }
}
