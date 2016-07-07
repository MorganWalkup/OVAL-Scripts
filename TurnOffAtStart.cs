using UnityEngine;
using System.Collections;

public class TurnOffAtStart : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
