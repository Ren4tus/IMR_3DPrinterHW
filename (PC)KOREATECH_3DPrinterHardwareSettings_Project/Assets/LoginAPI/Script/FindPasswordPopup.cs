using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FindPasswordPopup : LoginPopupBase
{
    
    public InputField m_Id;
    public InputField m_UserName;
    public Button m_Find;
    public Button m_Cancel;
    public ToggleGroup m_ToggleGrounp;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Find.onClick.AddListener(FindPassword);
        m_Cancel.onClick.AddListener(PopupClose);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (m_Id.isFocused)
            {
                m_UserName.Select();
            }

            if(m_UserName.isFocused)
            {
                m_Id.Select();
            }
        }
    }

    void Init(bool active)
    {
        if (!active) { gameObject.SetActive(false); }

        m_Id.text = "";
        m_UserName.text = "";

        Toggle[] toggles = m_ToggleGrounp.transform.GetComponentsInChildren<Toggle>();
        foreach (var v in toggles)
        {
            if (v.name == "Email") { v.isOn = true; }
            else { v.isOn = false; }
        }

    }

    public void FindPassword()
    {
        if (LoginNetworkManager.S.m_IsHttpWebRequesting) { return; }

        if (string.IsNullOrEmpty(m_Id.text))
        {
            LoginNetworkManager.S.OnError("아이디(이메일형식)을 입력해주세요.");
            m_Id.Select();
            return;
        }

        if(string.IsNullOrEmpty(m_UserName.text))
        {
            LoginNetworkManager.S.OnError("이름을 입력해주세요.");
            m_UserName.Select();
            return;
        }
        
        if (!FindActiveToogle()) { return; }
        
        LoginNetworkManager.S.ReqFindPassword(m_Id.text, m_UserName.text, FindActiveToogle().name, ResFindPassword);
    }

    Toggle FindActiveToogle()
    {
        Toggle[] toggles = m_ToggleGrounp.transform.GetComponentsInChildren<Toggle>();
        foreach(var v in toggles)
        {
            if(v.isOn) { return v;}
        }
    
        return null;
    }


    public override void PopupOpen(UnityAction done = null)
    {
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }
        StartCoroutine(Opening(m_Popup, done));
    }

    public override void PopupClose()
    {
        StartCoroutine(Closing(m_Popup, () => Init(false)));
    }

    void ResFindPassword(string data)
    {
        ResFindPassword findPassword = JsonUtility.FromJson<ResFindPassword>(data);

        if (findPassword.code != "10000")
        {
            LoginNetworkManager.S.OnError("아이디나 성명을 확인해 주세요.");
            Init(true);
            return;
        }

        LoginNetworkManager.S.OnNotify("비밀번호가 변경되었습니다.\n 이메일이나 휴대전화를 확인해 주세요.");
        PopupClose();

    }

}
