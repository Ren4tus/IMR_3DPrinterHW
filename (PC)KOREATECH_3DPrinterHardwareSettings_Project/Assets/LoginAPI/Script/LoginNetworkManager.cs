using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net;
using UnityEngine.Networking;
using System.Net.Sockets;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

public class LoginNetworkManager : LoginNetworkSingleton<LoginNetworkManager>
{
	//Step URL
	public string BaseURL { get { return StepURL.m_StepURL; } }

	//KOO
	List<VRCourseKoo2> koo_list = VRContentsKoo2.m_VRContentsKoo2;

	// 학생 접속 고유 식별자
	string m_Sah_seq;
	public string sah_seq
	{
		get { return m_Sah_seq; }
		set { m_Sah_seq = value; }
	}

	ResMemberInfo m_ResMemberInfo;
	public ResMemberInfo resMemberInfo
	{
		get { return m_ResMemberInfo; }
		set { m_ResMemberInfo = value; }
	}

	//학생 접속 고유 식별자(Request Class)
	[Serializable]
	class UserIdentifier
	{
		public string sah_seq;
	}

	[Serializable]
	class LogOutInfoRegister
	{
		public string code;
		public string message;
	}

	[HideInInspector]
	public bool m_IsHttpWebRequesting;

	// Session check Time Value 
	int m_SessionCheckTime = 30;

	// Next Scene
	int m_NextSceneLevel = 1;

	bool m_IsSessionOut;
	bool m_IsNetworkPopupOpen = false;
	bool m_OnceNetworkPopupOpen = false;
	bool m_IsCheckLoginState = true;
	private object m_VRContentsKoo2;

	// Start is called before the first frame update
	void Start()
	{
		Application.wantsToQuit += WantsToQuit;
	}

	/*
        POST(로그인)
        /api/login
        clinet_id : client ID
        id : 아이디
        password : 비밀번호
    */

