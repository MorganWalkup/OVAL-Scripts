using UnityEngine;
using System.Collections;

/* This script is a class declaration for the OVALPlayer prefab so it can easily be found in the scene.
 * It also initializes all of the prefab's components and children when it's first instantiated.
 */
public class OVALPlayer : Photon.MonoBehaviour {
	
	GameObject myAvatarObject;
	Avatar myAvatar;
	public float regroupSpeed = 2.0f;
	
	void OnEnable()
	{	
		if (photonView.isMine) 
		{
			Debug.Log("OVALPlayer initializing");
			//Activate children
			Transform child;
			for (int childIndex = 0; childIndex < gameObject.transform.childCount; childIndex++) {
				child = gameObject.transform.GetChild (childIndex);
				child.gameObject.SetActive (true);
			}
			
			//Activate Components
			gameObject.GetComponent<FlyAround> ().enabled = true;
			gameObject.GetComponent<DisableFlyAround> ().enabled = true;
			gameObject.GetComponent<InactivityTimeOut> ().enabled = true;
			
			//Activate Position Toggles
			PositionToggle[] positionToggles = gameObject.GetComponents<PositionToggle> ();
			foreach (PositionToggle toggle in positionToggles) {
				toggle.enabled = true;
			}
			
			//Set random color components for the laser pointer and indicator
			float r = Random.Range (0f, 1f);
			float g = Random.Range (0f, 1f);
			float b = Random.Range (0f, 1f);
			
			//Activate laser pointer and set color
			GameObject laser = gameObject.transform.FindChild ("LaserPointer").gameObject;
			Color laserColor = new Color (r, g, b, 0.7f);
			laser.GetComponent<LineRenderer> ().SetColors (laserColor, laserColor);
			laser.GetComponent<Laser> ().laserTip.GetComponent<MeshRenderer> ().material.SetColor ("_TintColor", laserColor);
			
			//Instantiate indicator and set color same as laser pointer
			GameObject myIndicatorObject = PhotonNetwork.Instantiate ("Indicator", Vector3.zero, new Quaternion (0f, 0f, 0f, 0f), 0);
			Indicator myIndicator = myIndicatorObject.GetComponent<Indicator> ();
			myIndicator.enabled = true;
			myIndicator.GetComponent<PhotonView> ().RPC ("SetColor", PhotonTargets.AllBuffered, r, g, b);
			//Assigns myIndicatorObject to an important variable in the laser script
			laser.GetComponent<Laser> ().indicatorObject = myIndicatorObject;
			
			//Set avatar color and disable avatar on my view
			myAvatarObject = gameObject.transform.Find ("Avatar").gameObject;
			myAvatar = myAvatarObject.GetComponent<Avatar> ();
			myAvatar.photonView.RPC ("SetColor", PhotonTargets.AllBuffered, r, g, b);
			myAvatar.photonView.RPC ("SetName", PhotonTargets.AllBuffered, ("OVAL " + PhotonNetwork.player.ID));
			myAvatar.Show (false);
			
			//Activate Jets
			gameObject.GetComponentInChildren<Jets>().enabled = true;
			
		}
		
	}
	
	
	/* Called when the master client on the Photon Network leaves the room.
	 * The player who joined immediately after the master client becomes the new master
	 */
	void OnMasterClientSwitched()
	{
		if(photonView.isMine && PhotonNetwork.isMasterClient)
		{
			//Find the master hierarchy in the scene
			GameObject masterHierarchy = GameObject.FindWithTag ("MasterHierarchy");
			//Activate master hierarchy's children on the new master client
			foreach(Transform child in masterHierarchy.transform)
			{
				child.gameObject.SetActive(true);
			}
			//Enable PlayerCenteredHierarchy script and reinitialize it.
			masterHierarchy.GetComponent<PlayerCenteredHierarchy>().enabled = true;
			masterHierarchy.GetComponent<PlayerCenteredHierarchy>().OnJoinedRoom();
		}
		
	}

	[PunRPC]
	void RegroupToMaster()
	{
		Vector3 target = Vector3.zero;

		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
		{
			if (player.GetComponentInChildren<PhotonView>().owner.isMasterClient) 
			{
				target = player.transform.position;
			}
		}

		transform.position = Vector3.Lerp (transform.position, target, regroupSpeed);
	}

}
