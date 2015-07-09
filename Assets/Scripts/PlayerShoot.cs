using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

	public GameObject bulletHolePrefab;
	public GameObject muzzleFlashPrefab;
	public GameObject bulletShellPrefab;
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

		if (GetComponentInChildren<Weapon> ().AddBullet (-1)) {
			GetComponentInChildren<WeaponSound> ().PlaySound ("Bullet");

			Destroy (Instantiate (muzzleFlashPrefab, gunTop.position, Quaternion.FromToRotation (Vector3.forward, gunTop.forward)), .5f);
			Destroy (Instantiate (bulletShellPrefab, gunTop.position, Quaternion.identity), 2f);

			RaycastHit hit = new RaycastHit ();
			Ray ray = new Ray (gunTop.position, gunTop.forward);

			if (Physics.Raycast (ray, out hit, 100f)) {
				if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground") {
					Destroy (Instantiate (bulletHolePrefab, hit.point, Quaternion.FromToRotation (hit.collider.transform.forward, hit.normal)), 2f);
				}
			}
		}
		else
			GetComponentInChildren<WeaponSound> ().PlaySound ("NoAmmo");

		yield return new WaitForSeconds (fireRate);
		holdFire = false;
	}
}
