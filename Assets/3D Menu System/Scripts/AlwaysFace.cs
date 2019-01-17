//Rotates a Object to look at the "Target" Object

using UnityEngine;
using System.Collections;

public class AlwaysFace : MonoBehaviour {


	public GameObject Target; //object this object will face towards
	public float Speed; //speed at whitch object will turn

	//allways us to freeze rotation on a specific Axis
	public bool FreezeX = false; 
	public bool FreezeY = false;
	public bool FreezeZ = false;
	

	// turn towards target on start
	void Start() 
	{
		//get direction
		Vector3 dir = Target.transform.position - transform.position;
		Quaternion Rotation = Quaternion.LookRotation(dir);

		//freeze axis
		if (FreezeX)
		{
			Rotation.x = 0f;
		}
		if (FreezeY)
		{
			Rotation.y = 0f;
		}
		if (FreezeZ)
		{
			Rotation.z = 0f;
		}

		gameObject.transform.rotation = Rotation; //rotate object
	}
	
	// turn towards target
	void FixedUpdate () 
	{
		//get direction
		Vector3 dir = Target.transform.position - transform.position;
		Quaternion Rotation = Quaternion.LookRotation(dir);

		//freeze axis
		if (FreezeX)
		{
			Rotation.x = 0f;
		}
		if (FreezeY)
		{
			Rotation.y = 0f;
		}
		if (FreezeZ)
		{
			Rotation.z = 0f;
		}

		gameObject.transform.rotation = Quaternion.Lerp (gameObject.transform.rotation,Rotation,Speed * Time.deltaTime); //rotate object at the correct speed

	}
}
