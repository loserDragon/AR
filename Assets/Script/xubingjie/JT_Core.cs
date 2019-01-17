using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using BestHTTP;
using System;
using System.IO;
using System.Net;
using LitJson;
using System.Text;
using System.Runtime.InteropServices;
using HighlightingSystem; 
using PaperPlaneTools;

public enum  JT_VR_MessageInfo
{
	ON_MESSAGE_QUERY_IPA,
	ON_SEND_HTTP_MSG
};

public class JT_Core :MonoBehaviour {
	[HideInInspector]
	public  string userid = "";//用户token

    public string userSignal = "";//用户标识，1- 学生，2 -老师
    public string userName = "";//用户名

    public List<videoPath> videoUrl = new List<videoPath>();
    public string videoBackSceneName = "main";

    public string pageId;//扫描到的书本的id

    [HideInInspector]
    public  bool  isReachableViaCarrierDataNetwork = false;  //false: 是否允许网络播放视频
	string  _RemoteUrl = "http://book.mantisdm.com/api/";

	public  static int language = 0; // 0:china     1:english
	public  static JT_Core instance;
	public  static bool    IsAuthorization = false;   //true：判断授权      false:不判断授权
	public  String url{get;private set;}
	public  String ServerIP{get;private set;}
	public  String ServerPort{get;private set;}
	public  string path{get;private set;}
	UnityEngine.Object SendHttpMsgLock = new UnityEngine.Object();
	UnityEngine.Object CheckHttpMsgReceiveLock = new UnityEngine.Object();
	
	public static string  videopath;

    [HideInInspector]
    public static bool isClickBtn = true;

