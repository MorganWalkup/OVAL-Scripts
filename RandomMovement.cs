using UnityEngine;
using System.Collections;

public class RandomMovement : MonoBehaviour {
	public float speed;
	public float time;
	public GameObject goal;
	//private Rigidbody rb;
	//private Quaternion random;
	// Use this for initialization
	void Start () {
		//rb = GetComponent<Rigidbody> ();
		StartCoroutine ("RandomDirection");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, goal.transform.position, speed);
		//rb.velocity = new Vector3 (rb.velocity.x, rb.velocity.y, speed);
	}

	IEnumerator RandomDirection (){
		yield return new WaitForSeconds (time);
		gameObject.transform.rotation = Random.rotation;
		StartCoroutine ("RandomDirection");
	}
}
