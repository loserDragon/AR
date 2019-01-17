using UnityEngine;
using System.Collections;

[RequireComponent (typeof (OptionScript))]
public class ChangeParticleSystemEmit_FocusEffect : MonoBehaviour {

	public ParticleSystem ThisParticleSystem; //Particle we will change

	public float Speed; //Speed to change the color
	
	public float UnFocusEmission; //Emmission when not in focus
	public float FocusEmission; //Emmission when in focus

	void FixedUpdate () 
	{
		if (ThisParticleSystem == null)
		{
			print ("there is no ParticleSystem");//print error
			return;
		}
		
		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			ThisParticleSystem.emissionRate = Mathf.Lerp (ThisParticleSystem.emissionRate,FocusEmission,Speed * Time.deltaTime); //change to focus emissionRate
		}
		else
		{
			ThisParticleSystem.emissionRate = Mathf.Lerp (ThisParticleSystem.emissionRate,UnFocusEmission,Speed * Time.deltaTime); //change to Unfocus emissionRate
		}
	}
}
