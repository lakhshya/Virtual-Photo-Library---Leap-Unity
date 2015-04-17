using UnityEngine;
using System.Collections;

/// <summary>
/// The CPU that controls the entire application.</summary>
/// <remarks>
/// Should be linked to n empty object</remarks>
public class CPU_Script : MonoBehaviour {

	public GameObject LeapShelf, LeapTable, Camera;
	public GameObject[] Books;
	public AudioClip bookClose, bookTransition;

	// Use this for initialization
	void Start () {
		Camera.GetComponent<CameraScript>().moveToShelfInit();
		//onBookSelect (Books[1]);
		//audioSource.Play ();
		//AudioSource.PlayClipAtPoint (audioClip, Vector3.zero);
		//onBookSelect (1);
	}

	public void onCameraInitFinish() {
		LeapShelf.GetComponent<LeapScriptShelf> ().setActive ();
	}

	public void onBookOpenFinish(GameObject book) {
		AudioSource.PlayClipAtPoint (bookClose, Vector3.zero);
		LeapTable.GetComponent<LeapScriptTable> ().setActive (book);
	}

	public void onBookCloseFinish(GameObject book) {
		AudioSource.PlayClipAtPoint (bookClose, Vector3.zero);
		book.GetComponent<BookScript>().moveToShelf();
		Camera.GetComponent<CameraScript>().moveToShelf();
		LeapTable.GetComponent<LeapScriptTable> ().setInactive ();
		AudioSource.PlayClipAtPoint (bookTransition, Vector3.zero);

	}

	public void onBookReachesShelf() {
		LeapShelf.GetComponent<LeapScriptShelf> ().setActive ();
	}

	public void onBookSelected(GameObject book) {
		book.GetComponent<BookScript>().moveToTable();
		Camera.GetComponent<CameraScript>().moveToTable();
		LeapShelf.GetComponent<LeapScriptShelf> ().setInactive ();
	}

	public void onBookSelect(int bookNum) {
		LeapShelf.GetComponent<LeapScriptShelf> ().setInactive ();
		Books[bookNum].GetComponent<BookScript>().moveToTable();
		Camera.GetComponent<CameraScript>().moveToTable();
		AudioSource.PlayClipAtPoint (bookTransition, Vector3.zero);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
