using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JT_ChangeTextColor_FocusEffect : MonoBehaviour {
	
	public MeshRenderer _MeshRenderer;
	public Texture2D[]  _Texture2D;
	public UITexture _UITexture;
	public Texture _ResultTexture;
	void FixedUpdate () 
	{
		if (_MeshRenderer == null ||  _Texture2D.Length < 2)
			return;
		
		if (gameObject.GetComponent<OptionScript>().Focus)
		{
			if (_Texture2D[0] != null)
				_MeshRenderer.material.mainTexture = _Texture2D[0];
			
			if (_ResultTexture != null)
				_UITexture.mainTexture = _ResultTexture;
		}
		else
		{
			if (_Texture2D[1] != null)
				_MeshRenderer.material.mainTexture = _Texture2D[1];
		}
	}
}
