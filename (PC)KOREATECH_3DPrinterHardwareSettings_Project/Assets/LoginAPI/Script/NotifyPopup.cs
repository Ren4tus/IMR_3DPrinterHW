using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class NotifyPopup : LoginPopupBase
{
    public Text m_Text;
    public Button m_Check;

	public delegate void CloseListener();
	public CloseListener m_CloseListener;


	string m_DefaultContent = "아이디나 비밀번호가 일치하지 않습니다.\n 확인 후 다시 입력해 주세요.";

    // Start is called before the first frame update
    void Start()
    {
        m_Check.onClick.AddListener(PopupClose);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SetText(string text)
    {
        if (string.IsNullOrEmpty(text)) { m_Text.text = m_DefaultContent; }

        m_Text.text = text;
    }

    public override void PopupOpen(UnityAction done = null)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(Opening(m_Popup, done));
    }

    public override void PopupClose()
    {
        StartCoroutine(Closing(m_Popup, PopupClosed));
    }

    void PopupClosed()
	{
		if (gameObject.activeSelf) gameObject.SetActive(false);
		if (m_CloseListener != null) { m_CloseListener(); }
    }
}
