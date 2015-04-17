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
	public GameObject pagePrefab;
	public GameObject CPU;
	public GameObject bookCover;
	public string folderName;
	public AudioClip bookOnTable, pageTurn;
	private string[] imageList;
	private PageScript pageLeftScript, pageRightScript;
	/// <summary>
	/// The current sheet number. Value starts from 1
	/// </summary>
	private int currentSheetNum;

	Vector3 tableVector = new Vector3 (0, 0, 0);
	Vector3 shelfVector;
	float moveSpeed;
	float rotateSpeed;
	int actionType;
	float moveMagnitude;

	public readonly static int DO_NOTHING = 0;
	public readonly static int TO_SHELF = 1;
	public readonly static int TO_TABLE = 2;

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
			imageList[i] = folderName + "/" + "Digital Universe (" + (i+init) +")";
		}

		GameObject obj = (GameObject)Instantiate (pagePrefab, new Vector3 (0.75f, 0, 0), Quaternion.identity);
		pageLeftScript = obj.GetComponent<PageScript> ();
		pageLeftScript.mInitialize (0, null, null);

		obj = (GameObject)Instantiate (pagePrefab, new Vector3 (0.75f, 0, 0), Quaternion.identity);
		pageRightScript = obj.GetComponent<PageScript> ();
		pageRightScript.mInitialize (1, imageList[0], imageList[1]);
	}

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

	public void onSwipeRight ()
	{
		if (currentSheetNum == 2) {
			AudioSource.PlayClipAtPoint (pageTurn, Vector3.zero);
			currentSheetNum--;
			PageScript temp = pageRightScript;
			pageRightScript = pageLeftScript;
			GameObject obj = (GameObject)Instantiate (pagePrefab, new Vector3 (-0.75f, 0, 0), Quaternion.Euler(new Vector3(0,180,0)));
			pageLeftScript = obj.GetComponent<PageScript> ();
			pageLeftScript.mInitialize (currentSheetNum - 1, null,null);
			pageRightScript.rotateRight (40, temp.gameObject);
		}else if (currentSheetNum > 1) {
			AudioSource.PlayClipAtPoint (pageTurn, Vector3.zero);
			currentSheetNum--;
			PageScript temp = pageRightScript;
			pageRightScript = pageLeftScript;
			GameObject obj = (GameObject)Instantiate (pagePrefab, new Vector3 (-0.75f, 0, 0), Quaternion.Euler(new Vector3(0,180,0)));
			pageLeftScript = obj.GetComponent<PageScript> ();
			pageLeftScript.mInitialize (currentSheetNum - 1, imageList [(currentSheetNum-1) * 2 - 2], imageList [(currentSheetNum-1) * 2 - 1]);
			pageRightScript.rotateRight (40, temp.gameObject);
		}
	}

	public void closeBook () {
		bookCover.GetComponent<BookCoverScript> ().closeBook (40, pageLeftScript.gameObject, pageRightScript.gameObject);
		pageLeftScript.rotateRight (50, null);
	}

	public void openBook () {
		mInitialize ();
		bookCover.GetComponent<BookCoverScript> ().openBook (50);
		pageLeftScript.rotateLeft (40, null);
	}

	public void moveToShelf() {
		actionType = TO_SHELF;
	}

	public void moveToTable() {
		actionType = TO_TABLE;
	}



	// Use this for initialization
	void Start ()
	{	
		shelfVector = transform.position;
		moveMagnitude = (tableVector - shelfVector).magnitude;
		moveSpeed = 0.2f * moveMagnitude;
		rotateSpeed = 0.2f * 90;
//		if(folderName == "Album1")
//			actionType = TO_TABLE;
		//openBook ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (actionType != DO_NOTHING) {
			if (actionType == TO_SHELF) {
				if ((transform.position - shelfVector).magnitude < 0.01f * moveMagnitude) {
					actionType = DO_NOTHING;
					transform.position = shelfVector;
					transform.eulerAngles = new Vector3(-90,0,0);
					CPU.GetComponent<CPU_Script>().onBookReachesShelf();
				} else {
					Vector3 vDirection = shelfVector - tableVector;
					transform.Translate (vDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
					transform.Rotate (Vector3.left * rotateSpeed * Time.deltaTime, Space.World);
				}
			} else if (actionType == TO_TABLE) {
				if ((transform.position - tableVector).magnitude < 0.01f * moveMagnitude) {
					actionType = DO_NOTHING;
					transform.position = tableVector;
					transform.eulerAngles = new Vector3(0,0,0);
					AudioSource.PlayClipAtPoint (bookOnTable, Vector3.zero);
					openBook();
				} else {
					Vector3 vDirection = tableVector - shelfVector;
					transform.Translate (vDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
					transform.Rotate (Vector3.right * rotateSpeed * Time.deltaTime, Space.World);
				}
			}
		}
	}
}
