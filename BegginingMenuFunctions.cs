using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BegginingMenuFunctions : MonoBehaviour {
	
	public int levelNumber;
	public GameObject open;
	public GameObject close;


	public void OpenNewLevel (){
		SceneManager.LoadScene(levelNumber);
	}

	public void ExitGame (){
		Application.Quit ();
	}

	public void OpenMenuCloseMenu (){
		open.SetActive (true);
		close.SetActive (false);
	}
}
