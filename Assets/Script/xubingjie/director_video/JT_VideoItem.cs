using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class JT_VideoItem : MonoBehaviour
{
    private RawImage Image;
    private string url;

    private string videoUrl;

    public bool isComplete = false;
    private void Awake()
    {
        Image = transform.GetComponent<RawImage>();
    }

    private int id;
    public void LoadIMage(string imageUrl ,string textUrl){
        for (int i = 0; i < JT_Core.instance.videoUrl.Count; i++)
        {
            if (JT_Core.instance.videoUrl[i].imageUrl == imageUrl){
                id = i;
                 videoUrl = JT_Core.instance.videoUrl[i].videoUrl;
                if (JT_Core.instance.videoUrl[i].texture) {
                    Image.texture = JT_Core.instance.videoUrl[i].texture;
                    isComplete = true;
                }
                else{
                    StartCoroutine(LoadImage1(textUrl));
                    StartCoroutine( LoadImage(imageUrl));
                }
                break;
            }
        }

    }


    public string GetVideoUrl() {
        return videoUrl;
    }

    public Texture2D SetImage() {
       return  JT_Core.instance.videoUrl[id].textTexture;
    }

    private IEnumerator LoadImage(string url)
    {
        UnityWebRequest request = UnityWebRequest.GetTexture(url);
        yield return request.Send();
        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        JT_Core.instance.videoUrl[id].texture = texture;
        Image.texture = texture;

    }

    private IEnumerator LoadImage1(string url)
    {
        WWW _www = new WWW(url);
        yield return _www;
        if(string.IsNullOrEmpty(_www.error)){
            Texture2D texture1 = _www.texture;
            JT_Core.instance.videoUrl[id].textTexture = texture1;
            isComplete = true;
        }

        //UnityWebRequest request1 = UnityWebRequest.GetTexture(url);
        //yield return request1.Send();
        //Texture2D texture1 = DownloadHandlerTexture.GetContent(request1);
        //JT_Core.instance.videoUrl[id].textTexture = texture1;
    }
}
