/* This script disables the FlyAround component of the masterClient OVALPlayer by widget button
* Attach this script to the OVAL3 UI
*/

using UnityEngine;
using System.Collections;

public class DisableFlyAround : MonoBehaviour {

	//The controlling button widget
	public ButtonDemoToggle button;
	FlyAround flyAround;

	bool previousButtonState;
	bool currentButtonState;

	// Use this for initialization
	void Start () {
		//Get FlyAround component of the master OVALPlayer
		flyAround = transform.root.GetComponent<FlyAround>();
		//Get initial condition of the button
		previousButtonState = button.ToggleState;
	}

	// Update is called once per frame
	void Update () {

		currentButtonState = button.ToggleState;

		//If the FlyARound component was not found
		if(!flyAround)
		{
			flyAround = transform.root.GetComponent<FlyAround>();
		}
		//If the FlyAround component was found, the button was toggled, and the button is on
		else if(ButtonToggled(previousButtonState, currentButtonState) && button.ToggleState)
		{
			flyAround.enabled = false;
		}
		//If the FlyAround component was found, the button was toggled, and the button is off
		else if(ButtonToggled(previousButtonState, currentButtonState) && !button.ToggleState)
		{
			flyAround.enabled = true;
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
}
