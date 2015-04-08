using UnityEngine;
using System.Collections;
using Leap;

public class LeapScript : MonoBehaviour
{

	Controller controller;
	bool isSwipeGesture;
	public GameObject bookObject;
	
	void Start ()
	{
		controller = new Controller ();
		controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE);
		isSwipeGesture = false;
	}
	
	void Update ()
	{
		Frame frame = controller.Frame ();
		string gestures = "";
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
			//Debug.Log ("onSwipe");
			if (swipeDirection == -1)
				bookObject.GetComponent<BookScript> ().onSwipeLeft ();
			else if (swipeDirection == +1)
				bookObject.GetComponent<BookScript> ().onSwipeRight ();

		} else if (!isSwipeGestureLocal)
			isSwipeGesture = false;
	}
}
