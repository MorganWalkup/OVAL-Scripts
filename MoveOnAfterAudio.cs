using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class MoveOnAfterAudio : MonoBehaviour {

	AudioClip audioClip;
	public TutorialManager tutorialManager;

	// When the script becomes enabled and active
	void OnEnable () {

		// Assign "audio" to the audio clip of this gameobject
		audioClip = GetComponent<AudioSource>().clip;

		// Wait for the length of the audio clip
		StartCoroutine("Wait", audioClip.length);
	}

	// Waits for "seconds"
	IEnumerator Wait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		tutorialManager.moveOn = true;
	}
}
