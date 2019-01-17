using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using BestHTTP;
using LitJson;
using System;
using isotope;
using UnityEngine.SceneManagement;
using Vuforia;
/*
一、加载服务器端数据
*/
public class JT_Menu : MonoBehaviour {
    public static JT_Menu instance;
    public Dictionary<int, JT_PAGEMSG> m_page = new Dictionary<int, JT_PAGEMSG>();
    public Transform book;
    public UISprite btn_Test;
    public UISprite Btn_KnowledgeBase;
    public UISprite Btn_usercenter;
    public UISprite Btn_Help;
    public UISprite send;
    public Transform wolinObj;

    public GameObject mask;
    public GameObject exitPanel;

    private float curScreenHeight;

    //private int versionCode;
    protected void Awake() {
        //string path = Utils.GetStreamingFilePath("VersionCode.txt");
        //Debug.LogError("path : " + path);
        //versionCode = JT_Core.instance.ReadINI("versionCode", path);
        //Debug.LogError("versionCode : " + versionCode);
        if (JT_Core.instance != null && JT_Core.instance.userName == "")
            JT_Core.instance.userid = PlayerPrefs.GetString(JT_PlayerPrefabsMsg.userid, "");

        if (JT_Core.instance != null && JT_Core.instance.userName == "") {
            JT_Core.instance.userName = PlayerPrefs.GetString(JT_PlayerPrefabsMsg.userName, "");
        }

        if (JT_Core.instance != null && JT_Core.instance.userSignal == "") {
            JT_Core.instance.userSignal = PlayerPrefs.GetString(JT_PlayerPrefabsMsg.userSignal, "");
        }

        //VideoPlaybackTapHandler _VideoPlaybackTapHandler = this.GetComponent<VideoPlaybackTapHandler>();
        //if(_VideoPlaybackTapHandler != null){
        //    _VideoPlaybackTapHandler = this.gameObject.AddComponent<VideoPlaybackTapHandler>();
        //}

        instance = this;
#if BuilHX_Android || BuilHX_IOS
		Btn_Help.gameObject.SetActive(false);
		btn_Test.spriteName = "HX_ar_btn_repository_n";
		Btn_KnowledgeBase.spriteName = "HX_ar_btn_ar_p";
		Btn_usercenter.spriteName = "HX_ar_btn_text_n";
		
#endif

        MessageDispatcher.AddListener("ON_TRACKING_FOUND", ON_TRACKING_FOUND);
        MessageDispatcher.AddListener("ON_TRACKING_LOST", ON_TRACKING_LOST);

        UIEventListener.Get(exitPanel.transform.Find("confirmBtn").gameObject).onClick += OnClickConfirmBtn;
        UIEventListener.Get(exitPanel.transform.Find("cancelBtn").gameObject).onClick += OnClickCancelBtn;
    }

    private void OnClickCancelBtn(GameObject go)
    {
        mask.SetActive(false);
        exitPanel.SetActive(false);
    }

    private void OnClickConfirmBtn(GameObject go)
    {
        Application.Quit();
    }

    void OnDestroy() {
        MessageDispatcher.RemoveListener("ON_TRACKING_FOUND", ON_TRACKING_FOUND);
        MessageDispatcher.RemoveListener("ON_TRACKING_LOST", ON_TRACKING_LOST);
    }

