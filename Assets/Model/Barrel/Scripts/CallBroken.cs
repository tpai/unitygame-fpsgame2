using UnityEngine;
using System.Collections;

public class CallBroken : MonoBehaviour {

	void Broken (Vector3 point) {
		GetComponent<MeshRenderer> ().enabled = false;
		GetComponent<MeshCollider> ().enabled = false;
		transform.parent.SendMessage ("Broken", point);
	}
}
