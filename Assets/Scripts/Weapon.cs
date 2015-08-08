using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class Weapon : PlayerBase {

	Transform gunTop;

	public bool meleeWeapon = false;
	public bool isKnifeAttacking = false;

	public GameObject bulletHitPrefab;
	public GameObject muzzleFlashPrefab;
	public GameObject bulletShellPrefab;

	public int fireDamage = 15;
	public float fireSpeed = .1f;
	public float fireRange = 100f;
	public int bulletCount = 30;
	public int bulletMaxCount = 30;
	
	Text ammoText;

	void OnEnable () {
		ammoText = GameObject.Find ("AmmoText").GetComponent<Text> ();
		ammoText.text = bulletCount.ToString ();
	}

	void Awake () {
		bulletCount = bulletMaxCount;
	}

	void Start () {
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
		RaycastHit[] hits = Physics.RaycastAll(ray, fireRange).OrderBy(h=>h.distance).ToArray();

		foreach (RaycastHit hit in hits) {

			if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground") {
				Destroy (Instantiate (bulletHitPrefab, hit.point, Quaternion.FromToRotation (hit.collider.transform.forward, hit.normal)), 2f);
				break;
			}
			
			if (hit.collider.tag == "Barrel") {
				hit.collider.SendMessage ("Broken", hit.point);
				break;
			}

			if (hit.collider.tag == "Enemy" || hit.collider.tag == "Bot") {
				PlayerSyncShoot.CmdTellServerWhoWasShot (hit.collider.name, fireDamage);
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
				ammoText.text = bulletCount.ToString ();
				return true;
			}
			if (bulletCount < 0) {
				bulletCount = 0;
				ammoText.text = bulletCount.ToString ();
				SendMessage ("PlaySound", "NoAmmo");
				return false;
			}
			ammoText.text = bulletCount.ToString ();
			SendMessage ("PlaySound", "Bullet");
			GunAttack ();
		}
		else {
			ammoText.text = "99";
			SendMessage ("PlaySound", "Bullet");
			KnifeAttack ();
		}
		return true;
	}
}
