using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
using LitJson;
using com.ootii.Messages;

public class JT_directory : MonoBehaviour {
	public GameObject JoystickImage;
	public PanGestureRecognizer PanGesture { get; private set; }
    public GameObject lableObj; 
	int number = 0;
	void Start () 
	{
        JT_Core.SetPortrait();
		PanGesture = new PanGestureRecognizer
		{
			AllowSimultaneousExecutionWithAllGestures = true,
			PlatformSpecificView = JoystickImage.gameObject,
			ThresholdUnits = 0.0f
            };
		PanGesture.Updated += PanGestureUpdated;
		FingersScript.Instance.AddGesture(PanGesture);
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            BtnClickClose();

        }
    }

    private void PanGestureUpdated(GestureRecognizer gesture, ICollection<GestureTouch> touches)
	{
		if (gesture.State == GestureRecognizerState.Began)
		{
			number = 0;
		}else
			if (gesture.State == GestureRecognizerState.Executing)
			{
				number += 1;
			}else
			{
				if (gesture.State == GestureRecognizerState.Ended)
				{
					if (Mathf.Abs(gesture.FocusX - gesture.StartFocusX) >  Mathf.Abs(gesture.FocusY - gesture.StartFocusY))
					{
						if (gesture.FocusX < gesture.StartFocusX)
							SendMsg((number).ToString(),"left");
						
						if (gesture.FocusX > gesture.StartFocusX)
							SendMsg((number).ToString(),"right");
					}else
					{
						if (gesture.FocusY < gesture.StartFocusY)
							SendMsg((number).ToString(),"down");
						
						if (gesture.FocusY> gesture.StartFocusY)
							SendMsg((number).ToString(),"up");
					}
				}
			}
	}
	
	
	void SendMsg(string number,string direction)
	{
		JT_MOUSE_DIRECTION_MSG _JT_MOUSE_DIRECTION_MSG = new JT_MOUSE_DIRECTION_MSG();
		_JT_MOUSE_DIRECTION_MSG.number = number;
		_JT_MOUSE_DIRECTION_MSG.direction = direction;
		MessageDispatcher.SendMessage(this, "MOUSE_DIRECTION_MSG", _JT_MOUSE_DIRECTION_MSG, EnumMessageDelay.IMMEDIATE);	
	}
	
	public void BtnClickClose()
	{
        lableObj.SetActive(true);
		Application.LoadLevel("main");
	}
}
