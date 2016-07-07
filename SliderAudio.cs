using UnityEngine;
using System.Collections;

public class SliderAudio : MonoBehaviour {
	
	SliderDemo sliderTop;
	
	AudioClip click;
	
	//the slider value from the previous frame
	private float previousSliderValue;
	//the slider value from the current frame
	private float currentSliderValue;
	
	
	
	// Use this for initialization
	void Start () {
		sliderTop = GetComponent<SliderDemo> ();
		click = gameObject.GetComponent<AudioSource>().clip;
		previousSliderValue = sliderTop.GetSliderFraction();
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
		currentSliderValue = sliderTop.GetSliderFraction();
		
		if (SliderChanged (previousSliderValue, currentSliderValue))
		{
			PlayAdjustedSound();
		}
		
		previousSliderValue = currentSliderValue;
	}
	
	
	
	/* Compares two different slider values, returns false if they are the same, true if they are different.
	 * prevValue: A boolean parameter, filled by "previousSliderValue" in this script.
	 * currentValue: A boolean parameter, filled by "currentSliderValue" in this script.
	 */
	public bool SliderChanged(float prevValue, float currentValue)
	{
		if ( currentValue != prevValue ) 
		{
			return true;
		} 
		else 
		{
			return false;
		}
	}
	
	void PlayAdjustedSound()
	{
		gameObject.GetComponent<AudioSource>().pitch = sliderTop.GetSliderFraction () + 0.5f;
		gameObject.GetComponent<AudioSource>().PlayOneShot (click);
	}
}
