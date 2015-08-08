using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZombieAttack : NetworkBehaviour {

	Transform myTransform;
	ZombieMovement movScript;

	float minDistance = 2f;
	float currentDistance;

	float attackRate = 3f;
	float nextAttack;

	public int damage = 30;

	[SerializeField] Material zombieGreen;
	[SerializeField] Material zombieRed;

	void Start () {
		myTransform = transform;
		movScript = GetComponent<ZombieMovement> ();

		if (isServer)
			StartCoroutine ( Attack () );
	}

	IEnumerator Attack () {
		for (;;) {
			yield return new WaitForSeconds (.2f);
			CheckIfTargetInRange ();
		}
	}

	void CheckIfTargetInRange () {
		if (movScript.targetTransform != null) {
			currentDistance = Vector3.Distance (movScript.targetTransform.position, myTransform.position);

			if (currentDistance < minDistance && Time.time > nextAttack) {
				nextAttack = Time.time + attackRate;

				movScript.targetTransform.GetComponent<HP>().AddHP (-damage);
				StartCoroutine ( ChangeZombieMat () ); // for the host player
				RpcChangeZombieAppearance ();
			}
		}
	}

	IEnumerator ChangeZombieMat () {
		GetComponent<Renderer> ().material = zombieRed;
		yield return new WaitForSeconds (attackRate / 2f);
		GetComponent<Renderer> ().material = zombieGreen;
	}

	[ClientRpc]
	void RpcChangeZombieAppearance () {
		StartCoroutine ( ChangeZombieMat ());
	}
}
