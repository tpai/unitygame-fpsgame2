using UnityEngine;
using System.Collections;

public class Backpack : MonoBehaviour {

	Transform weapon;

	void Start () {
		weapon = transform.Find ("Character");
		SwitchWeapon (3);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			SwitchWeapon (2);
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			SwitchWeapon (3);
		}
	}

	void SwitchWeapon (int id) {

		for (int i=1; i<=3; i++) {
			weapon.Find ("W"+i).gameObject.SetActive (false);
		}

		weapon.Find ("W"+id).gameObject.SetActive (true);
	}
}
