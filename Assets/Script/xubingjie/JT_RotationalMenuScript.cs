using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
using com.ootii.Messages;

public class JT_RotationalMenuScript : MonoBehaviour {
	private List<GameObject> ObjectList = new List<GameObject>(); //a list for all object in that should be in the in the RotationalMenu
	private List<GameObject> TransformList = new List<GameObject>(); //a list of emtpy gameobjects to mark locations around the RotationalMenu
	
	private GameObject EmptyGameObjects; //this is gameobject will store other gameobjects we will make
	
	public float Speed = 1f; //Speed of things GameObjects Moving
	public float Radius = 1f; //Radius of the RotationalMenu
	
	public bool ContinuouslyRefresh = true; //this should be set to true ...unless you want to save some CPU power
	
	private float AnglePiece = 0f; //the angle between one Option and another
	
	public int RotationIndex = 0;  //determines what position to rotate the RotationalMenu to
	
	public string Axis = "Horizontal"; //the input axis that will be used to rotate the options
	public bool vertical = false; //Vertical or Horizontal
	int newRotationIndex = 0;
	// Awake is called when the script instance is being loaded.
	
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		MessageDispatcher.AddListener ("MOUSE_DIRECTION_MSG", OnReceiveMsg);
	}
	
	void Start () 
	{
		//set the EmptyGameObject variable
		foreach (Transform child  in transform)
		{
			if (child.gameObject.tag == "EmptyGameObjects")
			{
				EmptyGameObjects = child.gameObject; 
			}
		}
		
		if (PlayerPrefs.HasKey("Scene" + Application.loadedLevel.ToString() + "FocusOption")) //see if we stored the last RotationIndex that was in Focus
		{
			newRotationIndex = RotationIndex = PlayerPrefs.GetInt("Scene" + Application.loadedLevel.ToString() + "FocusOption"); //assign that RotationIndex
		}
		
		RefreshList(); //sets both list
		AnglePiece = 360f/ObjectList.Count; //calculates the Angle between pieces
		UpdateTransformList(); //updates the list of the TransfromList
	}
	
	void Update()
	{
		if (ContinuouslyRefresh)//refreshes both list and the Angle between objects
		{
			RefreshList();
			AnglePiece = 360f/ObjectList.Count;
			UpdateTransformList();
		}
		
		if (RotationIndex == newRotationIndex)
			return;
		
		if (RotationIndex > newRotationIndex) //rotates the RotationalMenu
		{
			RotationIndex = RotationIndex - 1;
			
			//set and save RotationIndex
			PlayerPrefs.SetInt("Scene" + Application.loadedLevel.ToString() + "FocusOption",RotationIndex);
			PlayerPrefs.Save();
		}
		
		if (RotationIndex < newRotationIndex) //rotates the RotationalMenu
		{
			RotationIndex = RotationIndex + 1;
			
			//set and save RotationIndex
			PlayerPrefs.SetInt("Scene" + Application.loadedLevel.ToString() + "FocusOption",RotationIndex);
			PlayerPrefs.Save();
		}
		
		Speed = Mathf.Abs(Speed); //don't let speed be negative
	}
	

	void OnReceiveMsg (IMessage rMessage)
	{
		if (rMessage == null)
			return;
		
		JT_MOUSE_DIRECTION_MSG _JT_MOUSE_DIRECTION_MSG = ((JT_MOUSE_DIRECTION_MSG)rMessage.Data);
		if (_JT_MOUSE_DIRECTION_MSG.direction.Equals("left"))
		{
			newRotationIndex = RotationIndex + 1;
		}
		
		if (_JT_MOUSE_DIRECTION_MSG.direction.Equals("right"))
		{
			newRotationIndex = RotationIndex - 1;
		}
		
		if (_JT_MOUSE_DIRECTION_MSG.direction.Equals("down"))
		{
			newRotationIndex = RotationIndex + int.Parse(_JT_MOUSE_DIRECTION_MSG.number);
		}
		
		if (_JT_MOUSE_DIRECTION_MSG.direction.Equals("up"))
		{
			newRotationIndex = RotationIndex - int.Parse(_JT_MOUSE_DIRECTION_MSG.number);
		}
	}
	
	void FixedUpdate()
	{
		//move each object to the one with the same index number in the TransformList
		for (int i = 0; i < ObjectList.Count; i ++)
		{
			try
			{
				ObjectList[i].transform.localPosition = Vector3.Lerp (ObjectList[i].transform.localPosition,TransformList[i].transform.localPosition, Speed * Time.deltaTime);
			}
			catch(MissingReferenceException)
			{
				//this error could happen when an object gets destroyed
			}
		}
		
		//rotate the RotationalMenu to the correct correct RotationIndex 
		if (vertical)
		{
			gameObject.transform.localRotation = Quaternion.Lerp (gameObject.transform.localRotation,Quaternion.Euler( new Vector3( (RotationIndex * (AnglePiece/2f)) - ((AnglePiece/4f) * (ObjectList.Count % 2f)),gameObject.transform.localRotation.y ,gameObject.transform.localRotation.z)),Speed * Time.deltaTime);
		}
		else
		{
			gameObject.transform.localRotation = Quaternion.Lerp (gameObject.transform.localRotation,Quaternion.Euler( new Vector3(gameObject.transform.localRotation.x,  (RotationIndex * (AnglePiece/2f)) - ((AnglePiece/4f) * (ObjectList.Count % 2f)) ,gameObject.transform.localRotation.z)),Speed * Time.deltaTime);
		}
		
	}
	
	//this will be used when we refresh the TransfromList
	public void DeleteAllInTransformList()
	{
		//Destory all objects from the TransfromList
		for (int i = TransformList.Count - 1; i >= 0 ; i--)
		{
			DestroyImmediate(TransformList[i]);
		}
	}
	
	//refreshes both list ...needed after adding and removing objects from the RotationalMenu
	public void RefreshList()
	{
		ObjectList.Clear();
		DeleteAllInTransformList();
		TransformList.Clear();
		
		//create an empty gameobject to mark every location arround the center 
		foreach (Transform child  in transform)
		{
			if (child.gameObject.tag != "EmptyGameObjects")
			{
				ObjectList.Add (child.gameObject);
				
				GameObject ThisTransfrom = new GameObject();
				
				ThisTransfrom.transform.parent = EmptyGameObjects.transform; //make this empty a child of the EmptyGameObjects object
				TransformList.Add (ThisTransfrom); //add the object to TransformList
				
			}
		}
	}
	
	//updates the TransformList
	public void UpdateTransformList()
	{
		for (int i = 0; i < TransformList.Count; i ++)
		{
			//place each object in the center of the RotationalMenu
			TransformList[i].transform.localPosition = gameObject.transform.localPosition;
			TransformList[i].transform.localRotation = gameObject.transform.localRotation;
			TransformList[i].transform.localScale = gameObject.transform.localScale;
			
			//rotate it to the correct Angle
			if (vertical)
			{
				TransformList[i].transform.localRotation = Quaternion.Euler( new Vector3(i * AnglePiece,TransformList[i].transform.localRotation.y,TransformList[i].transform.localRotation.z));
			}
			else
			{
				TransformList[i].transform.localRotation = Quaternion.Euler( new Vector3(TransformList[i].transform.localRotation.x, i * AnglePiece ,TransformList[i].transform.localRotation.z));
			}
			
			
			//move it out by the correct radius
			TransformList[i].transform.localPosition = TransformList[i].transform.forward * Radius;
		}
	}
	
	//adds new object to the RotationalMenu
	public void AddNewObject(GameObject GO)
	{
		GameObject NewGameObject = Instantiate(GO,gameObject.transform.localPosition,gameObject.transform.localRotation) as GameObject;
		NewGameObject.transform.parent = gameObject.transform; //place it in center to start
		NewGameObject.GetComponent<OptionScript>().Focus = false; 
	}
	
	//removes an object from the RotationalMenu
	public void RemoveObject(GameObject GO)
	{
		DestroyImmediate(GO);
	}
	
	protected void OnDestroy()
	{
		MessageDispatcher.RemoveListener ("MOUSE_DIRECTION_MSG", OnReceiveMsg);
	}
}
