using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveCtrl_Rig : MonoBehaviour {

	public float MaxSpeed = 5f;
	public float MoveSpeed = 5f;
	public float JumpSpeed = 10f;
	public float CrounchSpeed = 5f;

	public Transform Hip;
	public Vector3 CrounchPosition = Vector3.up;
	public Vector3 StandPosition = Vector3.up;

	[Header("Keyboard Settings")]
	public KeyCode MoveForward = KeyCode.W;
	public KeyCode MoveBack = KeyCode.S;
	public KeyCode MoveLeft = KeyCode.A;
	public KeyCode MoveRight = KeyCode.D;
	public KeyCode JumpUp = KeyCode.Space;
	public KeyCode RunForward = KeyCode.LeftShift;
	public KeyCode WalkForward = KeyCode.LeftAlt;
	public KeyCode CrounchDown = KeyCode.LeftControl;

	private float isCrounch = 0;
	private float walkSpeed = 0;
	private bool isGround = false;
	private Rigidbody thisRig;
	private Vector3 move = Vector3.zero;
	private Vector3 airmove = Vector3.zero;

	void Start () {
		thisRig = gameObject.GetComponent<Rigidbody> ();
	}

	void Update () {
		Crounch ();
	}

	void LateUpdate () {
		Move ();
	}

	void OnTriggerStay (Collider col){
		isGround = true;
	}

	/* Events */

	void Move () {
		if (isGround) {
			move = Vector3.zero;
			walkSpeed = Input.GetKey (WalkForward) ? MoveSpeed / 3f : MoveSpeed;
			walkSpeed = Input.GetKey (RunForward) ? walkSpeed * 1.5f : walkSpeed;

			if (Input.GetKey (MoveForward))
				move.z += 1;
			if (Input.GetKey (MoveBack))
				move.z -= 1;
			if (Input.GetKey (MoveLeft))
				move.x -= 1;
			if (Input.GetKey (MoveRight))
				move.x += 1;
			if (Input.GetKey (JumpUp)) {
				thisRig.AddForce (Vector3.up * JumpSpeed, ForceMode.Impulse);
				isGround = false;
			}
			move = gameObject.transform.rotation * move.normalized;
			airmove = Vector3.zero;
		} else {
			if (Input.GetKey (MoveForward))
				airmove.z += 1;
			if (Input.GetKey (MoveBack))
				airmove.z -= 1;
			if (Input.GetKey (MoveLeft))
				airmove.x -= 1;
			if (Input.GetKey (MoveRight))
				airmove.x += 1;
			airmove = gameObject.transform.rotation * airmove.normalized;
			thisRig.AddForce (airmove * 2f, ForceMode.Acceleration);
		}

		thisRig.MovePosition (gameObject.transform.position + move * walkSpeed * Time.deltaTime);

		// Max Speed
		Vector3 velo = thisRig.velocity;
		float gravity = Mathf.Clamp (velo.y, -100f, MaxSpeed);
		velo = Vector3.ClampMagnitude (velo, MaxSpeed);
		velo.y = gravity;
		thisRig.velocity = velo;
	}

	void Crounch () {
		if (Input.GetKey (CrounchDown) && isGround) {
			isCrounch = isCrounch < 1f ? isCrounch + Time.deltaTime * CrounchSpeed : 1f;
		} else {
			isCrounch = isCrounch > 0 ? isCrounch -= Time.deltaTime * CrounchSpeed : 0;
		}
		Hip.localPosition = CrounchPosition * isCrounch + StandPosition * (1 - isCrounch);
	}

	public bool getGrounch () {
		return isCrounch == 1f;
	}
}
