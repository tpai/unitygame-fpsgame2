using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncTransform : NetworkBehaviour {
	
	[SyncVar] Vector3 syncPos;
	[SyncVar] Quaternion syncCamRot;
	[SyncVar] Quaternion syncCharRot;
	
	[SerializeField] Transform myTransform;
	[SerializeField] Transform camTransform;
	[SerializeField] float lerpRate = 15;
	
	void FixedUpdate () {
		TransmitPosRot ();
		LerpPosRot ();
	}
	
	void LerpPosRot () {
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);
			myTransform.rotation = Quaternion.Slerp (myTransform.rotation, syncCharRot, Time.deltaTime * lerpRate);
			camTransform.rotation = Quaternion.Slerp (camTransform.rotation, syncCamRot, Time.deltaTime * lerpRate);
		}
	}
	
	[Command]
	void CmdProvidePosRotToServer (Vector3 pos, Quaternion charRot, Quaternion camRot) {
		syncPos = pos;
		syncCharRot = charRot;
		syncCamRot = camRot;
	}
	
	[Client]
	void TransmitPosRot () {
		if (isLocalPlayer) {
			CmdProvidePosRotToServer (myTransform.position, myTransform.rotation, camTransform.rotation);
		}
	}
}
