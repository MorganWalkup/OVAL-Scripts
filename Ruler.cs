using UnityEngine;
using System.Collections;

/* This script controls the functionality of the ruler tool. 
 * A user places two spheres in 3D space and an updated TextMesh object displays the distance between them.
 */
public class Ruler: MonoBehaviour
{
	public Laser laser;
	public GameObject lowerLimit;
	public GameObject upperLimit;
	public LineRenderer rulerLR;
	public TextMesh rulerText;

	//Controls which part of the ruler to update. Should only ever have the values 0, 1, or 2.
	private int updateCase = 0;

	public bool hitCollider { get; set; }
	public Collider intersectingCollider { get; set; }
	private Collider lowerCollider;
	private Collider upperCollider;

	private void Start()
	{
		transform.SetParent (null);
		transform.position = Vector3.zero;
		ShowRuler (false);
	}


	private void Update()
	{
		//If Fire1 is held down
		if (Input.GetButton("Fire1")) 
		{
			//If Fire2 has just been released
			if (Input.GetButtonUp("Fire2"))
			{
				//Runs the code in a particular "case" depending on the value of "updateCase". 
				switch(updateCase)
				{
					case 0:
						//Moves lowerLimit object to the tip of the laser pointer
						lowerLimit.transform.position = laser.indicatorObjectSpot;

						//If the tip of the laserpointer is hitting a collider
						if(hitCollider)
						{
							lowerCollider = intersectingCollider;
						}
						else
						{
							lowerCollider = null;
						}
						lowerLimit.GetComponent<MeshRenderer>().enabled = true;

						updateCase++;
						break;

					case 1:
						//Moves upperLimit object to the tip of the laser pointer
						upperLimit.transform.position = laser.indicatorObjectSpot;

						//If the tip of the laser pointer is hitting a collider
						if(hitCollider)
						{
							upperCollider = intersectingCollider;
						}
						else
						{
							upperCollider = null;
						}

						ShowRuler(true);
						
						SetMeasurement(CompareColliders(lowerCollider, upperCollider));

						//Updates endpoints of the line renderer
						rulerLR.SetPosition(0,lowerLimit.transform.position);
						rulerLR.SetPosition(1, upperLimit.transform.position);

						updateCase++;
						break;

					case 2:
						ShowRuler(false);
						updateCase = 0;
						break;

					default:
						Debug.LogError ("updateCase not a valid case. Valid cases are 0, 1, and 2.");
						break;
				}
			}
		}
	}

	/* Sets the displayed value of the measurement taken by the ruler.
	 * bool rulerOnCollider: If true, measurement is adjusted based on collider's current scale. If false, measurement is given as-is, in Unity units (meters).
	 */
	void SetMeasurement(bool rulerOnCollider)
	{
		//Sets the rulerText object position to be halfway between lowerLimit and upperLimit
		rulerText.gameObject.transform.position = Vector3.Lerp(lowerLimit.transform.position, upperLimit.transform.position, 0.5f);
		float measurement = (Vector3.Distance (lowerLimit.transform.position, upperLimit.transform.position));
		if (rulerOnCollider) {
			rulerText.text = (measurement / FindGrandestParent(intersectingCollider.transform).localScale.x).ToString () + "\nMeters (Adjusted)";
		} else {
			//Sets the text above the line renderer to be the distance between lowerLimit and upperLimit
			rulerText.text = measurement.ToString() + "\nMeters (Raw)";
		}
	}


	/* Compares the top transforms of two colliders. Returns true if they are equal.
	 * Collider lowCollider: the collider intersecting with the lowerLimit object (in this script)
	 * Collider upCollider: the collider intersecting with the upperLimit object (in this script)
	 */
	bool CompareColliders(Collider lowCollider, Collider upCollider)
	{
		if (lowCollider == null || upCollider == null) 
		{
			return false;
		} 
		else 
		{
			Transform lowerGrand = FindGrandestParent (lowCollider.transform);
			Transform upperGrand = FindGrandestParent (upCollider.transform);
			if (lowerGrand == upperGrand) 
			{
				return true;
			} 
			else 
			{
				return false;
			}
		}
	}

	//Returns the top transform in a given transform's hierarchy.
	Transform FindGrandestParent (Transform trans)
	{
		while (trans.parent != null)
		{
			trans = trans.parent;
		}
		return trans;

	}


	/* Toggles the visibility of all parts of the ruler
	 * bool show: Shows ruler if true, hides ruler if false
	 */
	void ShowRuler(bool show)
	{
		lowerLimit.GetComponent<MeshRenderer> ().enabled = show;
		upperLimit.GetComponent<MeshRenderer> ().enabled = show;
		rulerText.GetComponent<MeshRenderer> ().enabled = show;
		rulerLR.enabled = show;
	}

}


