using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// This script takes a string entered by the user and sets the PUN Session Name for this instance of OVAL
public class GetPUNSessionName : MonoBehaviour {
		 
	public Dropdown sessionDropdown;
	public InputField sessionInputField;
 

 	// Initialize variables here
	void Start() {
		//sessionInputField.gameObject.SetActive(false);
		//SetPUNSessionName(sessionDropdown.options[0].text);

		// Initiating sessionDropdownValueChangedHandler to be called when an option is selected
    	sessionDropdown.onValueChanged.AddListener(delegate {
	    	sessionDropdownValueChangedHandler(sessionDropdown);
	    });
	}
	 

	// Called when this object is destroyed 
	void Destroy() {
	    sessionDropdown.onValueChanged.RemoveAllListeners();
	}
	 

	// Called when a dropdown option is selected
	private void sessionDropdownValueChangedHandler(Dropdown myDropdown) {

	    string dropdownSelection = myDropdown.options[myDropdown.value].text;
	    Debug.Log("selected: " + dropdownSelection);

	    if(dropdownSelection == "Custom")
	    {
	    	sessionInputField.gameObject.SetActive(true);
	    }
	    else
	    {
	    	sessionInputField.gameObject.SetActive(false);
	    	SetPUNSessionName(dropdownSelection);
	    }
	}


	public void SetPUNSessionName(string name) {

		PlayerPrefs.SetString("PUN Session Name", name);
		Debug.Log("String Set:" + name);
	
	}		
}
