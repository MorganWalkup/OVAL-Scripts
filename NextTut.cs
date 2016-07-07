using UnityEngine;
using System.Collections;

public class NextTut : MonoBehaviour {

	public float timeWait;
	public static int number;
	public GameObject[] turnOn = new GameObject[number] ;
	public GameObject turnOff;
	public bool waitForOn;
	//private ButtonController bc;
	//public GameObject onOrNot;
	// Use this for initialization
	void Start () {
		//bc = GameObject.FindGameObjectWithTag ("ButtonMaster").GetComponent<ButtonController> ();
		if (waitForOn == false) {
			StartCoroutine ("Timer");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (waitForOn) {
			if (Input.GetButtonDown ("Fire1")){
				StartCoroutine ("Timer");
			}
		}
	}
	IEnumerator Timer (){
		yield return new WaitForSeconds (timeWait);
		//turnOn.SetActive (true);
		for (int i = 0; i<=turnOn.Length-1; i++) {
			turnOn[i].SetActive (true);
		}
		turnOff.SetActive (false);
	}
}
