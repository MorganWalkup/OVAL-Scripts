using UnityEngine;
using System.Collections;

/* This script controls a "bubble" that contains models and allows users to manipulate them using Unity physics and Leap Motion hands.
 * It should be attached as a component to an independent "bubble" gameObject.
 * It also relies on a model container object with sphere collider, rotateOnSliderChange, and rigidbody components.
 */
public class BubbleManager : MonoBehaviour {
	
	//The size below which bubble manager triggers
	public float sizeToActivate = 0.9f;
	//Creates space between outside of bubble and model edge
	float cushion = 1.25f;
	//The bubble collider
	SphereCollider sphereColl;
	//Mesh Renderer of the bubble. Should most likely be using an XRay shader.
	MeshRenderer meshRend;
	
	//The model container
	public GameObject modelContainer;
	//The RigidBody of the model container
	Rigidbody rigBody;
	//Sphere collider of the model container object
	SphereCollider containerColl;
	//The rotation script attached to the model container object. This must be deactivated for the bubble to move freely.
	RotateOnSliderChange rotationScript;
	
	//The imported model
	GameObject model;
	//All mesh renderers of the imported model
	MeshRenderer[] meshRends;
	
	
	// Use this for initialization
	void Start () {
		sphereColl = GetComponent<SphereCollider>();
		sphereColl.enabled = false;
		meshRend = GetComponent<MeshRenderer>();
		rigBody = modelContainer.GetComponent<Rigidbody>();
		rigBody.useGravity = false;
		rigBody.isKinematic = true;
		containerColl = modelContainer.GetComponent<SphereCollider>();
		rotationScript = modelContainer.GetComponent<RotateOnSliderChange>();
	}
	
	
	//Update is called once per frame
	void Update () {
		//=============================================
		//Activating/deactivating based on model size:
		if(transform.localScale.x <= sizeToActivate)
		{
			rotationScript.enabled = false;
			meshRend.enabled = true;
			rigBody.isKinematic = false;
		}
		else
		{
			rigBody.isKinematic = true;
			meshRend.enabled = false;
			rotationScript.enabled = true;
		}
		//============================================
		
		//============================================
		//Resizing based on model:		
		model = GameObject.FindGameObjectWithTag ("ImportedModel");
		//Maximum extents of the imported model bounds
		float xMax = 0, 
		yMax = 0, 
		zMax = 0;
		//Minimum extents of the imported model bounds
		float xMin = 0,
		yMin = 0,
		zMin = 0;
		//The bound with greatest length (x, y, or z)
		float maxBound;
		//Coordinates for center of bubble/model
		float x1 = 0, 
		y1 = 0, 
		z1 = 0;
		//Length of the bounding box along each axis
		float x, y, z;
		
		if (model != null) {
			meshRends = model.GetComponentsInChildren<MeshRenderer>();
			
			for (int i = 0; i <= meshRends.Length-1; i++){
				
				//=======================================
				//Getting minimum and maximum points on the mesh along each axis
				if (i == 0){
					xMin = meshRends[i].bounds.min.x;
					yMin = meshRends[i].bounds.min.y;
					zMin = meshRends[i].bounds.min.z;
					xMax = meshRends[i].bounds.max.x;
					yMax = meshRends[i].bounds.max.y;
					zMax = meshRends[i].bounds.max.z;
				}
				if (meshRends[i].bounds.min.x <= xMin){
					xMin = meshRends[i].bounds.min.x;
				}
				if (meshRends[i].bounds.min.y <= yMin){
					yMin = meshRends[i].bounds.min.y;
				}
				if (meshRends[i].bounds.min.z <= zMin){
					zMin = meshRends[i].bounds.min.z;
				}
				if (meshRends[i].bounds.max.x >= xMax){
					xMax = meshRends[i].bounds.max.x;
				}
				if (meshRends[i].bounds.max.y >= yMax){
					yMax = meshRends[i].bounds.max.y;
				}
				if (meshRends[i].bounds.max.z >= zMax){
					zMax = meshRends[i].bounds.max.z;
				}
				//=======================================
				
				//Getting data to find the average center
				x1 += meshRends[i].bounds.center.x;
				y1 += meshRends[i].bounds.center.y;
				z1 += meshRends[i].bounds.center.z;
			}
			
			//Set the bubble's position to the average of all Mesh Renderer centers in the imported object
			if(meshRends.Length != 0)
				transform.position = new Vector3 (x1/meshRends.Length, y1/meshRends.Length, z1/meshRends.Length);
			
			//Size of the bounding box along each axis
			x = xMax - xMin;
			y = yMax - yMin;
			z = zMax - zMin;
			
			if(x > y && x > z)
				maxBound = x;
			else if (y > x && y > z)
				maxBound = y;
			else
				maxBound = z;	
			
			//Set the bubble's size to the max length of the bounding box, plus cushion
			transform.localScale = new Vector3(maxBound, maxBound, maxBound)*cushion;
			//Sets the model container's collider to be the appropriate size
			containerColl.radius = maxBound*cushion/(modelContainer.transform.localScale.x*2.0f);
		}
		//==========================================
		
	}
}
