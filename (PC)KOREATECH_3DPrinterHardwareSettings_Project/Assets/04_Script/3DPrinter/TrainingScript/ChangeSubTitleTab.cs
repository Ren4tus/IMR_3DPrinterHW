using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSubTitleTab : MonoBehaviour
{
    public Text txtMechanism;
    public Text txtMaterial;
    public Text txtPrinting;

    public Image mTabMechanism;
    public Image mTabMaterial;
    public Image mTabPrinting;
    
    private static bool blocking = false;

    void OnMouseOver()
    {
        switch (this.gameObject.tag)
        {
            case "SubTitleMechanism": // 작동개념 탭
                txtMechanism.color = new Color32(187, 187, 187, 255);
                txtMaterial.color = new Color32(85, 85, 85, 255);
                txtPrinting.color = new Color32(85, 85, 85, 255);

                mTabMechanism.color = new Color32(187, 187, 187, 255);
                mTabMaterial.color = new Color32(85, 85, 85, 255);
                mTabPrinting.color = new Color32(85, 85, 85, 255);
                /*if (Input.GetMouseButtonUp(0))
                {
                    txtMaterial.color = ;
                }*/
                break;
            case "SubTitleMaterial": // 조형재료 탭
                txtMechanism.color = new Color32(85, 85, 85, 255);
                txtMaterial.color = new Color32(187, 187, 187, 255);
                txtPrinting.color = new Color32(85, 85, 85, 255);

                mTabMechanism.color = new Color32(85, 85, 85, 255);
                mTabMaterial.color = new Color32(187, 187, 187, 255);
                mTabPrinting.color = new Color32(85, 85, 85, 255);

                break;
            case "SubTitlePrinting": // 프린팅 탭
                txtMechanism.color = new Color32(85, 85, 85, 255);
                txtMaterial.color = new Color32(85, 85, 85, 255);
                txtPrinting.color = new Color32(187, 187, 187, 255);

                mTabMechanism.color = new Color32(85, 85, 85, 255);
                mTabMaterial.color = new Color32(85, 85, 85, 255);
                mTabPrinting.color = new Color32(187, 187, 187, 255);
                break;
            default:
                break;
        }
    }

    private void OnMouseEnter()
    {
    }

    public static void SetBlocking(bool b)
    {
        blocking = b;
    }
}
