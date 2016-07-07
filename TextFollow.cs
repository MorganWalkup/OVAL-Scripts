using UnityEngine;
using System.Collections;

public class TextFollow : MonoBehaviour {
	//private GameObject label;
	private GameObject myPlayer;
	// Use this for initialization
	void Start () {
		//label = GameObject.FindGameObjectWithTag ("Label");


	}
	
	// Update is called once per frame
	void Update () {
		if (myPlayer == null) {
			myPlayer = GameObject.FindGameObjectWithTag ("Player");
		}
	if (myPlayer != null) {
			gameObject.transform.LookAt (myPlayer.transform, Vector3.up);
		}
	}
}
