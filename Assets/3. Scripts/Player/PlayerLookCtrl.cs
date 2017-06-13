using UnityEngine;
using System.Collections;

public class PlayerLookCtrl : MonoBehaviour {

	public Transform Body;
	public Transform Hip;

	public float SensitivityX = 10f;
	public float SensitivityY = 10f;
	public float MinimumX = -360f;
	public float MaximumX = 360f;
	public float LookMinimumX = -135f;
	public float LookMaximumX = 135f;
	public float MinimumY = -80f;
	public float MaximumY = 80f;

	public KeyCode MousePointKey = KeyCode.BackQuote;
	public KeyCode LookAround = KeyCode.C;

	private float rotationX = 0f;
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
	}

	void Rotate () {
		rotationX += Input.GetAxis("Mouse X") * SensitivityX / 5f;
		if (MinimumX > -360f && MaximumX < 360f)
			rotationX = Mathf.Clamp (rotationX, MinimumX, MaximumX);
		rotationY += Input.GetAxis("Mouse Y") * SensitivityY / 5f;
		if (MinimumY > -360f && MaximumY < 360f)
			rotationY = Mathf.Clamp (rotationY, MinimumY, MaximumY);

		if (Input.GetKeyUp (LookAround)) {
			rotationX = Body.localEulerAngles.y;
			gameObject.transform.localEulerAngles = Vector3.zero;
		}
		if (!Input.GetKey (LookAround)) {
			Body.localEulerAngles = new Vector3 (0, rotationX, 0);
			Hip.localEulerAngles = new Vector3 (-rotationY, 0, 0);
		} else {
			if (LookMinimumX > -360f && LookMaximumX < 360f)
				rotationX = Mathf.Clamp (rotationX, LookMinimumX + Body.localEulerAngles.y, LookMaximumX + Body.localEulerAngles.y);
			gameObject.transform.localEulerAngles = new Vector3 (-rotationY - Hip.localEulerAngles.x, rotationX - Body.localEulerAngles.y, 0);
		}
	}

	void MousePoint(){
		mouseVisible = !mouseVisible;
		Cursor.lockState = !mouseVisible ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = mouseVisible;
	}

	public void AddRotation(Vector2 angle){
		rotationX += angle.x;
		rotationY += angle.y;
	}
}
