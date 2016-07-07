using UnityEngine;
using UnityEngine.VR;
using System.Collections;

/*This script uses the Photon Unity Network to establish a room on the network.
 * It also handles initialization and instantiation of the player prefab in the scene.
 */
public class NetworkManager : MonoBehaviour {
	
	//The camera to use before the Player is instantiated
	//public Camera standbyCamera;
	//The player prefab to instantiate
	public GameObject playerPrefab;
	//If set to true, the program runs as expeced, without connecting to the Photon Unity Network
	public bool offlineMode = false;
	//An array of "SpawnSpots". Gameobjects detailing the locations where the Player should spawn.
	SpawnSpot[] spawnSpots = null;
	
	// Use this for initialization
	void Start () {
		//Finds all SpawnSpots in the scene
		spawnSpots = GameObject.FindObjectsOfType(typeof(SpawnSpot)) as SpawnSpot[];
		
		if (offlineMode) {
			PhotonNetwork.offlineMode = true;
			PhotonNetwork.CreateRoom ("null");
		} 
		else 
		{
			Connect ();
		}
		
	}
	
	//Connects to Photon Unity Network
	void Connect () {
		//Connects to all other OVAL instances using the same "PUN Session Name", as defined by the user in the main menu
		PhotonNetwork.ConnectUsingSettings (PlayerPrefs.GetString("PUN Session Name","default"));
	}
	
	void OnGUI(){
		//Prints a string detailing the connection state to the top left of the screen, before the Player is instantiated
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString () );
	}
	
	//Once a Lobby is joined
	void OnJoinedLobby() {
		//Join a random room in the lobby
		PhotonNetwork.JoinRandomRoom ();
	}
	
	//If we fail to join a random room
	void OnPhotonRandomJoinFailed(){
		//Create a room with a name corresponding to the given string (Here, our room has no name)
		PhotonNetwork.CreateRoom( null );
	}
	
	//Once a room is joined
	void OnJoinedRoom() {
		//Activate VR
		VRSettings.enabled = true;
		SpawnMyPlayer ();

	}
	
	
	//Handles the instantiation of the player prefab
	void SpawnMyPlayer() {
		
		//If there are no spawnSpots
		if (spawnSpots.Length == 0) {
			Debug.Log ("There are no spawnspots in the scene");
			//Initialize myPlayer to a new instantiated player prefab
			PhotonNetwork.Instantiate (playerPrefab.name, new Vector3 (0, 0, 0), new Quaternion (), 0);
		} 
		else 
		{
			//Choose a random SpawnSpot
			SpawnSpot mySpawnSpot = spawnSpots [Random.Range (0, spawnSpots.Length)];
			//Initialize myPlayer to a new instantiated player prefab
			PhotonNetwork.Instantiate (playerPrefab.name, mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		}
		
		//Disable standby camera
//		standbyCamera.enabled = false;
		
	}
}
