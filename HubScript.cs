///Place this on the same object as you place the other two scripts, as they interact via getcomponents.
//ALL variable must be public for this to work properly
using UnityEngine;
using System.Collections;

public class HubScript : MonoBehaviour {
	//Reference to your object that has an empty mesh filter on it and a mesh renderer with whatever your default texture will be.
	public GameObject blankObject;
	//This bool is changed by the file selection script to tell us to start the file "unfolder" script
	[HideInInspector]
	public bool getFile;
	//This is the file directory string, this is edited by the file selection script
	[HideInInspector]	
	public string filename;
	
	
	
	void Update () {
		//If the player recently selected an object, run the creator function
		if (getFile) {
			CreatorFunc (filename);
			//Reset the bool for the next time the player wants to change the Obj
			getFile = false;
		}
		
		
	}
	//This function takes a file directory name and feeds it into the "Unboxer" script, then uses that mesh on our blank object
	public void CreatorFunc (string filename){
		//First we need a blank mesh to edit
		Mesh importedMesh = null;
		//Then get the mesh from the OBJ importer script
		importedMesh = gameObject.GetComponent<ObjImporterX>().ImportFile(filename);
		
		//This next line is the "faster" obj importer, but is much less accurate
		//importedMesh = gameObject.GetComponent<FastObjImporter> ().ImportFile (filename);
		
		//finally apply the mesh to the blank object	
		blankObject.GetComponent<MeshFilter> ().mesh = importedMesh;
	}
}
