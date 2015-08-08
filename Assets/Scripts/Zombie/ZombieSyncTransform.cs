using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZombieSyncTransform : NetworkBehaviour {

	[SyncVar] Vector3 syncPos;
	[SyncVar] Quaternion syncRot;

	Vector3 lastPos;
	Quaternion lastRot;

	float posThreshold = .5f;
	float rotThreshold = 1f;

	void FixedUpdate () {
		if (
			Vector3.Distance (transform.position, lastPos) > posThreshold ||
			Quaternion.Angle (transform.rotation, lastRot) > rotThreshold
		) {
			lastPos = transform.position;
			lastRot = transform.rotation;

			syncPos = lastPos;
			syncRot = lastRot;
		}
	}

	void Update () {
		if (!isServer) {
			transform.position = Vector3.Lerp (transform.position, syncPos, Time.deltaTime * 10f);
			transform.rotation = Quaternion.Slerp (transform.rotation, syncRot, Time.deltaTime * 10f);
		}
	}
}
