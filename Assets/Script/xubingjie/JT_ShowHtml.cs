using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using com.ootii.Messages;
using BestHTTP;
using LitJson;
using System.Text.RegularExpressions;
using UniqAssets;

public class JT_ShowHtml : MonoBehaviour {
	
	public static string title;
	public static string url;
	// Use this for initialization
	// Awake is called when the script instance is being loaded.
	#if UNITY_IPHONE || UNITY_ANDROID
	int bookid;
	#if UNITY_IPHONE
	List<StoreKitProduct> _products;
	#endif
	#endif
	
	protected void Awake()
	{
		#if UNITY_IPHONE || UNITY_ANDROID
		InAppBrowserBridge bridge = FindObjectOfType<InAppBrowserBridge>();  
		if (bridge !=null){
			bridge.onJSCallback.AddListener(OnMessageFromJS);
			bridge.onBrowserClosed.AddListener(OnBrowserClosed);
		}
		#if UNITY_IPHONE
		initPay();
		#endif
		#endif
	}
	
	void OnBrowserClosed() {
		Application.LoadLevel("main");  
	}
	
	
	void Start () {
		JT_Core.SetPortraitAndLandscape();

		InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
		options.displayURLAsPageTitle = false;
		options.hidesTopBar = true; 
		options.pinchAndZoomEnabled = true;
		options.backButtonText = "关闭";
		options.pageTitle = title;
		options.androidBackButtonCustomBehaviour = true;
		options.shouldUsePlaybackCategory = true;
		InAppBrowser.OpenURL(url, options);

        #if UNITY_IPHONE
		StoreKitManager.productListReceivedEvent += allProducts =>
		{
			Debug.Log("******initPay OK**************");
			_products = allProducts;
		};
		
		StoreKitManager.purchaseSuccessfulEvent += allProducts =>
		{
			StoreKitTransaction _purchase = allProducts;
			Debug.Log( "---------------------");
			Debug.Log("productIdentifier = " + _purchase.productIdentifier);  //EBook_6
			Debug.Log("quantity = " + _purchase.quantity);  //EBook_6
			Debug.Log("Receipt = " + GetReceipt()); 
			Debug.Log("transaction_id = " + _purchase.transactionIdentifier); 
			Debug.Log( "---------------------");
			SendOrder( JT_Core.instance.userid,bookid,GetReceipt(),_purchase.productIdentifier,_purchase.transactionIdentifier);
		};
#endif
    }
	
	#if UNITY_IPHONE || UNITY_ANDROID
	void SendOrder(string uid,int bookid,string receipt,string priceid,string transactionId)
	{
		JT_HTTPSENDMSG _HTTPSENDMSG = new JT_HTTPSENDMSG();
		_HTTPSENDMSG.callback = GetOrder;
		_HTTPSENDMSG.interface_name = "order?client=2";
		JsonData date = new JsonData();
		date["action"] = "saveorder";
		date["token"] = uid;
		date["bookid"] = bookid;
		date["receipt"] = receipt;
		date["priceid"] = priceid;
		date["transactionId"] = transactionId;
		_HTTPSENDMSG.RawData = date.ToJson();
		MessageDispatcher.SendMessage(this, JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString(), _HTTPSENDMSG, EnumMessageDelay.IMMEDIATE);
	}
	
	
	void GetOrder(HTTPRequest request, HTTPResponse response)
	{
		int tBack = JT_Core.instance.CheckHttpMsgReceive(request, response);
		//Debug.Log(response.DataAsText);
	}
	#endif

	
	private void PostImage (Texture2D result) {
		//Debug.Log("------------------PostImage------------------");
		byte[] data = result.EncodeToPNG(); 
		string base64str = System.Convert.ToBase64String(data);
		string Result = string.Format("fnNativeGetImage('{0}!')",base64str);
		InAppBrowser.ExecuteJS(Result); 
	}
	
	
	#if UNITY_IPHONE 
	public void TakePhoto() 
	{
		Debug.Log("TakePhoto");
		iOSPhotoAndCamera.TakePhoto (true, (texture, state) => 
		{
			if (state == iOSPhotoAndCamera.State.kStateFileNotFound)
				Debug.Log("state = kStateFileNotFound");
			if (state == iOSPhotoAndCamera.State.kStateUserCancelled)
				Debug.Log("state = kStateUserCancelled");	
			if (state == iOSPhotoAndCamera.State.kStateSuccess)
				PostImage(texture);
		});
	}
	
	public void SelectPhoto()
	{
		Debug.Log("SelectPhoto");
		iOSPhotoAndCamera.SelectPhoto (true, (texture, state) => 
		{
			if (state == iOSPhotoAndCamera.State.kStateFileNotFound)
				Debug.Log("state = kStateFileNotFound");
			if (state == iOSPhotoAndCamera.State.kStateUserCancelled)
				Debug.Log("state = kStateUserCancelled");	
			if (state == iOSPhotoAndCamera.State.kStateSuccess)
				PostImage(texture);
		});
	}
	#else
	
