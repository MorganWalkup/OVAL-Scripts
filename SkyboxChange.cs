using UnityEngine;
using System.Collections;
using LMWidgets;

//This script changes the skybox when a Leap Motion Button is toggled. 
//Toggles between the scene's original skybox and a predefined alternative.
public class SkyboxChange : Photon.MonoBehaviour {

	//The button used to toggle position
	public ButtonDemoToggle button;
	//the button state from the previous frame
	private bool previousButtonState;
	//the button state from the current frame
	private bool currentButtonState;
	//The original skybox of the scene
	Material originalSkybox;
	//The alternative skybox
	public Material skybox;


	//Use this for initialization
	public void Start(){

		originalSkybox = RenderSettings.skybox;

		//the .ToggleState method for a button widget returns a boolean value. 
		//"True" if button is on. "False" if button is off.
		previousButtonState = button.ToggleState;

	}


	//Updates once per frame
	public void Update() 
	{

		currentButtonState = button.ToggleState;

		//Checks if button has been toggled and if the new state is "on"
		if (PhotonNetwork.isMasterClient && ButtonToggled(previousButtonState, currentButtonState)) {

			photonView.RPC("SwitchSkybox", PhotonTargets.AllBuffered);

		}

		previousButtonState = currentButtonState;
	}


	/* Compares two different toggle states, returns false if they are the same, true if they are different.
	 * prevState: A boolean parameter, filled by "previousButtonState" in this script.
	 * currentState: A boolean parameter, filled by "currentButtonState" in this script.
	 */
	public bool ButtonToggled(bool prevState, bool currentState)	
	{
		if (currentState == prevState) {
			return false;
		} else {
			return true;
		}
	}


	/* When called as an RPC function, switches between the default and optional skyboxes
	 */
	[PunRPC]
	public void SwitchSkybox()
	{
		if(RenderSettings.skybox == originalSkybox)
			RenderSettings.skybox = skybox;
		else if (RenderSettings.skybox == skybox)
			RenderSettings.skybox = originalSkybox;
		else
			Debug.LogError("Skybox not recognized");
	}
}
