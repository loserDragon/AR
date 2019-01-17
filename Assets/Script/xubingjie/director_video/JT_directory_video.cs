/* 
describe ：视频
author ：wolin 
createtime ：#CreateTime#
version：v 1.0
*/
//using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JT_directory_video : MonoBehaviour {

    private videoPath videoPath;

    GameObject playBnt;

    private RawImage _texture;
    public Texture2D _nornalSprite;
    public Text _text;

    int number = 0;

    void Start()
    {
        playBnt = this.transform.Find("Button").gameObject;
        playBnt.SetActive(false);
        _texture = this.GetComponent<RawImage>();

    }

  

    public void SetImage(videoPath _videoPath ,Texture2D _sprite ) {
        _texture.texture = _sprite;
        this.videoPath = _videoPath;
        playBnt.SetActive(true);
    }
    
}
