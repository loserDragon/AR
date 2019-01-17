using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using com.ootii.Messages;
using BestHTTP;
using LitJson;
using PaperPlaneTools;

public class JT_Authorization : MonoBehaviour   {
	int number;
	string  m_txtMsg;
	string  m_txtAuthorization;
	static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
	string Target;
	float _gray;
	string[] ShowMsg;

	protected void Awake()
	{
		ShowMsg = new string[5];
		number = 0;
		DontDestroyOnLoad(this);
		m_txtAuthorization = Target = m_txtMsg = "";
		MessageDispatcher.AddListener("ON_TRACKING_FOUND",ON_TRACKING_FOUND);
		MessageDispatcher.AddListener("ON_TRACKING_LOST",ON_TRACKING_LOST);
	}
	
	void Start () {
		ShowMsg[0] = "您可能是盗版软件受害者，请联系出版社。。。。。。";
		ShowMsg[1] = "软件未授权，请联系出版社。。。。。。";
		ShowMsg[2] = "确定";
		ShowMsg[3] = "完成";
		ShowMsg[4] = "消息";
	}
	
	
	void scan() //扫描计数
	{
		if (!JT_Core.instance.userid.Equals(""))
		{
			JT_HTTPSENDMSG _HTTPSENDMSG = new JT_HTTPSENDMSG();
			_HTTPSENDMSG.callback = null;
			_HTTPSENDMSG.interface_name = "util";
			JsonData date = new JsonData();
			date["action"] = "scan";
			_HTTPSENDMSG.RawData = date.ToJson();
			MessageDispatcher.SendMessage(this, JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString(), _HTTPSENDMSG, EnumMessageDelay.IMMEDIATE);	
		}
	}
	
	void ON_TRACKING_LOST(IMessage rMessage)
	{
		if (rMessage == null)
			return;
	}
	
	void ON_TRACKING_FOUND(IMessage rMessage)
    {
        if (rMessage == null)
			return;
        JT_Menu.instance.GetRemotePage(rMessage.Data.ToString());
		float _gray = CheckIdentify(rMessage.Data.ToString().Trim());
		if (_gray > 0)
		{
			if (!Target.Equals("") &&  !Target.Equals(rMessage.Data.ToString().Trim()))
				Target = "";
			
			if(Vuforia.VuforiaRenderer.Instance != null){
				if(Vuforia.VuforiaRenderer.Instance.VideoBackgroundTexture != null){
					if (Target.Equals(""))
					{
						//if (TrackerManager.Instance.GetTracker<ObjectTracker>() != null)
						//	TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
						//Target=rMessage.Data.ToString().Trim();
						//Texture2D t2d=CaptureScreenshot2( new Rect(0, 0, Screen.width, Screen.height));
						//ScreenshotTaken(t2d);
					}
				}
			}
		}
	}
	
