using UnityEngine;
using System.Collections;
using Leap;

public class MoveOnAfterThumbsUp : MonoBehaviour {

	//The tutorial manager controlling this scene
	public TutorialManager tutorialManager;

	//Thumbs Up Parameters
	public float fingerCurlMinDistance = 100.0f;
	Controller controller;

	// When the script becomes enabled and active
	void OnEnable () {

		//Initializing hand controller
		controller = new Controller();

	}

	//Called every frame
	void Update () {

		CheckForThumbsUp();

	}

	//Checks for thumbs up from the player
	void CheckForThumbsUp() {

		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
		Hand leftHand = null;
		Hand rightHand = null;

		//Assign left and right hands
		foreach(Hand hand in hands)
		{
			if(hand.IsLeft)
			{
				leftHand = hand;
			}
			else if(hand.IsRight)
			{
				rightHand = hand;
			}
		}

		//Check for thumbs up on left hand
		if (leftHand != null && leftHand.IsValid)
		{
			FingerList fingers = leftHand.Fingers;
			Finger thumb = fingers[0];
			bool leftFingersCurled = true;


			//Check that non-thumb fingers are curled
			for(int i = 1; i < fingers.Count; i++) {
				if(leftHand.PalmPosition.DistanceTo(fingers[i].TipPosition) > fingerCurlMinDistance) {
					leftFingersCurled = false;
				}
			}
			//Check for thumbs-up
			if(leftFingersCurled && thumb.IsExtended) {
				// Hold thumbs up for 1.0 seconds
				StartCoroutine("Wait", 1.0f);
			}
			else {
				StopCoroutine("Wait");
			}

		}

		//Check for thumbs up on right hand
		if(rightHand != null && rightHand.IsValid)
		{
			FingerList fingers = rightHand.Fingers;
			Finger thumb = fingers[0];
			bool rightFingersCurled = true;

			//Check that non-thumb fingers are curled
			for(int i = 1; i < fingers.Count; i++) {
				if(rightHand.PalmPosition.DistanceTo(fingers[i].TipPosition) > fingerCurlMinDistance) {
					rightFingersCurled = false;
				}
			}
			//Check for thumbs up
			if(rightFingersCurled && thumb.IsExtended) {
				// Hold thumbs up for 1.0 seconds
				StartCoroutine("Wait", 1.0f);
			}
			else {
				StopCoroutine("Wait");
			}
		}

	}

	// Waits for "seconds"
	IEnumerator Wait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		tutorialManager.moveOn = true;
	}
}
