using UnityEngine;
using System.Collections;

public class HideOnSliderChange : MonoBehaviour {
	
	//The slider widget
	public SliderDemo slider;
	//Transform of the plane
	public Transform plane;
	//The material attached to the object, using the plane cut shader
	public Material planeCutMaterial;
	//The value of the denominator to be used in a distance calculatiion
	float distanceDenom;
	//The initial local posittion of the plane
	Vector3 initialPlanePosition;
	//Variation betweeen plane positions at max slider value and min slider value
	public float planeMovementMagnitude = 1.0f;
	
	// Use this for initialization
	void Start () 
	{
		initialPlanePosition = plane.localPosition;
		distanceDenom = Mathf.Sqrt(Mathf.Pow(plane.up.x, 2f) + Mathf.Pow(plane.up.y, 2f) + Mathf.Pow(plane.up.z, 2f));
		planeCutMaterial.SetVector("_PlaneNormal", plane.up);
		planeCutMaterial.SetVector("_PlanePos", plane.position);
		planeCutMaterial.SetFloat("_DistanceDenom", distanceDenom);
	}
	
	//Updates once per frame
	void Update() 
	{
		UpdatePlane(plane, slider.transform.position);
		distanceDenom = Mathf.Sqrt(Mathf.Pow(plane.up.x, 2f) + Mathf.Pow(plane.up.y, 2f) + Mathf.Pow(plane.up.z, 2f));
		planeCutMaterial.SetVector("_PlaneNormal", plane.up);
		planeCutMaterial.SetVector("_PlanePos", plane.position);
		planeCutMaterial.SetFloat("_DistanceDenom", distanceDenom);
		
	}
	
	public void UpdatePlane(Transform plane, Vector3 sliderPos)
	{	
		//An unmoved slider outputs a slider value of zero
		float sliderValue = slider.GetSliderFraction() - 0.5f;
		plane.localPosition = new Vector3(initialPlanePosition.x, 
		                                  initialPlanePosition.y * Mathf.Pow( Mathf.Abs(sliderValue) ,1/planeMovementMagnitude)*Mathf.Sign(sliderValue),
		                                  initialPlanePosition.z);
	}
}
