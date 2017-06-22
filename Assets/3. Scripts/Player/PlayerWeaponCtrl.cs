using UnityEngine;
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
	private float flashAlpha = 0;
	private bool weaponIdle = false;

	private GameObject Target;
	private GameObject Look;
	private PlayerLookCtrl PLC;
	private PlayerMoveCtrl PMC;
	private PlayerInventoryCtrl PIC;
	private Animator WAnim;

	// Use this for initialization
	void Start () {
		currentBullet = MainWeapon.MaxBullet;
		Target = new GameObject ("Player Shoot Target");
		Look = new GameObject ("Player Look");
		Look.transform.SetParent (gameObject.transform);
		PLC = gameObject.GetComponentInChildren<PlayerLookCtrl> ();
		PMC = gameObject.GetComponent<PlayerMoveCtrl> ();
		PIC = gameObject.GetComponent<PlayerInventoryCtrl> ();
		WAnim = MainWeapon.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (MainWeapon == null)
			return;

		weaponIdle = WAnim.GetCurrentAnimatorStateInfo (0).IsName ("WeaponIdleAnim");
		WAnim.SetBool ("Run", Input.GetKey (PMC.RunForward) && PMC.getMove ());
		WAnim.updateMode = weaponIdle ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal;

		if (Input.GetKey (ShootKey) && !Input.GetKey(PLC.LookAround) && !Input.GetKey(PMC.RunForward) && !PIC.getState())
			Shoot ();
		if (currentBullet != MainWeapon.MaxBullet && Input.GetKeyDown(ReloadKey) && weaponIdle)
			Reload ();
		Aim ();
		if (currentDelay > 0)
			currentDelay -= Time.deltaTime;

		Color c = Color.white;
		c.a = flashAlpha;
		MainWeapon.ShootPosition.GetComponent<SpriteRenderer> ().color = c;
		flashAlpha = flashAlpha > 0 ? flashAlpha - Time.deltaTime * 5f : 0;
	}

	void LateUpdate () {
		Target.transform.position = RayGetPosition (MainWeapon.ShootPosition.transform);
		AimImage.GetComponent<RectTransform> ().anchoredPosition = Camera.main.WorldToScreenPoint (Target.transform.position) - new Vector3 (Screen.width / 2, Screen.height / 2, 0);
		AimImage.SetActive (isAim == 0f && !Input.GetKey(PMC.RunForward));
		Look.transform.position = PLC.transform.position;
		if (!Input.GetKey (PLC.LookAround))
			Look.transform.rotation = PLC.transform.rotation;
	}

	void Shoot () {
		PlayerCamCtrl PCC = gameObject.GetComponentInChildren<PlayerCamCtrl> ();
		if (currentBullet <= 0 || currentDelay > 0 || !weaponIdle)
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

		// Other Effect
		MuzzleFlash();
		MainWeapon.GetComponent<AudioSource> ().Play();

		// UI
		WeaponBullet.text = currentBullet.ToString ();
	}

	void Reload () {
		WAnim.SetTrigger ("Reload");
		StartCoroutine (SetMaxBullet (MainWeapon.ReloadTime));
	}

	IEnumerator SetMaxBullet(float time){
		yield return new WaitForSeconds (time);
		currentBullet = MainWeapon.MaxBullet;
		WeaponBullet.text = currentBullet.ToString ();
		MainWeapon.GetComponent<AudioSource> ().clip = MainWeapon.ShootSound;
	}

	void Aim () {
		if (!MainWeapon.HaveAim)
			return;

		bool aim =  Input.GetKey (AimKey);
		if (aim && WAnim.GetCurrentAnimatorStateInfo(0).IsName("WeaponIdleAnim")) {
			isAim = isAim < 1f ? isAim + Time.deltaTime / MainWeapon.AimDuration : 1f;
		} else {
			isAim = isAim > 0 ? isAim - Time.deltaTime / MainWeapon.AimDuration : 0;
		}

		MainWeapon.transform.localPosition = Vector3.Slerp(MainWeapon.NotAimPosition, MainWeapon.AimPosition, isAim);
	}

	Vector3 RayGetPosition (Transform Start){
		RaycastHit hit;
		if (Physics.Raycast (Start.position, Start.forward, out hit)) 
			return hit.point;
		return Start.position + Start.forward * 500f;
	}

	void MuzzleFlash () {
		MainWeapon.ShootPosition.Rotate (0, 0, Random.Range(0, 360f));
		MainWeapon.ShootPosition.localScale = Vector3.one * Random.Range (2f, 4f);
		flashAlpha = 1f;
	}
}
