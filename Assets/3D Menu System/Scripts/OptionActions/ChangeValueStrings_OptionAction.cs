//Allows you to change a value when an Option is in focus

using UnityEngine;
using System.Collections;
using System.Collections.Generic; //allows us to make a list

[RequireComponent (typeof (OptionScript))]
public class ChangeValueStrings_OptionAction : OptionAction_WithAxis {

	public TextMesh ValueText; //text to be displayed
	public List<string> OptionList = new List<string>(); //list of possible choices
	public int Value; //Index Value of Item in the List
	

	void Start()
	{
		if (PlayerPrefs.HasKey (gameObject.name)) //see if we saved this value
		{
			Value = PlayerPrefs.GetInt(gameObject.name); //place the int in the value variable
			ValueText.text = OptionList[Mathf.Abs(Value) % OptionList.Count]; //find the text that matches up with that index value
			//note: this uses the % operator
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
						Value = Value + 1;
						
						//set and save
						PlayerPrefs.SetInt(gameObject.name,Value);
						PlayerPrefs.Save();
						
						GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ActionAudio);
						
						ValueText.text = OptionList[Mathf.Abs(Value) % OptionList.Count];//find the text that matches up with that index value
					}


					//if one of the Axis was pressed we'll change the value down one
					if (Input.GetButtonDown(Axis) && Input.GetAxis(Axis) < 0f)
					{
						Value = Value - 1;
						
						//set and save
						PlayerPrefs.SetInt(gameObject.name,Value);
						PlayerPrefs.Save();
						
						GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ActionAudio);
						
						ValueText.text = OptionList[Mathf.Abs(Value) % OptionList.Count];//find the text that matches up with that index value
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
						
						ValueText.text = OptionList[Mathf.Abs(Value) % OptionList.Count];//find the text that matches up with that index value
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
