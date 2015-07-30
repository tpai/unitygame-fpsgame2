using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncTransform : NetworkBehaviour {
	
	[SyncVar] Vector3 syncPos;
	[SyncVar] Quaternion syncMyRot;
	[SyncVar] Quaternion syncCamRot;
	
	[SerializeField] Transform myTransform;
	[SerializeField] Transform camTransform;
	
	float lerpRate = 15;

	Vector3 lastPos;
	float posThreshold = .5f;
	Quaternion lastMyRot;
	Quaternion lastCamRot;
	float rotThreshold = 5f;

	void Update () {
		LerpPosRot ();
	}

	void FixedUpdate () {
		TransmitPosRot ();
	}

	void LerpPosRot () {
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);
			myTransform.rotation = Quaternion.Slerp (myTransform.rotation, syncMyRot, Time.deltaTime * lerpRate);
			camTransform.rotation = Quaternion.Slerp (camTransform.rotation, syncCamRot, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdProvidePosRotToServer (Vector3 pos, Quaternion charRot, Quaternion camRot) {
		syncPos = pos;
		syncMyRot = charRot;
		syncCamRot = camRot;
	}
	
	[Client]
	void TransmitPosRot () {
		if (isLocalPlayer) {
			if (
				Vector3.Distance (myTransform.position, lastPos) > posThreshold ||
				Quaternion.Angle (myTransform.rotation, lastMyRot) > rotThreshold ||
				Quaternion.Angle (camTransform.rotation, lastCamRot) > rotThreshold
			) {
				CmdProvidePosRotToServer (myTransform.position, myTransform.rotation, camTransform.rotation);
				lastPos = myTransform.position;
				lastMyRot = myTransform.rotation;
				lastCamRot = camTransform.rotation;
			}
		}
	}
}
