//This will change the color of a mesh when the option it is attached to is in focus.

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class ChangeMeshColor_FocusEffect : MonoBehaviour {

	public Renderer ThisRenderer; //this is the mesh renderer that will have it's color changed.

	public float Speed; //the speed at which the color will change

	public Color UnFocusColor; //color when it's not in focus
	public Color FocusColor; //color when it's in focus
	
	
	void FixedUpdate () 
	{
		if (ThisRenderer == null)
		{
			print ("there is no renderer"); //print error
			return;
		}

		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			ThisRenderer.material.color = Color.Lerp (ThisRenderer.material.color,FocusColor,Speed * Time.deltaTime); //if the object is in focus
		}
		else
		{
			ThisRenderer.material.color = Color.Lerp (ThisRenderer.material.color,UnFocusColor,Speed * Time.deltaTime);//if the object is not in focus
		}
	}
}
