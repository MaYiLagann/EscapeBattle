using UnityEngine;
using System.Collections;

public class PlayerLookCtrl : MonoBehaviour {
	
	public float SensitivityX = 10f;
	public float SensitivityY = 10f;
	public float MinimumX = -360f;
	public float MaximumX = 360f;
	public float MinimumY = -80f;
	public float MaximumY = 80f;

	public KeyCode MousePointKey = KeyCode.LeftAlt;

	private float rotationY = 0F;

	void Update () {
		if (!Input.GetKey (MousePointKey))
			Rotate ();
		MousePoint (Input.GetKey(MousePointKey));
	}

	void Rotate () {
		float rotationX = gameObject.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * SensitivityX;

		rotationY += Input.GetAxis("Mouse Y") * SensitivityY;
		rotationY = Mathf.Clamp (rotationY, MinimumY, MaximumY);

		gameObject.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
	}

	void MousePoint(bool isVisible){
		Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = isVisible;
	}

}
