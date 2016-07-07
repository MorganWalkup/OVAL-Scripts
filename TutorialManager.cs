using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {

	TutorialStep[] tutorialSteps;
	int activeStep = 0;
	[HideInInspector]
	public bool moveOn = false;

	// Use this for initialization
	void Start () {
		
		tutorialSteps = transform.GetComponentsInChildren<TutorialStep>(true);
		foreach(TutorialStep step in tutorialSteps)
		{
			step.gameObject.SetActive(false);
		}

		tutorialSteps[activeStep].gameObject.SetActive(true);

	}
	
	// Update is called once per frame
	void Update () {
		//if we should move on, end the current step and begin the next one
		if(moveOn && activeStep < tutorialSteps.Length - 1)
		{
			activeStep++;
			tutorialSteps[activeStep - 1].gameObject.SetActive(false);
			tutorialSteps[activeStep].gameObject.SetActive(true);
			moveOn = false;
		}
	}
}
