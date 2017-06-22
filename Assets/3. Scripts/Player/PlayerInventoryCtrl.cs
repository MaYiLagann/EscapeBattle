using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInventoryCtrl : MonoBehaviour {

	public GameObject WeaponInfo;
	public GameObject InventoryInfo;

	[Header("Keyboard Settings")]
	public KeyCode OpenInventory = KeyCode.Tab;

	private bool isOpen = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (OpenInventory))
			Open ();
	}

	void Open () {
		isOpen = !isOpen;
		WeaponInfo.SetActive (!isOpen);
		InventoryInfo.SetActive (isOpen);
		Cursor.lockState = !isOpen ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = isOpen;
	}

	public bool getState () {
		return isOpen;
	}
}
