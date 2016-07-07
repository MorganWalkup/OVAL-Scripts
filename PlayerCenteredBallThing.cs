using UnityEngine;
using System.Collections;

public class PlayerCenteredBallThing : Photon.MonoBehaviour {
	
	public PhotonView ovalPhotonView;
	public Transform centerEyeAnchor = null;
	public Vector3 positionOffset;
	public Vector3 rotationOffset;
	
	// Update is called once per frame
	void Update()
	{
		
		//If Fire2 is pressed, and Fire1 is not currently held down
		if(( Input.GetButtonDown("Fire2") && !Input.GetButton("Fire1") ) && (ovalPhotonView.isMine))
		{
			SetBallThingTransform();
		}
		
	}
	
	//Sets the ballthing to be the at the same position and rotation as the center eye anchor
	void SetBallThingTransform()
	{
		//Sets the y position and rotation of the hierarchy object to the y position of the center eye anchor
		transform.localPosition = Vector3.zero;
		transform.position = centerEyeAnchor.position + positionOffset;
		transform.localEulerAngles = new Vector3(0f, centerEyeAnchor.localEulerAngles.y, 0f) + rotationOffset;
		
	}
}