	public void ReqAuthenticate(string id, string pw, UnityAction<string> done)
	{
		if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw)) { return; }

		string url = BaseURL + "/api/login";

		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("Content-Type", "application/x-www-form-urlencoded");
		wWWForm.AddField("client_id", StepURL.m_Client_id);
		wWWForm.AddField("id", id);
		wWWForm.AddField("password", pw);

		WWW www = new WWW(url, wWWForm);

		Log("Request 로그인", url);

		StartCoroutine(WWWRequest(www, done));
	}

	/*
        POST(비밀번호 찾기)
        /api/users/{user-id}/find-password
        clinet_id : client ID
        name : 이름
        typeCode : 비밀번호 받을 유형 1: 이메일 2: 휴대전화
    */

	public void ReqFindPassword(string id, string userName, string toogleName, UnityAction<string> done)
	{
		string url = BaseURL + "/api/users/" + id + "/find-password";

		int typeCode = 0;
		switch (toogleName)
		{
			case "Email":
				typeCode = 1;
				break;
			case "Phone":
				typeCode = 2;
				break;
		}

		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("Content-Type", "application/x-www-form-urlencoded");
		wWWForm.AddField("client_id", StepURL.m_Client_id);
		wWWForm.AddField("name", userName);
		wWWForm.AddField("typeCode", typeCode);

		WWW www = new WWW(url, wWWForm);

		Log("Request 비밀번호찾기", url);

		StartCoroutine(WWWRequest(www, done));

	}

	void Log(string level, object data = null)
	{
		Debug.Log("Level : " + level + " Data : " + data);
	}

	/*
        POST(회원 정보)
        /api/users/me
        access_token : token
        client_id : client ID
    */

	public void ReqMemberInfo(string token)
	{
		string url = BaseURL + "/api/users/me";

		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("Content-Type", "application/x-www-form-urlencoded");
		wWWForm.AddField("access_token", token);
		wWWForm.AddField("client_id", StepURL.m_Client_id);

		WWW www = new WWW(url, wWWForm);

		Log("Request 회원정보", url);

		StartCoroutine(WWWRequest(www, ResMemberInfo));
	}

	void CheckLoginStatus()
	{
		StartCoroutine(CheckingLoginStatus());
	}

	/*
        POST(학생 접속시(로그인) 정보 등록)
        URL : /api/stdLogin
		sah_std_nm : 접속 학생 이름
		sah_std_id : 접속 학생 아이디
		sah_con_nm : 접속 콘텐츠 이름
		sah_con_id : 접속 콘텐츠 아이디
		sah_ip : 접속 아이피
    */

	public void ReqLoginUserInfoRegister()
	{

		string url = StepURL.m_OdaStepURL + "/api/stdLogin";
		string header = StepURL.m_Authorization;

		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", header);
		headers.Add("Content-Type", "application/json");

		//List<VRCourse> list = VRContents.m_VRContents;
		ContentsType type = VRContents.m_ContentsType;

		string contentID = "";
		string contentName = "";
		string courseID = "";

		try
		{
			//contentID = list.Find((v) => (int)type == Int32.Parse(v.CourseID)).ContentsID;
			//contentName = list.Find((v) => (int)type == Int32.Parse(v.CourseID)).Name;
			contentID = koo_list.Find((v) => (int)type == Int32.Parse(v.course_content_id)).course_content_id;
			contentName = koo_list.Find((v) => (int)type == Int32.Parse(v.course_content_id)).service_title;
			courseID = koo_list.Find((v) => (int)type == Int32.Parse(v.course_content_id)).course_id;

		}
		catch (Exception e)
		{
			OnError("회원 정보가 정확하지 않습니다.", DoProgramQuit);
			Debug.Log("ReqLoginUserInfoRegister : " + e);
		}

		if (string.IsNullOrEmpty(contentID) || string.IsNullOrEmpty(contentName))
		{
			OnError("회원 정보가 정확하지 않습니다.", DoProgramQuit);
			return;
		}

		RegisterUserInfo registerUserInfo = new RegisterUserInfo();

		registerUserInfo.sah_std_nm = resMemberInfo.body.name;
		registerUserInfo.sah_std_id = resMemberInfo.body.id;
		registerUserInfo.sah_std_idx = resMemberInfo.body.idx;
		registerUserInfo.sah_con_nm = contentName;
		registerUserInfo.sah_con_id = contentID;

		string ip = GetPublicIP();
		if (string.IsNullOrEmpty(ip))
		{
			ip = Client_IP;
		}
		registerUserInfo.sah_ip = ip;

		string json = JsonUtility.ToJson(registerUserInfo);

		byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
		WWW www = new WWW(url, postData, headers);

		Log("Request 로그인 정보등록", url);

		StartCoroutine(WWWRequest(www, ResUserInfoRegister));
	}


	/*
		POST(학생 로그아웃 정보 등록)
		URL : /api/stdLogout
		sah_seq : 식별자
	*/

	public void ReqLogOutUserInfoRegister(bool isSessionOut)
	{

		string url = StepURL.m_OdaStepURL + "/api/stdLogout";
		string header = StepURL.m_Authorization;

		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", header);
		headers.Add("Content-Type", "application/json");

		UserIdentifier identifier = new UserIdentifier();
		identifier.sah_seq = sah_seq;

		string json = JsonUtility.ToJson(identifier);

		byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
		WWW www = new WWW(url, postData, headers);

		Log("Request 로그아웃 정보등록", url);

		UnityAction<string> done;
		if (isSessionOut) { done = ResLogOutUserInfoRegisterBySession; }
		else { done = ResLogOutUserInfoRegister; }

		StartCoroutine(WWWRequest(www, done));
	}


	//KOO
	void ReqVtCourseList()
	{
		string url = StepURL.m_KoreatechURL + "/api/rest/getVtCourseList";



		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("Content-Type", "application/x-www-form-urlencoded");
		wWWForm.AddField("client_id", StepURL.m_Client_id);

		WWW www = new WWW(url, wWWForm);

		Log("가상훈련 과목 리스트 요청", url);

		StartCoroutine(WWWRequest(www, ResVtCourseList));
	}

	void ResVtCourseList(string data)
	{
		Log("가상훈련 과목 리스트 응답", data);

		ResVTCourseInfon resVtCourseInfo = JsonUtility.FromJson<ResVTCourseInfon>(data);


		int req_count = resVtCourseInfo.body.total_count;
		for (int i = 0; i < req_count; i++)
		{
			koo_list.Add(new VRCourseKoo2(resVtCourseInfo.body.list[i].course_id, resVtCourseInfo.body.list[i].service_title, resVtCourseInfo.body.list[i].course_content_id));
		}

		ReqContentTrainingStatus();

	}

	/*
		POST(가상훈련 콘텐츠의 훈련정보)
		URL : /api/rest/getEnrollCourse
		student_user_id : 학습자 IDX
		course_id : 과목 고유 IDX
	*/

	void ReqContentTrainingStatus()
	{
		Debug.Log("req test");
		string url = StepURL.m_KoreatechURL + "/api/rest/getEnrollCourse";

		//List<VRCourse> list = VRContents.m_VRContents;
		ContentsType type = VRContents.m_ContentsType;
		string courseID = "";
		string contentsID = "";
		try
		{
			//courseID = list.Find((v) => (int)type == Int32.Parse(v.CourseID)).CourseID;
			//courseID = koo_list.Find((v) => (int)type == Int32.Parse(v.course_id)).course_id;

			contentsID = koo_list.Find((v) => (int)type == Int32.Parse(v.course_content_id)).course_content_id;
			courseID = koo_list.Find((v) => (int)type == Int32.Parse(v.course_content_id)).course_id;

		}
		catch (Exception e)
		{
			OnError("회원 정보가 정확하지 않습니다.", DoProgramQuit);
			Debug.Log("ReqLoginUserInfoRegister : " + e);
		}

		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("Content-Type", "application/x-www-form-urlencoded");
		wWWForm.AddField("student_user_id", resMemberInfo.body.idx);
		wWWForm.AddField("course_id", courseID);

		WWW www = new WWW(url, wWWForm);

		Log("Request 수강신청 여부", url);

		StartCoroutine(WWWRequest(www, ResContentTrainingStatus));

	}

	void ResContentTrainingStatus(string data)
	{
		Log("Response 수강신청 여부", data);

		ResContentTrainingInfo resContentTraningInfo = JsonUtility.FromJson<ResContentTrainingInfo>(data);

		if (resContentTraningInfo == null)
		{
			OnError("회원 정보가 일치하지 않습니다.");
			return;
		}

		if (resContentTraningInfo.code != "10000")
		{
			OnError("해당 과목 수강신청 여부를 확인해 주세요.");
			ActiveRegisterCourse();
			return;
		}

		//Success
		//복습, 수강여부 1 : true, 0 : false
		if (resContentTraningInfo.body.is_review == "1" || resContentTraningInfo.body.is_enroll == "1")
		{
			ReqLoginUserInfoRegister();
			return;
		}

		OnError("해당 과목 수강신청 여부를 확인해 주세요.");
		ActiveRegisterCourse();
	}

	void ResLogOutUserInfoRegisterBySession(string data)
	{
		Log("Response 로그아웃 by Session Time Out", data);
		// if Request success,  {"code":"10000","message":"SUCCESS"}
		LogOutInfoRegister logOutInfoRegister = JsonUtility.FromJson<LogOutInfoRegister>(data);
		if (logOutInfoRegister == null) { Application.Quit(); }
		if (logOutInfoRegister.code == "10000") { m_IsSessionOut = true; }
		else { m_IsSessionOut = false; }
	}

	// LogOut 
	void ResLogOutUserInfoRegister(string data)
	{
		Log("Response 로그아웃 ", data);
		// if Request success,  {"code":"10000","message":"SUCCESS"}
		Application.wantsToQuit -= WantsToQuit;
		Application.Quit();
	}

	void ResUserInfoRegister(string data)
	{
		Log("Response 로그인 정보등록", data);
		ResRegisterUserLoginInfo resRegisterUserLoginInfo = JsonUtility.FromJson<ResRegisterUserLoginInfo>(data);

		if (resRegisterUserLoginInfo == null)
		{
			OnError("아이디 비밀번호를 확인해주세요.", DoProgramQuit);
		}

		if (resRegisterUserLoginInfo.code != "10000")
		{
			OnError("해당 과목 수강신청 여부를 확인해 주세요.");
			return;
		}

		sah_seq = resRegisterUserLoginInfo.body.sah_seq;
		m_IsCheckLoginState = true;
		CheckLoginStatus();

		//imrlab 10-07
		//로그인 성공 시 xapi 전송
		LoginSceneControl loginSceneControl = GameObject.Find("LoginSceneControl").GetComponent<LoginSceneControl>();
		XAPIApplication.S.SendLoginStatement(loginSceneControl.m_Id.text);
		//

		SceneManager.LoadScene(m_NextSceneLevel);
	}

	public string Client_IP
	{
		get
		{
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
			string ClientIP = string.Empty;
			for (int i = 0; i < host.AddressList.Length; i++)
			{
				if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
				{
					ClientIP = host.AddressList[i].ToString();
				}
			}
			return ClientIP;
		}
	}

	public string GetPublicIP()
	{
		string externalip = new WebClient().DownloadString("http://checkip.dyndns.org");
		externalip = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(externalip)[0].ToString();
		return externalip;
	}

	/*
		POST(학생 로그인 상태 체크)
		URL : /api/loginStatus
		sah_std_idx : 접속 학생 인덱스 값
		sah_seq : 학생 접속 고유 식별자
	*/

	void ReqUserLoginStatusCheck()
	{
		string url = StepURL.m_OdaStepURL + "/api/loginStatus";
		string header = StepURL.m_Authorization;

		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", header);
		headers.Add("Content-Type", "application/json");

		ReqLoginConditionCheck userLoginConditionCheck = new ReqLoginConditionCheck();

		userLoginConditionCheck.sah_std_idx = resMemberInfo.body.idx;
		userLoginConditionCheck.sah_seq = sah_seq;

		string json = JsonUtility.ToJson(userLoginConditionCheck);

		byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
		WWW www = new WWW(url, postData, headers);

		Log("Request 로그인 상태체크", url);

		StartCoroutine(WWWRequest(www, ResUserLoginStatusCheck));

	}

	void ResUserLoginStatusCheck(string data)
	{
		Log("Response 로그인 상태 확인", data);
		ResLoginConditionCheck resLoginConditionCheck = JsonUtility.FromJson<ResLoginConditionCheck>(data);

		if (resLoginConditionCheck.code == "9300" || resLoginConditionCheck.code != "10000")
		{
			GameObject go = Instantiate(GetLoginPopup());
			LoginPopupBase login = go.GetComponent<LoginPopup>().FindPopup(LoginPopupBase.PopupType.Error);
			login.PopupOpen();
			login.SetText("다른 콘텐츠에 로그인이 되었습니다. \n 다시 접속해 주세요.");
			login.GetComponent<ErrorPopup>().m_CloseListener = () =>
			{
				Application.wantsToQuit -= WantsToQuit;
				Application.Quit();
			};
			StopAllCoroutines();
			ReqLogOutUserInfoRegister(true);
			return;
		}
	}

	void ResMemberInfo(string data)
	{
		Log("Response 회원정보", data);
		ResMemberInfo memberInfo = JsonUtility.FromJson<ResMemberInfo>(data);

		// Invalid Sesesion 
		if (memberInfo.code == "22000" || memberInfo.code != "10000")
		{
			OnError("회원 정보가 정확하지 않습니다.", DoProgramQuit);
			return;
		}

		resMemberInfo = memberInfo;

		//KOO
		ReqVtCourseList();
		//ReqContentTrainingStatus();

	}

	public void CheckNetworkState()
	{
		StartCoroutine(CheckingNetworkState());
	}

	IEnumerator CheckingNetworkState()
	{
		while (true)
		{
			if (IsNetworkAvailable(0))
			{
				if (!m_IsNetworkPopupOpen) { m_OnceNetworkPopupOpen = false; }
				if (!m_IsCheckLoginState)
				{
					m_IsCheckLoginState = true;
					CheckLoginStatus();
				}
			}
			else
			{
				if (!m_IsNetworkPopupOpen && !m_OnceNetworkPopupOpen)
				{
					m_IsNetworkPopupOpen = true;
					m_OnceNetworkPopupOpen = true;
					m_IsCheckLoginState = false;
					GameObject g = Instantiate(GetLoginPopup());
					LoginPopupBase pb = g.GetComponent<LoginPopup>().FindPopup(LoginPopupBase.PopupType.Notify);

					pb.SetText("네트워크 연결이 끊어져 학습이력이 저장되지 않습니다.\n콘텐츠는 정상적으로 사용하실 수 있습니다.");
					pb.GetComponent<NotifyPopup>().m_CloseListener = () =>
					{
						DestroyImmediate(g);
						m_IsNetworkPopupOpen = false;
					};
					pb.PopupOpen();
				}
			}

			yield return null;
		}
	}

	GameObject GetLoginPopup()
	{
		GameObject go = Resources.Load("Prefab/LoginPopup UI") as GameObject;
		if (go != null) { return go; }
		return null;
	}

	IEnumerator CheckingLoginStatus()
	{
		while (m_IsCheckLoginState)
		{
			ReqUserLoginStatusCheck();
			yield return new WaitForSeconds(m_SessionCheckTime);
		}
	}

	IEnumerator WWWRequest(WWW www, UnityAction<string> done)
	{
		m_IsHttpWebRequesting = true;

		yield return www;

		if (www.error != null || www.url == null)
		{
			Debug.Log("WWW Call Error." + www.error);
			m_IsHttpWebRequesting = false;
			done(www.text);
			yield break;
		}

		if (www.isDone)
		{
			if (done != null) { done(www.text); }
			m_IsHttpWebRequesting = false;
		}

	}

	public void CheckContentID(string contentID)
	{
		string[] args = System.Environment.GetCommandLineArgs();
		string id = "";
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == contentID)
			{
				id = contentID;
			}
		}

		if (string.IsNullOrEmpty(id))
		{
			Application.wantsToQuit -= WantsToQuit;
			Application.Quit();
		}
	}

	bool WantsToQuit()
	{
		if (!IsNetworkAvailable(0))
		{
			if (sah_seq == null) { return true; }

			GameObject g = Instantiate(GetLoginPopup());
			LoginPopupBase pb = g.GetComponent<LoginPopup>().FindPopup(LoginPopupBase.PopupType.Notify);
			pb.SetText("네트워크 연결이 끊어져 학습이력이 저장되지 않았습니다.");
			pb.PopupOpen();
			pb.GetComponent<NotifyPopup>().m_CloseListener = () =>
			{
				Application.wantsToQuit -= WantsToQuit;
				Application.Quit();
			};
			return false;
		}

		if (sah_seq != null)
		{
			if (!m_IsSessionOut)
			{
				//Request Logout Userinfo
				ReqLogOutUserInfoRegister(false);
				return false;
			}
			return true;
		}
		return true;
	}

	public bool IsNetworkAvailable(long minimumSpeed)
	{
		if (!NetworkInterface.GetIsNetworkAvailable())
			return false;

		foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
		{
			// discard because of standard reasons
			if ((ni.OperationalStatus != OperationalStatus.Up) ||
				(ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
				(ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
				continue;

			// this allow to filter modems, serial, etc.
			// I use 10000000 as a minimum speed for most cases
			if (ni.Speed < minimumSpeed)
				continue;

			// discard virtual cards (virtual box, virtual pc, etc.)
			if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
				(ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
				continue;

			// discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
			if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
				continue;

			return true;
		}
		return false;
	}

	void DoProgramQuit()
	{
		Application.Quit();
	}

	/// <summary>
	/// Callback sent to all game objects before the application is quit.
	/// </summary>
	void OnApplicationQuit()
	{

	}

	void ActiveRegisterCourse()
	{
		GameObject errorPopup = GameObject.Find("Popup UI");
		if (errorPopup)
		{
			errorPopup.GetComponent<LoginPopup>().ActiveRegisterCourse();
		}
	}

	public void OnError(string error, UnityAction done = null)
	{
		GameObject errorPopup = GameObject.Find("Popup UI");
		if (errorPopup)
		{
			LoginPopupBase pb = errorPopup.GetComponent<LoginPopup>().FindPopup(LoginPopupBase.PopupType.Error);
			pb.PopupOpen(done);
			pb.SetText(error);
		}
	}

	public void OnNotify(string text, UnityAction done = null)
	{
		GameObject go = GameObject.Find("Popup UI");
		if (go)
		{
			LoginPopupBase pb = go.GetComponent<LoginPopup>().FindPopup(LoginPopupBase.PopupType.Notify);
			pb.PopupOpen(done);
			pb.SetText(text);
		}
	}
}
