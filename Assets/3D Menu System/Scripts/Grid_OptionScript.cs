//this is the Grid_OptionScript and it inherits from OptionScript

using UnityEngine;
using System.Collections;

public class Grid_OptionScript : OptionScript {

	//directions of other options
	public GameObject Up;
	public GameObject Down;
	public GameObject Left;
	public GameObject Right;

	//allows to detect other options near by and assign them to the preoper variable above
	public bool AutoDetect = true;

	private ControlCameraScript CCS; //the Script attached to the camera

	public AudioClip ChangeAudio; //when the user changes options
	public AudioClip ErrorAudio; //when the user fails at changing options

	//on start find the ControlCameraScript and detect other options
	void Start()
	{
		CCS =  GameObject.Find("Main Camera").GetComponent<ControlCameraScript>();
		AutoDetectOptions();
	}
	
	void Update()
	{
		if (Focus) //this will only be done if the this option is in focus
		{
			if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0f) //if Up is pressed
			{
				CCS.SetFocusOption(Up);
				PlayAudio(Up);

				return;
			}

			if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") < 0f) //if Down is pressed
			{
				CCS.SetFocusOption(Down);
				PlayAudio(Down);
				
				return;
			}

			if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") < 0f) //if Left is pressed
			{
				CCS.SetFocusOption(Left);
				PlayAudio(Left);
				
				return;
			}

			if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") > 0f) //if Right is pressed
			{
				CCS.SetFocusOption(Right);
				PlayAudio(Right);
				
				return;
			}

		}
	}

	public void PlayAudio(GameObject ThisOption) //plays the correct audio
	{
		if (ThisOption != null)
		{
			GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ChangeAudio);
		}
		else
		{
			GetComponent<AudioSource>().GetComponent<AudioSource>().PlayOneShot(ErrorAudio);
		}
	}

	public void AutoDetectOptions()
	{
		if (AutoDetect) //if AutoDetect is true...run the set of the method
		{
			RaycastHit RayCastUp;
			RaycastHit RayCastDown;
			RaycastHit RayCastLeft;
			RaycastHit RayCastRight;
			
			//find the Up option
			if (Physics.Raycast(transform.position, new Vector3(0f,1f,0f),out RayCastUp))
			{
				if (RayCastUp.collider.gameObject.tag == "Option")
				{
					Up = RayCastUp.collider.gameObject;
				}
			}
			
			//find the Down option
			if (Physics.Raycast(transform.position, new Vector3(0f,-1f,0f),out RayCastDown))
			{
				if (RayCastDown.collider.gameObject.tag == "Option")
				{
					Down = RayCastDown.collider.gameObject;
				}
			}
			
			//find the Left option
			if (Physics.Raycast(transform.position, new Vector3(-1f,0f,0f),out RayCastLeft))
			{
				if (RayCastLeft.collider.gameObject.tag == "Option")
				{
					Left = RayCastLeft.collider.gameObject;
                }
            }
            
            
			//find the Right option
            if (Physics.Raycast(transform.position, new Vector3(1f,0f,0f),out RayCastRight))
            {
                if (RayCastRight.collider.gameObject.tag == "Option")
                {
                    Right = RayCastRight.collider.gameObject;
                }
            }
        }
    }
}
