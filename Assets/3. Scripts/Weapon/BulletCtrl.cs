using UnityEngine;
using System.Collections;

public class BulletCtrl : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if (col.transform.tag == "Enemy") {
			Destroy (gameObject, 1f);
			gameObject.GetComponent<AudioSource> ().Play ();
			Destroy (gameObject.GetComponent<Rigidbody> ());
			Destroy (gameObject.GetComponent<SphereCollider> ());
		} else 
			Destroy (gameObject);
	}
}