	Texture2D CaptureScreenshot2(Rect rect)
	{
		Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24,false);    
		screenShot.ReadPixels(rect, 0, 0);    
		screenShot.Apply();
		return screenShot;  
	}
	
	
	float CheckIdentify(string pageid)
	{
        foreach (var item in JT_Menu.instance.m_page)
		{
			if (item.Value.pageid.Equals(pageid))
			{
				if (item.Value.identify == 1)
					scan();
				
				if (!item.Value.action.Trim().Equals(""))
				{
                    if (item.Value.action.Trim().IndexOf("http://") != -1)  //url地址
                        JT_Core.OpenUrl(item.Value.action, "");
                    else
                        if (item.Value.action.Trim().IndexOf("video://") != -1)    //视频
                    {
                        JT_Core.videopath = item.Value.action.Replace("video://", "").Trim();
                        Application.LoadLevel("video");
                    }
                    else {
                        Application.LoadLevel(item.Value.action);        //场景
                    }	
				}
				if (item.Value.identify == 1)
					return item.Value.gray;
			}
		}
		return 0f;
	}
	
	IEnumerator _isBlackWhite(Color[] color,int startLength,int Length)
	{
		yield return _endOfFrame;
		
		for (int i=startLength;i<Length;i +=500)
		{
			if (Target.Equals(""))
			{
				m_txtAuthorization = ShowMsg[3];
				yield break;
			}else
			{
				float rr = float.Parse(color[i].r.ToString("f1"));
				float gg = float.Parse(color[i].g.ToString("f1"));
				float bb = float.Parse(color[i].b.ToString("f1"));
				
				if (color[i] !=Color.black && color[i] != Color.white &&  rr-gg != 0 && gg -bb !=0  &&  rr-bb !=0)
				{
					if ( Mathf.Max(Mathf.Abs(color[i].r-color[i].g),Mathf.Abs(color[i].r-color[i].b),Mathf.Abs(color[i].g-color[i].b)) > _gray)
					{
						Target = "";
						m_txtAuthorization = ShowMsg[3];
						yield break;
					}
				}
				m_txtAuthorization = string.Format("分析:{0:F}%",float.Parse(i.ToString())/Length*100f);
			}
			yield return _endOfFrame;
		}
		
		Target = "";
		//new Alert (ShowMsg[4], ShowMsg[0])
		//	.SetPositiveButton ("确定", () => {
		//		OnMessageClose();
		//	})
		//	.Show ();
	}
	
	private void OnMessageClose() {
		number += 1;
		if (number > 3)
		{
			//TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
			//m_txtAuthorization =ShowMsg[3];
			//m_txtMsg = ShowMsg[1];
		}
	}
	
	
	IEnumerator _Start_isBlackWhite(Texture2D _Texture2D,int maxNum)
	{
		yield return _endOfFrame;
		
		Color[] _color = _Texture2D.GetPixels();
		if (TrackerManager.Instance.GetTracker<ObjectTracker>() != null)
			TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
		yield return _endOfFrame;
		
		int startNum = 0;  int endNum = 0;
		startNum = endNum;  endNum =startNum + _color.Length/maxNum;
		StartCoroutine(_isBlackWhite(_color,startNum,endNum));
		yield return _endOfFrame;
		
		startNum = endNum;  endNum =startNum + _color.Length/maxNum;
		StartCoroutine(_isBlackWhite(_color,startNum,endNum));
		yield return _endOfFrame;
		
		startNum = endNum;  endNum =startNum + _color.Length/maxNum;
		StartCoroutine(_isBlackWhite(_color,startNum,endNum));
		yield return _endOfFrame;
		
		startNum = endNum;  endNum =startNum + _color.Length/maxNum;
		StartCoroutine(_isBlackWhite(_color,startNum,endNum));
		yield return _endOfFrame;
		
		startNum = endNum;  endNum =startNum + _color.Length/maxNum;
		StartCoroutine(_isBlackWhite(_color,startNum,endNum));
		yield return _endOfFrame;
	}

	
	protected void OnGUI()
	{
		if (!m_txtAuthorization.Equals(""))
		{
			//GUIStyle style = new GUIStyle();
			//style.fontSize = 10;
			//style.normal.textColor=new Color(255,255,255);
			//GUI.Label (new Rect (0,0,100,50), m_txtAuthorization);
		}
		
		if (!m_txtMsg.Equals(""))
		{
			GUIStyle style = new GUIStyle();
			style.fontSize = 50;
			style.normal.textColor=new Color(0,0,0);
			GUI.Label (new Rect (Screen.width/2-Screen.width/3,Screen.height/2,1000,100), m_txtMsg,style);
		}
	}
	
	void ScreenshotTaken(Texture2D _Texture2D)
	{
		StartCoroutine(_Start_isBlackWhite(_Texture2D,5));
	}
	
	protected void OnDestroy()
	{
		MessageDispatcher.RemoveListener("ON_TRACKING_FOUND",ON_TRACKING_FOUND);
		MessageDispatcher.RemoveListener("ON_TRACKING_LOST",ON_TRACKING_LOST);
	}
}
