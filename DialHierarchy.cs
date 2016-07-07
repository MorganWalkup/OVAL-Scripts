using UnityEngine;
using System.Collections;
using LMWidgets;

/*This script does not interact directly with the item it is attached to. Therefore, it can be attached to any CONSTANTLY ACTIVE item in the scene.
* This script works by taking one button widget as a "parent" and a dial as a "child".
* When "parent" is toggled on, the child dial will activate and move to their positions as specified in the scene view.
* When "parent" is toggled off, the child dial will deactivate and move back into parent button.
*/
public class DialHierarchy : MonoBehaviour {
	
	//A "DemoDial" prefab gameObject
	public GameObject dialObject;
	//An array of all the box colliders in the dial prefab
	private BoxCollider[] boxColliders;
	//The mesh collider of the dial's cylinder
	private MeshCollider meshCollider;
	//An array of the active dial positions
	private Vector3 dialPosition;
	
	//Parent button object. Toggles enability, visibility, and position of the dial.
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
		boxColliders = dialObject.GetComponentsInChildren<BoxCollider>();
		meshCollider = dialObject.GetComponentInChildren<MeshCollider>();
		dialPosition = dialObject.transform.localPosition;
		parentPosition = parentButtonObject.transform.localPosition;
		parentButton = parentButtonObject.GetComponentInChildren<ButtonDemoToggle>();
	}
	
	// Update is called once per frame
	void Update () 
	{		
		//If parent button is on
		if (parentButton.ToggleState) 
		{	
			dialObject.SetActive (true);
			
			//If the dial has not reached its active position
			if (dialObject.transform.localPosition != dialPosition) 
			{
				if(boxColliders != null)
				{
				/*	foreach(BoxCollider collider in boxColliders)
					{
						//collider.enabled = false;
					} */
				}
				if(meshCollider != null)
				{
					meshCollider.enabled = false;
				}
				
				step = speed * Time.deltaTime;
				//Move the dial toward its active position
				dialObject.transform.localPosition = Vector3.MoveTowards (dialObject.transform.localPosition, dialPosition, step);
			} 
			//If the dial has reached its active position
			else
			{
				//Enable interaction with the dial
				if(boxColliders != null)
				{
				/*	foreach(BoxCollider collider in boxColliders)
					{
						//collider.enabled = true;
					} */
				}
				if(meshCollider != null)
				{
					meshCollider.enabled = true;
				}
				
			}
			
		} 
		//If parent button is off
		else 
		{		
			//Disable interaction with the dial
			if(boxColliders != null)
			{
			/*	foreach(BoxCollider collider in boxColliders)
				{
					//collider.enabled = false;
				} */
			}
			if(meshCollider != null)
			{
				meshCollider.enabled = false;
			}
			
			//If the dial has not reached the parent position
			if (dialObject.transform.localPosition != parentPosition)
			{
				step = speed * Time.deltaTime;
				//Move the dial toward the parent position
				dialObject.transform.localPosition = Vector3.MoveTowards(dialObject.transform.localPosition, parentPosition, step);
			}
			//If the dial has reached the parent position
			else
			{
				//Deactivate the dial
				dialObject.SetActive (false);
			}
		}
	}
}
