//This will pingpong the Color of a Mesh when the option it is attached to is in focus.

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class PulseMeshColor_FocusEffect : MonoBehaviour {

	public Renderer ThisRenderer; //this is the mesh renderer that will have it's color changed.

	public float Speed;//the speed at which the color will change

	public Color UnFocusColor; //color when it's not in focus
	public Color FocusColor1; //the colors we will switch between
	public Color FocusColor2; 

	private float MinMaxRatio; //this is the ratio between color 1 and 2
	private float FocusTime; //the this object wecame in focus

	// Update is called once per frame
	void FixedUpdate () 
	{
		if (ThisRenderer == null)
		{
			print ("there is no renderer"); //print error
			return;
		}

		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			MinMaxRatio = ((Mathf.Cos((Time.time - FocusTime) * Speed)/2f)*-1f + 0.5f); //if you want to know more about this line read the readme file
			ThisRenderer.material.color = Color.Lerp (FocusColor1,FocusColor2,MinMaxRatio); //change color
		}
		else
		{
			FocusTime = Time.time;
			ThisRenderer.material.color = Color.Lerp (ThisRenderer.material.color,UnFocusColor,Speed * Time.deltaTime); //change to Unfocus color
		}
	
	}
}
