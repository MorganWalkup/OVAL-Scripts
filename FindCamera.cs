using UnityEngine;
using System.Collections;

public class FindCamera : MonoBehaviour {
	private Canvas cv;
	private GameObject camer;
	// Use this for initialization
	void Start () {
		cv = GetComponent<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (camer == null) {
			camer = GameObject.FindGameObjectWithTag ("MainCamera");
		} else {
			cv.worldCamera = camer.GetComponent<Camera> ();
		}
	}
}
