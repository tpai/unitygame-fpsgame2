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

	void FixedUpdate () {
		if (Input.GetButtonDown ("Fire2")) {
			weaponAnim.SetBool ("Aim", true);
		}
		if (Input.GetButtonUp ("Fire2")) {
			weaponAnim.SetBool ("Aim", false);
		}
	}
}
