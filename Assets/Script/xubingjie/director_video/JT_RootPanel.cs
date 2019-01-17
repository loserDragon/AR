using UnityEngine;
using UnityEngine.UI;

public class JT_RootPanel : MonoBehaviour {

    Transform ImagePanel;

    public Sprite[] image;
    public RawImage _image;

    private void Start()
    {
        ImagePanel = this.transform.Find("ImagePanel");
        int id;
        foreach (Transform item in ImagePanel)
        {
            switch (item.name)
            {
                case "RawImage_5":
                    id = 4;
                    break;
                case "RawImage_4":
                    id = 3;
                    break;
                case "RawImage_3":
                    id = 2;
                    break;
                case "RawImage_2":
                    id = 1;
                    break;
                case "RawImage_1":
                    id = 0;
                    break;
                default:
                    Debug.Log("加载图片失败");
                    return ;
            }

          item.GetComponent<JT_VideoItem>().LoadIMage(JT_Core.instance.videoUrl[id].imageUrl, JT_Core.instance.videoUrl[id].textUrl) ;
        }
    }
   
}
