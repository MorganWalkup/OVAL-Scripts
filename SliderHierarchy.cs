using UnityEngine;
using System.Collections;
using LMWidgets;

/*This script does not interact directly with the item it is attached to. Therefore, it can be attached to any CONSTANTLY ACTIVE item in the scene.
* This script works by taking one button widget as a "parent" and sliders as "children".
* When "parent" is toggled on, the children sliders will activate and move to their positions as specified in the scene view.
* When "parent" is toggled off, children sliders will deactivate and move back into parent button.
*/
public class SliderHierarchy : MonoBehaviour {
	
	//An array of child sliders
	public GameObject[] childSliderObjects;
	//The slider top. A child of "childSliderObject"
	private BoxCollider childSliderTop;
	//An array of the active child slider positions
	private Vector3[] childPositions;
	
	//Parent button object. Toggles enability, visibility, and position of the child sliders.
	public GameObject parentButtonObject;
	//The actual "button" of the parent. A child of the "parent button OBJECT"
	private ButtonDemoToggle parentButton;
	//Parent button position.
	private Vector3 parentPosition;

	//The movement speed for child slider objects
	public float speed = 20;
	private float step;
	
	
	
	// Use this for initialization
	void Start () 
	{
		
		childPositions = new Vector3[childSliderObjects.Length];
		parentPosition = parentButtonObject.transform.localPosition;
		parentButton = parentButtonObject.GetComponentInChildren<ButtonDemoToggle>();
		
		int index = 0;
		
		//Initializes the childPositions array and moves the childSliders to the parent position.
		foreach (GameObject sliderObject in childSliderObjects) 
		{
			childPositions[index].Set (sliderObject.transform.localPosition.x, sliderObject.transform.localPosition.y, sliderObject.transform.localPosition.z);
			sliderObject.transform.localPosition = parentPosition;
			index++;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If parent button is on
		if (parentButton.ToggleState) 
		{
			int index = 0;
			
			foreach (GameObject sliderObject in childSliderObjects) 
			{
				sliderObject.SetActive (true);
				
				childSliderTop = sliderObject.GetComponentInChildren<BoxCollider>();
				
				//If the child slider has not reached its active position
				if (sliderObject.transform.localPosition != childPositions [index]) 
				{
					if(childSliderTop != null)
					{
						childSliderTop.enabled = false;
					}
					
					step = speed * Time.deltaTime;
					//Move the child slider toward its active position
					sliderObject.transform.localPosition = Vector3.MoveTowards (sliderObject.transform.localPosition, childPositions [index], step);
				} 
				//If the child slider has reached its active position
				else
				{
					if(childSliderTop != null)
					{
						//Enable interaction with the child slider
						childSliderTop.enabled = true;
					}
				}
				
				index++;
			}
		} 
		//If parent button is off
		else 
		{
			foreach (GameObject sliderObject in childSliderObjects)
			{
				
				childSliderTop = sliderObject.GetComponentInChildren<BoxCollider>();
				if(childSliderTop != null)
				{
					//Disable interaction with the child slider
					childSliderTop.enabled = false;
				}
				
				//If the child slider has not reached the parent position
				if (sliderObject.transform.localPosition != parentPosition)
				{
					step = speed * Time.deltaTime;
					//Move the child slider toward the parent position
					sliderObject.transform.localPosition = Vector3.MoveTowards(sliderObject.transform.localPosition, parentPosition, step);
				}
				//If the child slider has reached the parent position
				else
				{
					//Deactivate the child slider
					sliderObject.SetActive (false);
				}
			}
		}
	}
}
