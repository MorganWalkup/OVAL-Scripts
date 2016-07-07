using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingCircle : MonoBehaviour {

	public Image loadingCircleImg;

	// When the script becomes enabled and active
	void OnEnable () {
		// Call the "fill" coroutine
		StartCoroutine("Fill", null);
	}

	// When the script becomes disabled or inactive
	void OnDisable () {
		// Stop the "fill" coroutine
		StopCoroutine("Fill");
	}

	// Fills the loading circle over and over
	IEnumerator Fill ()
	{
		loadingCircleImg.fillAmount = 0f;

		while(true) {
			
			if(loadingCircleImg.fillAmount < 1.0f)
				loadingCircleImg.fillAmount += 0.005f;
			else
				loadingCircleImg.fillAmount = 0f;

			yield return null;
		}
	}

}
