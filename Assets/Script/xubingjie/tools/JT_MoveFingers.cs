using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JT_MoveFingers : MonoBehaviour {
	
	public static int number = 0;
	public GameObject Piaoka;
	public JT_Object[] _obj;
	
	void Start () 
	{
		MeshCollider _MeshCollider = gameObject.GetComponent<MeshCollider>();
		_MeshCollider =  (_MeshCollider == null) ? gameObject.AddComponent<MeshCollider>() : _MeshCollider;
		_MeshCollider.convex = true;
		Piaoka.transform.localPosition = new Vector3(1000,1000,1000);
		
	}
	
	void Update () {
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo))
			{
				if (hitInfo.collider.transform == transform)
				{
					if (hitInfo.collider.gameObject==Piaoka)
					{
						Piaoka.transform.localPosition = new Vector3(1000,1000,1000);
						number = 0;
						for (int i=0;i<_obj.Length;i++)
						{
							_obj[i].trage.SetActive(true);
						}
					}else
						StartCoroutine(OnMouseDownMove());
				}
			}
		}
	}
	
	IEnumerator OnMouseDownMove()  
	{  
		Vector3 ScreenSpace = Camera.main.WorldToScreenPoint(transform.position);  
		Vector3 offset = transform.position-Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,ScreenSpace.z));  
		while(Input.GetMouseButton(0))  
		{  	
			Vector3 curScreenSpace =  new Vector3(Input.mousePosition.x,Input.mousePosition.y,ScreenSpace.z);     
			Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace)+offset;          
			transform.position = CurPosition;  
			yield return new WaitForFixedUpdate();  
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.name.Equals("Tiger"))
		{
			number ++;
			gameObject.SetActive(false);
			if (number == 3)
			{
				transform.localPosition = _obj[int.Parse(transform.name)].oldTransform;
				Piaoka.transform.localPosition = new Vector3(-0.47f,0.743f,0.012f);
			}else
				transform.localPosition = _obj[int.Parse(transform.name)].oldTransform;
		}
	}
}



[System.Serializable]
public class JT_Object
{
	public GameObject trage;
	public Vector3 oldTransform;     
}

