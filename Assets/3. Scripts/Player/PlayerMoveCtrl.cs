using UnityEngine;
using System.Collections;

public class PlayerMoveCtrl : MonoBehaviour {

	public float MoveSpeed = 5f;
	public float JumpSpeed = 10f;

	[Header("Keyboard Settings")]
	public KeyCode MoveForward = KeyCode.W;
	public KeyCode MoveBack = KeyCode.S;
	public KeyCode MoveLeft = KeyCode.A;
	public KeyCode MoveRight = KeyCode.D;
	public KeyCode JumpUp = KeyCode.Space;
	public KeyCode RunForward = KeyCode.LeftShift;
	public KeyCode WalkForward = KeyCode.LeftAlt;

	private float walkSpeed = 0;
	private float upSpeed = 0;
	private bool isGround = true;
	private CharacterController thisChara;
	private Vector3 move;

	void Start () {
		thisChara = gameObject.GetComponent<CharacterController> ();
	}

	void Update () {
		Move ();
	}

	void OnTriggerEnter(Collider col){
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
				upSpeed = JumpSpeed;
				isGround = false;
			}
			move = gameObject.transform.rotation * move.normalized;
		}

		if (upSpeed > 0)
			upSpeed += Time.deltaTime * Physics.gravity.y * 2;
		else
			upSpeed = 0;

		thisChara.Move (move * walkSpeed * Time.deltaTime);
		thisChara.Move (gameObject.transform.up * (Physics.gravity.y + upSpeed) * Time.deltaTime);
	}
}
