using UnityEngine;
using System.Collections;

public class StartTut : MonoBehaviour {
	public GameObject player =  null;
	private bool done;
	public GameObject model;
	public GameObject scene;
	public GameObject tut1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		player = GameObject.FindGameObjectWithTag ("Player");
		if (player != null && done == false) {
			GameObject.Find ("Views").SetActive (false);
//			GameObject.Find ("Reset").SetActive (false);
			GameObject.Find ("Tour").SetActive (false);
			GameObject.Find ("Cockpit").SetActive (false);
			player.GetComponentInChildren <ButtonHierarchy>().enabled = false;
			scene.SetActive (false);
			model.SetActive (false);
			tut1.SetActive (true);
			done = true;
		}
	}
}
