using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveCtrl : MonoBehaviour {

	[Range(1, 1000)]
	public int TestFrame = 60;

	public float MoveSpeed = 5f;
	public float JumpSpeed = 10f;
	public float JumpDelay = 0.5f; // if delay small than 0, then have double jumped error
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
	private bool isJumped = false;
	private Rigidbody thisRig;
	private Vector3 move = Vector3.zero;

	void Start () {
		thisRig = gameObject.GetComponent<Rigidbody> ();
	}

	void Update () {
		TestFrame += (int)Input.mouseScrollDelta.y * 10;
		Application.targetFrameRate = TestFrame;
		Crounch ();
	}

	void LateUpdate () {
		Move ();
	}

	void OnTriggerStay (Collider col){
		isGround = true;
	}

	void OnTriggerExit (Collider col){
		isGround = false;
	}

	/* Events */

	void Move () {
		if (isGround) {
			move = Vector3.zero;
			walkSpeed = Input.GetKey (WalkForward) ? MoveSpeed / 3f : MoveSpeed;
			walkSpeed = Input.GetKey (RunForward) ? walkSpeed * 1.5f : walkSpeed;

			Jump ();
			if (Input.GetKey (MoveForward))
				move.z += 1;
			if (Input.GetKey (MoveBack))
				move.z += -1;
			if (Input.GetKey (MoveLeft))
				move.x += -1;
			if (Input.GetKey (MoveRight))
				move.x += 1;
			move = gameObject.transform.rotation * move.normalized;
		} else {
			if (Input.GetKey (MoveForward))
				move += gameObject.transform.forward * 1.5f * Time.deltaTime;
			if (Input.GetKey (MoveBack))
				move -= gameObject.transform.forward * 1.5f * Time.deltaTime;
			if (Input.GetKey (MoveLeft))
				move -= gameObject.transform.right * 1.5f * Time.deltaTime;
			if (Input.GetKey (MoveRight))
				move += gameObject.transform.right * 1.5f * Time.deltaTime;
			move = Vector3.ClampMagnitude (move, 1f);
		}

		thisRig.MovePosition (gameObject.transform.position + move * walkSpeed / 100f);

		// Set Max Gravity Velocity
		Vector3 velocity = thisRig.velocity;
		float gravity = Mathf.Clamp (thisRig.velocity.y, -1000f, 5f);
		velocity.y = gravity;
		thisRig.velocity = velocity;
	}

	void Crounch () {
		if (Input.GetKey (CrounchDown) && isGround) {
			isCrounch = isCrounch < 1f ? isCrounch + Time.deltaTime * CrounchSpeed : 1f;
		} else {
			isCrounch = isCrounch > 0 ? isCrounch -= Time.deltaTime * CrounchSpeed : 0;
		}
		Hip.localPosition = CrounchPosition * isCrounch + StandPosition * (1 - isCrounch);
	}

	void Jump () {
		if (isGround && !isJumped && Input.GetKey (JumpUp)) {
			thisRig.AddRelativeForce (Vector3.up * JumpSpeed, ForceMode.Impulse);
			isGround = false;
			isJumped = true;
			StartCoroutine (JumpCooldown (JumpDelay));
		}
	}

	IEnumerator JumpCooldown(float time){
		yield return new WaitForSeconds (time);
		isJumped = false;
	}

	public bool getGrounch () {
		return isCrounch == 1f;
	}

	public bool getMove () {
		return move != Vector3.zero;
	}
}
