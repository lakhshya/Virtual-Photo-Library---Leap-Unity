using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class PageScript : MonoBehaviour
{
	//public GameObject page;
	public GameObject imgFront, imgBack;
	private int sheetNum;
	private string imgNameFront, imgNameBack;
	private int actionType;
	private GameObject destroyObject;
	private float rotateSpeed;

	public readonly static int DO_NOTHING = 0;
	public readonly static int ROTATE_LEFT = 1;
	public readonly static int ROTATE_RIGHT = 2;

	public void rotateLeft(float rotateSpeed, GameObject destroyObject) {
		Destroy (this.destroyObject);
		this.destroyObject = destroyObject;
		this.rotateSpeed = rotateSpeed;
		actionType = ROTATE_LEFT;
	}

	public void rotateRight(float rotateSpeed, GameObject destroyObject) {
		Destroy (this.destroyObject);
		this.destroyObject = destroyObject;
		this.rotateSpeed = rotateSpeed;
		actionType = ROTATE_RIGHT;
	}

	public void mInitialize (int sheetNumber, string imgNameFront, string imgNameBack)
	{
		sheetNum = sheetNumber;
		this.imgNameFront = imgNameFront;
		this.imgNameBack = imgNameBack;

		if (imgNameFront != null) {
			imgFront = this.transform.Find ("Image Front").gameObject;
			Texture2D tex1 = (Texture2D)Resources.LoadAssetAtPath(imgNameFront, typeof(Texture2D));
			imgFront.GetComponent<Renderer> ().material.mainTexture = tex1;
		}

		if (imgNameBack != null) {
			imgBack = this.transform.Find ("Image Back").gameObject;
			Texture2D tex2 = (Texture2D)Resources.LoadAssetAtPath(imgNameBack, typeof(Texture2D));
			imgBack.GetComponent<Renderer> ().material.mainTexture = tex2;
		}
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
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
