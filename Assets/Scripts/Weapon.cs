using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	Transform gunTop;

	public bool meleeWeapon = false;
	public bool isKnifeAttacking = false;

	public GameObject bulletHitPrefab;
	public GameObject muzzleFlashPrefab;
	public GameObject bulletShellPrefab;

	public float fireSpeed = .1f;
	public float fireRange = 100f;
	public int bulletCount;
	public int bulletMaxCount = 30;

	void Start () {
		bulletCount = bulletMaxCount;
		gunTop = transform.Find ("GunTop");
	}

	void FixedUpdate () {
		Debug.DrawRay (gunTop.position, gunTop.forward * fireRange, Color.green);

		if (meleeWeapon && isKnifeAttacking) {
			RayShoot ();
		}
	}

	void GunAttack () {
		if (muzzleFlashPrefab != null)
			Destroy (Instantiate (muzzleFlashPrefab, gunTop.position, Quaternion.FromToRotation (Vector3.forward, gunTop.forward)), .5f);
		if (bulletShellPrefab != null)
			Destroy (Instantiate (bulletShellPrefab, gunTop.parent.position + Vector3.right * .1f, Quaternion.identity), 2f);

		RayShoot ();
	}

	void KnifeAttack () {
		isKnifeAttacking = true;
		Invoke ("CancelKnifeAttack", .5f);
	}

	void CancelKnifeAttack () {
		isKnifeAttacking = false;
	}

	void RayShoot () {
		Ray ray = new Ray (gunTop.position, gunTop.forward);
		RaycastHit[] hits = Physics.RaycastAll(ray, fireRange);

		foreach (RaycastHit hit in hits) {
			if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground") {
				Destroy (Instantiate (bulletHitPrefab, hit.point, Quaternion.FromToRotation (hit.collider.transform.forward, hit.normal)), 2f);
				break;
			}
			
			if (hit.collider.tag == "Enemy") {
				hit.collider.SendMessage ("Broken", hit.point);
				break;
			}
		}
	}

	public void Reload () {
		AddBullet (bulletMaxCount);
		SendMessage("PlaySound", "Reload");
	}
	
	public bool AddBullet (int amt) {
		if (!meleeWeapon) {
			bulletCount += amt;
			if (bulletCount >= bulletMaxCount) {
				bulletCount = bulletMaxCount;
				return true;
			}
			if (bulletCount < 0) {
				bulletCount = 0;
				SendMessage ("PlaySound", "NoAmmo");
				return false;
			}
			SendMessage ("PlaySound", "Bullet");
			GunAttack ();
		}
		else {
			SendMessage ("PlaySound", "Bullet");
			KnifeAttack ();
		}
		return true;
	}
}
