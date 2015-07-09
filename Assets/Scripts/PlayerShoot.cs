using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

	bool holdFire = false;
	Transform gunTop;

	void Start () {
		Cursor.visible = false;
		gunTop = transform.Find ("Character/Weapon/GunTop");
	}
	
	void Update () {

		if (!holdFire && Input.GetButton ("Fire1")) {
			StartCoroutine ("Fire");
		}

		Debug.DrawRay (gunTop.position, gunTop.forward * 100f, Color.green);
	}

	IEnumerator Fire () {
		holdFire = true;
		yield return new WaitForSeconds (.5f);
		holdFire = false;
	}
}
