using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenshotButtonControl : MonoBehaviour {

	public string button1;
	public string button2;
	public float timerAmount;
	public float flashDuration;
	public GameObject numberText;
	private Text actualNumber; 
	public GameObject crosshairs;
	public GameObject flash;

	private float currentNumber;
	private bool pressing;
	private bool started;
	private int screenshotCount;

	// Use this for initialization
	void Start () {
		actualNumber = numberText.GetComponent<Text> ();
		numberText.SetActive (false);
	}

	// Update is called once per frame
	void Update () {


		if (numberText.activeInHierarchy) {
			actualNumber.text = currentNumber.ToString ();
		}

		if (Input.GetButton (button1) && Input.GetButton (button2)) {
			pressing = true;
		} else {
			pressing = false;
		}


		if (!started) {
			currentNumber = timerAmount;
		}

		if (pressing && !started) {
			StartCoroutine ("OtherTimer");
			started = true;
		}


	}

	IEnumerator TimerShort (){
		yield return new WaitForSeconds (1f);
		if (pressing) {
			if (currentNumber == 0f) {
				numberText.SetActive (false);
				crosshairs.SetActive (false);

				//snapshot function
				//Debug.Log ("Screenshot taken");
				string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
				string screenshotFilename;
				string screenshotPath;
				do
				{
					screenshotCount++;
					screenshotFilename = "OVAL_Screenshot" + screenshotCount + ".jpg";
					Debug.Log ("Screenshot taken");
					screenshotPath = desktopPath + "\\" + screenshotFilename;
				} while (System.IO.File.Exists(screenshotPath));

				Debug.Log("Screenshot path: " + screenshotPath);
				Application.CaptureScreenshot(screenshotPath);
				//end Function


				//flash.SetActive (true);
				StartCoroutine ("Flash");
				started = false;

			} else {
				currentNumber -= 1f;
				StartCoroutine ("TimerShort");
			}

		} else {
			numberText.SetActive (false);
			crosshairs.SetActive (false);
			started = false;
		}
	}

	IEnumerator OtherTimer (){
		yield return new WaitForSeconds (1f);
		if (pressing) {
			numberText.SetActive (true);
			crosshairs.SetActive (true);
			StartCoroutine ("TimerShort");

		} else {
			numberText.SetActive (false);
			crosshairs.SetActive (false);
			started = false;
		}
	}

	IEnumerator Flash (){
		yield return new WaitForSeconds (flashDuration);
		flash.SetActive (true);
		yield return new WaitForSeconds (flashDuration);

		flash.SetActive (false);
	}

	void OnApplicationQuit (){
		StopAllCoroutines ();
	}
}
