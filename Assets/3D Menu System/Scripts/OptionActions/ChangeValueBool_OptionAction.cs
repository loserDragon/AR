//Allows you to change a value when an Option is in focus

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class ChangeValueBool_OptionAction : OptionAction_WithAxis {

	public TextMesh ValueText; //text to be displayed
	public bool Value; //the value we are changing
	

	void Start()
	{
		if (PlayerPrefs.HasKey (gameObject.name)) //see if we saved this value
		{
			//since playerpref does not store bools we are just storing it as a string
			ValueText.text = PlayerPrefs.GetString(gameObject.name); 

			if (ValueText.text == "On") //adjust the value based on the value of the string
			{
				Value = true;
			}
			else
			{
				Value = false;
			}

		}
		else
		{
			ValueText.text = "On"; 
		}

	}

	void Update () 
	{
		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			//if Axis is not blank
			if (Axis != "")
			{
				try
				{
					//if one of the Axis was pressed we'll change the value up one
					if (Input.GetButtonDown(Axis) && Input.GetAxis(Axis) > 0f)
					{
						Value = !Value;
						
						if (Value)
						{
							ValueText.text = "On";
						}
						else
						{
							ValueText.text = "Off";
						}
						
						//set and save
						PlayerPrefs.SetString(gameObject.name,ValueText.text);
						PlayerPrefs.Save();
						
						//play audio
						GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ActionAudio);
					}
					
					
					//if one of the Axis was pressed we'll change the value down one
					if (Input.GetButtonDown(Axis) && Input.GetAxis(Axis) < 0f)
					{
						Value = !Value;
						
						if (Value)
						{
							ValueText.text = "On";
						}
						else
						{
							ValueText.text = "Off";
						}
						
						//set and save
						PlayerPrefs.SetString(gameObject.name,ValueText.text);
						PlayerPrefs.Save();
						
						//play audio
						GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ActionAudio);
					}
				}
				catch(UnityException)
				{
					print ("your 'Axis' string value is not correct. Go to Edit -> Project Settings -> Input, then change the value to a correct one");
				}
			}
			
			//if Button is not blank
			if (Button != "")
			{
				try
				{
					if (Input.GetButtonDown(Button))
					{
						Value = !Value;
						
						if (Value)
						{
							ValueText.text = "On";
						}
						else
						{
							ValueText.text = "Off";
						}
						
						//set and save
						PlayerPrefs.SetString(gameObject.name,ValueText.text);
						PlayerPrefs.Save();
						
						//play audio
						GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ActionAudio);
					}
				}
				catch(UnityException)
				{
					print ("your 'Button' string value is not correct. Go to Edit -> Project Settings -> Input, then change the value to a correct one");
				}
			}

		}
	}
}
