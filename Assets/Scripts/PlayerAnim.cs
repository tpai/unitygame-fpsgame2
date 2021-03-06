﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class PlayerAnim : PlayerBase {
	
	public bool isSprinting = false;
	public bool isReloading = false;

	bool holdFire = false;

	Animator weaponAnim;
	Animator cameraAnim;

	void Start () {
		Cursor.visible = false;
	}

	void BindAnim () {
		Weapon = null;
		weaponAnim = transform.Find ("Character").GetComponentInChildren<Animator>();
	}

	void Update () {
		if (!Weapon.meleeWeapon && Input.GetButtonDown ("Fire2")) {
			weaponAnim.SetBool ("Aim", true);
		}
		if (!Weapon.meleeWeapon && Input.GetButtonUp ("Fire2")) {
			weaponAnim.SetBool ("Aim", false);
		}

		if (
			!holdFire &&
			!isSprinting &&
			!isReloading &&
			Input.GetButton ("Fire1")
		) {
			StartCoroutine ("Fire");
		}

		if (!Weapon.meleeWeapon && Input.GetKeyDown (KeyCode.R)) {
			isReloading = true;
			weaponAnim.SetTrigger ("Reload");
			Weapon.Reload ();
			Invoke ("ReloadComplete", 1f);
		}

		if (Input.GetKey (KeyCode.LeftShift)) {
			isSprinting = true;
			weaponAnim.SetBool ("Aim", false);
			weaponAnim.SetBool ("Sprint", true);
		}
		if (isSprinting && (
				Input.GetKeyUp (KeyCode.LeftShift) || 
			    (
					CrossPlatformInputManager.GetAxis("Horizontal") == 0f &&
			    	CrossPlatformInputManager.GetAxis("Vertical") == 0f
				)
			)
		) {
			isSprinting = false;
			weaponAnim.SetBool ("Sprint", false);
		}
	}

	void ReloadComplete () {
		isReloading = false;
	}

	IEnumerator Fire () {
		holdFire = true;
		if (Weapon.AddBullet (-1))weaponAnim.SetTrigger ("Shoot");
		yield return new WaitForSeconds (Weapon.fireSpeed);
		holdFire = false;
	}
}
