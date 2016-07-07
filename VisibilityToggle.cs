using UnityEngine;
using System;
using LMWidgets;

/* This script should be attached to a game object.
 * It works by enabling a gameobject's mesh renderer and its children's mesh renderers when a button widget is toggled to "on".
 */
public class VisibilityToggle : MonoBehaviour
{
	
	//The button used to toggle visibility
	public ButtonDemoToggle widgetButton;
	
	//Controls what button state matches which visibility state (if inverted = true, then button toggled "on" means object "off").
	public bool inverted = false;
	//Controls the state of the widgetButton on startup
	public bool buttonInitiallyOn;
	//If true, turns off the mesh renderers of all sibling game objects when this one is turned on.
	public bool siblingsHide = true;
	
	//the button state from the previous frame
	private bool previousButtonState;
	//the button state from the current frame
	private bool currentButtonState;
	
	public void Start()
	{
		
		//Sets the initial state of the object's Mesh Renderer based on the value of "buttonInitiallyOn"
		if (!inverted) {
			ShowObject (buttonInitiallyOn);
		}else{
			ShowObject (!buttonInitiallyOn);
		}
		
		//Sets the initial state of the button to the value of "buttonInitiallyOn"
		widgetButton.SetWidgetValue (buttonInitiallyOn);
		
		//the .ToggleState method for a button widget returns a boolean value. 
		//"True" if button is on. "False" if button is off.
		previousButtonState = widgetButton.ToggleState;
		
	}
	
	
	//Updates once per frame
	public void Update() 
	{
		currentButtonState = widgetButton.ToggleState;
		
		//Checks if button has been toggled and if the new state is "on"
		if(ButtonToggled(previousButtonState, currentButtonState) && currentButtonState) 
		{
			//Checks to see if inverted is set to true. If not, enables meshes.
			if(!inverted)
			{
				HideFamily();
				ShowObject (true);
			}
			else
			{
				ShowObject (false);
			}
			
		}
		//Checks if button has been toggled and if the new state is "off"
		else if(ButtonToggled (previousButtonState, currentButtonState) && !currentButtonState)
		{
			//Checks if inverted is set to true. If not, disables meshes.
			if(!inverted)
			{
				ShowObject (false);
			}
			else
			{
				HideFamily();
				ShowObject (true);
			}
		}
		
		previousButtonState = currentButtonState;
		
	}
	
	
	/* Enables or disables mesh renderer of the attached object based on value of "show". Then calls the showChildren function.
	 * bool show: Whether or not the method enables the mesh renderer. Renderer gets enabled if "true", disabled if "false".
	 */
	private void ShowObject (bool show)
	{
		if(gameObject.GetComponent<MeshRenderer>() != null)
		{
			gameObject.GetComponent<MeshRenderer>().enabled = show;
		}
		
		ShowChildren (gameObject, show);
	}
	
	
	/* Uses recursion to either disable or enable the mesh renderer of every child of the object utilizing this script. 
	 * Comment out this method and calls to it to leave children unaffected.
	 * GameObject currentObject: this method alters the children of currentObject
	 * bool show: "true" to enable mesh renderer, "false" to disable mesh renderer
	 */
	private void ShowChildren(GameObject currentObject, bool show)
	{
		if (currentObject.transform.childCount > 0) 
		{
			foreach (Transform child in currentObject.transform)
			{
				
				if(child.gameObject.GetComponent<MeshRenderer>() != null)
				{
					child.gameObject.GetComponent<MeshRenderer>().enabled = show;
				}
				
				ShowChildren(child.gameObject, show);
				
			}
			
		}
	}
	
	
	/* Disables the mesh renderer of: the object attached to this script, its children, its siblings, and their children.
	 */
	private void HideFamily()
	{
		if(siblingsHide)
		{
			if(gameObject.transform.parent != null)
			{
				ShowChildren(gameObject.transform.parent.gameObject, false);
			}
			else
			{
				Debug.LogError ("visibilityToggleButtonListener.cs/ siblingsTurnOff is set to true, but the attached object has no parent.");
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
	
	
	
}


