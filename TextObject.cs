using UnityEngine;
using System.Collections;

public class TextObject : MonoBehaviour {
	public bool hit;

	public GameObject textThing;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		hit = false;

	}
	void LateUpdate (){
		if (hit) {
			textThing.SetActive (true);
		} else {
			textThing.SetActive (false);
		}
	}
}
