using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	Transform gunTop;

	public GameObject bulletHitPrefab;
	public GameObject muzzleFlashPrefab;
	public GameObject bulletShellPrefab;

	public float fireSpeed = .1f;
	public int bulletCount;
	public int bulletMaxCount = 30;

	void Start () {
		bulletCount = bulletMaxCount;
		gunTop = transform.Find ("GunTop");
	}

	void FixedUpdate () {
		Debug.DrawRay (gunTop.position, gunTop.forward * 100f, Color.green);
	}

	void Attack () {
		GetComponent<WeaponSound> ().PlaySound ("Bullet");
		
		Destroy (Instantiate (muzzleFlashPrefab, gunTop.position, Quaternion.FromToRotation (Vector3.forward, gunTop.forward)), .5f);
		if (bulletShellPrefab != null)
			Destroy (Instantiate (bulletShellPrefab, gunTop.position, Quaternion.identity), 2f);
		
		RaycastHit hit = new RaycastHit ();
		Ray ray = new Ray (gunTop.position, gunTop.forward);
		
		if (Physics.Raycast (ray, out hit, 100f)) {
			if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground") {
				Destroy (Instantiate (bulletHitPrefab, hit.point, Quaternion.FromToRotation (hit.collider.transform.forward, hit.normal)), 2f);
			}
		}
	}

	public void Reload () {
		AddBullet (bulletMaxCount);
		GetComponent<WeaponSound> ().PlaySound ("Reload");
	}
	
	public bool AddBullet (int amt) {
		bulletCount += amt;
		if (bulletCount >= bulletMaxCount) {
			bulletCount = bulletMaxCount;
			return true;
		}

		if (bulletCount < 0) {
			bulletCount = 0;
			GetComponent<WeaponSound> ().PlaySound ("NoAmmo");
			return false;
		}

		Attack ();
		return true;
	}
}
