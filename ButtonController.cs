using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {
	public bool chill;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (Input.GetButtonDown ("Fire1")) {
			chill = true;
		}
		if (Input.GetButtonUp ("Fire1")) {
			chill = false;
		}
	}
}