    public static void OpenUrl(string url,string title)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			ShowDialog();
		}else
		{
			JT_ShowHtml.title = title;
			JT_ShowHtml.url = url;
			Debug.Log("url = "+url);
			Application.LoadLevel("ShowHtml");
		}
	}
	
	
	public static void SetPortrait()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;   
		Screen.autorotateToLandscapeLeft = true; 
		Screen.autorotateToLandscapeRight = true;  
		Screen.autorotateToPortrait = false;  
		Screen.autorotateToPortraitUpsideDown = false;  
		//	Screen.orientation = ScreenOrientation.AutoRotation;  
	}
	
	
	public static void SetLandscape()
	{
		//Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true; 
		Screen.autorotateToLandscapeRight = true;  
		Screen.autorotateToPortrait = true;  
		Screen.autorotateToPortraitUpsideDown = true;  
		//Screen.orientation = ScreenOrientation.AutoRotation;  
	}
	
	public static void SetPortraitAndLandscape()
	{
		//Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true; 
		Screen.autorotateToLandscapeRight = true;  
		Screen.autorotateToPortrait = true;  
		Screen.autorotateToPortraitUpsideDown = true;  
		//Screen.orientation = ScreenOrientation.AutoRotation;  
	}
	
	public static void ShowDialog()
	{
		string title = "提示";
		string msg = "网络连接失败，请检查网络设置。";
		string name = "确定";
		
		new Alert (title, msg)
			.SetPositiveButton (name, () => {
				OnMessageClose();
			})
			.Show ();
	}
	
	protected void Awake()
	{
		DontDestroyOnLoad(this);
		
		#if BuilHX_Android || BuilHX_IOS
		_RemoteUrl = "http://book.mantisdm.com/api/";
		#endif
		
		#if BuilDTGL_Android || BuilDTGL_IOS
		_RemoteUrl = "http://book.mantisdm.com/api/";
		#endif
		
		MessageDispatcher.AddListener (JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString (), OnReceiveMsg);
		instance = this;
		ServerIP = "10.0.0.251";
		ServerPort = "8200";
		path = System.Environment.CurrentDirectory;
		path = path+"/conf.ini"; 
		if (!File.Exists(path))
			initINI(path);
        ReadINI(path);
	}
	
	void OnReceiveMsg (IMessage rMessage)
	{
		if (rMessage == null)
			return;
		
		switch (rMessage.Type) {	
		case "ON_SEND_HTTP_MSG":
			JT_HTTPSENDMSG _HTTPSENDMSG = (JT_HTTPSENDMSG)rMessage.Data;
			SendHttpMsg(_HTTPSENDMSG);
			break;
			
		default:
			break;
		}
	}
	
	void SendHttpMsg(JT_HTTPSENDMSG HTTPSENDMSG)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			ShowDialog();
		}else
		{
			lock(SendHttpMsgLock)
			{
		#if BuilHX_Android || BuilHX_IOS
			_RemoteUrl = HTTPSENDMSG.interface_name.Equals("setting") ? "http://main.mantisdm.com/api/" : "http://book.mantisdm.com/api/";
		#endif
				HTTPRequest request = new HTTPRequest(new Uri(_RemoteUrl + HTTPSENDMSG.interface_name),HTTPMethods.Post, HTTPSENDMSG.callback); 
				request.RawData = Encoding.UTF8.GetBytes(HTTPSENDMSG.RawData);
				request.AddHeader("Authorization","Token "+JT_Core.instance.userid);
				//Debug.Log("------------------------------------"+_RemoteUrl+HTTPSENDMSG.interface_name+"------------------------------------");
				foreach(KeyValuePair<string,string>values in HTTPSENDMSG.AddField)
				{
					request.AddField(values.Key.ToString(), values.Value.ToString());
				}
				request.Timeout = TimeSpan.FromSeconds(5); 
				request.Send();
			}
		}
	}
	
	private static void OnMessageClose() {
        //Debug.Log("----------------OnMessageClose----------------------");
        //Application.Quit();
        isClickBtn = false;

    }
	
	void initINI(string path) //创建文件
	{
		INIParser ini = new INIParser();
		ini.Open(path);
		ini.WriteValue("StockServer","IP","");
		ini.WriteValue("HttpServer","IP","");
		ini.WriteValue("HttpServer","PORT","");
		ini.WriteValue("HttpServer","Language","");
		if (IsAuthorization)
		{
			ini.WriteValue("Authorization","KEY",SystemInfo.deviceUniqueIdentifier);
			ini.WriteValue("Authorization","VALUE","");
		}
		ini.Close();
	}

    #region 文本存放版本号
    public void WriteINI(string key, int value, string path)																																			
    {
        INIParser ini = new INIParser();
        ini.Open(path);
        ini.WriteValue(key, key, value);

        ini.Close();
    }
    public int ReadINI(string key, string path)//读取配置文件																																				
    {
        INIParser ini = new INIParser();
        if (!File.Exists(path)) {
            return 0;
        }
        ini.Open(path);
        //int versionCode = ini.ReadValue("versionCode", "2", versionCode).Equals("") ? versionCode : ini.ReadValue("versionCode", "2", ServerIP)
        int versionCode = ini.ReadValue("versionCode", "versionCode", 2).Equals("") ? 2 : ini.ReadValue("versionCode", "versionCode", 2);
        ini.Close();
        return versionCode;
    }
    #endregion



    void ReadINI(string path)//读取配置文件																																				
	{
		INIParser ini = new INIParser();
		ini.Open(path);
		ServerIP = ini.ReadValue("StockServer","IP",ServerIP).Equals("")?ServerIP:ini.ReadValue("StockServer","IP",ServerIP);
		language = ini.ReadValue("HttpServer","Language",language).Equals("")?language:ini.ReadValue("HttpServer","Language",language);
		
		string httpAddress = ini.ReadValue("HttpServer","IP",ServerIP).Equals("")?ServerIP:ini.ReadValue("HttpServer","IP",ServerIP);
		ServerPort = ini.ReadValue("HttpServer","PORT",ServerPort).Equals("")?ServerPort:ini.ReadValue("HttpServer","PORT",ServerPort);
		url = "http://"+httpAddress+":"+ServerPort+"/";//接口服务器地址
		
		if (IsAuthorization &&  SystemInfo.deviceUniqueIdentifier.Trim().Equals(ini.ReadValue("Authorization","KEY","----")))
		{
			IsAuthorization = !ini.ReadValue("Authorization","VALUE","*#06#").Trim().Equals(Md5Sum(ini.ReadValue("Authorization","KEY","----")));
		}
		ini.Close();
	}
	
	public string Md5Sum(string input)  
	{  
		System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();  
		byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes("xbj"+input+"xbh");  
		byte[] hash = md5.ComputeHash(inputBytes);  
		System.Text.StringBuilder sb = new System.Text.StringBuilder();  
		for (int i = 0; i < hash.Length; i++)  
		{  
			sb.Append(hash[i].ToString("X2"));
		}  
		return sb.ToString();  
	}
	
	public  int CheckHttpMsgReceive(HTTPRequest request, HTTPResponse response)
	{
		lock(CheckHttpMsgReceiveLock)
		{
			int error = 0;
			switch (request.State)
			{
			case HTTPRequestStates.Finished:
				//Debug.Log("Request Finished! Text received: \n" + response.DataAsText);
				break;
				
			case HTTPRequestStates.Error:
				error = 404;
				//Debug.Log("Request Finished with Error! " + (request.Exception != null ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No Exception"));
				break;
				
			case HTTPRequestStates.Aborted:
				//Debug.Log("Request Aborted!");
				error = 405;
				break;
				
			case HTTPRequestStates.ConnectionTimedOut:
				//Debug.Log("Connection Timed Out!");
				error = 406;
				break;
				
			case HTTPRequestStates.TimedOut:
				//Debug.Log("Processing the request Timed Out!");
				error = 407;
				break;
				
				
			default:
				//Debug.Log("An unknown error network !");
				error = 408;
				break;
				
			}
			#if UNITY_IPHONE || UNITY_ANDROID
			if (error != 0)
			{
				string title = "提示";
				string msg = "网络连接失败，请检查网络设置。";
				string name = "确定";
				
				new Alert (title, msg)
					.SetPositiveButton (name, () => {
						OnMessageClose();
					})
					.Show ();
			}
			#endif
			return error;
		}
	}
	
	protected void OnDestroy()
	{
		MessageDispatcher.RemoveListener (JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString (), OnReceiveMsg);
	}
}


