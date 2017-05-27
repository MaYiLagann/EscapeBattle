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

	void Start () {
	
	}

	void Update () {
		Move ();
	}

	void OnTriggerEnter(Collider col){
		isGround = true;
	}

	/* Events */

	void Move () {

		walkSpeed = Input.GetKey (WalkForward) ? MoveSpeed / 3f : MoveSpeed;

		CharacterController thisChara = gameObject.GetComponent<CharacterController> ();
		if (Input.GetKey (MoveForward)&& Input.GetKey(RunForward)) 
			thisChara.Move (gameObject.transform.forward * walkSpeed * Time.deltaTime * 1.5f);
		else if (Input.GetKey (MoveForward))
			thisChara.Move (gameObject.transform.forward * walkSpeed * Time.deltaTime);
		if(Input.GetKey(MoveBack))
			thisChara.Move (-gameObject.transform.forward * walkSpeed * Time.deltaTime);
		if(Input.GetKey(MoveLeft))
			thisChara.Move (-gameObject.transform.right * walkSpeed * Time.deltaTime);
		if(Input.GetKey(MoveRight))
			thisChara.Move (gameObject.transform.right * walkSpeed * Time.deltaTime);
		if (Input.GetKey (JumpUp) && isGround) {
			upSpeed = JumpSpeed;
			isGround = false;
		}

		if (upSpeed > 0)
			upSpeed += Time.deltaTime * Physics.gravity.y * 2;
		else
			upSpeed = 0;

		thisChara.Move (gameObject.transform.up * (Physics.gravity.y + upSpeed) * Time.deltaTime);
	}
}
