using UnityEngine;
using System.Collections;
using Leap;

/// <summary>
/// Script for the LEAP hand controller placed at the shelf.</summary>
/// <remarks>
/// Detects gestures and informs the CPU.</remarks>
public class LeapScriptShelf : MonoBehaviour
{

	/// <summary>
	/// Leap Hand Controller.</summary>
	Controller controller;
	/// <summary>
	/// GameObject of the CPU.</summary>
	public GameObject CPU;

	/// <summary>
	/// Called when the script is initialized.</summary>
	void Start ()
	{
		controller = new Controller ();
		setInactive ();
	}

	/// <summary>
	/// Activates the Leap Sensor Game Object.</summary>
	public void setActive ()
	{
		this.gameObject.SetActive (true);
	}

	/// <summary>
	/// Deactivates the Leap Sensor Game Object.</summary>
	public void setInactive ()
	{
		this.gameObject.SetActive (false);
	}

	/// <summary>
	/// Function called on every new frame. Detects gestures and forwards the event.</summary>
	void Update ()
	{
		int bookNum = 0;
		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		foreach (Hand hand in hands) {
			if (hand.PinchStrength == 1) {
				float x = hand.Fingers.FingerType (Finger.FingerType.TYPE_INDEX) [0].TipPosition.x;
				float y = hand.Fingers.FingerType (Finger.FingerType.TYPE_INDEX) [0].TipPosition.y;
				if (x >= 0) {
					if (y >= 140) {
						bookNum = 2;
					} else {
						bookNum = 3;
					}
				} else {
					if (y >= 140) {
						bookNum = 0;
					} else {
						bookNum = 1;
					}
				}
				CPU.GetComponent<CPU_Script> ().onBookSelect (bookNum);
				break;
			}
		}
	}
}
