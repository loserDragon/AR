using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JT_Welcome : MonoBehaviour {
	public string SceneName;
	void Start () {
#if UNITY_IPHONE
		UniqAssets.iOSPhotoAndCamera _iOSPhotoAndCamera = transform.GetComponent<UniqAssets.iOSPhotoAndCamera>();
		_iOSPhotoAndCamera = (_iOSPhotoAndCamera==null) ? gameObject.AddComponent<UniqAssets.iOSPhotoAndCamera>() : _iOSPhotoAndCamera;
#endif

#if BuilHX_Android || BuilHX_IOS
	    JT_Core.OpenUrl("http://main.mantisdm.com","");
#else
		this.Invoke("LoadScene",1f);
		
        //SceneManager.LoadSceneAsync(SceneName);
        //StartCoroutine(LoadScene(SceneName));
#endif
	}
	
	private	 void LoadScene(){
		Application.LoadLevel(SceneName);
	}


    private IEnumerator LoadScene( string  SceneName) {
        yield return SceneManager.LoadSceneAsync(SceneName);
    }
}
