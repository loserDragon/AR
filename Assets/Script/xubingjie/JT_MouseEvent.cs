using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;
using Vuforia;

public class JT_MouseEvent : MonoBehaviour {

	void Start () {
		MessageDispatcher.AddListener("ON_MESSAGE_OnSingleTapConfirmed", OnSingleTapConfirmed);
	}
	
	void OnSingleTapConfirmed(IMessage rMessage)
	{	
		JT_3DText m_3DText = Pick3DText(Input.mousePosition);


		if (m_3DText !=null)
		{
			m_3DText.ShowVideo();
		}else
		{
			CameraDevice.Instance.SetFocusMode(    
				CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		}
	}
	
	private JT_3DText Pick3DText(Vector3 screenPoint)
	{
		GameObject go = VuforiaManager.Instance.ARCameraTransform.gameObject;
		Camera[] cam = go.GetComponentsInChildren<Camera> ();
		Ray ray = cam[0].ScreenPointToRay(screenPoint);
		
		RaycastHit hit = new RaycastHit();
		JT_3DText[] _3DTexts = FindObjectsOfType<JT_3DText>();


		foreach (JT_3DText _3DText in _3DTexts){
			if (_3DText.GetComponent<Collider>().Raycast(ray, out hit, 100))
			{
				return _3DText;
			}
		}
		return null;
	}
	
	protected void OnDestroy()
	{
		MessageDispatcher.RemoveListener("ON_MESSAGE_OnSingleTapConfirmed", OnSingleTapConfirmed);
	}
}
