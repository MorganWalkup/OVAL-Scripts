using UnityEngine;
using System;
using LMWidgets;

/* This script should be attached to a gameObject with a light component
 * It works by relating the changes in a slider widget to changes in the intensity of a light gameobject
 */

[RequireComponent (typeof(Light))]

public class DimOnSliderChange: Photon.MonoBehaviour
{
	//Holds the slider
	public SliderDemo slider;
	//The initial intensity of the light gameObject
	private float initialIntensity = 0;
	//The light component of the gameobject
	private Light lightComponent;
	//Whether or not this client is allowed to dim the lights
	private bool dimAllowed;

	private float sliderValue;

	private float prevSliderValue;


	//Executes when the player joins the room on the Photon Network
	void OnJoinedRoom()
	{	
		
		if(PhotonNetwork.isMasterClient)
		{
			dimAllowed = true;
		}
		
		lightComponent = gameObject.GetComponent<Light>();
		initialIntensity = lightComponent.intensity;

		prevSliderValue = slider.GetSliderFraction();
	}

	//Executes when a new player becomes the master client
	void OnMasterClientSwitched()
	{
		if(PhotonNetwork.isMasterClient)
		{
			dimAllowed = true;
		}
	}

	//Called when a new user joins an existing room
	void OnPhotonPlayerConnected()
	{
		if(dimAllowed)
		{
			//Call the "Rescale" function on every instance of this object across the network
			photonView.RPC("Dim", PhotonTargets.All, slider.GetSliderFraction());
		}
	}

	//Updates once per frame
	void Update()
	{
		if(dimAllowed) 
		{
			sliderValue = slider.GetSliderFraction();

			if(sliderValue != prevSliderValue) 
			{
				//Sets the intensity of the light based on the value of the slider
				photonView.RPC("Dim", PhotonTargets.All, sliderValue);
			}

			prevSliderValue = sliderValue;
		}
	}

	/* GetSliderFraction() returns a float value between 0.0 and 1.0 based on the "active" portion of the slider
	* A slider set to max value is 1.5 times initial intensity. A slider set to minimum value is .5 times initial intensity.
	*/
	[PunRPC]
	void Dim(float sliderValue)
	{
		lightComponent.intensity = initialIntensity * (Mathf.Pow((sliderValue+0.5f) , 3.0f));
	}
	
}