using UnityEngine;
using System.Collections;

/* This script controls the audio played when a button is pressed.
 * It should be attached to a button with a ButtonDemoToggle component.
 */
public class ButtonAudio : MonoBehaviour {
	
	//The button
	ButtonDemoToggle button;
	
	//The audio to be played
	AudioClip click;
	
	//the button state from the previous frame
	private bool previousButtonState;
	//the button state from the current frame
	private bool currentButtonState;
	
	
	
	// Use this for initialization
	void Start () {
		button = GetComponent<ButtonDemoToggle> ();
		click = gameObject.GetComponent<AudioSource>().clip;
		previousButtonState = button.ToggleState;
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
		currentButtonState = button.ToggleState;
		
		if (ButtonToggled(previousButtonState, currentButtonState))
		{
			PlaySound();
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
	
	/* Plays an audio clip
	 */
	void PlaySound()
	{
		gameObject.GetComponent<AudioSource>().PlayOneShot (click);
	}
}
