// this script is inherited by the Grid_OptionScript and the RotationalMenu_OptionScript

using UnityEngine;
using System.Collections;

public abstract class OptionScript : MonoBehaviour {

	public bool Focus = false;

	//the rest of the code makes sure that only OptionScript can be attached to a GameObject at a time.
	void Reset() 
	{
		if (GetComponents<OptionScript>().Length > 1) 
		{
			print ("Only One OptionScript can be attached to one Object at a time.");
			Invoke("DestroyThis", 0);
		}
	}
	
	void DestroyThis() {
		DestroyImmediate(this);
	}

}
