/* This script customizes a LeapMotion Dial Widget to function as a file browser for 3D models. 
 * Additionally, it controls a coroutine to display a loading screen while new models are being imported.
 */
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LMWidgets;

public class DialFileBrowser : Photon.MonoBehaviour {

	//The starting folder for the filebrowser
	public string initialFolderPath = "c:/";
	//The dial graphics component of a dial prefab
	public DialGraphics dialGraphics;
	//The dial mode base component of a dial prefab
	public DialModeBase dialBase;
	//A button prefab
	public ButtonDemoToggle button;
	//The textmesh component of a 3D text object. Displays the name of the current folder
	public TextMesh currentDirectoryLabel;
	//Passes the file to the correct importer script
	public ImportManager importManager;
	//A list of the files and folders contained in the current folder
	List<FileSystemInfo> folderContents = new List<FileSystemInfo>();
	//A list of the names of the files and folders in the current folder
	List<string> fileAndFolderNames = new List<string>();
	//The string value of the selected dial label. Also the name of the file/folder
	string selectedName;
	//The index of selectedName within the fileAndFolderNames list.
	int selectedIndex;
	//The FULL name (directory path) of the selected file/folder
	FileSystemInfo selectedContent;
	//The folder currently being accessed by the filebrowser
	DirectoryInfo currentDirectory;
	//Whether or not to update the browser this frame
	public bool updateBrowser;
	//Button state from the previous frame
	bool previousButtonState;
	//Button state from the current frame
	bool currentButtonState;

	//A GameObject to be displayed as a loading screen
	public GameObject loadingScreen;
	//Don't know what this is
//	public GameObject particles;
	//The "ballthing" of the OVALPlayer
	GameObject ship;
	//The LeapOVRCameraRig of the OVALPlayer
	GameObject ovrCamera;
	//The HeadMountedHandController of the OVALPlayer
	GameObject handController;
	//Set to true when TurnStuffOff() is called. 
	bool off;
	//The container object for the imported model
	public GameObject container;
	//Tracks whether or not the model has changed (i.e. a new model has been imported)
	GameObject reference;



	// Use this for initialization
	void Start () {
		currentDirectory = new DirectoryInfo(initialFolderPath);
		currentDirectoryLabel.text = currentDirectory.Name;
		button.ToggleState = previousButtonState;
		GetFoldersAndFiles(currentDirectory);
		SetDial();

	}



	// Update is called once per frame
	void Update () {
		//Finding the ship gameObject
		if (ship == null && GameObject.FindGameObjectWithTag ("Player") != null) {
			ship = GameObject.FindGameObjectWithTag ("Player").transform.FindChild("ballthing").gameObject;
		}
		//Initializing other game objects
		handController = GameObject.Find ("HeadMountedHandController");
		ovrCamera = GameObject.Find ("LeapOVRCameraRig");

		currentButtonState = button.ToggleState;

		//If the button is toggled "on"
		if(ButtonToggled(previousButtonState, currentButtonState) && button.ToggleState) 
		{
			button.ToggleState = false;
			updateBrowser = true;
		}

		//If updateBrowser is set to true, checks the selected item on the dial and performs operations depending on whether it is a folder or file
		if(updateBrowser)
		{
			selectedName = dialGraphics.hilightTextVolume.CurrentHilightValue;

			if(selectedName != "" && selectedName != null)
			{
				selectedIndex = fileAndFolderNames.IndexOf(selectedName);

				selectedContent = folderContents[selectedIndex];

				//If the selected label points to a directory
				if(Directory.Exists(selectedContent.FullName))
				{
					currentDirectory = new DirectoryInfo(selectedContent.FullName);

					currentDirectoryLabel.text = currentDirectory.Name;

					GetFoldersAndFiles(currentDirectory);

					SetDial();
				}
				//If the selected label points to a file
				else if(File.Exists(selectedContent.FullName))
				{
					photonView.RPC("TurnStuffOff", PhotonTargets.All);

					StartCoroutine ("Wait");
				}

			}

			updateBrowser = false;
		}

		previousButtonState = currentButtonState;

		//If TurnStuffOff has already been called
		if (off == true && reference != container.transform.GetChild(0).gameObject) {
			photonView.RPC("TurnStuffOn", PhotonTargets.All);
		}

	}


	/* Fetches all folders and files within the currentDirectory and assigns them to the folderContents list.
	 * If we want to restrict the filetypes appearing in the browser, this will be the function to edit.
	 */
	void GetFoldersAndFiles(DirectoryInfo currentDirectory) 
	{
		fileAndFolderNames.Clear();
		folderContents.Clear();

		if(currentDirectory.Parent != null)
		{
			folderContents.Add(currentDirectory.Parent);
		}
		folderContents.AddRange(currentDirectory.GetDirectories());		
		folderContents.AddRange(currentDirectory.GetFiles("*obj"));

		foreach(FileSystemInfo content in folderContents)
		{
			fileAndFolderNames.Add(content.Name);
		}
	}

	/* Fills the dial with file and folder names in the current directory. Calls other important initializations.
	 */
	void SetDial()
	{
		dialGraphics.DialLabels = fileAndFolderNames;
		dialBase.steps = fileAndFolderNames.Count;
		dialBase.CurrentStep = 0;
		dialBase.Awake();
		dialGraphics.Start();	
	}


	/* Compares two different toggle states, returns false if they are the same, true if they are different.
	 * prevState: A boolean parameter, filled by "previousButtonState" in this script.
	 * currentState: A boolean parameter, filled by "currentButtonState" in this script.
	 */
	bool ButtonToggled(bool prevState, bool currentState)
	{
		if (currentState == prevState) {
			return false;
		} else {
			return true;
		}
	}

	/* Calls the importManager's HandleFile function inside a coroutine so a loading screen can be displayed
	 */
	IEnumerator Wait (){
		yield return new WaitForSeconds (.1f);
		importManager.HandleFile(selectedContent, currentDirectory);
	}

	/* Deactivates certain gameObjects
	 */
	[PunRPC]
	void TurnStuffOff(){
		off = true;
		reference = container.transform.GetChild(0).gameObject;
		loadingScreen.SetActive(true);
		ship.SetActive(false);
//		particles.SetActive(false);
		handController.transform.localScale = new Vector3 (.001f,.001f,.001f);
		ovrCamera.transform.localRotation = Quaternion.Euler(0,-90,0);
	}

	/* Reactivates objects disabled by TurnStuffOff
	 */
	[PunRPC]
	void TurnStuffOn(){
		off = false;
		loadingScreen.SetActive(false);
		ship.SetActive(true);
//		particles.SetActive(true);
		handController.transform.localScale = new Vector3 (1f, 1f, 1f);
		ovrCamera.transform.localRotation = Quaternion.Euler(0f,0f,0f);
	}
}