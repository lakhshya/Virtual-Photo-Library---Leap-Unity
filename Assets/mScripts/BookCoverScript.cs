using UnityEngine;
using System.Collections;

/// <summary>
/// Script for the over of the book.</summary>
/// <remarks>
/// Provides functionality to close and open the book cover</remarks>
public class BookCoverScript : MonoBehaviour
{
	/// <summary>
	/// GameObject of the Left Cover of the book.</summary>
	public GameObject leftCover;
	/// <summary>
	/// GameObject of the Right Cover of the book.</summary>
	public GameObject rightCover;
	/// <summary>
	/// GameObject of the book.</summary>
	public GameObject book;
	/// <summary>
	/// GameObject of the CPU.</summary>
	public GameObject CPU;
	/// <summary>
	/// Stores the action/animation being performed on the book cover at the moment.</summary>
	private int actionType;
	/// <summary>
	/// Value for actionType. Do nothing.</summary>
	public readonly static int DO_NOTHING = 0;
	/// <summary>
	/// Value for actionType. Open the book.</summary>
	public readonly static int BOOK_OPEN = 1;
	/// <summary>
	/// Value for actionType. Close the book.</summary>
	public readonly static int BOOK_CLOSE = 2;
	/// <summary>
	/// GameObject of the object to destroy.</summary>
	private GameObject destroyObject1;
	/// <summary>
	/// GameObject of the object to destroy.</summary>
	private GameObject destroyObject2;
	/// <summary>
	/// Speed at which the book should rotate while opening and closing.</summary>
	private float rotateSpeed;

	/// <summary>
	/// Opens the book.</summary>
	/// <param name="rotateSpeed"> Speed at which book should be opened.</param>
	public void openBook (float rotateSpeed)
	{
		actionType = BOOK_OPEN;
		this.rotateSpeed = rotateSpeed;
	}

	/// <summary>
	/// Closes the book.</summary>
	/// <param name="rotateSpeed"> Speed at which book should be closed.</param>
	/// <param name="destroyObject1"> Page object that should be destroyed.</param>
	/// <param name="destroyObject2"> Page object that should be destroyed.</param>
	public void closeBook (float rotateSpeed, GameObject destroyObject1, GameObject destroyObject2)
	{
		actionType = BOOK_CLOSE;
		this.rotateSpeed = rotateSpeed;
		this.destroyObject1 = destroyObject1;
		this.destroyObject2 = destroyObject2;
	}

	/// <summary>
	/// Called when the script is initialized.</summary>
	void Start ()
	{
	}
	
	/// <summary>
	/// Function called on every new frame. Performs the animations of book open and book close.</summary>
	void Update ()
	{
		if (actionType != DO_NOTHING) {
			if (actionType == BOOK_OPEN) {
				leftCover.transform.RotateAround (Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);
				if (leftCover.transform.eulerAngles.y >= 358.5f) {
					actionType = DO_NOTHING;
					leftCover.transform.localEulerAngles = new Vector3 (0, 0, 0);
					CPU.GetComponent<CPU_Script> ().onBookOpenFinish (book);
				}
			} else if (actionType == BOOK_CLOSE) {
				leftCover.transform.RotateAround (Vector3.zero, Vector3.down, rotateSpeed * Time.deltaTime);
				if (leftCover.transform.eulerAngles.y <= 181.5f) {
					actionType = DO_NOTHING;
					Destroy (destroyObject1);
					Destroy (destroyObject2);
					leftCover.transform.localEulerAngles = new Vector3 (0, 0, 180);
					CPU.GetComponent<CPU_Script> ().onBookCloseFinish (book);
				}
			}
		}
	}
}
