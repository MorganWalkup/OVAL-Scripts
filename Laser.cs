using UnityEngine;
using System.Collections;

/* This script gives users a method of indicating points of interest and measuring distance in the scene.
 * An updating line renderer gives users a "laser pointer" to aim an indicatorObject when "Fire2" is held down.
 * When "Fire2" is released, a small indicatorObject is instantiated across the network at the end of the line renderer.
 * When "Fire1" AND "Fire2" are released, ends of a ruler are instantiated at the end of the line renderer.
 */
[RequireComponent (typeof(LineRenderer))]

public class Laser : Photon.MonoBehaviour {
	
	//The line renderer component
	private LineRenderer lr;
	//The gameObject functioning as the tip of the laser pointer
	public GameObject laserTip;
	//The center eye anchor of the player controller
	public Transform centerEyeAnchor;
	//A ray pointing in the forward direction of the center eye anchor
	private Ray centerEyeForward;
	//The distance of the endpoint of lr down centerEyeForward
	private float lrLength;
	//The endpoint for the line renderer
	private Vector3 lrEndPoint;
	
	//The indicatorObject placed when "Fire2" is released. Assigned through OVALplayer script.
	public GameObject indicatorObject { get; set; }
	//The indicator component of the indicatorObject
	private Indicator indicator;
	//The point where the indicatorObject appears
	public Vector3 indicatorObjectSpot { get; set; }
	
	//A ruler tool. Necessary for this script to send important values to the ruler script.
	public Ruler ruler;
	
	
	// Use this for initialization
	void Start () {
		transform.position = centerEyeAnchor.position;
		transform.parent = centerEyeAnchor;
		
		lr = GetComponent<LineRenderer> ();
		lr.enabled = false;
		
		centerEyeForward = new Ray (centerEyeAnchor.position, centerEyeAnchor.forward);
		indicator = indicatorObject.GetComponent<Indicator>();
	}
	
	
	// Update is called once per frame
	void Update () {
		
		//If "Left Button" has just been pressed
		if (Input.GetButtonDown("Fire1")) 
		{
			laserTip.SetActive(true);
		}
		
		//If "Left Button" is currently held down
		else if	(Input.GetButton("Fire1")) 
		{
			lr.enabled = true;
			
			//Sets the origin of centerEyeForward to the location of centerEyeAnchor
			centerEyeForward.origin = centerEyeAnchor.position;
			//Sets the direction of centerEyeForward to the forward vector of centerEyeAnchor
			centerEyeForward.direction = centerEyeAnchor.forward;
			
			//Change this line to set the default distance of the laser pointer further or closer to the user
			lrLength = 5.0f;

			//Sets the default end point to be "lrLength" units down the centerEyeForward ray.
			lrEndPoint = centerEyeForward.GetPoint(lrLength);
			
			//Sets the first point of lr to be just above and to the right of the center eye anchor. 
			lr.SetPosition (0, (centerEyeAnchor.position + centerEyeAnchor.right + centerEyeAnchor.up));
			
			
			RaycastHit hit;
			
			//Essentially, if centerEyeForward intersects a collider
			if (Physics.Raycast(centerEyeAnchor.position, centerEyeAnchor.forward, out hit)) 
			{
				lr.SetPosition (1, hit.point);
				indicatorObjectSpot = hit.point;
				ruler.intersectingCollider = hit.collider;
				ruler.hitCollider = true;

				if (hit.collider.gameObject.CompareTag ("TextObject")){
					hit.collider.gameObject.GetComponent<TextObject>().hit = true;
					if (hit.collider.gameObject.GetComponentInChildren<SpecificTextFollow>() != null && hit.collider.gameObject.GetComponentInChildren<SpecificTextFollow>().taken == false){
						hit.collider.gameObject.GetComponentInChildren<SpecificTextFollow>().myPlayer = gameObject;
						hit.collider.gameObject.GetComponentInChildren<SpecificTextFollow>().taken = true;
					}
				}
			} 
			//If centerEyeForward does not hit a collider
			else 
			{
				lr.SetPosition (1, lrEndPoint);
				indicatorObjectSpot = lrEndPoint;
				ruler.hitCollider = false;
			}
			
			//Moves the laser tip to indicatorObjectSpot
			laserTip.transform.position = indicatorObjectSpot;
		} 
		
		//If "Fire2" has just been released
		else if (Input.GetButtonUp("Fire1")) 
		{
			laserTip.SetActive (false);
			indicatorObject.transform.position = indicatorObjectSpot;
			indicator.indicatorPlaced = true;
			//Plays audio attached to the indicatorObject over the network
			indicatorObject.GetComponent<PhotonView>().RPC("PlaySound", PhotonTargets.All, null);
			
		}
		
		//If nothing is happening with "Fire2"
		else 
		{
			lr.enabled = false;
		}
		
	}
}

