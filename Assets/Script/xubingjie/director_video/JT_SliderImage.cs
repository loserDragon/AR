using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JT_SliderImage : MonoBehaviour {
    Vector3 downPos;
    Vector3 upPos;
    public RawImage _iage;

    public Button playBtn;
    public GameObject textObj;
    Touch oldTouch1;
    private bool isChange = false;

    private void Start()
    {
        JT_Core.SetPortrait();
        playBtn.onClick.AddListener(OnClickPlayBtn);
    }

    private void OnClickPlayBtn()
    {
        JT_Core.videopath = EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().GetVideoUrl();
        Application.LoadLevel("video");
        JT_Core.instance.videoBackSceneName = "directory_video";
    }


    public void OnClickCloseBtn()
    {
        textObj.SetActive(true);
        Application.LoadLevel("main");
        JT_Core.instance.videoBackSceneName = "main";
        JT_Core.SetLandscape();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            OnClickCloseBtn();
        }

        if (EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().isComplete) {
            _iage.texture =  EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().SetImage();
        }
#if UNITY_ANDROID || UNITY_IPHONE 
        if (Input.touchCount <= 0)
        {
            return;
        }
        Touch newTouch1 = Input.GetTouch(0);
        if (newTouch1.phase == TouchPhase.Began && Input.touchCount == 1)
        {
            oldTouch1 = newTouch1;
            return;
        }
        if (newTouch1.phase == TouchPhase.Ended && Input.touchCount == 1)
        {
            float roa_x = newTouch1.position.x - oldTouch1.position.x;
            if (roa_x > 0.3f)
            {
                isChange = true;
                EnhanceScrollView.GetInstance.OnBtnLeftClick();
                _iage.texture = EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().SetImage();
                string videoUrl = EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().GetVideoUrl();
                if (videoUrl.Contains("mp4")|| videoUrl.Contains("m3u8"))
                {
                    playBtn.gameObject.SetActive(true);
                }
                else
                {
                    playBtn.gameObject.SetActive(false);
                }
            }
            else if (roa_x < -0.3f)
            {
                isChange = true;
                EnhanceScrollView.GetInstance.OnBtnRightClick();

                _iage.texture = EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().SetImage();
                string videoUrl = EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().GetVideoUrl();
                if (videoUrl.Contains("mp4")||videoUrl.Contains("m3u8"))
                {
                    playBtn.gameObject.SetActive(true);
                }
                else
                {
                    playBtn.gameObject.SetActive(false);
                }
            }

            isChange = false;
            return;
        }
#endif

#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            downPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            upPos = Input.mousePosition;
            if ((downPos.x - upPos.x) > 0.3f)
            {//left move
                EnhanceScrollView.GetInstance.OnBtnRightClick();
                _iage.texture = EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().SetImage();
                string videoUrl = EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().GetVideoUrl();
                if (videoUrl.Contains("mp4"))
                {
                    playBtn.gameObject.SetActive(true);
                }
                else
                {
                    playBtn.gameObject.SetActive(false);
                }
            }
            else if ((downPos.x - upPos.x) < -0.3f)
            {//right move
                EnhanceScrollView.GetInstance.OnBtnLeftClick();
                _iage.texture = EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().SetImage();
                string videoUrl = EnhanceScrollView.GetInstance.CurCenterItem.GetComponent<JT_VideoItem>().GetVideoUrl();
                if (videoUrl.Contains("mp4"))
                {
                    playBtn.gameObject.SetActive(true);
                }
                else
                {
                    playBtn.gameObject.SetActive(false);
                }
            }
        }
#endif
    }
}
