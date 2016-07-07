using UnityEngine;
using System.Collections;

/* This script should be attached to a game object with a photon view component.
 * It works by toggling a gameobject's mesh renderer and its children's mesh renderers across the network when a button widget is toggled.
 */
public class NetworkVisibilityToggle : Photon.MonoBehaviour {
	
	//The button used to toggle visibility
	public ButtonDemoToggle button;
	
	//Controls what button state matches which visibility state (if inverted = true, then button toggled "on" means object "off").
	public bool inverted = false;
	//Controls the state of the button on startup
	public bool buttonInitiallyOn = false;
	//If true, turns off the mesh renderers of all sibling game objects when this one is turned on. Useful for switching through identical models with different materials or shaders.
	public bool hideSiblings = true;
	//If true, hide colliders on the hidden gameObject
	public bool hideCollider = true;
	
	//If true, visibility can only be toggled by the Master Client of the Photon Network (First user to join the game)
	//If false, button should NOT be part of a player-centered hierarchy
	public bool controlledByHost = false;
	//Whether or not the user can toggle visibility. Based on value of controlledByHost and PhotonNetwork.isMasterClient
	private bool toggleAllowed;
	
	//the button state from the previous frame
	private bool previousButtonState;
	//the button state from the current frame
	private bool currentButtonState;
	
	
	//Executes when the player joins the room on the Photon Network
	public void OnJoinedRoom()
	{	
		toggleAllowed = GetTogglePermission();
		
		if(PhotonNetwork.isMasterClient)
		{
			//The .ToggleState method for a button widget returns a boolean value. 
			//"True" if button is on. "False" if button is off.
			button.ToggleState = buttonInitiallyOn;
			
			//Sets the initial state of the object's Mesh Renderer based on the value of the button
			if (!inverted) {
				//Runs ShowObject(buttonInitiallyOn) on all instantiations of this script across the network
				photonView.RPC ("ShowObject", PhotonTargets.All, button.ToggleState);
			}else{
				photonView.RPC ("ShowObject", PhotonTargets.All, !button.ToggleState);
			}
		}
		
		previousButtonState = button.ToggleState;
		
	}

	//Executes when a new player becomes the master client
	void OnMasterClientSwitched()
	{
		toggleAllowed = GetTogglePermission();
	}
	
	
	//Updates once per frame
	public void Update() 
	{
		currentButtonState = button.ToggleState;
		
		//Checks if user is allowed to toggle, if button has been toggled, and if the new state is "on"
		if(toggleAllowed && ButtonToggled(previousButtonState, currentButtonState) && currentButtonState) 
		{
			//Checks to see if inverted is set to true. If not, enables meshes.
			if(!inverted)
			{
				photonView.RPC ("HideFamily", PhotonTargets.AllBuffered, null);
				photonView.RPC ("ShowObject", PhotonTargets.AllBuffered, true);
			}
			else
			{
				photonView.RPC ("ShowObject", PhotonTargets.AllBuffered, false);
			}
			
		}
		//Checks if user is allowed to toggle, if button has been toggled and if the new state is "off"
		else if(toggleAllowed && ButtonToggled (previousButtonState, currentButtonState) && !currentButtonState)
		{
			//Checks if inverted is set to true. If not, disables meshes.
			if(!inverted)
			{
				photonView.RPC ("ShowObject", PhotonTargets.AllBuffered, false);
			}
			else
			{
				photonView.RPC ("HideFamily", PhotonTargets.AllBuffered, null);
				photonView.RPC ("ShowObject", PhotonTargets.AllBuffered, true);
			}
		}
		
		previousButtonState = currentButtonState;
		
	}
	
	
	/* Enables or disables mesh renderer of the attached object based on value of "show". Then calls the showChildren function.
	 * bool show: Whether or not the method enables the mesh renderer. Renderer gets enabled if "true", disabled if "false".
	 */
	[PunRPC]
	private void ShowObject (bool show)
	{
		if(gameObject.GetComponent<MeshRenderer>() != null)
		{
			gameObject.GetComponent<MeshRenderer>().enabled = show;
		}
		
		if (hideCollider && gameObject.GetComponent<Collider> () != null) 
		{
			gameObject.GetComponent<Collider>().enabled = show;
		}
		
		ShowChildren(gameObject.transform, show);
	}
	
	
	/* Uses recursion to either disable or enable the mesh renderer of every child of the object utilizing this script. 
	 * Comment out this method and calls to it to leave children unaffected.
	 * Transform currentObject: this method alters the children of currentObject
	 * bool show: "true" to enable mesh renderer, "false" to disable mesh renderer
	 */
	[PunRPC]
	private void ShowChildren(Transform currentObject, bool show)
	{
		if (currentObject.childCount > 0) 
		{
			foreach (Transform child in currentObject)
			{
				
				if(child.gameObject.GetComponent<MeshRenderer>() != null)
				{
					child.gameObject.GetComponent<MeshRenderer>().enabled = show;
				}
				
				if(hideCollider && child.gameObject.GetComponent<Collider>() != null)
				{
					child.gameObject.GetComponent<Collider>().enabled = show;
				}
				
				ShowChildren(child, show);
				
			}
			
		}
	}
	
	
	/* Disables the mesh renderer of: the object attached to this script, its children, its siblings, and their children.
	 */
	[PunRPC]
	private void HideFamily()
	{
		if(hideSiblings)
		{
			if(gameObject.transform.parent != null)
			{
				ShowChildren(gameObject.transform.parent, false);
			}
			else
			{
				Debug.LogError ("visibilityToggleButtonListener.cs/ hideSiblings is set to true, but the attached object has no parent.");
			}
		}
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
	
	/* Determines whether or not the user is allowed to toggle visibility
	 */
	private bool GetTogglePermission()
	{
		if (!controlledByHost) 
		{
			return true;
		} 
		else if (controlledByHost && PhotonNetwork.isMasterClient) 
		{
			return true;
		} 
		else 
		{
			return false;
		}
	}
}
