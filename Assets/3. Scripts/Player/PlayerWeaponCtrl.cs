﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerWeaponCtrl : MonoBehaviour {

	public WeaponInfo MainWeapon;
	public GameObject Bullet;
	public GameObject AimImage;

	[Header("Weapon UI Setting")]
	public Text WeaponName;
	public Text WeaponBullet;

	public KeyCode ShootKey = KeyCode.Mouse0;
	public KeyCode AimKey = KeyCode.Mouse1;
	public KeyCode ReloadKey = KeyCode.R;

	private float isAim = 0; // 1 = Aimed, 0 = NotAimed
	private int currentBullet = 0;
	private float currentDelay = 0;

	private GameObject Target;
	private GameObject Look;
	private PlayerLookCtrl PLC;

	// Use this for initialization
	void Start () {
		currentBullet = MainWeapon.MaxBullet;
		Target = new GameObject ("Player Shoot Target");
		Look = new GameObject ("Player Look");
		Look.transform.SetParent (gameObject.transform);
		PLC = GetComponentInChildren<PlayerLookCtrl> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (MainWeapon == null)
			return;

		Target.transform.position = RayGetPosition (Look.transform);
		AimImage.GetComponent<RectTransform> ().anchoredPosition = Camera.main.WorldToScreenPoint (Target.transform.position) - new Vector3 (Screen.width / 2, Screen.height / 2, 0);
		Debug.DrawLine (Camera.main.transform.position, Target.transform.position, Color.cyan);

		if (Input.GetKey (ShootKey) && !Input.GetKey(PLC.LookAround))
			Shoot ();
		if (currentBullet != MainWeapon.MaxBullet && Input.GetKeyDown(ReloadKey))
			Reload ();
		Aim ();
		if (currentDelay > 0)
			currentDelay -= Time.deltaTime;
	}

	void LateUpdate () {
		Look.transform.position = PLC.transform.position;
		if (!Input.GetKey (PLC.LookAround))
			Look.transform.rotation = PLC.transform.rotation;
	}

	void Shoot () {
		PlayerCamCtrl PCC = gameObject.GetComponentInChildren<PlayerCamCtrl> ();
		if (currentBullet <= 0 || currentDelay > 0 || !MainWeapon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("WeaponIdleAnim"))
			return;

		// Create Bullet
		GameObject obj = Instantiate (Bullet);
		Destroy (obj, 10f);
		obj.transform.position = MainWeapon.ShootPosition.position;
		obj.transform.LookAt (Target.transform);
		obj.GetComponent<Rigidbody> ().velocity = obj.transform.forward * MainWeapon.BulletSpeed;

		// Bullet System
		currentDelay = MainWeapon.ShootDelay;
		currentBullet -= 1;
		if (PCC.getScale () != 0) {
			Vector2 r = new Vector2 (Random.Range (-1f, 1f), 1f) * MainWeapon.Reaction;
			r = isAim == 0 ? r : r / 2;
			PLC.AddRotation (r);
		}

		// Camera Effect
		PCC.ScaleEffect (MainWeapon.Reaction, MainWeapon.ReactionDuration);
		if (isAim == 0) 
			PCC.RotationEffect (MainWeapon.Reaction / 2f, MainWeapon.ReactionDuration);

		// UI
		WeaponBullet.text = currentBullet.ToString ();
	}

	void Reload () {
		MainWeapon.GetComponent<Animator> ().SetTrigger ("Reload");
		StartCoroutine (SetMaxBullet (MainWeapon.ReloadTime));
	}

	IEnumerator SetMaxBullet(float time){
		yield return new WaitForSeconds (time);
		currentBullet = MainWeapon.MaxBullet;
		WeaponBullet.text = currentBullet.ToString ();
	}

	void Aim () {
		if (!MainWeapon.HaveAim)
			return;

		bool aim =  Input.GetKey (AimKey);
		if (aim && MainWeapon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("WeaponIdleAnim")) {
			isAim = isAim < 1f ? isAim + Time.deltaTime / MainWeapon.AimDuration : 1f;
		} else {
			isAim = isAim > 0 ? isAim - Time.deltaTime / MainWeapon.AimDuration : 0;
		}

		AimImage.SetActive (isAim == 0f);
		MainWeapon.transform.localPosition = Vector3.Slerp(MainWeapon.NotAimPosition, MainWeapon.AimPosition, isAim);
	}

	Vector3 RayGetPosition (Transform Start){
		RaycastHit hit;
		if (Physics.Raycast (Start.position, Start.forward, out hit)) {
			if(Vector3.Distance(Start.position, hit.point) > 1f)
			return hit.point;
		}
		return Start.rotation * Vector3.forward * 500f;
	}
}
