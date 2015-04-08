using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

	Vector3 bookVector = new Vector3 (0, 0, -2.6f);
	Vector3 shelfVector = new Vector3 (0, 2.3f, -2.82f);
	float moveSpeed;
	float rotateSpeed;
	bool moveCamera = false;
	int direction;
	public readonly static int TO_SHELF = 1;
	public readonly static int TO_BOOK = 2;
	float moveMagnitude;

	// Use this for initialization
	void Start ()
	{
		moveMagnitude = (bookVector - shelfVector).magnitude;
		moveSpeed = 0.2f * moveMagnitude;
		rotateSpeed = 0.2f * 90;
		moveCamera = true;
		direction = TO_BOOK;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (moveCamera) {
			if (direction == TO_SHELF) {
				if ((transform.position - shelfVector).magnitude < 0.01f * moveMagnitude) {
					moveCamera = false;
					transform.position = shelfVector;
					transform.eulerAngles = new Vector3(-90,0,0);
				} else {
					Vector3 vDirection = shelfVector - bookVector;
					transform.Translate (vDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
					transform.Rotate (Vector3.left * rotateSpeed * Time.deltaTime, Space.World);
				}
			} else if (direction == TO_BOOK) {
				if ((transform.position - bookVector).magnitude < 0.01f * moveMagnitude) {
					moveCamera = false;
					transform.position = bookVector;
					transform.eulerAngles = new Vector3(0,0,0);
				} else {
					Vector3 vDirection = bookVector - shelfVector;
					transform.Translate (vDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
					transform.Rotate (Vector3.right * rotateSpeed * Time.deltaTime, Space.World);
				}
			}
		}
	}
}
