//This will activate and deactivate a GameObject when the option it is attached to is in focus.

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class ActivateObject_FocusEffect : MonoBehaviour {

	public GameObject ThisGameObject; //this is the GameObject that will be activated and deactivated


	void FixedUpdate () 
	{
		if (ThisGameObject == null)
		{
			print ("there is no GameObject"); //if you didn't assign a gameobject i will be letting you know :)
			return;
		}
		
		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			ThisGameObject.SetActive(true); //set active to true if in focus
		}
		else
		{
			ThisGameObject.SetActive(false); //set active to false if not in focus
		}
	}
}
