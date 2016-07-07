using UnityEngine;
using System;
using LMWidgets;

/* This script should be attached to a gameObject
 * It works by relating the changes in an array of slider widgets to changes in the rotation of a gameobject
 * If there is no x,y, or z slider supplied, the x, y, or, z rotation (respectively) of the object stays constant
 */
public class RotateOnSliderChange: Photon.MonoBehaviour
{
	//Holds the x, y, and z rotation sliders
	public SliderDemo[] sliders = new SliderDemo[3];
	
	//The initial rotation of the gameObject
	private float[] initialRotation = new float[3];

	//The rotation of the gameObject in the last frame
	private float[] prevRotation = new float[3];
	
	//The new rotation of the gameObject
	private float[] newRotation = new float[3];

	//Whether or not rotation is allowed by this client
	private bool rotateAllowed = false;
	
	
	//Executes when the player joins the room on the Photon Network
	void OnJoinedRoom()
	{	
		
		if(PhotonNetwork.isMasterClient)
		{
			rotateAllowed = true;
		}
		
		initialRotation [0] = transform.localEulerAngles.x;
		initialRotation [1] = transform.localEulerAngles.y;
		initialRotation [2] = transform.localEulerAngles.z;

		prevRotation[0] = initialRotation[0]; 
		prevRotation[1] = initialRotation[1]; 
		prevRotation[2] = initialRotation[2];
	}


	//Executes when a new player becomes the master client
	void OnMasterClientSwitched()
	{
		if(PhotonNetwork.isMasterClient)
		{
			rotateAllowed = true;
		}
	}

	//Updates once per frame
	void Update()
	{

		if(rotateAllowed)
		{
			//iterates over every item in sliders and newRotation
			for (int index = 0; index < 3; index++) 
			{
				if(sliders[index] != null)
				{
					/* GetSliderFraction() returns a float value between 0.0 and 1.0 based on the "active" portion of the slider
					 * A slider set to max value is 180 degree rotation. A slider set to minimum value is -180 degree rotation.
					 */
					newRotation[index] = ((sliders[index].GetSliderFraction() - 0.5f) * 360) + initialRotation[index];
				}
				else
				{
					newRotation[index] = initialRotation[index];
				}
			}

			if( (newRotation[0] != prevRotation[0]) || (newRotation[1] != prevRotation[1]) || (newRotation[2] != prevRotation[2]) )
			{
				//Call the "Rotate" function on every instance of this object across the network
				photonView.RPC("Rotate", PhotonTargets.All, newRotation[0], newRotation[1], newRotation[2]);
			}

			prevRotation[0] = newRotation[0]; 
			prevRotation[1] = newRotation[1]; 
			prevRotation[2] = newRotation[2];
		}
		
	}


	//Called when a new user joins an existing room
	void OnPhotonPlayerConnected()
	{
		if(rotateAllowed)
		{
			//Call the "Rotate" function on every instance of this object across the network
			photonView.RPC("Rotate", PhotonTargets.All, newRotation[0], newRotation[1], newRotation[2]);
		}
	}


	//Rotates all instances of the object on the network to the new rotation given by the slider positions
	[PunRPC]
	private void Rotate(float x, float y, float z)
	{
		//Sets the rotation of the object to the values stored in newRotation
		transform.localEulerAngles = new Vector3 (x, y, z);
	}
	
}

