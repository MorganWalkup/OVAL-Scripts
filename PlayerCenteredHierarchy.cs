using UnityEngine;
using System.Collections;


/* This script should be attached to a gameObject used for a UI Hierarchy.
 * The purpose of this script is to control the position and rotation of a player-centered hierarchy due to the variations in height of Oculus users.
 * It also includes the ability to manually update the hierarchy position through a button press in-game, 
 * but this is mostly for debugging and a more elegant solution should be found for the final product.
 */
public class PlayerCenteredHierarchy : MonoBehaviour {
	
	GameObject ovrCameraRig = null;
	public bool includedInPlayerPrefab = false;
	public Transform centerEyeAnchor = null;
	public GameObject playerController = null;
	
	
	public void OnJoinedRoom()
	{
		if (PhotonNetwork.isMasterClient) 
		{
			//Calls SetPlayerController half a second after the scene starts. This gives the center eye anchor time to update
			Invoke("SetPlayerController", 0.5f);
			Invoke("SetHierarchyTransform", 0.7f);
		} 
		else if(!includedInPlayerPrefab)
		{
			ThrowHierarchyToSpace();
			//Deactivate children of the hierarchy object
			foreach(Transform child in transform)
			{
				child.gameObject.SetActive(false);
			}
			//Deactivate this script
			this.enabled = false;
		}
		
	}
	
	
	void Start()
	{
		if (includedInPlayerPrefab) 
		{
			transform.SetParent (playerController.transform);
			Invoke("SetHierarchyTransform", 0.4f);
		}
	}
	
	
	void Update()
	{
		
		//If Fire2 is pressed, and Fire1 is not currently held down
		if(( Input.GetButtonDown("Fire2") && !Input.GetButton("Fire1") ) && (PhotonNetwork.isMasterClient || includedInPlayerPrefab))
		{
			SetHierarchyTransform();
		}
		
		//	if(centerEyeAnchor != null)
		//	{
		LookAtPlayer();
		//	}
	}
	
	//Finds the player controller object and defines some variables based on it
	void SetPlayerController()
	{
		if (playerController == null) 
		{
			playerController = FindObjectOfType<OVALPlayer>().gameObject;
		}
		
		transform.SetParent (playerController.transform);

		ovrCameraRig = playerController.GetComponentInChildren<OVRCameraRig>(true).gameObject;
		
		centerEyeAnchor = ovrCameraRig.transform.Find("TrackingSpace").Find("CenterEyeAnchor");
		
		if (centerEyeAnchor == null)
		{
			Debug.LogError ("No Center Eye Anchor found in scene");
		}
		if (playerController == null)
		{
			Debug.LogError ("No OVRPlayer found in scene");
		}
	}
	
	
	// Use this for initialization
	void SetHierarchyTransform()
	{
		//Sets the y position and rotation of the hierarchy object to the y position of the center eye anchor
		transform.localPosition = Vector3.zero;
		transform.position = centerEyeAnchor.position;
		transform.localEulerAngles = new Vector3(0f, centerEyeAnchor.localEulerAngles.y, 0f);
		
	}
	
	//Throws the hierarchy to a distant location. For use in all clients other than the master.
	void ThrowHierarchyToSpace()
	{
		transform.localPosition = new Vector3(-100f,-100f, -100f);
	}
	
	/* Causes all child objects to face the centerEyeAnchor of the player. 
	 */
	void LookAtPlayer()
	{
		foreach(Transform child in transform)
		{
			child.LookAt(centerEyeAnchor,transform.up);
			child.Rotate(0f,180f,0f, Space.Self);
		}
	}
	
}
