//allows us to goto a Scene after a set period of time
//this is great if you want to animate something first :)

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class GoToScene_WDelay_OptionAction : OptionAction_WithButton {

	public int SceneNumber = 0; //the scene number we'll go to
    public float Delay = 0.25f; //the delay after the button press

    private float PressTime; //the time the button was pressed
	private bool ActionTriggered = false; //weather or not the Action has been triggered yet
	

	// Update is called once per frame
	void Update () 
	{
		try
		{
			if (gameObject.GetComponent<OptionScript>().Focus  && Input.GetButtonDown(Button) )
			{
				//set PressTime and Action Trigger
	            PressTime = Time.time;
				ActionTriggered = true;

				//play audio
				GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ActionAudio);

			}
		}
		catch(UnityException)
		{
			print ("your 'Button' string value is not correct. Go to Edit -> Project Settings -> Input, then change the value to a correct one");
		}

		//wait for the delay
		if (Time.time - PressTime >= Delay && ActionTriggered)
        {
            Application.LoadLevel (SceneNumber); //change scenes
        }
	}
}
