using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaperPlaneTools;
using RenderHeads.Media.AVProVideo;
public class JT_AR_Video : MonoBehaviour {
	public  MediaPlayer MyMediaPlayer ;
	public  Text _text;
	public  Image _RePlay;
    public GameObject watermark2;

    public Image watermark1;
    public Sprite bg1;
    public Sprite bg2;

    public Slider mSlider;
    public Text CurTimer;

    private string totalTime;
    // Awake is called when the script instance is being loaded.
    protected void Awake()
	{
        _RePlay.gameObject.SetActive(false);
	}
	void	Test(){
		
		MyMediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL,"http://gl.file.mantisdm.com/uploads/newcourseware/NO5/5_1_2_3/79c1d6ca2e5c6e0e36c71be0fa9dd1ec.m3u8",true);
	}

	protected void Start()
	{
		
		//this.Invoke("Test",10);
		
        JT_Core.SetPortrait();
        if (JT_Core.instance.pageId == "1002" || JT_Core.instance.pageId == "1003"){
            watermark1.sprite = bg2;
        }
        else {
            watermark1.sprite = bg1;
        }
        
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork &&  !JT_Core.instance.isReachableViaCarrierDataNetwork) {
			string title = "提示";
			string msg = "您当前设置不允许再移动网络环境下播放视频";
			string OK = "确定";

			new Alert (title, msg)
				.SetPositiveButton (name, () => {
					btnClickeVideo();
				})
				.Show ();
		}else
		{
			PlayVideo();
		}
		
	}

    // private void Update(){
    //     if (MyMediaPlayer!= null && JT_Core.instance!=null && JT_Core.instance.videoBackSceneName != "main") {
	//         if (MyMediaPlayer.Control.IsFinished()){
    //             btnClickeVideo();
    //         }
    //     }
    // }

    void PlayVideo()
	{
		if (MyMediaPlayer !=null && JT_Core.videopath != null){
			//MyMediaPlayer.videoPath = JT_Core.videopath;
			_text.color = Color.yellow;
			_text.text = "视频加载中.....";
			MyMediaPlayer.Events.AddListener(EventHandler);
			
			MyMediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL,JT_Core.videopath,false);
            //Debug.LogError(myMPMP.videoPath);
            //if (!myMPMP.videoPath.Trim().Equals(""))
            //{
            //    _text.color = Color.yellow;
            //    _text.text = "视频加载中.....";

            //    myMPMP.OnPlaybackCompleted += _OnPlaybackCompleted;
            //	myMPMP.OnLoaded += _OnLoaded;
            //	myMPMP.Load();
            //}

        }
	}
	
	private	 void	 EventHandler(MediaPlayer _MediaPlayer,MediaPlayerEvent.EventType eventtype,ErrorCode err){
		switch (eventtype)
		{
			case MediaPlayerEvent.EventType.ReadyToPlay:
			    _text.text = "";
			    MyMediaPlayer.Play();
                totalTime = ConvertToTimeData(MyMediaPlayer.Info.GetDurationMs());
                break;
			case MediaPlayerEvent.EventType.FinishedPlaying:
				_RePlay.gameObject.SetActive(true);
			    btnClickeVideo();
				break;
			default:
				break;
		}
		
	}

    private void Update() {
        if (MyMediaPlayer!= null && MyMediaPlayer.Control!=null && MyMediaPlayer.Control.IsPlaying() ) {
            mSlider.value = MyMediaPlayer.Control.GetCurrentTimeMs() / MyMediaPlayer.Info.GetDurationMs();
        }

        if (MyMediaPlayer && MyMediaPlayer.Info != null && MyMediaPlayer.Info.GetDurationMs() > 0f) {
            float time = MyMediaPlayer.Control.GetCurrentTimeMs();
            float duration = MyMediaPlayer.Info.GetDurationMs();
            float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);
            CurTimer.text = ConvertToTimeData (time) + "/"+ totalTime;
            mSlider.value = d;
        }
    }

    /// <summary>
    /// 1980(豪秒)->00:00
    /// </summary>
    /// <param name="timer"></param>
    private string ConvertToTimeData(float timer) {

        float Second = (timer / 1000);
        //int Second = (int)(timer % 1000);

        string prefix_Minute = "00";
        string prefix_Second = "00";

        int minute = (int)(Second / 60);
        int second = (int)(Second % 60);
        Debug.LogError(minute+"|||"+ second);
        if (minute < 10) {
            prefix_Minute = "0" + minute;
        }
        else {
            prefix_Minute = minute.ToString();
        }

        if (second < 10) {
            prefix_Second = "0" + second;
        }
        else {
            prefix_Second = second.ToString();
        }

        return prefix_Minute + ":" + prefix_Second;
    }



    // private void _OnLoaded(MPMP mpmp)
    // {
    // 	_text.text = "";
    // 	//myMPMP.Play();
    //     //myMPMP.

    // }

    // private void _OnPlaybackCompleted(MPMP mpmp)
    // {
    // 	_RePlay.gameObject.SetActive(true);
    // }

    public void btnClickeIsPlayOrStopVideo()
	{
        //if (myMPMP.IsPlaying())
		//	myMPMP.Pause();
		//else
		//{
		//	_RePlay.gameObject.SetActive(false);
		//	myMPMP.Play();
		//}
	}
	
	public void btnClickeVideo()
	{
        if (JT_Core.instance.videoBackSceneName == "directory_video")
            watermark2.gameObject.SetActive(true);
        _text.color = Color.yellow;
        _text.text ="退出中，请稍候..........";
		if (MyMediaPlayer != null	)
			MyMediaPlayer.Stop();
		Application.LoadLevel(JT_Core.instance.videoBackSceneName);
    }
	
	// This function is called when the MonoBehaviour will be destroyed.
	// protected void OnDestroy()
	// {
	// 	//myMPMP.OnPlaybackCompleted -= _OnPlaybackCompleted;
	// 	//myMPMP.OnLoad -= _OnLoaded;
	// }
}
