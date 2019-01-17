//This will change the Color of a TextMesh when the option it is attached to is in focus.

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class ChangeTextColor_FocusEffect : MonoBehaviour {

	public TextMesh ThisTextMesh; //TextMesh to change the color of

	public float Speed; //Speed to change the color

	public Color UnFocusColor; //color when not in focus
	public Color FocusColor; //color when in focus


	void FixedUpdate () 
	{
		if (ThisTextMesh == null)
		{
			print ("there is no TextMesh");//print error
			return;
		}

		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			ThisTextMesh.color = Color.Lerp (ThisTextMesh.color,FocusColor,Speed * Time.deltaTime); //change to focus color
		}
		else
		{
			ThisTextMesh.color = Color.Lerp (ThisTextMesh.color,UnFocusColor,Speed * Time.deltaTime); //change to Unfocus color
		}
	}
}
