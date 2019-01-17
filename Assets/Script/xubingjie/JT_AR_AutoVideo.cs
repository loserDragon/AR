using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class JT_AR_AutoVideo : MonoBehaviour {
	public  MediaPlayer myMediaPlayer;
	public  static string  videoPath="";
	// Use this for initialization
	void Start () {
		PlayVideo();
	}
	
	public void PlayVideo()
	{
		if (myMediaPlayer !=null && videoPath != null	){
			myMediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL,videoPath,true);
			
			//myMediaPlayer.videoPath = videoPath;
			//if (!myMediaPlayer.videoPath.Trim().Equals(""))
			//{
			//	myMediaPlayer.OnPlaybackCompleted += _OnPlaybackCompleted;
			//	myMediaPlayer.OnLoaded += _OnLoaded;
			//	myMediaPlayer.Load();
			//}
		}
	}
	
	//private void _OnPlaybackCompleted(MPMP mpmp)
	//{
	//	myMediaPlayer.Load();
	//}
	
	//private void _OnLoaded(MPMP mpmp)
	//{
	//	myMediaPlayer.Play();
	//}
}
