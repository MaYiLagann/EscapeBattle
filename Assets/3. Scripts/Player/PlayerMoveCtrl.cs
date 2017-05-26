using UnityEngine;
using System.Collections;

public class PlayerMoveCtrl : MonoBehaviour {

	public KeyCode ForwardKey = KeyCode.W;
	public KeyCode BackKey = KeyCode.S;
	public KeyCode LeftKey = KeyCode.A;
	public KeyCode RightKey = KeyCode.D;

	public float Speed = 10f;
	public Transform Head;

	private Rigidbody Rigid;

	// Use this for initialization
	void Start () {
		Rigid = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	void Move () {
		if (Head == null) {
			Debug.Log ("Head can't be null");
			return;
		}
		Vector3 rot = Head.rotation.eulerAngles - gameObject.transform.rotation.eulerAngles;
		rot.x = 0;
		gameObject.transform.rotation = Quaternion.Euler (rot);

		float spd = Speed * Time.deltaTime;
		Vector3 move = Vector3.zero;
		if (Input.GetKey (ForwardKey)) {
			move.z++;
		}
		if (Input.GetKey (BackKey)) {
			move.z--;
		}
		if (Input.GetKey (LeftKey)) {
			move.x--;
		}
		if (Input.GetKey (RightKey)) {
			move.x++;
		}

		Rigid.MovePosition (gameObject.transform.position + gameObject.transform.forward * move.z * spd);
	}
}
