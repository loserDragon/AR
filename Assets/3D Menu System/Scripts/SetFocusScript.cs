//Sets the "Focus" variable on objects that contain a script that inherits from the OptionScript.cs
//This is used in the RotationalMenu

using UnityEngine;
using System.Collections;

public class SetFocusScript : MonoBehaviour {

	//when an object enters with this we'll set the focus to true
	public void OnTriggerEnter(Collider ThisCollider) 
	{
		ThisCollider.gameObject.GetComponent<OptionScript>().Focus = true;
	}

	//when an object exits this we'll set the focus to false
	public void OnTriggerExit(Collider ThisCollider) 
	{
		ThisCollider.gameObject.GetComponent<OptionScript>().Focus = false;
	}
}
