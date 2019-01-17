using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JT_BtnClickPlayOrStop : MonoBehaviour {
	private Image image;
	public  Image otherImage;
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		image = this.GetComponent<Image>();
	}
	public void btnClickeIsPlayOrStopVideo()
	{
		image.enabled = !image.enabled;
		otherImage.enabled = !otherImage.enabled;
	}
}
