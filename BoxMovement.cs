using UnityEngine;
using System.Collections;

public class BoxMovement : MonoBehaviour {

	public float speed;
	public GameObject point1;
	public GameObject end;
	public GameObject point3;
	public float rotationx;
	public bool hitend;

	bool right;

	Rigidbody rigid;
	private GameObject target;


	// Use this for initialization
	void Start () {
		target = end;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, target.transform.position, speed);
		//if (right){
		//transform.position = Vector3.Lerp (transform.position, end.transform.position, speed);
		//}
		//if (!right){
		//	transform.position = Vector3.Lerp (transform.position, start.transform.position, speed);
		//}
	}

	void OnTriggerEnter (Collider other){
		if (other.gameObject.CompareTag ("Beginning")) {
			//right = false;
			target = point1;
		}
		if (other.gameObject.CompareTag ("Middle")) {
			//right = true;
//			target = point2;
		}
		if (other.gameObject == point3) {
			target = point3;
		}
	}
}
