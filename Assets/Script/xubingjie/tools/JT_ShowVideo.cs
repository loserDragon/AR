using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;

public class JT_ShowVideo : MonoBehaviour {
	public  MediaPlayer myMediaPlayer ;
	public  string videoname;
	public  string SceneName;

    public Slider _slider;

    private bool _wasPlayingOnScrub;
    private float _setVideoSeekSliderValue;

    void Start () {
		if (!videoname.Equals(""))
		{
			myMediaPlayer.Events.AddListener(EventHandler);
			myMediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder,videoname,false);
		}
	}
	
	private void	EventHandler(MediaPlayer _MediaPlayer,MediaPlayerEvent.EventType eventtype,ErrorCode err){
		switch (eventtype)
		{
			case MediaPlayerEvent.EventType.ReadyToPlay:
				myMediaPlayer.Play();
                break	;
			case MediaPlayerEvent.EventType.FinishedPlaying:
				UnityEngine	.SceneManagement.SceneManager.LoadScene(SceneName);
				break	;
			default:	
				break;
		}
	}

    private void Update() {
        if (myMediaPlayer && myMediaPlayer.Info != null && myMediaPlayer.Info.GetDurationMs() > 0f) {
            float time = myMediaPlayer.Control.GetCurrentTimeMs();
            float duration = myMediaPlayer.Info.GetDurationMs();
            float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);

            // Debug.Log(string.Format("time: {0}, duration: {1}, d: {2}", time, duration, d));
            _setVideoSeekSliderValue = d;
            _slider.value = d;
        }
    }


    public void OnVideoSeekSlider() {
        if (myMediaPlayer && _slider && _slider.value != _setVideoSeekSliderValue) {
            myMediaPlayer.Control.Seek(_slider.value * myMediaPlayer.Info.GetDurationMs());
        }
    }
    public void OnVideoSliderDown() {
        if (myMediaPlayer) {
            _wasPlayingOnScrub = myMediaPlayer.Control.IsPlaying();
            if (_wasPlayingOnScrub) {
                myMediaPlayer.Control.Pause();
            }
            OnVideoSeekSlider();
        }
    }
    public void OnVideoSliderUp() {
        if (myMediaPlayer && _wasPlayingOnScrub) {
            myMediaPlayer.Control.Play();
            _wasPlayingOnScrub = false;
        }
    }
}
