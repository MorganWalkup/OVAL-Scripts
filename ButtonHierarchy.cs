using UnityEngine;
using System.Collections;
using LMWidgets;

/*This script does not interact directly with the item it is attached to. Therefore, it can be attached to any CONSTANTLY ACTIVE item in the scene.
* This script works by taking one button widget as a "parent" and other button widgets as "children".
* When "parent" is toggled on, the children buttons will activate and move to their positions as specified in the scene view.
* When "parent" is toggled off, children will deactivate and move back into parent button.
*/
public class ButtonHierarchy : MonoBehaviour {
	
	//Array of children button objects (leap widgets' button prefabs)
	public GameObject[] childButtonObjects;
	//The actual "button". A child of a "child button OBJECT"
	private ButtonDemoToggle childButton;
	//An array of the active child button object positions
	private Vector3[] childPositions;

	//Determines whether or not childButtonObjects[0] is on if all other buttons are off
	public bool firstChildButtonIsDefault = true;

	//If false, child buttons are not collapsible or otherwise controlled by a parent button
	public bool parentButtonExists = true;
	//Parent button object. Toggles enability, visibility, and position of the child button objects.
	public GameObject parentButtonObject;
	//The actual "button" of the parent. A child of the "parent button OBJECT"
	private ButtonDemoToggle parentButton;
	//Parent button position.
	private Vector3 parentPosition;

	//Determines whether or not children buttons turn off when parent button is turned off.
	public bool childrenOffWhenClosed = true;
	//Determines whether or not turning on a child button will disable its siblings.
	public bool disableOtherButtons = true;

	//The movement speed for child buttons
	public float speed = 20;
	private float step;

	//The states of all child buttons in the previous frame
	private bool[] previousChildButtonStates;
	//The states of all child buttons in the current frame
	private bool[] currentChildButtonStates;


	
	//Initializes variables at startup
	public void Start()
	{
		//Initializing variables
		childPositions = new Vector3[childButtonObjects.Length];
		previousChildButtonStates = new bool[childButtonObjects.Length];
		currentChildButtonStates = new bool[childButtonObjects.Length];

		if (parentButtonExists) 
		{
			parentPosition = parentButtonObject.transform.localPosition;
			parentButton = parentButtonObject.GetComponentInChildren<ButtonDemoToggle> ();
		}

		int index = 0;
		
		//Runs through each object in childButtonObjects, initializing important values
		foreach (GameObject buttonObject in childButtonObjects) {

			//childButton is the ACTUAL button in each button object
			childButton = buttonObject.GetComponentInChildren<ButtonDemoToggle>();
			//initializes childButton to off
			childButton.SetWidgetValue(false);
			//Initializes values of childPositions
			childPositions[index].Set(buttonObject.transform.localPosition.x, buttonObject.transform.localPosition.y, buttonObject.transform.localPosition.z);
			//Initializes values of previousChildButtonStates
			previousChildButtonStates[index] = childButton.ToggleState;

			if(parentButtonExists)
			{
				//Moves each buttonObject to the position of the parentButtonObject
				buttonObject.transform.localPosition = parentPosition;
			}
		
			index++;
		}


	}


	//Updates once per frame
	public void Update() 
	{

		//the .ToggleState method for a button widget returns a boolean value. 
		//"True" if button is on. "False" if button is off.

		//If parent button is on or it doesn't exist:
		if(!parentButtonExists || parentButton.ToggleState)
		{
			//Tracks if any child buttons are currently on
			bool noChildButtonsOn = true;

			int index = 0;
			
			foreach (GameObject buttonObject in childButtonObjects)
			{
				ActivateButtonObject(buttonObject, true);

				childButton = buttonObject.GetComponentInChildren<ButtonDemoToggle>();

				//if the buttonObject has not reached its active position
				if (buttonObject.transform.localPosition != childPositions[index])
				{
					if(childButton != null) 
					{
						//Disables the button, to prevent accidental toggling while it's moving.
						childButton.gameObject.SetActive(false);

					}
					//Movement speed for the object
					step = speed * Time.deltaTime;
					//Moves the buttonObject toward its active position
					buttonObject.transform.localPosition = Vector3.MoveTowards(buttonObject.transform.localPosition, childPositions[index], step);
				}
				//when the buttonObject has reached its active position
				else
				{
					//Enables the button for use by player
					childButton.gameObject.SetActive (true);

					//If any button of a buttonObject in the array "childButtonObjects" is on, noChildButtonsOn is set to "false"
					if (childButton.ToggleState)
					{
						noChildButtonsOn = false;
					}

					//If every button has been checked, no child buttons are on, and firstChildButtonIsDefault is set to true: 
					//turn on the button of the first buttonObject in childButtonObjects
					if(index == (childButtonObjects.Length - 1) && noChildButtonsOn && firstChildButtonIsDefault)
					{
						childButtonObjects[0].GetComponentInChildren<ButtonDemoToggle>().SetWidgetValue(true);
					}
					
					currentChildButtonStates[index] = childButton.ToggleState;

					if (ButtonToggled(previousChildButtonStates[index], currentChildButtonStates[index]) && disableOtherButtons)
					{
						DisableOtherButtons(index);
					}
				}
				
				index++;
			}
			
		}

		//If parentButton is off and it exists:
		else
		{
			foreach (GameObject buttonObject in childButtonObjects) 
			{
				//childButton is the ACTUAL button in each buttonObject
				childButton = buttonObject.GetComponentInChildren<ButtonDemoToggle>();
				
				if(childButton != null)
				{
					childButton.gameObject.SetActive(false);

					if(childrenOffWhenClosed)
					{
						childButton.SetWidgetValue(false);
					}
				}

				//if the buttonObject has not reached the parent position
				if (buttonObject.transform.localPosition != parentPosition)
				{
					step = speed * Time.deltaTime;
					//Moves the buttonObject toward the parent position
					buttonObject.transform.localPosition = Vector3.MoveTowards(buttonObject.transform.localPosition, parentPosition, step);
				}
				//when the buttonObject has reached the parent position
				else
				{
					ActivateButtonObject(buttonObject, false);
				}
			}
		}
		
		
	}

	/* Compares two different toggle states, returns false if they are the same, true if they are different.
	 * prevState: A boolean parameter, filled by "previousChildButtonStates[index]" in this script.
	 * currentState: A boolean parameter, filled by "currentChildButtonStates[index]" in this script.
	 */
	public bool ButtonToggled(bool previousButtonState, bool currentButtonState)
	{
		if (previousButtonState != currentButtonState) {
			return true;
		} else {
			return false;
		}
	}

	
	/* Turns off all buttons of buttonObjects in the array "childButtonObjects", except for the button at toggledIndex.
	 * toggledIndex: the index of the buttonObject in the array "childButtonObjects" whose button was toggled.
	 */ 
	public void DisableOtherButtons(int toggledIndex)
	{
		foreach (GameObject buttonObject in childButtonObjects) 
		{
			if (System.Array.IndexOf(childButtonObjects, buttonObject) != toggledIndex)
			{
				childButton = buttonObject.GetComponentInChildren<ButtonDemoToggle>();
				if(childButton != null)
				{
					childButton.SetWidgetValue (false);
				}
			}
		}
	}


	/* Activates or deactivates a button object based on the value of "enable"
	 * enable: a boolean parameter. "True" to activate an object, "false" to deactivate it.
	 */ 
	public void ActivateButtonObject(GameObject buttonObject, bool enable)
	{
		foreach(Transform child in buttonObject.transform)
		{
			child.gameObject.SetActive(enable);
		}
	}
}