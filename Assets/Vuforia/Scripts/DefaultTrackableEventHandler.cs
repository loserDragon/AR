/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using com.ootii.Messages; 
namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }
        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS

        private void OnTrackingFound()
        {

            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);


            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                if (component.transform.parent.name == "Shoupiaoji" && !component.enabled) {
                    component.transform.parent.localPosition = new Vector3(0.044f, 0.255f, 0.018f);
                    component.transform.parent.localRotation = Quaternion.Euler(new Vector3(0f, 0, 0));
                    component.transform.parent.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    if (component.transform.parent.GetComponent<JT_TouchEnlarge>() == null) {
                        JT_TouchEnlarge _JT_TouchEnlarge = component.transform.parent.gameObject.AddComponent<JT_TouchEnlarge>();
                        _JT_TouchEnlarge._RotateDir = RotateDir.isRotate_YX;
                        _JT_TouchEnlarge.minSize = 0.08f;
                        _JT_TouchEnlarge.maxSize = 0.8f;
                        _JT_TouchEnlarge.factor = 800;
                    }
                }
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }
	        MessageDispatcher.SendMessage(this, "ON_TRACKING_FOUND", mTrackableBehaviour.TrackableName, EnumMessageDelay.IMMEDIATE);	
            Debug.LogError("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }
	        MessageDispatcher.SendMessage(this, "ON_TRACKING_LOST", mTrackableBehaviour.TrackableName, EnumMessageDelay.IMMEDIATE);	
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}
