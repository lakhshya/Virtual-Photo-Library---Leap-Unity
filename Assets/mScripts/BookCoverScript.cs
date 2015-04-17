using UnityEngine;
using System.Collections;

public class BookCoverScript : MonoBehaviour
{

	public GameObject leftCover, rightCover, book, CPU;
	private int actionType;
	public readonly static int DO_NOTHING = 0;
	public readonly static int BOOK_OPEN = 1;
	public readonly static int BOOK_CLOSE = 2;
	private GameObject destroyObject1, destroyObject2;
	private float rotateSpeed;

	public void openBook (float rotateSpeed)
	{
		actionType = BOOK_OPEN;
		this.rotateSpeed = rotateSpeed;
	}

	public void closeBook (float rotateSpeed, GameObject destroyObject1, GameObject destroyObject2)
	{
		actionType = BOOK_CLOSE;
		this.rotateSpeed = rotateSpeed;
		this.destroyObject1 = destroyObject1;
		this.destroyObject2 = destroyObject2;
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (actionType != DO_NOTHING) {
			if (actionType == BOOK_OPEN) {
				leftCover.transform.RotateAround (Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);
				if (leftCover.transform.eulerAngles.y >= 358.5f) {
					actionType = DO_NOTHING;
					leftCover.transform.localEulerAngles = new Vector3(0,0,0);
					CPU.GetComponent<CPU_Script>().onBookOpenFinish(book);
				}
			} else if (actionType == BOOK_CLOSE) {
				leftCover.transform.RotateAround (Vector3.zero, Vector3.down, rotateSpeed * Time.deltaTime);
				if (leftCover.transform.eulerAngles.y <= 181.5f) {
					actionType = DO_NOTHING;
					Destroy (destroyObject1);
					Destroy (destroyObject2);
					leftCover.transform.localEulerAngles = new Vector3(0,0,180);
					CPU.GetComponent<CPU_Script>().onBookCloseFinish(book);
				}
			}
		}
	}
}
