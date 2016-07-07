using UnityEngine;
using System;
using LMWidgets;

//This script works by relating the change in a slider widget to the change in the scale of a gameobject
public class RescaleOnSliderChange : Photon.MonoBehaviour
{
	//The initial scale of the gameobject
	private Vector3 initialScale;
	//Value to rescale the object by
	private float rescaleValue;
	//Value to rescale the object declared in the previous frame
	private float prevRescaleValue;
	//Exponential for rescaleAmount; the ratio of change in slider to change in scale
	public float rescaleMagnitude = 8.0f;
	//The slider widget
	public SliderDemo slider; 
	//Whether or not rescale is allowed. Depends on PhotonNetwork.isMasterClient and status of OnJoinedRoom
	private bool rescaleAllowed = false;
	
	
	//Executes when the player joins the room on the Photon Network
	void OnJoinedRoom()
	{	
		
		if(PhotonNetwork.isMasterClient)
		{
			rescaleAllowed = true;
		}
		
		initialScale = transform.localScale;
		
		prevRescaleValue = 1;
	}

	//Executes when a new player becomes the master client
	void OnMasterClientSwitched()
	{
		if(PhotonNetwork.isMasterClient)
		{
			rescaleAllowed = true;
		}
	}


	//Updates once per frame
	void Update() 
	{ 
		if(rescaleAllowed)
		{
			//.GetSliderFraction() returns a float value between 0.0 and 1.0 based on the "active" portion of the slider
			rescaleValue = slider.GetSliderFraction()+0.5f;

			if(rescaleValue != prevRescaleValue)
			{
				//Call the "Rescale" function on every instance of this object across the network
				photonView.RPC("Rescale", PhotonTargets.All, initialScale, rescaleValue, rescaleMagnitude);
			}		

			prevRescaleValue = rescaleValue;
		}

	}


	//Called when a new user joins an existing room
	void OnPhotonPlayerConnected()
	{
		if(rescaleAllowed)
		{
			//Call the "Rescale" function on every instance of this object across the network
			photonView.RPC("Rescale", PhotonTargets.All, initialScale, rescaleValue, rescaleMagnitude);
		}
	}
	
	
	//Rescales the object across the network
	[PunRPC]
	void Rescale(Vector3 initScale, float rescaleValue, float rescaleMag)
	{
		//This line sets the new scale to be the sum of the initial scale and the slider fraction. 
		//For a game object initialized to a scale of 1, the max scale is 2.
		transform.localScale = new Vector3 (
			(initScale.x * Mathf.Pow((rescaleValue) , rescaleMag) ), 
			(initScale.y * Mathf.Pow((rescaleValue) , rescaleMag) ), 
			(initScale.z * Mathf.Pow((rescaleValue) , rescaleMag) ) 
			);
	}
}

