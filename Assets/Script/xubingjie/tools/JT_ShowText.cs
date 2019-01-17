using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JT_ShowText : MonoBehaviour {

	
	public RectTransform UIContainer;
	public RectTransform UITextFront;
	public RectTransform TooltipCanvas;
	
	void Update () {
		if (UIContainer != null  && UITextFront!=null  &&  TooltipCanvas != null )
			TooltipCanvas.sizeDelta = UIContainer.sizeDelta = new Vector2(UITextFront.sizeDelta.x,UITextFront.sizeDelta.y);
	}
}
