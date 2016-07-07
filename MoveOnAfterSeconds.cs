using UnityEngine;
using System.Collections;

public class MoveOnAfterSeconds : MonoBehaviour {

	public TutorialManager tutorialManager;
	public float seconds = 2.0f;

	// When this scipt is enabled and active
	void OnEnable () {

		StartCoroutine("Wait", seconds);
	}

	// Waits for "seconds"
	IEnumerator Wait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		tutorialManager.moveOn = true;
	}
}
