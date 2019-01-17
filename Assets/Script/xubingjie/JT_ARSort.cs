using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JT_ARSort : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int num = 1;  float P_X = 0f;
		foreach (Transform child in gameObject.transform)  
		{
			if (num%2 == 0) //双数
			{
				child.localPosition = new Vector3(P_X,0,5000);
			}else//单数
			{
				child.localPosition = new Vector3(P_X,0,5500);
			}
			P_X += 100;  num ++;
		}
	}
}
