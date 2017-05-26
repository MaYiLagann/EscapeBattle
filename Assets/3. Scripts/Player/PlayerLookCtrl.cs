using UnityEngine;
using System.Collections;

public class PlayerLookCtrl : MonoBehaviour {

	public Transform Body;

	public float SensitivityX = 10f;
	public float SensitivityY = 10f;
	public float MinimumX = -360f;
	public float MaximumX = 360f;
	public float MinimumY = -80f;
	public float MaximumY = 80f;

	public KeyCode MousePointKey = KeyCode.LeftAlt;
	public KeyCode LookForward = KeyCode.C;

	private bool isLookforward = true;
	private float rotationY = 0f;
	private bool mouseVisible = true;

	void Start () {
		MousePoint ();
	}

	void Update () {
		if (!mouseVisible)
			Rotate ();
		if(Input.GetKeyDown(MousePointKey))
			MousePoint ();
		if (Input.GetKeyDown (LookForward))
			isLookforward = !isLookforward;
	}

	void Rotate () {
		float rotationX = gameObject.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * SensitivityX;

		rotationY += Input.GetAxis("Mouse Y") * SensitivityY;
		rotationY = Mathf.Clamp (rotationY, MinimumY, MaximumY);

		if (isLookforward) {
			gameObject.transform.localEulerAngles = new Vector3 (-rotationY, 0, 0);
			Body.Rotate (0, rotationX, 0);
		} else {
			gameObject.transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
		}
	}

	void MousePoint(){
		mouseVisible = !mouseVisible;
		Cursor.lockState = !mouseVisible ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = mouseVisible;
	}

}
