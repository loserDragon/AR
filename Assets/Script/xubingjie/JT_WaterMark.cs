using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class JT_WaterMark : MonoBehaviour {

	public Vector2[]  Pos;
	int num = 0;
	void Start () {
		
        if (JT_Core.instance.pageId == "1002" || JT_Core.instance.pageId == "1003")
        {
            gameObject.SetActive(false);
        }
        else
        {
            Invoke("Timer", 0f);
        }
	}
	
	void Timer() {
		if (Pos.Length > 0)
		{
			num =  (num >= 3) ? 0 : num+1;
			transform.GetComponent<RectTransform>().anchoredPosition = Pos[num];
		}
		Invoke("Timer", Random.Range(0,10));
	}
}
