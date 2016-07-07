/* Manages the importing of all 3D models by checking for file type and calling the appropriate scripts
 * Can be attached to anything. Recommended: "Scripts" gameObject
 */
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ImportManager : Photon.MonoBehaviour {

	//A gameobject holding any manipulation components (rotate, scale, etc). Will be the parent of our imported object.
	public GameObject container;
	//The slider used by the RescaleOnSliderChange component of the container. Has to be reset on every import.
	SliderDemo rescaleSlider;
	//A list holding textures for a file
	List<string> texturePaths = new List<string>();
	//Josh's variable, something about size
	public float standardSize;
	//Josh's variable, something about size
	private float x;


	public void Start(){
		rescaleSlider = container.GetComponent<RescaleOnSliderChange>().slider;
	}

	/*This function takes a file path and a directory, and determines what functions to use based on the file type
	 */
	public void Update (){
		if (container.transform.childCount > 1) {
			//photonView.RPC("DestroyOldObj", PhotonTargets.AllBufferedViaServer);
			Destroy (container.transform.GetChild (0).gameObject);
		}
	}

	public void HandleFile (FileSystemInfo file, DirectoryInfo directory)
	{
		rescaleSlider.SetPositionFromFraction(0.5f);

		string extension = file.Extension;

		//Check the type of file
		if (extension == ".obj")
		{
			HandleObj(file, directory);
		}

	}


	/*Performs the operations necessary to import an obj file
	 */
	private void HandleObj(FileSystemInfo file, DirectoryInfo directory)
	{
		//Gets the name of the file, without the extension
		string name = file.Name.Substring(0 , file.Name.IndexOf('.'));
		//Holds the path of the material file, if there is one
		string mtlPath = "";
		//Checks to see if material file exists
		bool mtlExists = false;

		//Clear texture paths
		photonView.RPC("ClearTexturePathList", PhotonTargets.AllBuffered);

		//Look for materials and textures
		foreach(FileSystemInfo extraFile in directory.GetFiles())
		{
			if(extraFile.Name == name + ".mtl")
			{
				mtlExists = true;
				mtlPath = extraFile.FullName;
			}
			else if(extraFile.Name.Contains(name) && (extraFile.Extension == ".jpg" || extraFile.Extension == ".png") )
			{
				//Adds the .jpg or .png file to the texturePathList 
				photonView.RPC("SyncTexturePathList", PhotonTargets.AllBuffered, extraFile.FullName);
			}
		}

		if (mtlExists)
		{
			photonView.RPC("ImportObjFromPath", PhotonTargets.AllBuffered, file.FullName, mtlPath);
		}
		else
		{
			photonView.RPC("ImportObjFromPath", PhotonTargets.AllBuffered, file.FullName);
		}
	}


	/* Calls the ObjImporter script to create a gameobject out of an obj string
	 * string objPath: The file path of the obj file to import
	 */
	[PunRPC]
	private void ImportObjFromPath(string objPath)
	{
		//Converts file at given location into a string of text
		string objText = File.ReadAllText(objPath);
		//Imports the obj file using the objText. Assigns it to importObject for easy access
		//GameObject importObject = ObjImporter.Import(objText);
		//Sets the container object as the parent of importObject
		//importObject.transform.SetParent(container.transform);
		//Sets importObject position to be the same as the container's position
		//importObject.transform.position = container.transform.position;


		//Import new model
		GameObject importObject = ObjImporter.Import(objText);

		//Recenter imported model
		MeshRenderer mr;
		float temp = 0;
		//If importObject has a single mesh renderer
		if (importObject.GetComponent<MeshRenderer> () != null) {
			mr = importObject.GetComponent<MeshRenderer> ();
			temp = mr.bounds.size.x;
		//Else if importObject has multiple renderers
		} else {
			MeshRenderer[] mrs;
			mrs = importObject.GetComponentsInChildren<MeshRenderer>();
			float x1 = 0;
			float x2 = 0;
			for (int i = 0; i <= mrs.Length-1; i++){


				if (mrs[i].bounds.min.x <= x1){
					x1 = mrs[i].bounds.min.x;
				}

				if (mrs[i].bounds.max.x >= x2){
					x2 = mrs[i].bounds.max.x;
				}
				temp = x2 - x1;
			}
		}
		x = standardSize / (temp);
		importObject.transform.localScale = new Vector3 (x, x, x*-1f); 

		importObject.transform.SetParent(container.transform);
		importObject.transform.position = container.transform.position;
	}


	/* Calls the ObjImporter script to create a gameobject out of an obj string
	 * string objPath: The file path of the obj file to import
	 * string mtlPath: The file path of the material to import with the obj
	 */
	[PunRPC]
	private void ImportObjFromPath(string objPath, string mtlPath)
	{
		Texture2D[] textures = new Texture2D[texturePaths.Count];
		for(int i = 0; i < texturePaths.Count; i++)
		{
			textures[i] = LoadPNG(texturePaths[i]);
		}
		//Converts file at given location into a string of text
		string objText = File.ReadAllText(objPath);
		string mtlText = File.ReadAllText(mtlPath);
		//Imports the obj file using the objText. Assigns it to importObject for easy access
		//GameObject importObject = ObjImporter.Import(objText, mtlText, textures);
		//Sets the container object as the parent of importObject
		//importObject.transform.SetParent(container.transform);
		//Sets importObject position to be the same as the container's position
		//importObject.transform.position = container.transform.position;


		//Import new model
		GameObject importObject = ObjImporter.Import(objText, mtlText, textures);

		//Recenter imported model
		MeshRenderer mr;
		float temp = 0;
		if (importObject.GetComponent<MeshRenderer> () != null) {
			mr = importObject.GetComponent<MeshRenderer> ();
			temp = mr.bounds.size.x;
		} else {
			MeshRenderer[] mrs;
			mrs = importObject.GetComponentsInChildren<MeshRenderer>();
			float x1 = 0;
			float x2 = 0;
			for (int i = 0; i <= mrs.Length-1; i++){


				if (mrs[i].bounds.min.x <= x1){
					x1 = mrs[i].bounds.min.x;
				}

				if (mrs[i].bounds.max.x >= x2){
					x2 = mrs[i].bounds.max.x;
				}
				temp = x2 - x1;
			}
		}
		x = standardSize / (temp);
		importObject.transform.localScale = new Vector3 (x, x, x*-1f); 

		importObject.transform.SetParent(container.transform);
		importObject.transform.position = container.transform.position;
	}


	/* Calls the ObjImporter script to create a gameobject out of an obj string
	 * string objText: The text of the obj file to import
	 */
	private void ImportObj(string objText)
	{
		//Passes the objText to the importer and sets the result to be a child of the container object
		//GameObject importObject = ObjImporter.Import(objText);
		//importObject.transform.SetParent(container.transform);
		//importObject.transform.localPosition = Vector3.zero;

		//Import new model
		GameObject importObject = ObjImporter.Import(objText);

		//Recenter imported model
		MeshRenderer mr;
		float temp = 0;
		if (importObject.GetComponent<MeshRenderer> () != null) {
			mr = importObject.GetComponent<MeshRenderer> ();
			temp = mr.bounds.size.x;
		} else {
			MeshRenderer[] mrs;
			mrs = importObject.GetComponentsInChildren<MeshRenderer>();
			float x1 = 0;
			float x2 = 0;
			for (int i = 0; i <= mrs.Length-1; i++){


				if (mrs[i].bounds.min.x <= x1){
					x1 = mrs[i].bounds.min.x;
				}

				if (mrs[i].bounds.max.x >= x2){
					x2 = mrs[i].bounds.max.x;
				}
				temp = x2 - x1;
			}
		}
		x = standardSize / (temp);
		importObject.transform.localScale = new Vector3 (x, x, x*-1f);

		importObject.transform.SetParent(container.transform);
		importObject.transform.position = container.transform.position;
	}


	/*When called as an RPC function, simultaneously updates the texturePathList on all clients
	 *string texPath: The texture filepath to add to the list
	 */
	[PunRPC]
	private void SyncTexturePathList(string texPath)
	{
		texturePaths.Add(texPath);
	}


	/*When called as an rpc function, simultaneously clears the texturePathList on all clients
	 */
	[PunRPC]
	private void ClearTexturePathList()
	{
		texturePaths.Clear();
	}


	/* Calls the ObjImporter script to create a gameobject out of an obj string
	 * string objText: The text of the obj file to import
	 */
	private void ImportObj(string objText, string mtlText, Texture2D[] textures)
	{
		GameObject importObject = ObjImporter.Import(objText, mtlText, textures);
		importObject.transform.SetParent(container.transform);
		importObject.transform.localPosition = Vector3.zero;
	}


	/* Given a filepath, converts the file in that location into a string of text
	 * string filepath: The path to the file you wish to convert
	 */
	private string FileToString(string filepath)
	{
		//Pulling text out of the obj and converting it to a string
		string fileText = File.ReadAllText(filepath);
		return fileText;
	}


	/* Given a filepath to a png, loads that file into unity.
	 * string filepath: path to the png file
	 */
	public static Texture2D LoadPNG(string filePath) {

		Texture2D tex = null;
		byte[] fileData;

		if (File.Exists(filePath))
		{
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
		}
		return tex;
	}


	/* When called as an RPC function, destroys the imported model (a child of "container") on all clients.
	 * Can't use PhotonNetwork.Destroy because imported model 
	 * is not created with PhotonNetwork.Instantiate
	 */
	[PunRPC]
	private void DestroyOldObj()
	{
		if (container.transform.childCount > 0) {
			Destroy (container.transform.GetChild (0).gameObject);
		}
	}

}