    private void Update() {
        if (Screen.height > Screen.width && btn_Test.width != 140) {//竖屏
            curScreenHeight = Convert.ToSingle(1280) / Screen.width * Screen.height;

            btn_Test.width = 140;
            btn_Test.height = 140;

            Btn_KnowledgeBase.width = 140;
            Btn_KnowledgeBase.height = 140;

            Btn_usercenter.width = 140;
            Btn_usercenter.height = 140;

            Btn_Help.width = 140;
            Btn_Help.height = 140;

            send.width = 140;
            send.height = 140;

            btn_Test.transform.localPosition = new Vector3(-540, -(curScreenHeight /2 -100));
            Btn_KnowledgeBase.transform.localPosition = new Vector3(-370, -(curScreenHeight / 2 - 100));
            Btn_usercenter.transform.localPosition = new Vector3(-200, -(curScreenHeight / 2 - 100));
            Btn_Help.transform.localPosition = new Vector3(555, (curScreenHeight / 2 - 85));
            send.transform.localPosition = new Vector3(-30, -(curScreenHeight / 2 - 100));

            if (exitPanel != null || exitPanel.GetComponent<UISprite>() != null)
            {
                exitPanel.GetComponent<UISprite>().width = 918;
                exitPanel.GetComponent<UISprite>().height = 442;
            }
        }
        else if(Screen.height < Screen.width && btn_Test.width != 74) {//横屏
            curScreenHeight = Convert.ToSingle(1280) / Screen.width * Screen.height;
            btn_Test.width = 74;
            btn_Test.height = 74;

            Btn_KnowledgeBase.width = 74;
            Btn_KnowledgeBase.height = 74;

            Btn_usercenter.width = 74;
            Btn_usercenter.height = 74;

            Btn_Help.width = 74;
            Btn_Help.height = 74;

            send.width = 74;
            send.height = 74;
            //btn_Test.transform.localPosition = new Vector3(-580, -290);
            //Btn_KnowledgeBase.transform.localPosition = new Vector3(-480, -290);
            //Btn_usercenter.transform.localPosition = new Vector3(-380, -290);


            btn_Test.transform.localPosition = new Vector3(-580, -(curScreenHeight / 2 - 70));
            Btn_KnowledgeBase.transform.localPosition = new Vector3(-480, -(curScreenHeight / 2 - 70));
            Btn_usercenter.transform.localPosition = new Vector3(-380, -(curScreenHeight / 2 - 70));
            send.transform.localPosition = new Vector3(-271, -(curScreenHeight / 2 - 70));
            Btn_Help.transform.localPosition = new Vector3(590, (curScreenHeight / 2 - 50));

            if (exitPanel != null || exitPanel.GetComponent<UISprite>() != null)
            {
                exitPanel.GetComponent<UISprite>().width = 500;
                exitPanel.GetComponent<UISprite>().height = 300;

            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            mask.SetActive(true);
            exitPanel.SetActive(true);
        }

    }


    private void ON_TRACKING_LOST(IMessage rMessage) {
        if (rMessage.Data.ToString() != "1003")
            return;
        wolinObj.gameObject.SetActive(false);
        wolinObj.GetComponent<JT_TouchEnlarge>().SetOriginPos();
    }

    private void ON_TRACKING_FOUND(IMessage rMessage) {
        JT_Core.instance.pageId = rMessage.Data.ToString();
  
        if (rMessage.Data.ToString() == "1002" && JT_Core.instance.videoUrl.Count >= 5) {
            SceneManager.LoadScene("directory_video");
            
            return;
        }
        if (rMessage.Data.ToString() != "1003")
            return;

        if (wolinObj != null) {
            wolinObj.gameObject.SetActive(true);
            wolinObj.GetComponent<JT_TouchEnlarge>().SetOriginPos();
        }
    }


    protected void Start() {
        //CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_DEFAULT);
        //CameraDevice.Instance.Start();
        //CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

        JT_Core.SetLandscape();
        if (GameObject.Find("Authorization") == null) {
            GameObject obj = Instantiate(new GameObject());
            obj.name = "Authorization";
            obj.AddComponent<JT_Authorization>();
        }
#if UNITY_IPHONE || UNITY_ANDROID
        QuerySetting();
#else
			GetRemotePage();
#endif
    }

    void QuerySetting() {
        string targer = string.Empty;


#if BuilHX_Android
			targer = "BuilHX_Android1";
#endif

#if BuilHX_IOS
			targer = "BuilHX_IOS1";
#endif

#if BuilDTGL_Android
        targer = "BuilDTGL_Android";
#endif

#if BuilDTGL_IOS
			targer = "BuilDTGL_IOS";
#endif
        if (targer != string.Empty) {
            JT_HTTPSENDMSG _HTTPSENDMSG = new JT_HTTPSENDMSG();
            _HTTPSENDMSG.callback = GetSetting;
            _HTTPSENDMSG.interface_name = "setting";
            JsonData date = new JsonData();
            date["action"] = "index";
            date["targer"] = targer;
            _HTTPSENDMSG.RawData = date.ToJson();
            MessageDispatcher.SendMessage(this, JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString(), _HTTPSENDMSG, EnumMessageDelay.IMMEDIATE);
        }
    }

    void GetSetting(HTTPRequest request, HTTPResponse response) {
        int tBack = JT_Core.instance.CheckHttpMsgReceive(request, response);
        if (tBack == 0) {
            JsonData data = JsonMapper.ToObject(response.DataAsText.ToString());
            if (data != null) {
                JsonData videoData = data["video"];
                JsonData imageData = data["img"];
                JsonData textData = data["word"];
                data = data["data"];
                if (data != null) {
                    JT_SETTINGMSG.help = data["help"].ToString();
                    JT_SETTINGMSG.knowledgebase = data["knowledgebase"].ToString();
                    JT_SETTINGMSG.test = data["test"].ToString();
                    JT_SETTINGMSG.usercenter = data["usercenter"].ToString();
                    JT_SETTINGMSG.tailoringRect = int.Parse(data["tailoringrect"].ToString());
                }
                for (int i = 0; i < videoData.Count; i++) {
                    videoPath _videoPath = new videoPath();
                    _videoPath.videoUrl = videoData[i].ToString();
                    _videoPath.imageUrl = imageData[i].ToString();
                    _videoPath.textUrl = textData[i].ToString();
                    JT_Core.instance.videoUrl.Add(_videoPath);
                }
            }
            if (isExe) {
                isExe = false;
                OpenUrl(id);
            }
        }
    }

    public void GetRemotePage(string pageid = "0") {
        if (!pageid.Equals("0")) {
            Transform _Transform = book.Find(pageid);
            if (_Transform != null) {
                Transform[] _Transforms = _Transform.gameObject.GetComponentsInChildren<Transform>();
                if (_Transforms.Length > 1) {
                    return;
                }
            }
        }
        JT_HTTPSENDMSG _HTTPSENDMSG = new JT_HTTPSENDMSG();
        _HTTPSENDMSG.callback = GetPage;
        _HTTPSENDMSG.interface_name = "page?client=2&version="+ 4;
        JsonData date = new JsonData();
        date["action"] = "getdata";
        date["pageid"] = pageid;
        _HTTPSENDMSG.RawData = date.ToJson();
        MessageDispatcher.SendMessage(this, JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString(), _HTTPSENDMSG, EnumMessageDelay.IMMEDIATE);
    }

    void GetPage(HTTPRequest request, HTTPResponse response) {
        int tBack = JT_Core.instance.CheckHttpMsgReceive(request, response);
        if (tBack == 0) {
            JsonData data = JsonMapper.ToObject(response.DataAsText.ToString());
            if (data != null) {
                JT_Core.instance.isReachableViaCarrierDataNetwork = data["isdown"].ToString().Equals("1") ? true : false;  //是否运行再非wifi环境下面播放视频
                data = data["data"];
                if (data != null && !data.Equals("[]")) {
                    m_page.Clear();

                    for (int i = 0; i < data.Count; i++) {
                        JT_PAGEMSG _JT_PAGEMSG = new JT_PAGEMSG();
                        _JT_PAGEMSG.versionid = int.Parse(data[i]["versionid"].ToString());
                        _JT_PAGEMSG.pageid = data[i]["pageid"].ToString();
                        _JT_PAGEMSG.id = int.Parse(data[i]["id"].ToString());
                        _JT_PAGEMSG.type = int.Parse(data[i]["type"].ToString());
                        _JT_PAGEMSG.url = data[i]["url"].ToString();
                        _JT_PAGEMSG.meno = data[i]["meno"].ToString();
                        _JT_PAGEMSG.p_x = float.Parse(data[i]["p_x"].ToString());
                        _JT_PAGEMSG.p_y = float.Parse(data[i]["p_y"].ToString());
                        _JT_PAGEMSG.p_z = float.Parse(data[i]["p_z"].ToString());
                        _JT_PAGEMSG.r_x = float.Parse(data[i]["r_x"].ToString());
                        _JT_PAGEMSG.r_y = float.Parse(data[i]["r_y"].ToString());
                        _JT_PAGEMSG.r_z = float.Parse(data[i]["r_z"].ToString());
                        _JT_PAGEMSG.s_x = float.Parse(data[i]["s_x"].ToString());
                        _JT_PAGEMSG.s_y = float.Parse(data[i]["s_y"].ToString());
                        _JT_PAGEMSG.s_z = float.Parse(data[i]["s_z"].ToString());
                        _JT_PAGEMSG.gray = float.Parse(data[i]["gray"].ToString());
                        _JT_PAGEMSG.identify = int.Parse(data[i]["identify"].ToString());
                        _JT_PAGEMSG.action = data[i]["action"].ToString();
                        _JT_PAGEMSG.createdate = data[i]["createdate"].ToString();

                        if (m_page.ContainsKey(_JT_PAGEMSG.id))
                            m_page.Remove(_JT_PAGEMSG.id);
                        m_page.Add(_JT_PAGEMSG.id, _JT_PAGEMSG);
                    }

#if UNITY_IPHONE || UNITY_ANDROID
                    BuildAR();
#else
					MessageDispatcher.SendMessage(this, "ON_MESSAGE_PAGE", null, EnumMessageDelay.IMMEDIATE);
#endif
                }
            }
        }
    }

    public void BuildAR() {
        if (book == null) {
            return;
        }
        foreach (KeyValuePair<int, JT_PAGEMSG> _m_page in m_page) {
            if (book.Find(_m_page.Value.pageid) != null) {
                GameObject _findbook = book.Find(_m_page.Value.pageid).gameObject;
                if (_findbook != null) {
                    int type = _m_page.Value.type;
                
                    UnityEngine.Object _obj = Resources.Load("AR/" + type.ToString());
#if UNITY_IPHONE || UNITY_ANDROID

#else
					JT_ThisSetting _JT_ThisSetting = ((GameObject) _obj).GetComponent<JT_ThisSetting>();
					_JT_ThisSetting = (_JT_ThisSetting == null) ? ((GameObject) _obj).AddComponent<JT_ThisSetting>() : _JT_ThisSetting;					
#endif

                    if (_obj != null) {
                        GameObject ar = Instantiate(_obj as GameObject);
                        if (ar != null) {
                            switch (type) {
                                case 0:  //0:视频   
                                    if (ar != null) {
                                        JT_3DText _JT_3DText = ar.GetComponent<JT_3DText>();
                                        _JT_3DText.type = 0;
                                        _JT_3DText.path = _m_page.Value.url;
                                    }
                                    break;

                                case 1:  //1：网页
                                    if (ar != null) {
                                        JT_3DText _JT_3DText = ar.GetComponent<JT_3DText>();
                                        _JT_3DText.type = 1;
                                        _JT_3DText.path = _m_page.Value.url;
                                    }
                                    break;

                                case 2:  // 2：文字描述->点击播放视频
                                    if (ar != null) {
                                        JT_3DText _JT_3DText = ar.GetComponent<JT_3DText>();
                                        _JT_3DText.type = 0;
                                        _JT_3DText.path = _m_page.Value.url;
                                        //TextMesh _TextMesh = ar.GetComponent<TextMesh>();
                                        //_TextMesh.text = _m_page.Value.meno;
                                    }
                                    break;

                                case 3://3:文字-点击打开网页
                                    {
                                        JT_3DText _JT_3DText = ar.GetComponent<JT_3DText>();
                                        _JT_3DText.type = 1;
                                        _JT_3DText.path = _m_page.Value.url;
                                        //TextMesh _TextMesh = ar.GetComponent<TextMesh>();
                                        //_TextMesh.text = _m_page.Value.meno;
                                    }
                                    break;

                                case 4:  //显示模型
                                    {
                                        string myurl = _m_page.Value.url;
                                        Debug.Log(myurl);
                                        int index = myurl.LastIndexOf("/");
                                        if (index != -1) {
                                            string url = myurl.Substring(0, index + 1);
                                            string name = myurl.Substring(index + 1, myurl.Length - index - 1);

#if UNITY_IPHONE || UNITY_ANDROID
                                            StartCoroutine(LoadAssetBundle(url, name, ar, _m_page.Value.meno));
#else
							
										Debug.Log("url = "+url + "   name = "+  name);
										StartCoroutine(LoadAssetBundle(url,name,ar,_m_page.Value.meno));  
						
#endif
                                        }
                                    }
                                    break;

                                case 5:   //纯背景
                                    {
#if UNITY_IPHONE || UNITY_ANDROID

#else
									MeshCollider _MeshCollider =  ar.GetComponent<MeshCollider>();
									_MeshCollider = (_MeshCollider==null) ? ar.AddComponent<MeshCollider>() : _MeshCollider;
#endif
                                    }
                                    break;

                                case 6:   //纯文字显示
                                    {
#if UNITY_IPHONE || UNITY_ANDROID

#else
									BoxCollider _BoxCollider =  ar.GetComponent<BoxCollider>() ;
									_BoxCollider = (_BoxCollider==null) ? ar.AddComponent<BoxCollider>() : _BoxCollider;
#endif
                                        TextMesh _TextMesh = ar.GetComponent<TextMesh>();
                                        _TextMesh.text = _m_page.Value.meno;
                                    }
                                    break;

                                default:
                                    ar = null;
                                    break;
                            }
                            if (ar != null) {
                                _findbook.SetActive(true);
                                ar.name = _m_page.Key.ToString();
                                ar.transform.parent = _findbook.transform;
                                ar.transform.localPosition = new Vector3(_m_page.Value.p_x, _m_page.Value.p_y, _m_page.Value.p_z);
                                ar.transform.localRotation = Quaternion.Euler(_m_page.Value.r_x, _m_page.Value.r_y, _m_page.Value.r_z);
                                ar.transform.localScale = new Vector3(_m_page.Value.s_x, _m_page.Value.s_y, _m_page.Value.s_z);
                            }
                        }
                    }
                }
            }
        }
    }


    IEnumerator LoadAssetBundle(string BaseURL, string AssetBundleName, GameObject m_findbook, string name) {
        Debug.Log("url=" + BaseURL + "  AssetBundleName=" + AssetBundleName);
        AssetBundleContainer container = AssetBundleManager.Instance.LoadBundleAsync(BaseURL + AssetBundleManager.GetPlatformFolder(Application.platform) + "/" + AssetBundleName);
        while (!container.IsReady)
            yield return 0;
        if (container.IsError) {
            Debug.LogError(container.ErrorMsg);
            yield break;
        }
        foreach (var asset in container.FileList) {
            Debug.Log(asset.Name + " in " + container.name);
        }

        var flag = container.AssetBundle.LoadAsset<GameObject>(name);

        if (flag) {
            var go = Instantiate(flag, Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
            go.transform.SetParent(m_findbook.transform, false);
        }
    }

    //-------------------------首页菜单------------------------------------
    private int id = -1;//记录按钮的id
    private bool isExe = false;//重新请求接口，

    public void btnClickUserCenter() {
        id = 0;
        //JT_Core.OpenUrl(JT_SETTINGMSG.usercenter,"");
        OpenUrl(id);
    }

    public void btnClickHelp() {
        id = 1;
        //JT_Core.OpenUrl(JT_SETTINGMSG.help,"");
        OpenUrl(id);
    }

    public void btnClickTest() {
        id = 2;
        //JT_Core.OpenUrl(JT_SETTINGMSG.test,"");
        OpenUrl(id);
    }

    public void btnClickKnowledgeBase() {
#if BuilHX_Android || BuilHX_IOS
#else
        id = 3;
        //JT_Core.OpenUrl(JT_SETTINGMSG.knowledgebase,"");
        OpenUrl(id);
#endif
    }

    private void OpenUrl(int id) {
        string url = null;
        switch (id) {
            case 0:
                url = JT_SETTINGMSG.usercenter;
                break;
            case 1:
                url = JT_SETTINGMSG.help;
                break;
            case 2:
                url = JT_SETTINGMSG.test;
                break;
            case 3:
                url = JT_SETTINGMSG.knowledgebase;
                break;
            default:
                break;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            JT_Core.ShowDialog();
            return;
        }
        if (string.IsNullOrEmpty(url)) {
            isExe = true;
            Start();
            return;
        }
        JT_Core.OpenUrl(url, "");
    }
    //--------------------------------------------------------------------------------------------
}
