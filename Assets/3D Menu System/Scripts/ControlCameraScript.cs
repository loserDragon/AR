//this script will rotate or shift the camera based on the location of the focused option
//not this is only needed for grid menus

using UnityEngine;
using System.Collections;

public class ControlCameraScript : MonoBehaviour {


	public GameObject FocusOption; //Object in focus

	public enum ControlCameraTypes{Shift = 0 , Rotate = 1}; //set dropdown options
	public ControlCameraTypes ControlCameraType =  ControlCameraTypes.Shift; //by default it's shift
	
	public float Speed = 1f; //Speed while moving the camera
	public float Z = -10f; //Distance from Z axis

	private float SwitchTime; //the time when a switch occured

	void Start()
	{
		if (PlayerPrefs.HasKey("Scene" + Application.loadedLevel.ToString() + "FocusOption")) //see if we stored the last option that was in Focus
		{
			string TempString = PlayerPrefs.GetString("Scene" + Application.loadedLevel.ToString() + "FocusOption"); //storing the string

			if (TempString != "")
			{
				FocusOption = GameObject.Find(PlayerPrefs.GetString("Scene" + Application.loadedLevel.ToString() + "FocusOption")).gameObject; //find and assign that option
				FocusOption.GetComponent<Grid_OptionScript>().Focus = true; //make sure that option has focus
			}
		}
	}

	public void SetFocusOption(GameObject NewFocusOption)
	{
		if (NewFocusOption == null) //if the new option is null then end this method
		{
			return;
		}

		FocusOption.GetComponent<Grid_OptionScript>().Focus = false; //turn the current Option to Focus = false

		FocusOption = NewFocusOption; //assign the FocusOption Variable a new Option

		SwitchTime =  Time.time; //record the time

		//set and save
		PlayerPrefs.SetString("Scene" + Application.loadedLevel.ToString() + "FocusOption",FocusOption.name); 
		PlayerPrefs.Save();
	}

	void Update()
	{
		//after a tenth of a second we will make sure the new focus option has Focus set to true
		if (Time.time - SwitchTime >= 0.1f)
		{
			FocusOption.GetComponent<Grid_OptionScript>().Focus = true;
			SwitchTime = 0f;
		}
	}


	void FixedUpdate () 
	{
		if (FocusOption != null) //if we have focus on an option
		{
			if (ControlCameraType == ControlCameraTypes.Shift) //if the camera is set to shift...move the camera to a new location
			{
				Vector3 ThisVector3 = new Vector3(FocusOption.transform.localPosition.x,FocusOption.transform.localPosition.y, Z);
				gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, ThisVector3 , Speed * Time.deltaTime);
			}
			else if (ControlCameraType == ControlCameraTypes.Rotate) //if the camera is set to rotate...rotate the camera to that driection
			{
				Vector3 dir = FocusOption.transform.position - transform.position;
				Quaternion Rotation = Quaternion.LookRotation(dir);
				gameObject.transform.rotation = Quaternion.Lerp (gameObject.transform.rotation,Rotation,Speed * Time.deltaTime);
			}

		}
	}
}
