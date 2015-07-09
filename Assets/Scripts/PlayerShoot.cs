using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

	public GameObject bulletHolePrefab;
	public float fireRate = .1f;

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

		RaycastHit hit = new RaycastHit ();
		Ray ray = new Ray (gunTop.position, gunTop.forward);

		if (Physics.Raycast (ray, out hit, 100f)) {
			if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground") {
				Instantiate (bulletHolePrefab, hit.point, Quaternion.FromToRotation (Vector3.forward, hit.normal));
			}
		}

		yield return new WaitForSeconds (fireRate);
		holdFire = false;
	}
}
