using UnityEngine;
using System.Collections;

public class ResizeTour : MonoBehaviour {
	public GameObject model;
	private MeshRenderer[] mR;
	private MeshRenderer mRFinal;
	private float d = 15;
	//private GameObject compare; 
	// Use this for initialization
	void Awake () {
		//compare = gameObject;
		model = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		float x = 0;
		float y= 0;
		float z= 0;
		float x1= 0;
		float y1= 0;
		float z1= 0;
		float xMin = 0;
		float yMin = 0;
		float zMin = 0;
		float xMax = 0;
		float yMax = 0;
		float zMax = 0;
		model = GameObject.FindGameObjectWithTag ("ImportedModel");

		if (model != null) {
			mR = model.GetComponentsInChildren<MeshRenderer>();
			mRFinal = model.GetComponentInChildren<MeshRenderer>();
			//compare = model;
		}

		if (model != null && mRFinal != null && d != 0f) {
			for (int i = 0; i <= mR.Length-1; i++){
				//x += mR[i].bounds.size.x;
				//y += mR[i].bounds.size.y;
				//z += mR[i].bounds.size.z;
				if (i == 0){
					xMin = mR[i].bounds.min.x;
					yMin = mR[i].bounds.min.y;
					zMin = mR[i].bounds.min.z;

				}
				if (mR[i].bounds.min.x <= xMin){
					xMin = mR[i].bounds.min.x;
				}
				if (mR[i].bounds.min.y <= yMin){
					yMin = mR[i].bounds.min.y;
				}
				if (mR[i].bounds.min.z <= zMin){
					zMin = mR[i].bounds.min.z;
				}
				if (mR[i].bounds.max.x >= xMax){
					xMax = mR[i].bounds.max.x;
				}
				if (mR[i].bounds.max.y >= yMax){
					yMax = mR[i].bounds.max.y;
				}
				if (mR[i].bounds.max.z >= zMax){
					zMax = mR[i].bounds.max.z;
				}
			
				x1 += mR[i].bounds.center.x;
				y1 += mR[i].bounds.center.y;
				z1 += mR[i].bounds.center.z;
			}
				x = xMax - xMin;
				y = yMax - yMin;
				z = zMax - zMin;
			
			//x = mRFinal.bounds.size.x;
			//y = mRFinal.bounds.size.y;
			//z = mRFinal.bounds.size.z;
			gameObject.transform.localScale = new Vector3(x/d,y/d,z/d);
			gameObject.transform.position = new Vector3 (x1/mR.Length,y1/mR.Length,z1/mR.Length);
		}

	}
}
