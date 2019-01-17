using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class ChangeLightIntensity_FocusEffect : MonoBehaviour {
	
	public Light ThisLight; //Particle we will change
	
	public float Speed; //Speed to change the color
	
	public float UnFocusIntensity; //Emmission when not in focus
	public float FocusIntensity; //Emmission when in focus
	
	void FixedUpdate () 
	{
		if (ThisLight == null)
		{
			print ("there is no Light");//print error
			return;
		}
		
		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			ThisLight.intensity = Mathf.Lerp (ThisLight.intensity,FocusIntensity,Speed * Time.deltaTime); //change to focus emissionRate
		}
		else
		{
			ThisLight.intensity = Mathf.Lerp (ThisLight.intensity,UnFocusIntensity,Speed * Time.deltaTime); //change to Unfocus emissionRate
		}
	}
}
