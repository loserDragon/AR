//Allows you to change a value when an Option is in focus

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class ChangeValueInt_OptionAction : OptionAction_WithAxis {

	public TextMesh ValueText; //text to be displayed
	public int Value; //the value we'll be changing
	public int minValue; //max value
	public int maxValue; //min value
	

	void Start()
	{
		if (PlayerPrefs.HasKey (gameObject.name)) //see if we saved this value
		{
			Value = PlayerPrefs.GetInt(gameObject.name); //assign the value to the variable we created for it
		}

		ValueText.text = Value.ToString();
		
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
						Value = Value + 1;
						
						//set and save
						PlayerPrefs.SetInt(gameObject.name,Value);
						PlayerPrefs.Save();
						
						GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ActionAudio);
						
						ValueText.text = Value.ToString();
					}
					
					
					//if one of the Axis was pressed we'll change the value down one
					if (Input.GetButtonDown(Axis) && Input.GetAxis(Axis) < 0f)
					{
						Value = Value - 1;
						
						//set and save
						PlayerPrefs.SetInt(gameObject.name,Value);
						PlayerPrefs.Save();
						
						GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ActionAudio);
						
						ValueText.text = Value.ToString();
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
						Value = Value + 1;
						
						//set and save
						PlayerPrefs.SetInt(gameObject.name,Value);
						PlayerPrefs.Save();
						
						GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ActionAudio);
						
						ValueText.text = Value.ToString();
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
