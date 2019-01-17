//allows us to Quit the application

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class Quit_OptionAction : OptionAction_WithButton {

	// Update is called once per frame
	void Update () 
	{
		try
		{
			if (gameObject.GetComponent<OptionScript>().Focus  && Input.GetButtonDown(Button))
			{
				Application.Quit();
			}
		}
		catch(UnityException)
		{
			print ("your 'Button' string value is not correct. Go to Edit -> Project Settings -> Input, then change the value to a correct one");
		}
		
	}
}
