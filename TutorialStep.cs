using UnityEngine;
using System.Collections;

public class TutorialStep : MonoBehaviour {
	
	public GameObject[] objectsToDeactivate;
	public GameObject[] objectsToActivate;
	public MonoBehaviour[] componentsToDisable;
	public MonoBehaviour[] componentsToEnable;


	// When the script becomes enabled and active
	void OnEnable () {

		foreach(GameObject obj in objectsToActivate)
		{
			obj.SetActive(true);
		}
		foreach(GameObject obj in objectsToDeactivate)
		{
			obj.SetActive(false);
		}
		foreach(MonoBehaviour comp in componentsToEnable)
		{
			comp.enabled = true;
		}
		foreach(MonoBehaviour comp in componentsToDisable)
		{
			comp.enabled = false;
		}

	}

	// When the script becomes disabled or inactive
	void OnDisable() {

	}

}
