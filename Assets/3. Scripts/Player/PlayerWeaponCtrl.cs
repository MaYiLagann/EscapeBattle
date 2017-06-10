using UnityEngine;
using System.Collections;

public class PlayerWeaponCtrl : MonoBehaviour {

	public WeaponInfo MainWeapon;
	public GameObject Bullet;
	public GameObject AimImage;

	public KeyCode ShootKey = KeyCode.Mouse0;
	public KeyCode AimKey = KeyCode.Mouse1;

	private float isAim = 0; // 1 = Aimed, 0 = NotAimed
	private int currentBullet = 0;
	private float currentDelay = 0;
	private GameObject Target;
	private PlayerLookCtrl PLC;

	// Use this for initialization
	void Start () {
		currentBullet = MainWeapon.MaxBullet;
		Target = new GameObject ("Player Shoot Target");
		PLC = GetComponentInChildren<PlayerLookCtrl> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (MainWeapon == null)
			return;

		Target.transform.position = RayGetPosition (PLC.transform);
		Debug.DrawLine (Camera.main.transform.position, Target.transform.position, Color.cyan);

		if (Input.GetKey (ShootKey) && !Input.GetKey(PLC.LookForward))
			Shoot ();
		Aim ();
		if (currentDelay > 0)
			currentDelay -= Time.deltaTime;
	}

	void Shoot () {
		if (currentBullet <= 0 || currentDelay > 0)
			return;

		currentDelay = MainWeapon.ShootDelay;

		GameObject obj = Instantiate (Bullet);
		Destroy (obj, 10f);
		obj.transform.position = MainWeapon.ShootPosition.position;
		obj.transform.LookAt (Target.transform);
		obj.GetComponent<Rigidbody> ().velocity = obj.transform.forward * MainWeapon.BulletSpeed;
	}

	void Aim () {
		if (!MainWeapon.HaveAim)
			return;

		bool aim =  Input.GetKey (AimKey);
		if (aim) {
			isAim = isAim < 1f ? isAim + Time.deltaTime / MainWeapon.AimDuration : 1f;
		} else {
			isAim = isAim > 0 ? isAim - Time.deltaTime / MainWeapon.AimDuration : 0;
		}

		AimImage.SetActive (!aim);
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
