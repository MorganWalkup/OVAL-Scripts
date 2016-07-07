using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevelButtonListener : MonoBehaviour {
	
	public ButtonDemoToggle button;
	public string levelName;
	
	private bool previousButtonState;
	private bool currentButtonState;
	
	
	// Use this for initialization
	void Start () {
		previousButtonState = button.ToggleState;
	}
	
	
	// Update is called once per frame
	void Update () {
		
		currentButtonState = button.ToggleState;
		if(ButtonToggled(previousButtonState, currentButtonState))
		{
			SceneManager.LoadScene (levelName);
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