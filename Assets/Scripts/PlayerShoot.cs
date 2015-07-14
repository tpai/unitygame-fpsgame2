﻿using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

	bool holdFire = false;
	bool isReloading = false;

	void Start () {
		Cursor.visible = false;
	}
	
	void Update () {
		if (!holdFire && Input.GetButton ("Fire1")) {
			StartCoroutine ("Fire");
		}

		if (!isReloading && Input.GetKeyDown (KeyCode.R)) {
			isReloading = true;
			GetComponentInChildren<Weapon> ().Reload ();
			Invoke ("ReloadComplete", 1.5f);
		}
	}

	public void ReloadComplete () {
		isReloading = false;
	}

	IEnumerator Fire () {
		holdFire = true;
		GetComponentInChildren<Weapon> ().AddBullet (-1);
		yield return new WaitForSeconds (GetComponentInChildren<Weapon>().fireSpeed);
		holdFire = false;
	}
}
