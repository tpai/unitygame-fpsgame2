using UnityEngine;
using System.Collections;

public class PlayerAnim : MonoBehaviour {

	public bool isSprinting = false;
	public bool isReloading = false;
	public Animator weaponAnim;
	Animator cameraAnim;

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
			!isSprinting &&
			!isReloading &&
			!GetComponent<PlayerShoot> ().holdFire &&
			GetComponentInChildren<Weapon> ().bulletCount > 0 &&
			Input.GetButton ("Fire1")
		) {
			weaponAnim.SetTrigger ("Shoot");
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			isReloading = true;
			GetComponentInChildren<Weapon> ().Reload ();
			Invoke ("ReloadComplete", 1f);
			weaponAnim.SetTrigger ("Reload");
		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			isSprinting = true;
			weaponAnim.SetBool ("Sprint", true);
		}
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			isSprinting = false;
			weaponAnim.SetBool ("Sprint", false);
		}
	}

	void ReloadComplete () {
		isReloading = false;
	}
}
