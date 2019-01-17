using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JT_Shouciyanpiaoji : MonoBehaviour {

	// Use this for initialization
	public Texture[] _Texture;
	public Renderer _Renderer;
	void Move0 () {
		_Renderer.material.mainTexture = _Texture[0];
	}
	
	void Move1 () {
		_Renderer.material.mainTexture = _Texture[1];
	}
	
	void Move2 () {
		_Renderer.material.mainTexture = _Texture[2];
	}
}
