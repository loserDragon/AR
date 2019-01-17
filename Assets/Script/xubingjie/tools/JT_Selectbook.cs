using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using LitJson;
using com.ootii.Messages;

public class JT_Selectbook : MonoBehaviour {
	public UILabel booktitle;
	public UILabel bookname;
	public UIPopupList _UIPopupList;
	string title = "课本：";
	string firsttitle;
	[HideInInspector]
	public static bool isselectbook = false;
	public  static JT_Selectbook instance;
	public GameObject tools;
	
	void Start () {
		instance = this;
		JT_HTTPSENDMSG _HTTPSENDMSG = new JT_HTTPSENDMSG();
		_HTTPSENDMSG.callback = GetPopupList;
		_HTTPSENDMSG.interface_name = "page?client=2";
		JsonData date = new JsonData();
		date["action"] = "getcurricula";
		_HTTPSENDMSG.RawData = date.ToJson();
		MessageDispatcher.SendMessage(this, JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString(), _HTTPSENDMSG, EnumMessageDelay.IMMEDIATE);
		firsttitle = bookname.text;
	}
	
	void GetPopupList(HTTPRequest request, HTTPResponse response)
	{
		int tBack = JT_Core.instance.CheckHttpMsgReceive(request, response);
		if (tBack == 0)
		{
			JsonData data = JsonMapper.ToObject(response.DataAsText.ToString());
			if (data != null)
			{
				data = data["data"];
				for (int i=0;i<data.Count;i++)
				{
					if (_UIPopupList != null)
					{
						_UIPopupList.items.Add(string.Format("{0}: {1}",data[i]["id"].ToString(),data[i]["curricula_name"].ToString()));
					}
				}
			}
			_UIPopupList.gameObject.SetActive(true);
		}
	}
	
	protected void Update()
	{
		isselectbook = !bookname.text.Equals(firsttitle);
		if (!bookname.text.Equals(firsttitle))
		{
			tools.SetActive(true);
			_UIPopupList.gameObject.SetActive(false);
			booktitle.text = string.Format("{0}{1}",title , bookname.text);
		}else
			tools.SetActive(false);
	}
}
