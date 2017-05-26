using UnityEngine;
using System.Collections;

public class PlayerCamCtrl : MonoBehaviour {

	private Transform Cam;

	// Use this for initialization
	void Start () {
		Cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		CamCtrl ();
	}

	void CamCtrl () {
		Cam.position = gameObject.transform.position;
		Cam.rotation = gameObject.transform.rotation;
	}
}
