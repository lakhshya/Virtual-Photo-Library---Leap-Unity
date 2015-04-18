using UnityEngine;
using System.Collections;
using System;
using System.IO;

/// <summary>
/// Script for the book.</summary>
/// <remarks>
/// Controls the book's positions, movement, pages, cover etc.</remarks>
public class BookScript : MonoBehaviour
{
	/// <summary>
	/// GameObject of the page model prefab.</summary>
	public GameObject pagePrefab;
	/// <summary>
	/// GameObject of the CPU.</summary>
	public GameObject CPU;
	/// <summary>
	/// GameObject of the book cover associated with the book.</summary>
	public GameObject bookCover;
	/// <summary>
	/// Name of the folder that the album represents.</summary>
	public string folderName;
	/// <summary>
	/// Audio Clip to be played when the book lands on the table.</summary>
	public AudioClip bookOnTable;
	/// <summary>
	/// Audio Clip to be played when a page is flipped.</summary>
	public AudioClip pageTurn;
	/// <summary>
	/// List of the names of the images in the folder named folderName.</summary>
	private string[] imageList;
	/// <summary>
	/// Instance of the PageScript object of the left page.</summary>
	private PageScript pageLeftScript;
	/// <summary>
	/// Instance of the PageScript object of the right page.</summary>
	private PageScript pageRightScript;
	/// <summary>
	/// The current sheet number. Value starts from 1.
	/// </summary>
	private int currentSheetNum;

	/// <summary>
	/// 3D vector storing the position of the book on the table.</summary>
	Vector3 tableVector = new Vector3 (0, 0, 0);
	/// <summary>
	/// 3D vector storing the position of the book on the shelf.</summary>
	Vector3 shelfVector;
	/// <summary>
	/// Speed at which the book moves from the shelf to the table and back.</summary>
	float moveSpeed;
	/// <summary>
	/// Speed at which the book rotates when it moves from the shelf to the table and back.</summary>
	float rotateSpeed;
	/// <summary>
	/// Stores the action/animation being performed on the book at the moment.</summary>
	int actionType;
	/// <summary>
	/// Scalar distance between the animations of the book.</summary>
	float moveMagnitude;

	/// <summary>
	/// Value for actionType. Do nothing.</summary>
	public readonly static int DO_NOTHING = 0;
	/// <summary>
	/// Value for actionType. Move book to the shelf.</summary>
	public readonly static int TO_SHELF = 1;
	/// <summary>
	/// Value for actionType. Move book to the table.</summary>
	public readonly static int TO_TABLE = 2;

	/// <summary>
	/// Initializes the book by creating the first left and right pages. Loads the images into imageList.</summary>
	public void mInitialize ()
	{
		currentSheetNum = 1;
//		string relativeFolder = "/Users/lakhshya/Documents/Code/Virtual-Photo-Library---Leap-Unity/Assets/Resources/" + folderName;
//		imageList = Directory.GetFiles (@relativeFolder, "*.JPG");
//		for (int i = 0; i < imageList.Length; i++) {
//			imageList [i] = folderName + "/" + Path.GetFileNameWithoutExtension (imageList[i]);
//		}

		imageList = new string[40];
		int init = 0;
		if (folderName.Contains ("0"))
			init = 1;
		else if (folderName.Contains ("1"))
			init = 41;
		else if (folderName.Contains ("2"))
			init = 81;
		if (folderName.Contains ("3"))
			init = 121;
		for (int i =0; i<40; i++) {
			imageList [i] = folderName + "/" + "Digital Universe (" + (i + init) + ")";
		}

		GameObject obj = (GameObject)Instantiate (pagePrefab, new Vector3 (0.75f, 0, 0), Quaternion.identity);
		pageLeftScript = obj.GetComponent<PageScript> ();
		pageLeftScript.mInitialize (0, null, null);

		obj = (GameObject)Instantiate (pagePrefab, new Vector3 (0.75f, 0, 0), Quaternion.identity);
		pageRightScript = obj.GetComponent<PageScript> ();
		pageRightScript.mInitialize (1, imageList [0], imageList [1]);
	}

	/// <summary>
	/// Called when a left swipe is detected. Turns the right page if there are pages left. Plays the pageTurn audio clip.</summary>
	public void onSwipeLeft ()
	{
		if (currentSheetNum * 2 + 2 <= imageList.Length) {
			AudioSource.PlayClipAtPoint (pageTurn, Vector3.zero);
			currentSheetNum++;
			PageScript temp = pageLeftScript;
			pageLeftScript = pageRightScript;
			GameObject obj = (GameObject)Instantiate (pagePrefab, new Vector3 (0.75f, 0, 0), Quaternion.identity);
			pageRightScript = obj.GetComponent<PageScript> ();
			pageRightScript.mInitialize (currentSheetNum, imageList [currentSheetNum * 2 - 2], imageList [currentSheetNum * 2 - 1]);
			pageLeftScript.rotateLeft (40, temp.gameObject);
		}
	}

