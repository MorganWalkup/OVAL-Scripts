using UnityEngine;
using System.Collections;

/* This script is a class declaration for Avatar. 
 * It provides methods for changing an avatar's color, setting the in-game name, and hiding the avatar
 */ 
public class Avatar : Photon.MonoBehaviour {
	
	
	//The Text floating above the avatar object
	GameObject userNameObject;
	
	/* Sets color of the avatar
	 * r: The red value of the new color
	 * g: The green value of the new color
	 * b: The blue value of the new color
	 */
	[PunRPC]
	public void SetColor(float r, float g, float b)
	{
		Color avatarColor = new Color (r, g, b);
		//Sets the "_Tintcolor" variable of the avatar's material to avatarColor. 
		GetComponent<MeshRenderer> ().material.SetColor ("_TintColor", avatarColor);
	}
	
	/* Sets the text floating above the avatar object
	 * name: The new text string
	 */
	[PunRPC]
	public void SetName(string name)
	{
		//The text object should be set as the first child of the avatar object in the inspector
		userNameObject = transform.GetChild (0).gameObject;
		//Sets the text field of the textmesh to "name"
		userNameObject.GetComponent<TextMesh>().text = name;
	}
	
	/* Controls the visibility of the avatar
	 * show: "true" shows the avatar, "false" hides it
	 */
	public void Show(bool show)
	{
		GetComponent<MeshRenderer> ().enabled = show;
		userNameObject = transform.GetChild(0).gameObject;
		userNameObject.GetComponent<MeshRenderer> ().enabled = show;
	}
}
