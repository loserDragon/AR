//This will change the Rotation of a GameObject when the option it is attached to is in focus.

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class ChangeRotation_FocusEffect : MonoBehaviour {

	public Transform ThisTransfrom; //GameObject to Rotate

	public float Speed; //speed of rotation

	public Vector3 UnFocusRotation; //rotation when not in focus
	public Vector3 FocusRotation; //rotation when in focus
	

	void FixedUpdate () 
	{
		if (ThisTransfrom == null)
		{
			print ("there is no Transform"); //print error
			return;
		}
		
		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			ThisTransfrom.localRotation = Quaternion.Lerp (ThisTransfrom.localRotation,Quaternion.Euler(FocusRotation),Speed * Time.deltaTime); //change to focus rotation
		}
		else
		{
			ThisTransfrom.localRotation = Quaternion.Lerp (ThisTransfrom.localRotation,Quaternion.Euler(UnFocusRotation),Speed * Time.deltaTime); //change to unfocus rotation
		}
	}
}
