using UnityEngine;
using System.Collections;

//Attach to the 'scripts' gameobject, not to an OVALPlayer
public class Regroup : Photon.MonoBehaviour {

	public ButtonDemoToggle regroupButton;
	private GameObject[] players;
	public GameObject player1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{

		if (regroupButton.ToggleState) 
		{
			players = GameObject.FindGameObjectsWithTag("Player");

			foreach(GameObject player in players)
			{
				if (!player.GetComponentInChildren<PhotonView>().owner.isMasterClient) 
				{
					player.GetComponentInChildren<PhotonView>().RPC("RegroupToMaster", PhotonTargets.All, null);
				}
			}
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