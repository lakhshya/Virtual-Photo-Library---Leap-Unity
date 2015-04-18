using UnityEngine;
using System.Collections;

/// <summary>
/// Script for the CPU that controls the entire application.</summary>
/// <remarks>
/// Should be linked to an empty object</remarks>
public class CPU_Script : MonoBehaviour
{

	/// <summary>
	/// GameObject of the Leap Controller at the shelf.</summary>
	public GameObject LeapShelf;
	/// <summary>
	/// GameObject of the Leap Controller at the table.</summary>
	public GameObject LeapTable;
	/// <summary>
	/// GameObject of the Camera.</summary>
	public GameObject Camera;
	/// <summary>
	/// Array of the GameObjects of the books.</summary>
	public GameObject[] Books;
	/// <summary>
	/// Audio Clip to be played on book close.</summary>
	public AudioClip bookClose;
	/// <summary>
	/// Audio Clip to be played on book transition/animation.</summary>
	public AudioClip bookTransition;

	/// <summary>
	/// Called when the script is initialized.</summary>
	void Start ()
	{
		Camera.GetComponent<CameraScript> ().moveToShelfInit ();
		//onBookSelect (Books[1]);
		//audioSource.Play ();
		//AudioSource.PlayClipAtPoint (audioClip, Vector3.zero);
		//onBookSelect (1);
	}

	/// <summary>
	/// Called when camera has finished its initial animation. Activates the Leap Controller at the shelf.</summary>
	public void onCameraInitFinish ()
	{
		LeapShelf.GetComponent<LeapScriptShelf> ().setActive ();
	}

	/// <summary>
	/// Called when the book has finished opening. Plays audio effects.</summary>
	/// <param name="book"> GameObject of the book that finished opening.</param>
	public void onBookOpenFinish (GameObject book)
	{
		AudioSource.PlayClipAtPoint (bookClose, Vector3.zero);
		LeapTable.GetComponent<LeapScriptTable> ().setActive (book);
	}

	/// <summary>
	/// Called when the book has finished closing. Plays audio effects.</summary>
	/// <param name="book"> GameObject of the book that finished closing.</param>
	public void onBookCloseFinish (GameObject book)
	{
		AudioSource.PlayClipAtPoint (bookClose, Vector3.zero);
		book.GetComponent<BookScript> ().moveToShelf ();
		Camera.GetComponent<CameraScript> ().moveToShelf ();
		LeapTable.GetComponent<LeapScriptTable> ().setInactive ();
		AudioSource.PlayClipAtPoint (bookTransition, Vector3.zero);

	}

	/// <summary>
	/// Called when the book reaches the shelf and the animation is over.</summary>
	public void onBookReachesShelf ()
	{
		LeapShelf.GetComponent<LeapScriptShelf> ().setActive ();
	}

	/// <summary>
	/// Called when a book has been selected from the shelf.</summary>
	/// <param name="book"> GameObject of the book that has been selected.</param>
	public void onBookSelected (GameObject book)
	{
		book.GetComponent<BookScript> ().moveToTable ();
		Camera.GetComponent<CameraScript> ().moveToTable ();
		LeapShelf.GetComponent<LeapScriptShelf> ().setInactive ();
	}

	/// <summary>
	/// Called when a book has been selected from the shelf.</summary>
	/// <param name="book"> Index of the book that has been selected.</param>
	public void onBookSelect (int bookNum)
	{
		LeapShelf.GetComponent<LeapScriptShelf> ().setInactive ();
		Books [bookNum].GetComponent<BookScript> ().moveToTable ();
		Camera.GetComponent<CameraScript> ().moveToTable ();
		AudioSource.PlayClipAtPoint (bookTransition, Vector3.zero);

	}
	
	/// <summary>
	/// Function called on every new frame. Detects gestures and forwards the event.</summary>
	void Update ()
	{
	
	}
}
