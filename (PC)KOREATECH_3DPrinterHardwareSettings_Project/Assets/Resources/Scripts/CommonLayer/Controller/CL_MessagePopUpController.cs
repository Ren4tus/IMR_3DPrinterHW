/* 
 * MessagePopUpController
 * 조작 가능한 팝업을 생성하는 스크립트
   
 * SetCallback 함수를 이용하여 특정 버튼 클릭 시 작업을 처리할 수 있습니다.
 * 콜백을 등록하지 않을 경우, 기본 동작(버튼 클릭 시 팝업 닫힘)만 작동합니다.
 * 두번째 인자인 buttontype(DialogResponse)으로 버튼별 콜백 지정이 가능합니다. 

 * 사용 예시 : SetCallback(함수명, MessagePopUpController.DialogResponse)

 * [MessagePopUpController.DialogResponse]
 * 예 버튼     : MessagePopUpController.DialogResponse.YES
 * 아니요 버튼 : MessagePopUpController.DialogResponse.NO
 * 확인 버튼   : MessagePopUpController.DialogResponse.OK
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CL_MessagePopUpController : MonoBehaviour
{
    public delegate void Callback();
    private Callback callbackOK = null;
    private Callback callbackYES = null;
    private Callback callbackNO = null;

    // Define
    public enum DialogType { NOTICE = 0, WARNING = 1, CAUTION = 2 }
    public enum DialogResponse { OK, YES, NO, ERROR };

    public GameObject[] Dialogs;
    public GameObject MessageBox;
    public GameObject OneButton;
    public GameObject YesOrNoButton;

    private Button OKButton;
    private Button YesButton;
    private Button NoButton;

    private DialogResponse thisResult;

    [Header("Resolution")]
    public Image BlockPanel;
    public Material BlurMat;
    public Color black;
    public Color white;

    private void Awake()
    {
        // 버튼 배정
        OKButton = OneButton.GetComponent<Button>();
        YesButton = YesOrNoButton.transform.Find("YesButton").gameObject.GetComponent<Button>();
        NoButton = YesOrNoButton.transform.Find("NoButton").gameObject.GetComponent<Button>();

        // 기본 리스너 추가
        OKButton.onClick.AddListener(PopUpClose);
        YesButton.onClick.AddListener(PopUpClose);
        NoButton.onClick.AddListener(PopUpClose);
    }

    private void DialogInit()
    {
        for (int i = 0; i < Dialogs.Length; i++)
            Dialogs[i].SetActive(false);

        ResolutionSet();
    }

    public void PopUp(string message, DialogType type)
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.MessagePopUp);

        SetCallback(null, DialogResponse.OK);

        DialogInit();
        Dialogs[(int)type].SetActive(true);
        MessageBox.GetComponent<Text>().text = message;

        YesOrNoButton.SetActive(false);
        OneButton.SetActive(true);
        OKButton.interactable = true;

        GetComponent<Animator>().SetTrigger("PopupOpen");
    }
    public void PopUp(string message, DialogType type, Callback Function)
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.MessagePopUp);

        // 콜백 지정
        SetCallback(Function, DialogResponse.OK);

        DialogInit();
        Dialogs[(int)type].SetActive(true);
        MessageBox.GetComponent<Text>().text = message;

        YesOrNoButton.SetActive(false);
        OneButton.SetActive(true);
        OKButton.interactable = true;

        GetComponent<Animator>().SetTrigger("PopupOpen");
    }

    public void YesOrNoPopUp(string message, DialogType type, Callback yesButtonFuncion, Callback noButtonFuntion)
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.MessagePopUp);

        // 콜백 지정
        SetCallback(yesButtonFuncion, DialogResponse.YES);
        SetCallback(noButtonFuntion, DialogResponse.NO);

        DialogInit();
        Dialogs[(int)type].SetActive(true);
        MessageBox.GetComponent<Text>().text = message;

        OneButton.SetActive(false);
        YesOrNoButton.SetActive(true);
        YesButton.interactable = true;
        NoButton.interactable = true;

        GetComponent<Animator>().SetTrigger("PopupOpen");
    }

    // 팝업이 닫힐 때, 지정해 둔 콜백 실행
    public void PopUpClose()
    {
        CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.MessagePopUpClose);

        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "OKButton":
                thisResult = DialogResponse.OK;
                callbackOK?.Invoke();
                callbackOK = null;
                break;

            case "YesButton":
                thisResult = DialogResponse.YES;
                callbackYES?.Invoke();
                callbackYES = null;
                break;

            case "NoButton":
                thisResult = DialogResponse.NO;
                callbackNO?.Invoke();
                callbackYES = null;
                break;

            default:
                thisResult = DialogResponse.ERROR;
                break;
        }

        OKButton.interactable = false;
        YesButton.interactable = false;
        NoButton.interactable = false;

        GetComponent<Animator>().SetTrigger("PopupClose");
    }

    // Callback 관련
    public void SetCallback(Callback call, DialogResponse buttontype)
    {
        switch (buttontype)
        {
            case DialogResponse.OK:
                callbackOK = call;
                break;

            case DialogResponse.YES:
                callbackYES = call;
                break;

            case DialogResponse.NO:
                callbackNO = call;
                break;

            default:
                callbackOK = call;
                callbackYES = call;
                callbackNO = call;
                break;
        }
    }

    internal void YesOrNoPopUp(string v1, DialogType nOTICE, object v2, object p)
    {
        throw new NotImplementedException();
    }

    private void ResolutionSet()
    {
        if (Screen.width == 1280 && Screen.height == 1024)
        {
            BlockPanel.material = null;
            BlockPanel.color = black;
        }
        else
        {
            BlockPanel.material = BlurMat;
            BlockPanel.color = white;
        }
    }
}