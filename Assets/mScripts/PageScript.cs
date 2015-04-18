using UnityEngine;
using System.Collections;
using System;
using System.IO;

/// <summary>
/// Script for each page of the book.</summary>
/// <remarks>
/// Provides functionality for page rotation and putting pictures</remarks>
public class PageScript : MonoBehaviour
{
	/// <summary>
	/// GameObject that displays the front image.</summary>
	public GameObject imgFront;
	/// <summary>
	/// GameObject that displays the back image.</summary>
	public GameObject imgBack;
	/// <summary>
	/// Sheet number of the page. Begins from 1.</summary>
	private int sheetNum;
	/// <summary>
	/// File name of the front image.</summary>
	private string imgNameFront;
	/// <summary>
	/// File name of the back image.</summary>
	private string imgNameBack;
	/// <summary>
	/// Stores the action/animation being performed on the page at the moment.</summary>
	private int actionType;
	/// <summary>
	/// GameObject of the page to be destroyed.</summary>
	private GameObject destroyObject;
	/// <summary>
	/// Speed at which the page rotates when it is swiped/turned.</summary>
	private float rotateSpeed;

	/// <summary>
	/// Value for actionType. Do nothing.</summary>
	public readonly static int DO_NOTHING = 0;
	/// <summary>
	/// Value for actionType. Rotate the page to the left.</summary>
	public readonly static int ROTATE_LEFT = 1;
	/// <summary>
	/// Value for actionType. Rotate the page to the right.</summary>
	public readonly static int ROTATE_RIGHT = 2;

	/// <summary>
	/// Rotates the page to the left.</summary>
	/// <param name="rotateSpeed"> Speed at which page should be turned.</param>
	/// <param name="destroyObject"> Page object that should be destroyed.</param>
	public void rotateLeft(float rotateSpeed, GameObject destroyObject) {
		Destroy (this.destroyObject);
		this.destroyObject = destroyObject;
		this.rotateSpeed = rotateSpeed;
		actionType = ROTATE_LEFT;
	}

	/// <summary>
	/// Rotates the page to the right.</summary>
	/// <param name="rotateSpeed"> Speed at which page should be turned.</param>
	/// <param name="destroyObject"> Page object that should be destroyed.</param>
	public void rotateRight(float rotateSpeed, GameObject destroyObject) {
		Destroy (this.destroyObject);
		this.destroyObject = destroyObject;
		this.rotateSpeed = rotateSpeed;
		actionType = ROTATE_RIGHT;
	}

	/// <summary>
	/// Initializes the page object. Loads images from the resources folder and assigns it to the page as textures.</summary>
	/// <param name="sheetNumber"> Sheet number of the page.</param>
	/// <param name="imgNameFront"> Name of the image to be displayed on the front of the page.</param>
	/// <param name="imgNameBack"> Name of the image to be displayed on the back of the page.</param>
	public void mInitialize (int sheetNumber, string imgNameFront, string imgNameBack)
	{
		sheetNum = sheetNumber;
		this.imgNameFront = imgNameFront;
		this.imgNameBack = imgNameBack;

		if (this.imgNameFront != null) {
			imgFront = this.transform.Find ("Image Front").gameObject;
//			Texture2D tex1 = (Texture2D)Resources.LoadAssetAtPath(imgNameFront, typeof(Texture2D));
			Texture2D tex1 = Resources.Load (@imgNameFront) as Texture2D;
			imgFront.GetComponent<Renderer> ().material.mainTexture = tex1;
		}

		if (this.imgNameBack != null) {
			imgBack = this.transform.Find ("Image Back").gameObject;
//			Texture2D tex2 = (Texture2D)Resources.LoadAssetAtPath(imgNameBack, typeof(Texture2D));
			Texture2D tex2 = Resources.Load (@imgNameBack) as Texture2D;
			imgBack.GetComponent<Renderer> ().material.mainTexture = tex2;
		}
	}

	/// <summary>
	/// Called when the script is initialized.</summary>
	void Start ()
	{

	}
	
	/// <summary>
	/// Function called on every new frame. Performs the animations of page turn left and right.</summary>
	void Update ()
	{
		if (actionType != 0) {
			if (actionType == ROTATE_LEFT) {
				transform.RotateAround (Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);
				if (transform.eulerAngles.y >= 178.5f) {
					actionType = DO_NOTHING;
					Destroy(destroyObject);
				}
			}
			else if (actionType == ROTATE_RIGHT) {
				transform.RotateAround (Vector3.zero, Vector3.down, rotateSpeed * Time.deltaTime);
				if (transform.eulerAngles.y <= 1.5f) {
					actionType = DO_NOTHING;
					Destroy(destroyObject);
				}
			}
		}
	}
}
