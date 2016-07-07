using UnityEngine;
using System.Collections;

public class MoveOnAfterMovement : MonoBehaviour {

	public TutorialManager tutorialManager;
	public GameObject ovalplayer;
	private Vector3 startingPosition;

	// When the script becomes enabled and active
	void OnEnable () {
		startingPosition = ovalplayer.transform.position;
	}


	void Update() {
		if(ovalplayer.transform.position != startingPosition)
		{
			StartCoroutine("Wait", 5);
		}
	}
	// Waits for "seconds"
	IEnumerator Wait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		tutorialManager.moveOn = true;
	}
}
