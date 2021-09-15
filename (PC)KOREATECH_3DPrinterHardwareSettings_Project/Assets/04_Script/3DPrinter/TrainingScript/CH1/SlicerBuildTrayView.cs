using UnityEngine;
using UnityEngine.UI;

public class SlicerBuildTrayView : MonoBehaviour
{
    public Text BtnText;
    public Text InnerFillBtnText;

    private Vector3 EqaulAngle = new Vector3(-180f, 220f, -30f);
    private Vector3 FrontAngle = new Vector3(-180f, 200f, 0f);
    private Vector3 SideAngle = new Vector3(-180f, 200f, 90f);
    private Vector3 TopAngle = new Vector3(180f, 270f, 0f);

    private Vector3 ViewPosition = new Vector3(2.05f, -0.8f, 5.8f);
    private Vector3 TopViewPosition = new Vector3(0.7f, -0.8f, 5.8f);

    private int num = 0;
    private int innerFill = 0;

    public static SlicerBuildTrayView instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        innerFill = 0;
    }

    public void InnerFillTextChange()
    {
        innerFill++;

        if (innerFill >= 4)
            innerFill = 0;

        switch(innerFill)
        {
            case 0:
                InnerFillBtnText.text = "내부 채움 : 기본(50%)";
                break;
            case 1:
                InnerFillBtnText.text = "내부 채움 : 무겁게(100%)";
                break;
            case 2:
                InnerFillBtnText.text = "내부 채움 : 가볍게(20%)";
                break;
            case 3:
                InnerFillBtnText.text = "내부 채움 : 속빈(0%)";
                break;
        }
    }

    public int InnerFill()
    {
        return innerFill;
    }

    public void ViewChange()
    {
        num++;

        if (num >= 4)
            num = 0;

        switch(num)
        {
            case 0:
                ViewAngle("모델뷰-등각", ViewPosition, EqaulAngle);
                break;
            case 1:
                ViewAngle("모델뷰-정면", ViewPosition, FrontAngle);
                break;
            case 2:
                ViewAngle("모델뷰-측면", ViewPosition, SideAngle);
                break;
            case 3:
                ViewAngle("모델뷰-평면", TopViewPosition, TopAngle);
                break;
            default:
                Debug.Log("Setting Error");
                break;
        }
    }

    private void ViewAngle(string text, Vector3 pos, Vector3 angle)
    {
        BtnText.text = text;

        this.transform.localPosition = pos;
        this.transform.localRotation = Quaternion.Euler(angle);
    }

    public int GetTrayViewNumber()
    {
        return num;
    }
}
