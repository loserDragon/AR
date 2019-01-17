using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

public class JT_ThisSetting : MonoBehaviour {
	#if UNITY_IPHONE || UNITY_ANDROID
	
	#else
	Highlighter _Highlighter;
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		_Highlighter = gameObject.GetComponent<Highlighter>();
		_Highlighter = (_Highlighter == null)? gameObject.AddComponent<Highlighter>() : _Highlighter;
	}
	
	void Update()
	{  
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo))
			{
				if (hitInfo.collider.transform == transform)
				{
					AddThisSettingClass();
				}
			}
		}
	}
	
	public void AddThisSettingClass()
	{
		ShowHighlighter(gameObject);
		if (!JT_Tools.instance._Highlighter.ContainsKey(gameObject))
		{
			JT_ThisSettingClass _JT_ThisSettingClass = new JT_ThisSettingClass();
			_JT_ThisSettingClass.isSelect = false;
			_JT_ThisSettingClass.isDel = false;
			_JT_ThisSettingClass._Highlighter = _Highlighter;
			_JT_ThisSettingClass._JT_PAGEMSG = new JT_PAGEMSG();
			_JT_ThisSettingClass._JT_PAGEMSG.type  = -1;
			JT_Tools.instance._Highlighter.Add(gameObject,_JT_ThisSettingClass);
			if (JT_Menu.instance.m_page.ContainsKey(int.Parse(gameObject.name)))
				setCoordinates(JT_Menu.instance.m_page[int.Parse(gameObject.name)].type,transform);
		}
	}
	
	
	protected void LateUpdate()
	{
		EditModel();
	}
	
	void EditModel()
	{
		if (!JT_Tools.instance._Highlighter.ContainsKey(gameObject))
			return;
		
		if (!JT_Tools.instance._Highlighter[gameObject].isDrag && JT_Tools.instance._Highlighter[gameObject].isSelect)
		{
			int num = JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.type;
			if (num == -1)
				return;

				//------------------------------------------------------位置坐标-------------------------------------------
			if (!JT_Tools.instance.toolsUI[num].P_x.text.Equals("") &&  !JT_Tools.instance.toolsUI[num].P_y.text.Equals("")  &&  !JT_Tools.instance.toolsUI[num].P_z.text.Equals("")) 
			{
				transform.localPosition = new Vector3(float.Parse(JT_Tools.instance.toolsUI[num].P_x.text),float.Parse(JT_Tools.instance.toolsUI[num].P_y.text),float.Parse(JT_Tools.instance.toolsUI[num].P_z.text));
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.p_x = transform.localPosition.x;
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.p_y = transform.localPosition.y;
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.p_z = transform.localPosition.z;
			}
			
			if (!JT_Tools.instance.toolsUI[num].R_x.text.Equals("") &&  !JT_Tools.instance.toolsUI[num].R_y.text.Equals("")  &&  !JT_Tools.instance.toolsUI[num].R_z.text.Equals("")) 
			{
				transform.localRotation = Quaternion.Euler(float.Parse(JT_Tools.instance.toolsUI[num].R_x.text),float.Parse(JT_Tools.instance.toolsUI[num].R_y.text),float.Parse(JT_Tools.instance.toolsUI[num].R_z.text));
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.r_x = transform.eulerAngles.x;
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.r_y = transform.eulerAngles.y;
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.r_z = transform.eulerAngles.z;
			}
			
			if (!JT_Tools.instance.toolsUI[num].S_x.text.Equals("") &&  !JT_Tools.instance.toolsUI[num].S_y.text.Equals("")  &&  !JT_Tools.instance.toolsUI[num].S_z.text.Equals("")) 
			{
				transform.localScale = new Vector3(float.Parse(JT_Tools.instance.toolsUI[num].S_x.text),float.Parse(JT_Tools.instance.toolsUI[num].S_y.text),float.Parse(JT_Tools.instance.toolsUI[num].S_z.text));
				if (transform.localScale.x >0  &&  transform.localScale.y > 0 &&  transform.localScale.z > 0)
				{
					JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.s_x = transform.localScale.x;
					JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.s_y = transform.localScale.y;
					JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.s_z = transform.localScale.z;
				}
			}
				//---------------------------------------------------------------------------------------------------------
			JT_3DText _JT_3DText = null;
			TextMesh _TextMesh = null;
			
			switch(num)
			{
			case 0:		//0:视频 
				_JT_3DText = gameObject.GetComponent<JT_3DText>();
				if (_JT_3DText != null)
				{
					_JT_3DText.type = 0;
					JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.url = _JT_3DText.path = JT_Tools.instance.toolsUI[num].url.text;
				}
				break;
				
			case 1:    // 1：网页 
				_JT_3DText = gameObject.GetComponent<JT_3DText>();
				if (_JT_3DText != null)
				{
					_JT_3DText.type = 1;
					JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.url = _JT_3DText.path = JT_Tools.instance.toolsUI[num].url.text;
				}
				break;
				
			case 2:    //2：文字-点击播放视频
				_JT_3DText = gameObject.GetComponent<JT_3DText>();
				if (_JT_3DText != null)
				{
					_JT_3DText.type = 0;
					JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.url = _JT_3DText.path = JT_Tools.instance.toolsUI[num].url.text;
				}
				break;
				
			case 3:   //3:文字-点击打开网页
				_JT_3DText = gameObject.GetComponent<JT_3DText>();
				if (_JT_3DText != null)
				{
					_JT_3DText.type = 1;
					JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.url = _JT_3DText.path = JT_Tools.instance.toolsUI[num].url.text;
				}
				break;
				
			case 4:   //4：显示模型
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.url  = JT_Tools.instance.toolsUI[num].url.text;
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.meno = JT_Tools.instance.toolsUI[num].meno.text;
				break;
				
			case 5:   // 5：纯背景
				break;
				
			case 6:   //  6：纯文字
				
				_TextMesh = gameObject.GetComponent<TextMesh>();
				if (_TextMesh != null)
				{
					if (!JT_Tools.instance.toolsUI[num].meno.text.Trim().Equals(""))
						JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.meno = _TextMesh.text = JT_Tools.instance.toolsUI[num].meno.text;
				}
				
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.url  = JT_Tools.instance.toolsUI[num].url.text;
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.meno = JT_Tools.instance.toolsUI[num].meno.text;
				
				
				break;
			}
		}
	}
	
	void ShowHighlighter(GameObject obj)
	{
		foreach(KeyValuePair<GameObject,JT_ThisSettingClass>values in JT_Tools.instance._Highlighter)
		{
			if (!values.Key.Equals(obj))
			{
				values.Value.isSelect=false;
				int num = values.Value._JT_PAGEMSG.type;
				JT_Tools.instance.toolsUI[num].tager.SetActive(false);
				values.Value._Highlighter.FlashingOff();
			}
		}
		
		foreach(KeyValuePair<GameObject,JT_ThisSettingClass>values in JT_Tools.instance._Highlighter)
		{
			
			if (values.Key.Equals(obj))
			{
				values.Value.isSelect=true;
				int type = values.Value._JT_PAGEMSG.type;
				if (type != -1)
				{
					JT_Tools.instance.toolsUI[type].tager.SetActive(true);
					JT_Tools.instance.toolsUI[type].url.text = values.Value._JT_PAGEMSG.url;
					if (!JT_Tools.instance.toolsUI[type].url.Equals(JT_Tools.instance.toolsUI[type].meno))
						JT_Tools.instance.toolsUI[type].meno.text = values.Value._JT_PAGEMSG.meno;
				}
				values.Value._Highlighter.FlashingOn(Color.red,Color.blue);
				setCoordinates(type,values.Key.transform);
				if (!JT_Tools.instance._Highlighter[gameObject].isDrag)
					StartCoroutine(OnMouseDownMove());
				
				return;
			}
		}
	}
	
	void setCoordinates(int type,Transform _Transform)
	{
		if (JT_Tools.instance._Highlighter.ContainsKey(gameObject))
		{
			JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.p_x = _Transform.localPosition.x;
			JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.p_y = _Transform.localPosition.y;
			JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.p_z = _Transform.localPosition.z;
			
			JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.r_x = _Transform.eulerAngles.x;
			JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.r_y = _Transform.eulerAngles.y;
			JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.r_z = _Transform.eulerAngles.z;
			
			if (_Transform.localScale.x > 0)
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.s_x = _Transform.localScale.x;
			if (_Transform.localScale.y > 0)
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.s_y = _Transform.localScale.y;
			if (_Transform.localScale.z > 0)
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.s_z = _Transform.localScale.z;
			
			
			if (JT_Menu.instance.m_page.ContainsKey(int.Parse(gameObject.name)))
			{
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.type = JT_Menu.instance.m_page[int.Parse(gameObject.name)].type;
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.id = JT_Menu.instance.m_page[int.Parse(gameObject.name)].id;
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.url = JT_Menu.instance.m_page[int.Parse(gameObject.name)].url;
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.meno = JT_Menu.instance.m_page[int.Parse(gameObject.name)].meno;
			}else
			{
				JT_Tools.instance._Highlighter[gameObject]._JT_PAGEMSG.type = int.Parse(gameObject.name);
			}
		}
		
		if (type != -1)
		{
			if (JT_Tools.instance.toolsUI.Length > type) 
			{
				JT_Tools.instance.toolsUI [type].P_x.text = _Transform.localPosition.x.ToString ();
				JT_Tools.instance.toolsUI [type].P_y.text = _Transform.localPosition.y.ToString ();
				JT_Tools.instance.toolsUI [type].P_z.text = _Transform.localPosition.z.ToString ();
				
				JT_Tools.instance.toolsUI [type].R_x.text = _Transform.eulerAngles.x.ToString ();
				JT_Tools.instance.toolsUI [type].R_y.text = _Transform.eulerAngles.y.ToString ();
				JT_Tools.instance.toolsUI [type].R_z.text = _Transform.eulerAngles.z.ToString ();
				
				if (_Transform.localScale.x > 0)
					JT_Tools.instance.toolsUI [type].S_x.text = _Transform.localScale.x.ToString ();
				if (_Transform.localScale.y > 0)
					JT_Tools.instance.toolsUI [type].S_y.text = _Transform.localScale.y.ToString ();
				if (_Transform.localScale.z > 0)
					JT_Tools.instance.toolsUI [type].S_z.text = _Transform.localScale.z.ToString ();
			}
		}
	}
	
	
	IEnumerator OnMouseDownMove()  
	{  
		JT_Tools.instance._Highlighter[gameObject].isDrag = true;
		Vector3 ScreenSpace = Camera.main.WorldToScreenPoint(transform.position);  
		Vector3 offset = transform.position-Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,ScreenSpace.z));  
		while(Input.GetMouseButton(0))  
		{  	
			Vector3 curScreenSpace =  new Vector3(Input.mousePosition.x,Input.mousePosition.y,ScreenSpace.z);     
			Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace)+offset;          
			transform.position = CurPosition;  
			yield return new WaitForFixedUpdate();  
		}
		JT_Tools.instance._Highlighter[gameObject].isDrag = false;
	}
	#endif
}
