using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class BookScript : MonoBehaviour
{
	public GameObject pagePrefab;
	public GameObject bookCover;
	private string[] imageList;
	private PageScript pageLeftScript, pageRightScript;
	private int currentSheetNum; //starts from 1

	public void mInitialize (string folderName)
	{
		currentSheetNum = 1;
		imageList = Directory.GetFiles (@folderName, "*.jpg");
		for (int i = 0; i < imageList.Length; i++) {
			imageList [i] = Path.GetFileNameWithoutExtension (imageList[i]);
		}
//		GameObject obj = (GameObject)Instantiate (pagePrefab, new Vector3 (-0.75f, 0, 0), Quaternion.Euler(new Vector3(0,180,0)));
//		pageLeftScript = obj.GetComponent<PageScript> ();
//		pageLeftScript.mInitialize (0, null, null);

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
		Debug.Log (currentSheetNum);

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
		//Destroy (pageLeftScript.gameObject);
		//Destroy (pageRightScript.gameObject);
		bookCover.GetComponent<BookCoverScript> ().closeBook (40, pageLeftScript.gameObject, pageRightScript.gameObject);
		pageLeftScript.rotateRight (50, null);
	}

	public void openBook (string folderName) {
		mInitialize (folderName);
		bookCover.GetComponent<BookCoverScript> ().openBook (50);
		pageLeftScript.rotateLeft (40, null);
	}

	// Use this for initialization
	void Start ()
	{	
		//openBook ("/Users/lakhshya/Documents/Code/Leap Project/C# Unity Leap/Assets/Resources");

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
