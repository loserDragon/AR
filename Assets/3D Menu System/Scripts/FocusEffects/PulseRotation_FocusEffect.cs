//This will pingpong the Rotation of a GameObject when the option it is attached to is in focus.

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class PulseRotation_FocusEffect : MonoBehaviour {

	public Transform ThisTransfrom; //this is the mesh GameObject that will Rotate.

	public float Speed; //the speed at which we will rotate it

	public Vector3 UnFocusRotation; //rotation when it's not in focus
	public Vector3 FocusRotation1; //the rotation we will switch between
	public Vector3 FocusRotation2;

	private float MinMaxRatio; //this is the ratio between rotation 1 and 2
	private float FocusTime; //the this object became in focus


	void Update () 
	{
		if (ThisTransfrom == null)
		{
			print ("there is no Transform"); //print error
			return;
		}

		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			MinMaxRatio = ((Mathf.Cos((Time.time - FocusTime) * Speed)/2f)*-1f + 0.5f); //if you want to know more about this line read the readme file
			ThisTransfrom.localRotation = Quaternion.Lerp (Quaternion.Euler(FocusRotation1),Quaternion.Euler(FocusRotation2),MinMaxRatio); //change rotation
		}
		else
		{
			FocusTime = Time.time;
			ThisTransfrom.localRotation = Quaternion.Lerp (ThisTransfrom.localRotation,Quaternion.Euler(UnFocusRotation),Speed * Time.deltaTime); //change to Unfocus rotation
		}
		
	}
}
