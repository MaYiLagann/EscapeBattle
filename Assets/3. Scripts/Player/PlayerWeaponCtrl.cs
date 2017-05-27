using UnityEngine;
using System.Collections;

public class PlayerWeaponCtrl : MonoBehaviour {

	public WeaponInfo MainWeapon;
	public GameObject Bullet;

	public KeyCode ShootKey = KeyCode.Mouse0;
	public KeyCode AimKey = KeyCode.Mouse1;

	private int currentBullet = 0;
	private float currentDelay = 0;
	private GameObject Target;

	// Use this for initialization
	void Start () {
		currentBullet = MainWeapon.MaxBullet;
		Target = new GameObject ("Player Shoot Target");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (ShootKey))
			Shoot ();
		Aim ();
		if (currentDelay > 0)
			currentDelay -= Time.deltaTime;
	}

	void LateUpdate () {
		Target.transform.position = RayGetPosition (Camera.main.transform);
		if (MainWeapon.HaveAim && Input.GetKey (AimKey))
			MainWeapon.transform.localEulerAngles = Vector3.zero;
		else
			MainWeapon.transform.LookAt (Target.transform);
		Debug.DrawLine (Camera.main.transform.position, Target.transform.position, Color.cyan);
	}

	void Shoot () {
		if (currentBullet <= 0 || currentDelay > 0)
			return;

		currentDelay = MainWeapon.ShootDelay;

		GameObject obj = Instantiate (Bullet);
		Destroy (obj, 10f);
		obj.transform.position = MainWeapon.ShootPosition.position;
		obj.transform.rotation = MainWeapon.ShootPosition.rotation;
		obj.GetComponent<Rigidbody> ().velocity = obj.transform.forward * MainWeapon.BulletSpeed;
	}

	void Aim () {
		Animator anim = MainWeapon.GetComponentInChildren<Animator> ();
		anim.SetBool ("Aim", MainWeapon.HaveAim && Input.GetKey(AimKey));
	}

	Vector3 RayGetPosition (Transform Start){
		RaycastHit hit;
		if (Physics.Raycast (Start.position, Start.forward, out hit)) {
			if(Vector3.Distance(Start.position, hit.point) > 1f)
			return hit.point;
		}
		return Start.forward * 100f;
	}
}