	/// <summary>
	/// Called when a right swipe is detected. Turns the left page if there are pages left. Plays the pageTurn audio clip.</summary>
	public void onSwipeRight ()
	{
		if (currentSheetNum == 2) {
			AudioSource.PlayClipAtPoint (pageTurn, Vector3.zero);
			currentSheetNum--;
			PageScript temp = pageRightScript;
			pageRightScript = pageLeftScript;
			GameObject obj = (GameObject)Instantiate (pagePrefab, new Vector3 (-0.75f, 0, 0), Quaternion.Euler (new Vector3 (0, 180, 0)));
			pageLeftScript = obj.GetComponent<PageScript> ();
			pageLeftScript.mInitialize (currentSheetNum - 1, null, null);
			pageRightScript.rotateRight (40, temp.gameObject);
		} else if (currentSheetNum > 1) {
			AudioSource.PlayClipAtPoint (pageTurn, Vector3.zero);
			currentSheetNum--;
			PageScript temp = pageRightScript;
			pageRightScript = pageLeftScript;
			GameObject obj = (GameObject)Instantiate (pagePrefab, new Vector3 (-0.75f, 0, 0), Quaternion.Euler (new Vector3 (0, 180, 0)));
			pageLeftScript = obj.GetComponent<PageScript> ();
			pageLeftScript.mInitialize (currentSheetNum - 1, imageList [(currentSheetNum - 1) * 2 - 2], imageList [(currentSheetNum - 1) * 2 - 1]);
			pageRightScript.rotateRight (40, temp.gameObject);
		}
	}

	/// <summary>
	/// Closes the book and initiates it's animation back to the shelf.</summary>
	public void closeBook ()
	{
		bookCover.GetComponent<BookCoverScript> ().closeBook (40, pageLeftScript.gameObject, pageRightScript.gameObject);
		pageLeftScript.rotateRight (50, null);
	}

	/// <summary>
	/// Opens the book. Should be called only after it has reached the table.</summary>
	public void openBook ()
	{
		mInitialize ();
		bookCover.GetComponent<BookCoverScript> ().openBook (50);
		pageLeftScript.rotateLeft (40, null);
	}

	/// <summary>
	/// Moves the book to the shelf from the table.</summary>
	public void moveToShelf ()
	{
		actionType = TO_SHELF;
	}

	/// <summary>
	/// Moves the book to the table from the shelf.</summary>
	public void moveToTable ()
	{
		actionType = TO_TABLE;
	}


	/// <summary>
	/// Called when the script is initialized.</summary>
	void Start ()
	{	
		shelfVector = transform.position;
		moveMagnitude = (tableVector - shelfVector).magnitude;
		moveSpeed = 0.2f * moveMagnitude;
		rotateSpeed = 0.2f * 90;
	}
	
	/// <summary>
	/// Function called on every new frame. Performs the animations of book from the shelf to the table and back.</summary>
	void Update ()
	{
		if (actionType != DO_NOTHING) {
			if (actionType == TO_SHELF) {
				if ((transform.position - shelfVector).magnitude < 0.01f * moveMagnitude) {
					actionType = DO_NOTHING;
					transform.position = shelfVector;
					transform.eulerAngles = new Vector3 (-90, 0, 0);
					CPU.GetComponent<CPU_Script> ().onBookReachesShelf ();
				} else {
					Vector3 vDirection = shelfVector - tableVector;
					transform.Translate (vDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
					transform.Rotate (Vector3.left * rotateSpeed * Time.deltaTime, Space.World);
				}
			} else if (actionType == TO_TABLE) {
				if ((transform.position - tableVector).magnitude < 0.01f * moveMagnitude) {
					actionType = DO_NOTHING;
					transform.position = tableVector;
					transform.eulerAngles = new Vector3 (0, 0, 0);
					AudioSource.PlayClipAtPoint (bookOnTable, Vector3.zero);
					openBook ();
				} else {
					Vector3 vDirection = tableVector - shelfVector;
					transform.Translate (vDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
					transform.Rotate (Vector3.right * rotateSpeed * Time.deltaTime, Space.World);
				}
			}
		}
	}
}
