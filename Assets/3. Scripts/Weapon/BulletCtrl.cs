using UnityEngine;
using System.Collections;

public class BulletCtrl : MonoBehaviour {


	void OnCollisionEnter(Collision col){
		Destroy (gameObject);
	}
}
