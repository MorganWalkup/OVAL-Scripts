using UnityEngine;
using System.Collections;

public class MoveOnAfterButton : MonoBehaviour {

	public TutorialManager tutorialManager;
	// The button to toggle
	public ButtonDemoToggle button;
	// The unique tag name of a button not in the scene at start
	public string buttonTagName;
	// If true, moves to the nest TutorialStep when button toggled on. If false, moves on when button toggled off
	public bool triggerWhenOn = true;
	// The button state from the previous frame
	bool previousButtonState;
	// The button state from the current frame
	bool currentButtonState;


	// Use to initialize
	void Start() {

		if(!button)
		{
			button = GameObject.FindWithTag(buttonTagName).GetComponent<ButtonDemoToggle>();
		}

	}

	// When the script becomes enabled and active
	void OnEnable() {

		previousButtonState = button.ToggleState;

	}

	// Updates every frame
	void Update() {

		currentButtonState = button.ToggleState;

		if(triggerWhenOn && ButtonToggled(previousButtonState, currentButtonState) && button.ToggleState)
		{
			tutorialManager.moveOn = true;
		}
		else if(!triggerWhenOn && ButtonToggled(previousButtonState, currentButtonState) && !button.ToggleState)
		{
			tutorialManager.moveOn = true;
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
