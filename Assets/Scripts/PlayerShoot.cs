using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

	bool holdFire = false;

	void Start () {
		Cursor.visible = false;
	}
	
	void Update () {
		if (!holdFire && Input.GetButton ("Fire1")) {
			StartCoroutine ("Fire");
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			GetComponentInChildren<Weapon> ().Reload ();
		}
	}

	IEnumerator Fire () {
		holdFire = true;
		GetComponentInChildren<Weapon> ().AddBullet (-1);
		yield return new WaitForSeconds (GetComponentInChildren<Weapon>().fireSpeed);
		holdFire = false;
	}
}