public class JT_PAGEMSG {
	public int versionid{get;set;}
	public string pageid{get;set;}
	public int id{get;set;}
	public int type{get;set;}
	public string url{get;set;}
	public string meno{get;set;}
	public float p_x{get;set;}
	public float p_y{get;set;}
	public float p_z{get;set;}
	public float r_x{get;set;}
	public float r_y{get;set;}
	public float r_z{get;set;}
	public float s_x{get;set;}
	public float s_y{get;set;}
	public float s_z{get;set;}
	public float gray{get;set;}
	public int identify{get;set;}
	public string action{get;set;}
	public string createdate{get;set;}
}

public class JT_ThisSettingClass {
	public bool isDel;      //是否删除
	public bool isSelect{get;set;}    //是否选中   
	public bool isDrag{get;set;}    //是否拖动中
	public JT_PAGEMSG _JT_PAGEMSG{get;set;}
	public Highlighter _Highlighter{get;set;}
}

public class JT_HTTPSENDMSG {
	public string interface_name{get;set;}//接口名称
	public string RawData{get;set;}
	public Dictionary<string, string> AddField = new Dictionary<string, string>();//接口参数
	public OnRequestFinishedDelegate callback{ get; set; }//接口回调方法
}


public class JT_SETTINGMSG {
	public static string help{get;set;}
	public static string usercenter{get;set;}
	public static string test{get;set;}
	public static string knowledgebase{get;set;}
	public static int tailoringRect = 0;
}



public class JT_MOUSE_DIRECTION_MSG
{
	public  string number{get;set;}
	public  string direction{get;set;}
}

[StructLayout( LayoutKind.Sequential, CharSet=CharSet.Auto )]    
public class OpenFileDlg  
{  
		public int      structSize = 0;  
		public IntPtr   dlgOwner = IntPtr.Zero;   
		public IntPtr   instance = IntPtr.Zero;  
		public String   filter = null;  
		public String   customFilter = null;  
		public int      maxCustFilter = 0;  
		public int      filterIndex = 0;  
		public String   file = null;  
		public int      maxFile = 0;  
		public String   fileTitle = null;  
		public int      maxFileTitle = 0;  
		public String   initialDir = null;  
		public String   title = null;     
		public int      flags = 0;   
		public short    fileOffset = 0;  
		public short    fileExtension = 0;  
		public String   defExt = null;   
		public IntPtr   custData = IntPtr.Zero;    
		public IntPtr   hook = IntPtr.Zero;    
		public String   templateName = null;   
		public IntPtr   reservedPtr = IntPtr.Zero;   
		public int      reservedInt = 0;  
		public int      flagsEx = 0;  
}

[System.Serializable]
public class JT_ToolsObject
{
	public GameObject tager;     
	public UILabel url;     
	public UILabel meno;     
	public UILabel P_x;      
	public UILabel P_y;    
	public UILabel P_z;
	public UILabel R_x;      
	public UILabel R_y;    
	public UILabel R_z;
	public UILabel S_x;      
	public UILabel S_y;    
	public UILabel S_z;
}


public class videoPath {
    public string imageUrl { get; set; }
    public string videoUrl { get; set; }
    public string textUrl { get; set; }
    public Texture2D texture{ get; set; }
    public Texture2D textTexture { get; set; }
}