	void OpenGalleryCamera(bool isCam)
	{
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaClass galleryBinder = new AndroidJavaClass("com.gs.launchgallery.UnityBinder");
		galleryBinder.CallStatic("openCameraGallery", unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"), isCam);
	}
	
	public void OnPhotoPick(string filePath)
	{
		StartCoroutine(LoadImageinImageView(filePath));
	}

	IEnumerator LoadImageinImageView(string filePath)
	{
		WWW www = new WWW("file://" + filePath);
		yield return www;
		Texture2D tempTexture = new Texture2D(www.texture.width, www.texture.height);
		PostImage(tempTexture);
	}
	
	public void TakePhoto() 
	{
		OpenGalleryCamera(true);
	}
	
	public void SelectPhoto()
	{
		OpenGalleryCamera(false);
	}
	#endif
	
	void OnMessageFromJS(string jsMessage) {
		if (jsMessage.Equals("GetImageFromGallery")) {
			//Debug.Log("GetImageFromGallery");
			SelectPhoto();
			return;
		}
		
		if (jsMessage.Equals("GetImageFromCamera")) {
			//Debug.Log("GetImageFromCamera");
			TakePhoto();
			return;
		}
		
		if (jsMessage.Equals("ClearCache")) {
			//Debug.Log("ClearCache");
			InAppBrowser.ClearCache();
			return;
		}
		
		if (jsMessage.Equals("CloseBrowser")) {
			//Debug.Log("CloseBrowser");
			InAppBrowser.CloseBrowser(); 
			Application.LoadLevel("main"); 
			
			return;
		}
		
		if (jsMessage.Equals("UserLogout")) {//注销
            //Debug.Log("UserLogout");

			JT_Core.instance.userid = "";
			PlayerPrefs.SetString(JT_PlayerPrefabsMsg.userid,"");

            JT_Core.instance.userName = "";
            PlayerPrefs.SetString(JT_PlayerPrefabsMsg.userName, "");

            JT_Core.instance.userSignal = "";
            PlayerPrefs.SetString(JT_PlayerPrefabsMsg.userSignal, "");
            return;
		}
		
		//登录接口:      UserLogin_token:4bfaa219d96116b664afce16a6720b34
		if (jsMessage.IndexOf("UserLogin_token#") != -1) {//用户登录
                                                          //Debug.Log("UserLogin_token");
            jsMessage = jsMessage.Replace("UserLogin_token#","").Trim();
            string[] str = jsMessage.Split(';');
            jsMessage = str[0];

            JT_Core.instance.userSignal = str[1].Replace("GroupId#", "").Trim();
            PlayerPrefs.SetString(JT_PlayerPrefabsMsg.userSignal, JT_Core.instance.userSignal);

            JT_Core.instance.userName = str[2].Replace("UserName#", "").Trim();
            PlayerPrefs.SetString(JT_PlayerPrefabsMsg.userName, JT_Core.instance.userName);

            if (!JT_Core.instance.userid.Equals(jsMessage))
			{
				JT_Core.instance.userid = jsMessage;
				PlayerPrefs.SetString(JT_PlayerPrefabsMsg.userid, JT_Core.instance.userid);  
				JT_Menu.instance.GetRemotePage();  //获取可以读取的信息
			}
			return;
		}
		
		#if UNITY_IPHONE
		
		if (jsMessage.IndexOf("OpenIAP_") != -1) {
			
			//Debug.Log("---------------OpenIAP----------------\n" + jsMessage + "\n---------------OpenIAP----------------");
			
			if (_products !=null && _products.Count > 0)
			{
				jsMessage = jsMessage.Replace("OpenIAP_","");
				//Debug.Log("--------OpenIAP = " + jsMessage);
			
				string []strArray = Regex.Split(jsMessage,"#",RegexOptions.IgnoreCase); 
				string priceid = ""; 
				for (int i=0;i<strArray.Length;i++)
				{
					if (strArray[i].IndexOf("bookid:") != -1)
					{
						bookid =int.Parse(strArray[i].Replace("bookid:",""));
						//Debug.Log("bookid="+bookid);
					}
			
					if (strArray[i].IndexOf("priceid:") != -1)
					{
						priceid = strArray[i].Replace("priceid:","");
						//Debug.Log("priceid="+priceid);
					}
			
					if (strArray[i].IndexOf("token:") != -1)
					{
						JT_Core.instance.userid = strArray[i].Replace("token:","");
						//Debug.Log("userid="+priceid);
					}
				}
			
				if (!priceid.Equals("") &&  !JT_Core.instance.userid.Equals(""))
				{
					//Debug.Log("priceid="+priceid);
					for (int j=0;j<_products.Count;j++)
					{
						if (_products[j].productIdentifier.Equals(priceid))
						{
							StoreKitBinding.purchaseProduct( priceid, 1 );
							return;
						}
					}
				}
			}
			return;
		}
		#endif
	}
	
	#if UNITY_IPHONE
	
	string GetReceipt()
	{
		var receiptPath = StoreKitBinding.getAppStoreReceiptLocation().Replace( "file://", string.Empty );
		string receiptBase64 = "";
		if( System.IO.File.Exists( receiptPath ) )
		{
			var receiptData = System.IO.File.ReadAllBytes( receiptPath );
			receiptBase64 = System.Convert.ToBase64String( receiptData );
		}
		return receiptBase64;
	}
	
	
	void initPay()
	{
		JT_HTTPSENDMSG _HTTPSENDMSG = new JT_HTTPSENDMSG();
		_HTTPSENDMSG.callback = GetIAP;
		_HTTPSENDMSG.interface_name = "iap?client=2";
		JsonData date = new JsonData();
		date["action"] = "getdata";
		_HTTPSENDMSG.RawData = date.ToJson();
		MessageDispatcher.SendMessage(this, JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString(), _HTTPSENDMSG, EnumMessageDelay.IMMEDIATE);
	}
	
	void GetIAP(HTTPRequest request, HTTPResponse response)
	{
		int tBack = JT_Core.instance.CheckHttpMsgReceive(request, response);
		if(tBack == 0)
		{
			JsonData data = JsonMapper.ToObject(response.DataAsText.ToString());
			if (data != null  &&  data.Count  >0)
			{
				string[] productIdentifiers = new string[data.Count];
				for (int i=0;i<data.Count;i++)
				{
					productIdentifiers[i] = data[i]["title"].ToString();
				}
				StoreKitBinding.requestProductData( productIdentifiers );
			}
		}
	}
	#endif
}
