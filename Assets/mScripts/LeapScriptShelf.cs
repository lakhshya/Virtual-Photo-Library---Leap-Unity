using UnityEngine;
using System.Collections;
using Leap;

public class LeapScriptShelf : MonoBehaviour {

	Controller controller;
	public GameObject CPU;

	void Start ()
	{
		controller = new Controller ();
		setInactive ();
	}
	
	public void setActive ()
	{
		this.gameObject.SetActive (true);
	}
	
	public void setInactive ()
	{
		this.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		int bookNum = 0;
		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
		foreach (Hand hand in hands) {
			if(hand.PinchStrength ==1 ) {
				float x = hand.Fingers.FingerType(Finger.FingerType.TYPE_INDEX)[0].TipPosition.x;
				float y = hand.Fingers.FingerType(Finger.FingerType.TYPE_INDEX)[0].TipPosition.y;
				if(x >= 0 ) {
					if(y>=140) {
						bookNum = 2;
					}else {
						bookNum = 3;
					}
				}
				else {
					if(y>=140) {
						bookNum = 0;
					}else {
						bookNum = 1;
					}
				}
				CPU.GetComponent<CPU_Script>().onBookSelect(bookNum);
				break;
			}
		}
	}
}
