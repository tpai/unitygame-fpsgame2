using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZombieMovement : NetworkBehaviour {

	NavMeshAgent navAgent;
	Transform myTransform;
	public Transform targetTransform;
	LayerMask raycastLayer;
	public float radius = 100f;

	void Start () {
		navAgent = GetComponent<NavMeshAgent> ();
		myTransform = transform;
		raycastLayer = 1 << LayerMask.NameToLayer ("Player");
		StartCoroutine (DoCheck ());
	}

	void SearchForTarget () {
		if (!isServer)
			return;

		if (targetTransform == null) {
			Collider[] hits = Physics.OverlapSphere (myTransform.position, radius, raycastLayer);

			if (hits.Length > 0) {
				int randomInt = Random.Range(0, hits.Length);
				Transform closestTarget = hits[randomInt].transform;
				foreach (Collider hit in hits) {
					if (Vector3.Distance (myTransform.position, hit.transform.position) < 
					    Vector3.Distance (myTransform.position, closestTarget.position)
					) {
						closestTarget = hit.transform;
					}
				}
				targetTransform = closestTarget.transform;
			}
		}

		// lost target when character died
		if (targetTransform != null && targetTransform.GetComponent<HP> ().isDead) {
			targetTransform = null;
		}
	}

	void MoveToTarget () {
		if (isServer && targetTransform != null) {
			navAgent.SetDestination (Vector3.MoveTowards(targetTransform.position, myTransform.position, 1.5f));
			transform.LookAt (targetTransform.position);
		}
	}

	IEnumerator DoCheck () {
		for (;;) {
			SearchForTarget ();
			MoveToTarget ();
			yield return new WaitForSeconds (.2f);
		}
	}
}
