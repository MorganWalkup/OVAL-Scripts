using UnityEngine;
using System.Collections;

//This script changes the position and rotation of an attached gameObject when the specified button is toggled
public class PositionToggle : MonoBehaviour {
	
	//The button used to toggle position
	public ButtonDemoToggle widgetButton;
	//The position the attached object should move to.
	public Vector3 finalPosition = new Vector3(0f,0f,0f);
	//The current position of the attached object.
	private Vector3 currentPosition;
	
	//Determines if the button changes rotation of the attached object
	public bool changeRotation = false;
	//The rotation the attached object should turn to
	public Vector3 finalRotation = new Vector3 (0, 0, 0);
	
	
	//the button state from the previous frame
	private bool previousButtonState;
	//the button state from the current frame
	private bool currentButtonState;
	
	public void Start()
	{
		
		//Initializes currentPosition to the position of the gameObject on startup
		currentPosition = gameObject.transform.localPosition;
		
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
			gameObject.transform.localPosition = finalPosition;
			
			if(changeRotation)
			{
				gameObject.transform.localEulerAngles = finalRotation;
			}
			
			currentPosition = gameObject.transform.localPosition;
			
			
		}
		
		//If the gameObject's current position is different from the final position, turn the button off.
		if(currentPosition != finalPosition)
		{
			widgetButton.SetWidgetValue(false);
		}
		
		currentPosition = gameObject.transform.localPosition;
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
}
