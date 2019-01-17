//allows us to go back to a screen by hitting the button

using UnityEngine;
using System.Collections;

public class BackToSceneMenu : MonoBehaviour {

	public int SceneNum = 0;//the SceneNumber
	public string Button; //the input button that will trigger the event

	void Update () 
	{
		if (Input.GetButtonDown(Button)) // if Left Alt is pressed we'll load the Previous Scene
		{
			Application.LoadLevel (SceneNum); 
		}
	}
}
