using UnityEngine;
using UnityEngine.UI;

public class DummyCtrl : MonoBehaviour {
	
	// Update is called once per frame
	void FixedUpdate () {
		Text distance = gameObject.GetComponentInChildren<Text> ();
		distance.text = ((int)Vector3.Distance (gameObject.transform.position, GameObject.FindGameObjectWithTag ("Player").transform.position)).ToString() + " m";
	}
}
