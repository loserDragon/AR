using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Runtime.InteropServices; 
using HighlightingSystem;
using BestHTTP;
using LitJson;
using com.ootii.Messages;
using isotope;

public class JT_Tools : MonoBehaviour {

	[DllImport("Comdlg32.dll",SetLastError=true,ThrowOnUnmappableChar=true, CharSet = CharSet.Auto)]     
	public static extern bool GetOpenFileName([ In, Out ] OpenFileDlg ofd );  
	
	public Texture newTexture;
	public Renderer _render;
	public static JT_Tools instance;
	public Dictionary <GameObject,JT_ThisSettingClass> _Highlighter = new Dictionary <GameObject,JT_ThisSettingClass>();
	public JT_ToolsObject[] toolsUI;
	public GameObject UIloadObj;
	public UILabel btnClickSave;
	public GameObject tools;
	
	GameObject mainObject = null;
	int m_pageid; string BaseURL; 
	string AssetBundleName;
	Texture oldTexture;
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		instance = this;
		BaseURL = "http://ftp.jitengedu.com/";
		MessageDispatcher.AddListener ("ON_MESSAGE_PAGE", OnReceiveMsg);
	}
	
	void OnReceiveMsg (IMessage rMessage)
	{
		Destroy(JT_Menu.instance.book.gameObject);
		GameObject _object = new GameObject();
		_object.name = "Book";
		_object.transform.position = Vector3.zero;
		JT_Menu.instance.book = _object.transform;
		
		foreach(KeyValuePair<int,JT_PAGEMSG>values in JT_Menu.instance.m_page)
		{
			JT_PAGEMSG _JT_PAGEMSG = values.Value;
			if (JT_Menu.instance.book.Find(_JT_PAGEMSG.pageid.ToString())==null)
			{
				GameObject obj = new GameObject();
				obj.transform.parent = JT_Menu.instance.book;
				obj.name = _JT_PAGEMSG.pageid.ToString();
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = new Vector3(50f,50f,50f);
			}
		}
		UIloadObj.SetActive(true);
		JT_Menu.instance.BuildAR();
		Reset();
	}
	

	
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		if (Input.GetKeyUp(KeyCode.Delete))
		{
			foreach(KeyValuePair<GameObject,JT_ThisSettingClass>values in JT_Tools.instance._Highlighter)
			{
				if (values.Value.isSelect)
				{
					if (values.Value._Highlighter.gameObject != null)
						Reset(values.Value._Highlighter.gameObject);
				}
			}
		}
		
		if (Input.GetKeyUp(KeyCode.Space))
		{
			if (oldTexture != null  &&  newTexture != null)
				_render.material.mainTexture = _render.material.mainTexture.Equals(newTexture) ? oldTexture : newTexture;
			else
				Debug.Log("KeyCode.Space");
		}
	}
	
	//--------------------------------------菜单----------------------------------------------------------
	public void btnAddVideo()//添 加 视 频
	{
		GameObject obj = CreateObject("Video",0);
	}
	
	public void btnAddBrowse()  //添 加 浏 览 器
	{
		GameObject obj = CreateObject("Browse",1);

	}
	
	public void btnAddTextToVideo()//添加视频（文本）
	{
		GameObject obj = CreateObject("TextToVideo",2);

	}
	
	public void btnAddTextToBrowse()  //添加浏览器（文本）
	{
		GameObject obj = CreateObject("TextToBrowse",3);
	}
	
	public void btnAddModel()//添 加 模 型
	{
		AssetBundleName = "123.unity3d";
		GameObject obj = CreateObject("Model",4);
		StartCoroutine(LoadAssetBundleModel(obj));
	}
	
	public void btnAddBackground()//添 加 背 景
	{
		GameObject obj = CreateObject("background",5);
	}
	
	public void btnAddText()//添 加 文 字
	{
		GameObject obj = CreateObject("Text",6);
	}
	
	public void btnLoad(string pageid)  //加载
	{
		if (pageid.Equals(""))
			return;
		
		m_pageid = int.Parse(pageid);
		Reset(); 
		Transform obj = JT_Menu.instance.book.Find(m_pageid.ToString());
		if (obj != null)
		{
			btnClickSave.text = "更 新";
			obj.gameObject.SetActive(true);
			tools.SetActive(true);
			return;
		}
		showMessageDialog("提示","数据不存在！");
	}
	
	public void btnSelectSlider(float value)
	{
		float _jiaodu = value * 360;
		Debug.Log(_jiaodu);
	}
	
	public void btnSave()  //新增  or   更新
	{
		JsonData data = new JsonData();
		if  (btnClickSave.text == "新 增")
		{
			if (!JT_Selectbook.isselectbook)
			{
				showMessageDialog("提示","请选择课本！");
				return;
			}
			
			string versionid = JT_Selectbook.instance.bookname.text.Trim();
			data["versionid"] = versionid.Substring(0,versionid.IndexOf(":"));
			data["action"] = "adddata";
			data["pagelist"] = new JsonData();
			foreach(KeyValuePair<GameObject,JT_ThisSettingClass>values in JT_Tools.instance._Highlighter)
			{
				if (!values.Value.isDel)
				{
					JsonData _data = new JsonData();
					_data["meno"] = (values.Value._JT_PAGEMSG.meno == null) ? "" :values.Value._JT_PAGEMSG.meno;
					_data["url"] =  (values.Value._JT_PAGEMSG.url == null) ? "" :values.Value._JT_PAGEMSG.url;
					_data["p_x"] = values.Value._JT_PAGEMSG.p_x.ToString();
					_data["p_y"] = values.Value._JT_PAGEMSG.p_y.ToString();
					_data["p_z"] = values.Value._JT_PAGEMSG.p_z.ToString();
					_data["r_x"] = values.Value._JT_PAGEMSG.r_x.ToString();
					_data["r_y"] = values.Value._JT_PAGEMSG.r_y.ToString();
					_data["r_z"] = values.Value._JT_PAGEMSG.r_z.ToString();
					_data["s_x"] = values.Value._JT_PAGEMSG.s_x.ToString();
					_data["s_y"] = values.Value._JT_PAGEMSG.s_y.ToString();
					_data["s_z"] = values.Value._JT_PAGEMSG.s_z.ToString();
					_data["type"] = values.Value._JT_PAGEMSG.type.ToString();
					data["pagelist"].Add(_data);
				}
			}
			
			JT_HTTPSENDMSG _HTTPSENDMSG = new JT_HTTPSENDMSG();
			_HTTPSENDMSG.callback = SaveBookdata;
			_HTTPSENDMSG.interface_name = "page?client=2";
			_HTTPSENDMSG.RawData = data.ToJson();
			MessageDispatcher.SendMessage(this, JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString(), _HTTPSENDMSG, EnumMessageDelay.IMMEDIATE);
		}else
		{
			string versionid = JT_Selectbook.instance.bookname.text.Trim();
			data["action"] = "updata";
			data["id"] = m_pageid.ToString();
			data["pagelist"] = new JsonData();
			foreach(KeyValuePair<GameObject,JT_ThisSettingClass>values in JT_Tools.instance._Highlighter)
			{
				if (values.Key.activeSelf)
				{
					JsonData _data = new JsonData();
					_data["meno"] = (values.Value._JT_PAGEMSG.meno == null) ? "" :values.Value._JT_PAGEMSG.meno;
					_data["id"] = values.Value._JT_PAGEMSG.id;
					_data["url"] = values.Value._JT_PAGEMSG.url;
					_data["p_x"] = values.Value._JT_PAGEMSG.p_x.ToString();
					_data["p_y"] = values.Value._JT_PAGEMSG.p_y.ToString();
					_data["p_z"] = values.Value._JT_PAGEMSG.p_z.ToString();
					_data["r_x"] = values.Value._JT_PAGEMSG.r_x.ToString();
					_data["r_y"] = values.Value._JT_PAGEMSG.r_y.ToString();
					_data["r_z"] = values.Value._JT_PAGEMSG.r_z.ToString();
					_data["s_x"] = values.Value._JT_PAGEMSG.s_x.ToString();
					_data["s_y"] = values.Value._JT_PAGEMSG.s_y.ToString();
					_data["s_z"] = values.Value._JT_PAGEMSG.s_z.ToString();
					_data["type"] = values.Value._JT_PAGEMSG.type.ToString();
					data["pagelist"].Add(_data);
				}
			}
			
			JT_HTTPSENDMSG _HTTPSENDMSG = new JT_HTTPSENDMSG();
			_HTTPSENDMSG.callback = updateBookdata;
			_HTTPSENDMSG.interface_name = "page?client=2";
			_HTTPSENDMSG.RawData = data.ToJson();
			MessageDispatcher.SendMessage(this, JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString(), _HTTPSENDMSG, EnumMessageDelay.IMMEDIATE);
		}
		Debug.Log(data.ToJson());
	}
	
	
	public void selectFileDialog () {   //  选 择 图 片
		OpenFileDlg pth = new OpenFileDlg();  
		pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);  
		pth.filter = "jpg (*.jpg)";  
		pth.file = new string(new char[512]);  
		pth.maxFile = pth.file.Length;  
		pth.fileTitle = new string(new char[64]);  
		pth.maxFileTitle = pth.fileTitle.Length;  
		pth.initialDir = Application.dataPath;  // default path  
		pth.title = "标题";  
		pth.defExt = "jpg";  
		pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;  
		if (GetOpenFileName(pth))  
		{  
			string filepath=pth.file;//选择的文件路径;  
			StartCoroutine(Load(filepath));
		}
	}
	
	
	IEnumerator Load(string filepath)
	{
		WWW www = new WWW("file://"+filepath);
		yield return www;   
		if(www != null && string.IsNullOrEmpty(www.error))
		{
			oldTexture = _render.material.mainTexture =  www.texture as Texture;
		}
	}
	//--------------------------------------菜单------------------------------------------------------------------------------------------------------------
	
	IEnumerator LoadAssetBundleModel(GameObject objchild)
	{
		AssetBundleContainer container = AssetBundleManager.Instance.LoadBundleAsync( this.BaseURL + AssetBundleManager.GetPlatformFolder( Application.platform ) + "/" + this.AssetBundleName );
		while( !container.IsReady )
			yield return 0;
		if( container.IsError )
		{
			Debug.LogError( container.ErrorMsg );
			yield break;
		}
		
		GameObject flag = container.AssetBundle.LoadAsset<GameObject>("Flag");
		if( flag )
		{
			GameObject go = Instantiate( flag, base.transform.position, base.transform.rotation );
			go.transform.SetParent( objchild.transform, false );
		}
	}
	
	void SaveBookdata(HTTPRequest request, HTTPResponse response)
	{
		int tBack = JT_Core.instance.CheckHttpMsgReceive(request, response);
		if (tBack == 0)
		{
			JsonData data = JsonMapper.ToObject(response.DataAsText.ToString());
			if (data != null)
			{
				int code = int.Parse(data["code"].ToString());
				if (code == 200)
				{
					string id = data["data"].ToString();
					string msg= "数据保存成功，请记录ID编号；";
					showMessageDialog("消息",string.Format("{0}\n ID = {1}",msg,id));
					Reset();
					JT_Menu.instance.GetRemotePage();
				}else
				{
					showMessageDialog("警告","数据保存失败！");
				}
			}
		}
	}
	
	void updateBookdata(HTTPRequest request, HTTPResponse response)
	{
		int tBack = JT_Core.instance.CheckHttpMsgReceive(request, response);
		if (tBack == 0)
		{
			JsonData data = JsonMapper.ToObject(response.DataAsText.ToString());
			if (data != null)
			{
				int code = int.Parse(data["code"].ToString());
				if (code == 200)
				{
					string id = data["data"].ToString();
					string msg= "数据保更新成功，请记录ID编号；";
					showMessageDialog("消息",string.Format("{0}\n ID = {1}",msg,id));
					Reset();
					JT_Menu.instance.GetRemotePage();
					btnClickSave.text ="新 增";
				}else
				{
					showMessageDialog("警告","数据保存失败！");
				}
			}
		}
	}
	
	void Reset(GameObject obj = null)
	{
		if (obj == null)
		{
			_Highlighter.Clear();
			Destroy(mainObject);
			foreach (Transform child in JT_Menu.instance.book)
				child.gameObject.SetActive(false);
		}else
		{
			Debug.Log(obj.name);
			_Highlighter[obj].isDel = true;
			if (obj.transform.parent.transform.parent.gameObject == mainObject)
				obj.transform.parent.gameObject.SetActive(false);
			else
				obj.SetActive(false);
		}
		
		for (int i=0;i<toolsUI.Length;i++)
			toolsUI[i].tager.SetActive(false);
		

	}
	
	
	public void showMessageDialog(string title,string meno)
	{
		GameObject dialog = Resources.Load("MessageDialog") as GameObject;
		if (dialog != null)
		{
			dialog = Instantiate(dialog);
			JT_MessageDialog _JT_MessageDialog = dialog.GetComponent<JT_MessageDialog>();
			if (_JT_MessageDialog != null)
			{
				_JT_MessageDialog.title.text = title;
				_JT_MessageDialog.meno.text = meno;
			}
		}
	}
	
	
	GameObject CreateObject(string name,int type)
	{
		if (mainObject == null)
		{
			mainObject = new GameObject();
			mainObject.transform.position = Vector3.zero;
			mainObject.transform.rotation = Quaternion.Euler(0,0,0);
			mainObject.transform.localScale= new Vector3(1,1,1);
			mainObject.name = "mainObject";
		}
		
		GameObject objparent = new GameObject();
		objparent.transform.parent = mainObject.transform;
		objparent.transform.position = Vector3.zero;
		objparent.transform.rotation = Quaternion.Euler(0,0,0);
		objparent.transform.localScale= new Vector3(50f,50f,50f);
		objparent.name = name;
		
		GameObject objchild =  Instantiate(Resources.Load(string.Format("AR/{0}",type)) as GameObject);
		objchild.transform.parent = objparent.transform;
		objchild.transform.localPosition = new Vector3(0f,0.0001f,0f);
		switch(type)
		{
		case 2:
		case 3:
			objchild.transform.localRotation = Quaternion.Euler(90,180,0);
			break;
			
		case 4:
			objchild.AddComponent<JT_Model>();
			objchild.transform.localRotation = Quaternion.Euler(0,0,0);
			break;
			
		case 5:
			objchild.AddComponent<MeshCollider>();
			break;
			
		case 6:
			objchild.AddComponent<BoxCollider>();
			break;
			
		default:
			objchild.transform.localRotation = Quaternion.Euler(0,0,0);
			break;
		}
		
		objchild.transform.localScale = new Vector3(0.02f,0.02f,0.02f);
		
		objchild.AddComponent<JT_ThisSetting>();
		objchild.name = type.ToString();
		return objchild;
	}
	
	protected void OnDestroy()
	{
		MessageDispatcher.RemoveListener ("ON_MESSAGE_PAGE", OnReceiveMsg);
	}
}
