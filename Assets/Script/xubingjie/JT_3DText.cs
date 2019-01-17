using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JT_3DText : MonoBehaviour {
	public string  path = string.Empty;
	public int  type = 0;  //0:视频    1：网页    
	public void ShowVideo()
	{
		if (path == string.Empty)
			return;
		//Debug.Log("path ---------------------------" + path);
		#if UNITY_IPHONE  || UNITY_ANDROID
		Handheld.Vibrate();
		#endif
		
		switch(type)
		{
		case 0:
			JT_Core.videopath = path;
			Application.LoadLevel("video");
			
            break;
			
		case 1:
			JT_Core.OpenUrl(path,"");
			break;
		}
		path = string.Empty;
	}

}
