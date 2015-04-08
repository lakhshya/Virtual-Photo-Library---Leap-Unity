using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class BookScript : MonoBehaviour
{
	public GameObject pagePrefab;
	public GameObject bookCover;
	public string folderName;
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
		string relativeFolder = "/Users/lakhshya/Documents/Code/Virtual-Photo-Library---Leap-Unity/Assets/Resources/" + folderName;
		imageList = Directory.GetFiles (@relativeFolder, "*.jpg");
		for (int i = 0; i < imageList.Length; i++) {
			imageList [i] = "Assets/Resources/" + folderName + "/" + Path.GetFileName (imageList[i]);
			Debug.Log (imageList[i]);
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
			currentSheetNum--;
			PageScript temp = pageRightScript;
			pageRightScript = pageLeftScript;
			GameObject obj = (GameObject)Instantiate (pagePrefab, new Vector3 (-0.75f, 0, 0), Quaternion.Euler(new Vector3(0,180,0)));
			pageLeftScript = obj.GetComponent<PageScript> ();
			pageLeftScript.mInitialize (currentSheetNum - 1, null,null);
			pageRightScript.rotateRight (40, temp.gameObject);
		}else if (currentSheetNum > 1) {
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

	// Use this for initialization
	void Start ()
	{	
		shelfVector = transform.position;
		moveMagnitude = (tableVector - shelfVector).magnitude;
		moveSpeed = 0.2f * moveMagnitude;
		rotateSpeed = 0.2f * 90;
		//if(folderName == "Album1")
		//	actionType = TO_TABLE;
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
