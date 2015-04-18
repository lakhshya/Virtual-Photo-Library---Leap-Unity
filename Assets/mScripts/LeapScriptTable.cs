using UnityEngine;
using System.Collections;
using Leap;

/// <summary>
/// Script for the LEAP hand controller placed at the table.</summary>
/// <remarks>
/// Detects gestures and informs the CPU.</remarks>
public class LeapScriptTable : MonoBehaviour
{
	/// <summary>
	/// Leap Hand Controller.</summary>
	Controller controller;
	/// <summary>
	/// True if a swipe gesture has been detected in the previous frame.</summary>
	bool isSwipeGesture;
	/// <summary>
	/// The GameObject of the book that is currently open.</summary>
	private GameObject bookObject;

	/// <summary>
	/// Called when the script is initialized.</summary>
	void Start ()
	{
		controller = new Controller ();
		setInactive ();
	}

	/// <summary>
	/// Activates the Leap Sensor Game Object.</summary>
	/// <param name="bookObject"> GameObject of the book that is currently on the table.</param>
	public void setActive (GameObject bookObject)
	{
		this.bookObject = bookObject;
		this.gameObject.SetActive (true);
		controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE);
		isSwipeGesture = false;
	}

	/// <summary>
	/// Deactivates the Leap Sensor Game Object.</summary>
	public void setInactive ()
	{
		this.gameObject.SetActive (false);
		controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE, false);
		isSwipeGesture = false;
	}

	/// <summary>
	/// Function called on every new frame. Detects gestures and forwards the event.</summary>
	void Update ()
	{
		Frame frame = controller.Frame ();

		bool isSwipeGestureLocal = false;
		int swipeDirection = -1;		// -1 = left +1 = right
		foreach (Gesture gesture in frame.Gestures())
			if (gesture.Type == Gesture.GestureType.TYPE_SWIPE) {
				isSwipeGestureLocal = true;
				if ((new SwipeGesture (gesture)).Direction.x > 0) {
					swipeDirection = 1;
				}
			}
		if (!isSwipeGesture && isSwipeGestureLocal) {
			isSwipeGesture = true;
			if (swipeDirection == -1)
				bookObject.GetComponent<BookScript> ().onSwipeLeft ();
			else if (swipeDirection == +1)
				bookObject.GetComponent<BookScript> ().onSwipeRight ();

		} else if (!isSwipeGestureLocal) {
			isSwipeGesture = false;
		}

		HandList hands = frame.Hands;
		
		if (hands.Count == 2) {
			float handA_x = 0, handB_x = 0;
			foreach (Hand hand in hands) {
				if (handA_x == 0) {
					handA_x = hand.PalmPosition.x;
				} else {
					handB_x = hand.PalmPosition.x;
				}
			}
			
			if (Mathf.Abs (handA_x - handB_x) < 30) {
				bookObject.GetComponent<BookScript> ().closeBook ();
			}
			
		}

	}
}
