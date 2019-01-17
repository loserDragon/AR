using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenGalleryPlugin : MonoBehaviour {

    public Text logText;
    public void OnClickOpenGalleryCamera(bool isCam)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaClass galleryBinder = new AndroidJavaClass("com.gs.launchgallery.UnityBinder");
        galleryBinder.CallStatic("openCameraGallery", unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"), isCam);
    }

    WWW www;
	public void OnPhotoPick(string filePath)
    {
		logText.text = filePath;
		StartCoroutine(LoadImageinImageView(filePath));
    }
	public Image img;
    Sprite sp;
	IEnumerator LoadImageinImageView(string filePath)
    {
        www = new WWW("file://" + filePath);
        yield return www;

        Texture2D tempTexture = new Texture2D(www.texture.width, www.texture.height);
        tempTexture.SetPixels32(www.texture.GetPixels32());
        tempTexture.Apply();
        www = null;
		sp = Sprite.Create(tempTexture, new Rect(0,0, tempTexture.width, tempTexture.height), new Vector2(0.5f,0.5f));
		img.sprite = sp;
	}
}
