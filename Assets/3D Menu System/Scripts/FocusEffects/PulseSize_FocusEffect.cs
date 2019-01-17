//This will pingpong the Size of a GameObject when the option it is attached to is in focus.

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class PulseSize_FocusEffect : MonoBehaviour {

	public Transform ThisTransfrom; //this is the mesh GameObject that will change the size of.

	public float Speed; //the speed at which we will change the size

	public Vector3 UnFocusSize; //Size when it's not in focus
	public Vector3 FocusSize1; //the Sizes we will switch between
	public Vector3 FocusSize2;

	private float MinMaxRatio; //this is the ratio between Size 1 and 2
	private float SwitchTime; //the this object became in focus


	void Update () 
	{
		if (ThisTransfrom == null)
		{
			print ("there is no Transform"); //print error
			return;
		}
		
		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			MinMaxRatio = ((Mathf.Cos((Time.time - SwitchTime) * Speed)/2f)*-1f + 0.5f); //if you want to know more about this line read the readme file
			ThisTransfrom.localScale = Vector3.Lerp (FocusSize1,FocusSize2,MinMaxRatio);  //change Size
		}
		else
		{
			SwitchTime = Time.time;
			ThisTransfrom.localScale = Vector3.Lerp (ThisTransfrom.localScale,UnFocusSize,Speed * Time.deltaTime); //change to Unfocus Size
		}
		
	}
}
