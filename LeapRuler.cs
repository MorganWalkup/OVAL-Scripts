using UnityEngine;
using System.Collections;
using Leap;

//This script creates a Leap-Gesture-enabled ruler tool. It should be attached to a 'ruler' gameobject on the OVALPlayer
//containing an upper limit, lower limit, line renderer, and 3d text object
public class LeapRuler : MonoBehaviour {

	//Ruler Components
	public GameObject lowerLimit;
	public GameObject upperLimit;
	public LineRenderer rulerLR;
	public TextMesh rulerText;

	//Gesture Parameters
	Controller controller;
	public float pinchStrengthNeeded = 0.9f;
	public float grabStrengthNeeded = 0.9f;
	private GameObject hmhController;

	//Ruler Parameters
	public Collider intersectingCollider { get; set; }
	private Collider lowerCollider;
	private Collider upperCollider;


	// Use this for initialization
	private void Start()
	{
		//Initializing hand controller
		controller = new Controller();
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
		controller.Config.SetFloat("Gesture.Swipe.MinLength", 130.0f);
		controller.Config.SetFloat("Gesture.Swipe.MinVelocity", 500f);
		controller.Config.Save();

		//Getting head mounted hand controller
		hmhController= GameObject.Find("HeadMountedHandController");

		//Initializing ruler object
		transform.SetParent (null);
		transform.position = hmhController.transform.position;
	}
	

	// Update is called once per frame
	void Update () {
		
		UpdateOnGestures();

		//If lower and upper limits are active in the scene
		if(lowerLimit.activeInHierarchy && upperLimit.activeInHierarchy)
		{
			SetMeasurement(false);

			rulerLR.enabled = true;
			rulerText.gameObject.GetComponent<MeshRenderer>().enabled = true;

			//Updates endpoints of the line renderer
			rulerLR.SetPosition(0,lowerLimit.transform.position);
			rulerLR.SetPosition(1, upperLimit.transform.position);
		}
		else
		{
			rulerLR.enabled = false;
			rulerText.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}


	/* Sets the displayed value of the measurement taken by the ruler.
	 * bool rulerOnCollider: If true, measurement is adjusted based on collider's current scale. If false, measurement is given as-is, in Unity units (meters).
	 */
	void SetMeasurement(bool rulerOnCollider)
	{
		//Sets the rulerText object position to be halfway between lowerLimit and upperLimit
		//rulerText.gameObject.transform.position = Vector3.Lerp(lowerLimit.transform.position, upperLimit.transform.position, 0.5f);
		float measurement = (Vector3.Distance (lowerLimit.transform.position, upperLimit.transform.position));
		//if (rulerOnCollider) {
		//	rulerText.text = (measurement / FindGrandestParent(intersectingCollider.transform).localScale.x).ToString () + "\nMeters (Adjusted)";
		//} else {
			//Sets the text above the line renderer to be the distance between lowerLimit and upperLimit
			rulerText.text = "Measurement:\n" + measurement.ToString() + "\nMeters (Raw)";
		//}
	}


	/* Checks the current state of the Leap Motion hands to detect gestures and perform actions
	 */
	void UpdateOnGestures()
	{
		Frame frame = controller.Frame();
		GestureList gestures = frame.Gestures();
		HandList hands = frame.Hands;
		Hand leftHand = null;
		Hand rightHand = null;

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


		if (leftHand != null && leftHand.IsValid)
		{
			FingerList fingers = leftHand.Fingers;
			Finger thumb = fingers[0];
			Bone thumbTip = thumb.Bone(Bone.BoneType.TYPE_DISTAL);

			if(leftHand.PinchStrength > pinchStrengthNeeded && lowerLimit.activeInHierarchy)
			{
				//Turn on lowerLimit
				//lowerLimit.SetActive(true);
				//Converting local coordinates of thumbTip to unity meters
				Vector3 local_thumbTip = thumbTip.NextJoint.ToUnityScaled();
				//Using hand controller to get world coordinates of thumbTip
				Vector3 world_thumbTip = hmhController.transform.TransformPoint(local_thumbTip);
				//Move lowerLimit to thumbTip
				lowerLimit.transform.position = world_thumbTip;
			}
			else
			{
				//lowerLimit.SetActive(false);
			}
		}

		if(rightHand != null && rightHand.IsValid && upperLimit.activeInHierarchy)
		{
			FingerList fingers = rightHand.Fingers;
			Finger thumb = fingers[0];
			Bone thumbTip = thumb.Bone(Bone.BoneType.TYPE_DISTAL);

			if(rightHand.PinchStrength > pinchStrengthNeeded)
			{
				//Turn on lowerLimit
				//upperLimit.SetActive(true);
				//Converting local coordinates of thumbTip to unity meters
				Vector3 local_thumbTip = thumbTip.NextJoint.ToUnityScaled();
				//Using hand controller to get world coordinates of thumbTip
				Vector3 world_thumbTip = hmhController.transform.TransformPoint(local_thumbTip);
				//Move lowerLimit to thumbTip
				upperLimit.transform.position = world_thumbTip;
			}
			else
			{
				//upperLimit.SetActive(false);
			}
		}


		//"Swipe to Activate" Gesture
		for (int i=0; i < gestures.Count; i++)
		{
			Gesture gesture = gestures[i];
			if(gesture.Type == Gesture.GestureType.TYPESWIPE)
			{
				SwipeGesture Swipe = new SwipeGesture(gesture);
				Vector swipeDirection = Swipe.Direction;

				if(leftHand != null && rightHand != null && leftHand.GrabStrength > grabStrengthNeeded && rightHand.PinchStrength > pinchStrengthNeeded && swipeDirection.x < 0 && swipeDirection.z < 0)
				{
					upperLimit.SetActive(true);
					lowerLimit.SetActive(true);
				}
				else if(leftHand != null && rightHand != null && leftHand.GrabStrength > grabStrengthNeeded && rightHand.PinchStrength > pinchStrengthNeeded && swipeDirection.x > 0 && swipeDirection.z > 0)
				{
					upperLimit.SetActive(false);
					lowerLimit.SetActive(false);
				}
			}
		}
	}
}
