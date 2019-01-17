//This will change the size of a GameObject when the option it is attached to is in focus.

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class ChangeSize_FocusEffect : MonoBehaviour {

	public Transform ThisTransfrom; //GameObject to change the size of

	public float Speed; //speed of size change

	public Vector3 UnFocusSize; //size when not in focus
	public Vector3 FocusSize; //size when in focus
	
	void FixedUpdate () 
	{
		if (ThisTransfrom == null)
		{
			print ("there is no Transform"); //print error
			return;
		}
		
		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			ThisTransfrom.localScale = Vector3.Lerp (ThisTransfrom.localScale,FocusSize,Speed * Time.deltaTime);  //change to focus size
		}
		else
		{
			ThisTransfrom.localScale = Vector3.Lerp (ThisTransfrom.localScale,UnFocusSize,Speed * Time.deltaTime); //change to Unfocus size
		}
	}
}
