using UnityEngine;
using System.Collections;

//This script allows users to control the dial file browser with the 3D mouse
//It should be attached to the Dial_Physics gameobject, which is a child of the FileBrowserDial
public class MouseControlledDial : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		//Not sure how the axes are defined on SpaceNavigator. Change to SpaceNavigator.Translation.x if controls are sideways.
		transform.Rotate(0, SpaceNavigator.Translation.z, 0, Space.Self);
	}
}
