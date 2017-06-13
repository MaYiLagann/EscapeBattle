using UnityEngine;
using System.Collections;

public class PlayerCamCtrl : MonoBehaviour {

	public float FieldOfView = 75f;

	private Transform Cam;

	private float scaleEffectSize;
	private float scaleEffectTime;
	private float currentScaleSize;

	private float posEffectSize;
	private float posEffectTime;
	private float currentPosSize;
	private Vector3 posDirection;

	private float rotEffectSize;
	private float rotEffectTime;
	private float currentRotSize;
	private Vector3 rotDirection;

	// Use this for initialization
	void Start () {
		Cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		CamCtrl ();
		PositionEffectRun ();
		RotationEffectRun ();
		ScaleEffectRun ();
	}

	void CamCtrl () {
		Cam.position = gameObject.transform.position;
		Cam.rotation = gameObject.transform.rotation;
	}

	public void ScaleEffect (float size, float time) {
		scaleEffectSize = size;
		scaleEffectTime = time;
		currentScaleSize = size;
	}

	void ScaleEffectRun () {
		if (scaleEffectSize == 0 || scaleEffectTime == 0)
			return;
		
		currentScaleSize = currentScaleSize > 0 ? currentScaleSize - scaleEffectSize * Time.deltaTime / scaleEffectTime : 0;

		if (currentScaleSize == 0) {
			scaleEffectTime = 0;
			scaleEffectSize = 0;
			return;
		}

		Camera.main.fieldOfView = FieldOfView + currentScaleSize;
	}

	public void PositionEffect (float size, float time) {
		posEffectSize = size;
		posEffectTime = time;
		currentPosSize = size;
		posDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
	}

	void PositionEffectRun () {
		if (posEffectSize == 0 || posEffectTime == 0)
			return;

		currentPosSize = currentPosSize > 0 ? currentPosSize - posEffectSize * Time.deltaTime / posEffectTime : 0;

		if (currentPosSize == 0) {
			posEffectTime = 0;
			posEffectSize = 0;
			return;
		}

		Cam.position = gameObject.transform.position + gameObject.transform.rotation * posDirection * currentPosSize;
	}

	public void RotationEffect (float size, float time) {
		rotEffectSize = size;
		rotEffectTime = time;
		currentRotSize = size;
		rotDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
	}

	void RotationEffectRun () {
		if (rotEffectSize == 0 || rotEffectTime == 0)
			return;

		currentRotSize = currentRotSize > 0 ? currentRotSize - rotEffectSize * Time.deltaTime / rotEffectTime : 0;

		if (currentRotSize == 0) {
			rotEffectTime = 0;
			rotEffectSize = 0;
			return;
		}

		Vector3 rot = rotDirection * currentRotSize + gameObject.transform.rotation.eulerAngles;
		Cam.rotation = Quaternion.Euler (rot);
	}

	public float getScale(){
		return currentScaleSize;
	}
}
