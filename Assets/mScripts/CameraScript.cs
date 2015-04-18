using UnityEngine;
using System.Collections;

/// <summary>
/// Script for the Camera Game Object.</summary>
/// <remarks>
/// Provides functionality for camera movements and animations</remarks>
public class CameraScript : MonoBehaviour
{
	/// <summary>
	/// GameObject of the CPU.</summary>
	public GameObject CPU;
	/// <summary>
	/// 3D vector storing the position of the camera when focus is on the table.</summary>
	Vector3 bookVector = new Vector3 (0, 0, -2.6f);
	/// <summary>
	/// 3D vector storing the position of the camera when focus is on the shelf.</summary>
	Vector3 shelfVector = new Vector3 (0, 2.3f, -2.82f);
	/// <summary>
	/// Speed at which the camera moves from the shelf to the table and back.</summary>
	float moveSpeed;
	/// <summary>
	/// Speed at which the camera rotates when it moves from the shelf to the table and back.</summary>
	float rotateSpeed;
	/// <summary>
	/// Stores the action/animation being performed on the camera at the moment.</summary>
	int actionType;
	/// <summary>
	/// Scalar distance between the animations of the camera.</summary>
	float moveMagnitude;

	/// <summary>
	/// Value for actionType. Do nothing.</summary>
	public readonly static int DO_NOTHING = 0;
	/// <summary>
	/// Value for actionType. Move camera to the shelf.</summary>
	public readonly static int TO_SHELF = 1;
	/// <summary>
	/// Value for actionType. Move camera to the table.</summary>
	public readonly static int TO_TABLE = 2;
	/// <summary>
	/// Value for actionType. Move camera initally to the shelf from the text. Should be used only during program start and never again.</summary>
	public readonly static int TO_SHELF_INIT = 4;

	/// <summary>
	/// Called when the script is initialized. Assigns values to the moveMagnitude, moveSpeed and rotateSpeed.</summary>
	void Start ()
	{
		moveMagnitude = (bookVector - shelfVector).magnitude;
		moveSpeed = 0.2f * moveMagnitude;
		rotateSpeed = 0.2f * 90;
		//actionType = TO_TABLE;
	}

	/// <summary>
	/// Moves the camera to the shelf from the table.</summary>
	public void moveToShelf ()
	{
		actionType = TO_SHELF;
	}

	/// <summary>
	/// Moves the camera to the shelf from the initial text focus location of the camera. Should be used only during program start and never again.</summary>
	public void moveToShelfInit ()
	{
		actionType = TO_SHELF_INIT;
	}
	
	/// <summary>
	/// Moves the camera to the table from the shelf.</summary>
	public void moveToTable ()
	{
		actionType = TO_TABLE;
	}

	
	/// <summary>
	/// Function called on every new frame. Performs the animations of camera from the shelf to the table and back.</summary>
	void Update ()
	{
		if (actionType != DO_NOTHING) {
			if (actionType == TO_SHELF) {
				if ((transform.position - shelfVector).magnitude < 0.01f * moveMagnitude) {
					actionType = DO_NOTHING;
					transform.position = shelfVector;
					transform.eulerAngles = new Vector3 (-90, 0, 0);
				} else {
					Vector3 vDirection = shelfVector - bookVector;
					transform.Translate (vDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
					transform.Rotate (Vector3.left * rotateSpeed * Time.deltaTime, Space.World);
				}
			} else if (actionType == TO_TABLE) {
				if ((transform.position - bookVector).magnitude < 0.01f * moveMagnitude) {
					actionType = DO_NOTHING;
					transform.position = bookVector;
					transform.eulerAngles = new Vector3 (0, 0, 0);
				} else {
					Vector3 vDirection = bookVector - shelfVector;
					transform.Translate (vDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
					transform.Rotate (Vector3.right * rotateSpeed * Time.deltaTime, Space.World);
				}
			} else if (actionType == TO_SHELF_INIT) {
				if ((transform.position - shelfVector).magnitude < 0.05f * moveMagnitude) {
					actionType = DO_NOTHING;
					transform.position = shelfVector;
					CPU.GetComponent<CPU_Script> ().onCameraInitFinish ();
				} else {
					Vector3 vDirection = shelfVector - transform.position;
					transform.Translate (vDirection.normalized * moveSpeed * Time.deltaTime * 5, Space.World);
				}
			} 
		}
	}
}
