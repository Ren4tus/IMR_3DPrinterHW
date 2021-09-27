using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoginSceneControl : MonoBehaviour
{
	public ContentsType m_ContentsType;
	public InputField m_Id;
	public InputField m_Pw;
	public Button m_Check;
	public Button m_Close;

	void Awake()
	{
		LoginNetworkManager networkManager = LoginNetworkManager.S;
		//string s =  VRContents.m_VRContents.Find((v) => (int)m_ContentsType == Int32.Parse(v.CourseID)).ContentsID;
		//networkManager.CheckContentID(s);
	}

	// Start is called before the first frame update
	void Start()
	{
        // for Editor
		if (m_ContentsType == ContentsType.NONE)
		{
			LoginNetworkManager.S.OnError("컨텐츠 타입을 설정해 주세요.", null);
			return;
		}

		m_Check.onClick.AddListener(RequestLogin);
		m_Close.onClick.AddListener(OnProgramQuit);

		VRContents.m_ContentsType = m_ContentsType;

	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Return))
		{
			RequestLogin();
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (m_Id.isFocused)
			{
				m_Pw.Select();
			}

			if (m_Pw.isFocused)
			{
				m_Id.Select();
			}
		}

	}

	void OnProgramQuit()
	{
		Application.Quit();
	}

	void RequestLogin()
	{
		if (LoginNetworkManager.S.m_IsHttpWebRequesting) { return; }

		if (string.IsNullOrEmpty(m_Id.text))
		{
			LoginNetworkManager.S.OnError("아이디를 입력하세요.");
			return;
		}

		if (string.IsNullOrEmpty(m_Pw.text))
		{
			LoginNetworkManager.S.OnError("비밀번호를 입력하세요.");
			return;
		}

		if (!LoginNetworkManager.S.IsNetworkAvailable(0))
		{
			LoginNetworkManager.S.OnError("네트워크 연결 상태를 확인해 주세요.");
			return;
		}

		LoginNetworkManager.S.ReqAuthenticate(m_Id.text, m_Pw.text, ResAuthenticate);
	}


	void ResAuthenticate(string data)
	{
		Authenticate authenticate = JsonUtility.FromJson<Authenticate>(data);

		if (authenticate.code != "10000")
		{
			LoginNetworkManager.S.OnError("아이디나 비밀번호를 확인해 주세요.");
			return;
		}

		string token = authenticate.body.access_token;
		LoginNetworkManager.S.ReqMemberInfo(token);
		LoginNetworkManager.S.CheckNetworkState();
		
	}

}
