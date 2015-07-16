using UnityEngine;
using System.Collections;

public class PlayerAnim : MonoBehaviour {

	Animator cameraAnim;
	public Animator weaponAnim;

	void BindAnim () {
		foreach (Transform weapon in transform.Find ("Character")) {
			weaponAnim = weapon.GetComponentInChildren<Animator>();
		}
	}

	void Update () {
		if (Input.GetButtonDown ("Fire2")) {
			weaponAnim.SetBool ("Aim", true);
		}
		if (Input.GetButtonUp ("Fire2")) {
			weaponAnim.SetBool ("Aim", false);
		}

		if (
			!GetComponent<PlayerShoot> ().holdFire &&
			weaponAnim.gameObject.GetComponent<Weapon> ().bulletCount > 0 &&
			Input.GetButton ("Fire1")
		) {
			weaponAnim.SetTrigger ("Shoot");
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			weaponAnim.SetTrigger ("Reload");
		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			weaponAnim.SetBool ("Sprint", true);
		}
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			weaponAnim.SetBool ("Sprint", false);
		}
	}
}
