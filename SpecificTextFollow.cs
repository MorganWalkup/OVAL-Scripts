using UnityEngine;
using System.Collections;

public class SpecificTextFollow : MonoBehaviour {

	//private GameObject label;
	public GameObject myPlayer;
	public bool taken;
	// Use this for initialization
	void Awake () {
		//label = GameObject.FindGameObjectWithTag ("Label");
		if (transform.localScale.x > 0) {
			transform.localScale = new Vector3 (-transform.localScale.x,transform.localScale.y,transform.localScale.z);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (taken) {
			gameObject.transform.LookAt (myPlayer.transform, Vector3.up);
		}


	}

	void LateUpdate (){
		taken = false;
	}
}
