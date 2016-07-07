using UnityEngine;
using System.Collections;

public class Jets : MonoBehaviour {
	public GameObject jet1;
	public GameObject jet2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (SpaceNavigator.Translation.z > 0) {
			jet1.SetActive (true);
			jet2.SetActive (true);
		} else if (SpaceNavigator.Translation.z == 0 && SpaceNavigator.Translation.x != 0) {
			jet1.SetActive (true);
			jet2.SetActive (true);
		} else if (SpaceNavigator.Translation.z == 0 && SpaceNavigator.Translation.y != 0) {
			jet1.SetActive (true);
			jet2.SetActive (true);
		}  else {
			jet1.SetActive (false);
			jet2.SetActive (false);
		}
	}
}
