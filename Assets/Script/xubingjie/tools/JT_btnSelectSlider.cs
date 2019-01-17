using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JT_btnSelectSlider : MonoBehaviour {

	public UILabel X;
	public UILabel Y;
	public UILabel Z;
	
	float m_P = 0.01f;
	float m_R = 360f;
	float m_S = 0.1f;
	
	public void btnSelectSlider_P(float value)
	{
		string _jiaodu = (value * m_P).ToString();
		if (Y != null)
			Y.text = _jiaodu;
	}
	
	public void btnSelectSlider_R(float value)
	{
		string _jiaodu = (value * m_R).ToString();
		if (Y != null)
			Y.text = _jiaodu;
	}
	
	public void btnSelectSlider_S(float value)
	{
			string _jiaodu = (value * m_S).ToString();
			if (X != null)
				X.text = _jiaodu;
	}
	
	public void btnSelectSlider_S2(float value)
	{
		string _jiaodu = (value * m_S).ToString();
		if (Y != null)
			Y.text = _jiaodu;
		if (Z != null)
			Z.text = _jiaodu;
	}
	
	// This function is called when the object becomes enabled and active.
	protected void OnEnable()
	{
		Transform _obj = gameObject.transform.Find("ControlHorizontalSlider");
		if (_obj != null)
		{
			UISlider  _UISlider = _obj.gameObject.GetComponent<UISlider>();
			if (_UISlider != null)
			{
				string type = gameObject.name;
				switch(type)
				{
				case "P":
					_UISlider.value = float.Parse(Y.text) / m_P;	
					break;
					
				case "R":
					_UISlider.value = float.Parse(Y.text) / m_R;	
					break;
					
				case "S":
					_UISlider.value = float.Parse(X.text) / m_S;	
					break;
					
				case "S2":
					_UISlider.value = float.Parse(Z.text) / m_S;	
					break;
				}
			}
		}
	}
}
