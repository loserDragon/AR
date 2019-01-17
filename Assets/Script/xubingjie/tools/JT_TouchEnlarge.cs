using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RotateDir {
    isRotate_YX = 0,
    isRotate_YZ = 1,
}
public class JT_TouchEnlarge : MonoBehaviour {
	private Touch oldTouch1; 
	private Touch oldTouch2; 
	Transform myTransform;

    Quaternion myQuater;
    Quaternion roa ;

    public RotateDir _RotateDir;
    public float minSize = 0.3f;
    public float maxSize = 1f;
    public int factor = 100;

    protected void Awake(){
        myTransform = transform;
        roa = myTransform.localRotation;
    }

	public void SetOriginPos(){
        if (myTransform == null) {
            myTransform = transform;
            roa = myTransform.localRotation;
        }
        myTransform.localRotation = roa ;
		myTransform.localScale = Vector3.one ;
	}
    void Update () {
		if ( Input.touchCount <= 0 ){  
			return;  
		}  

		Touch newTouch1 = Input.GetTouch (0);  
		if(newTouch1.phase == TouchPhase.Began && Input.touchCount == 1){
			oldTouch1 = newTouch1;  
			return;
		}
		if(newTouch1.phase == TouchPhase.Moved&&Input.touchCount == 1){
			float roa_x = newTouch1.position .x - oldTouch1.position.x ;
			float roa_y = newTouch1.position .y - oldTouch1.position.y ;

            if (_RotateDir == RotateDir.isRotate_YX) {
                myTransform.Rotate(roa_y / 50f, -roa_x / 50f,0, Space.World);
            }
            else {
                myTransform.Rotate(0, -roa_x / 50f, -roa_y / 50f, Space.World);
            }
			
            //if (myTransform.localRotation.y < (roa.y- 90)) {
            //    myTransform.localRotation = Quaternion.Euler(myTransform.localRotation.x,0, myTransform.localRotation.z);
            //}
            //if (myTransform.localRotation.y > 90){
            //    myTransform.localRotation = Quaternion.Euler(myTransform.localRotation.x, 180, myTransform.localRotation.z);
            //}
            //if (myTransform.localRotation.z < 0)
            //{
            //    myTransform.localRotation = Quaternion.Euler(myTransform.localRotation.x, myTransform.localRotation.y, 0);
            //}
            //if (myTransform.localRotation.z > 90)
            //{
            //    myTransform.localRotation = Quaternion.Euler(myTransform.localRotation.x, myTransform.localRotation.y, 180);
            //}
        }

        if (Input.touchCount <= 1)
        {
            return;
        }
        Touch newTouch2 = Input.GetTouch (1);  
	

		if( newTouch2.phase == TouchPhase.Began ){  
			oldTouch2 = newTouch2;  
			oldTouch1 = newTouch1;  
			return;  
		}  
		
		float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);  
		float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);  
  
		float offset = newDistance - oldDistance;  
  
		float scaleFactor = offset / factor;  
		Vector3 localScale = myTransform.localScale;  
		Vector3 scale = new Vector3(localScale.x + scaleFactor,  
			localScale.y + scaleFactor,   
			localScale.z + scaleFactor);

        myTransform.localScale = scale;
        if (scale.x > maxSize && scale.y > maxSize && scale.z > maxSize) {
            Debug.LogError("maxSize :" + maxSize);
            myTransform.localScale = new Vector3(maxSize, maxSize, maxSize);
        }
        if (scale.x < minSize && scale.y < minSize && scale.z < minSize) {
            Debug.LogError("minSize :" + minSize);
			myTransform.localScale = new Vector3(minSize, minSize, minSize); 
		}  
  
		oldTouch1 = newTouch1;  
		oldTouch2 = newTouch2;  
	}
}
