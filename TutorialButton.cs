using UnityEngine;
using System.Collections;

public class TutorialButton : MonoBehaviour {

	public bool on = false;

	public ButtonDemoGraphics onGraphics;
	public ButtonDemoGraphics offGraphics;
	public ButtonDemoGraphics midGraphics;
	public ButtonDemoGraphics botGraphics;
	
	public Color MidGraphicsOnColor = new Color(0.0f, 0.5f, 0.5f, 1.0f);
	public Color BotGraphicsOnColor = new Color(0.0f, 1.0f, 1.0f, 1.0f);
	public Color MidGraphicsOffColor = new Color(0.0f, 0.5f, 0.5f, 0.1f);
	public Color BotGraphicsOffColor = new Color(0.0f, 0.25f, 0.25f, 1.0f);

	
	private void TurnsOnGraphics()
	{
		onGraphics.SetActive(true);
		offGraphics.SetActive(false);
		midGraphics.SetColor(MidGraphicsOnColor);
		botGraphics.SetColor(BotGraphicsOnColor);
	}
	
	private void TurnsOffGraphics()
	{
		onGraphics.SetActive(false);
		offGraphics.SetActive(true);
		midGraphics.SetColor(MidGraphicsOffColor);
		botGraphics.SetColor(BotGraphicsOffColor);
	}
	// Update is called once per frame
	void Update () {
	if (on) {
			TurnsOnGraphics ();
		} else {
			TurnsOffGraphics ();
		}
	}
}
