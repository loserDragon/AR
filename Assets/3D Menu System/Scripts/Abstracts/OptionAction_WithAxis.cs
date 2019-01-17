//only used for Other Scripts to inhert from

using UnityEngine;
using System.Collections;

public abstract class OptionAction_WithAxis : OptionAction {
	
	//the Name of the Button Input and the Axis Input 
	//(Edit -> Project Settings -> Input)
	public string Button;
	public string Axis;
	
	//audio to play on change
	public AudioClip ActionAudio;
}
