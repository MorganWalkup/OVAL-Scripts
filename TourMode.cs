using UnityEngine;
using System.Collections;

//This script changes the position and rotation of an attached gameObject when the specified button is toggled
public class TourMode : MonoBehaviour {
	
	//The button used to toggle position
	public ButtonDemoToggle widgetButton;
	//The position the attached object should move to.
	//The current position of the attached object.
	private Vector3 currentPosition;

	//The Lerp speed
	public float speed;
	public float rotateSpeed;

	//the button state from the previous frame
	private bool previousButtonState;
	//the button state from the current frame
	private bool currentButtonState;

	public GameObject point1;
	
	private GameObject target;
	private GameObject looking;
	private bool thing;
	private FlyAround flying;

	private Vector3 test;
	private Quaternion targetlook;
	

	public void Awake()
	{
		point1 = GameObject.FindGameObjectWithTag ("TourStart");
		flying = GetComponent<FlyAround> ();
		target = point1;
		looking = point1.GetComponent<PointChangeTour>().lookAtDestination;

		//Initializes currentPosition to the position of the gameObject on startup
		currentPosition = gameObject.transform.localPosition;
		
		//the .ToggleState method for a button widget returns a boolean value. 
		//"True" if button is on. "False" if button is off.
		previousButtonState = widgetButton.ToggleState;
		

	}
	
	
	//Updates once per frame
	public void Update() 
	{
		//point1 = GameObject.FindGameObjectWithTag ("TourStart");
		currentButtonState = widgetButton.ToggleState;
		
		//Checks if button has been toggled and if the new state is "on"
		if(ButtonToggled(previousButtonState, currentButtonState) && currentButtonState) 
		{
			if (!thing){
				flying.enabled =false;

			point1.SetActive (true);
				looking = point1.GetComponent<PointChangeTour> ().lookAtDestination;
				thing = true;
			}

		
			//widgetButton.SetWidgetValue(false);
		}

		if (ButtonToggled (previousButtonState, currentButtonState) && !currentButtonState) {
			if (thing){
				flying.enabled = true;
			}
			thing = false;

			//target.SetActive (false);
		//widgetButton.SetWidgetValue(false);
		}

		if (thing == true){
			
			transform.position = Vector3.Lerp (transform.position, target.transform.position, speed);

			targetlook = Quaternion.LookRotation(looking.transform.position - transform.position, Vector3.up);
			//transform.LookAt (looking.transform);
			gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, targetlook, Time.deltaTime * rotateSpeed);    
			
		}

		previousButtonState = currentButtonState;
	}


	/* Compares two different toggle states, returns false if they are the same, true if they are different.
	 * prevState: A boolean parameter, filled by "previousButtonState" in this script.
	 * currentState: A boolean parameter, filled by "currentButtonState" in this script.
	 */


	public void OnTriggerEnter (Collider other){
		if (other.gameObject.GetComponent<PointChangeTour> () != null) {
			//other.gameObject.GetComponent<PointChangeTour> ().nextPoint.SetActive (true);
			target = other.gameObject.GetComponent<PointChangeTour> ().nextPoint;
			looking = other.gameObject.GetComponent<PointChangeTour> ().lookAtDestination;
			//if (other.gameObject.CompareTag ("TourStart")){
			//other.gameObject.SetActive (false);
			//}
		}
	}

	public bool ButtonToggled(bool prevState, bool currentState)
	{
		if (currentState == prevState) {
			return false;
		} else {
			return true;
		}
	}
}