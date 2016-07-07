using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimerBeginLevel : MonoBehaviour {
	public float delay;
	public int levelNumber;
	// Use this for initialization
	void Awake () {
		StartCoroutine ("Timer");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator Timer(){
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (levelNumber);
	}
}
