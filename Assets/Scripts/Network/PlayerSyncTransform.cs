using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[NetworkSettings (channel=0, sendInterval=.033f)]
public class PlayerSyncTransform : NetworkBehaviour {
	
	[SyncVar (hook="SyncPosValues")] Vector3 syncPos;
	[SyncVar (hook="SyncMyRotValues")] Quaternion syncMyRot;
	[SyncVar (hook="SyncCamRotValues")] Quaternion syncCamRot;
	
	[SerializeField] Transform myTransform;
	[SerializeField] Transform camTransform;

	[SerializeField] bool useHistoricalLerping;
	List<Vector3> syncPosList = new List<Vector3>();
	List<Quaternion> syncMyRotList = new List<Quaternion>();
	List<Quaternion> syncCamRotList = new List<Quaternion>();
	float lerpPosRate;
	float lerpRotRate;
	float normalLerpRate = 16f;
	float fasterLerpRate = 27f;
	float posCloseEnough = .11f;
	float rotCloseEnough = 5f;

	Vector3 lastPos;
	float posThreshold = .5f;
	Quaternion lastMyRot;
	Quaternion lastCamRot;
	float rotThreshold = 1f;

	void Start () {
		lerpPosRate = normalLerpRate;
		lerpRotRate = normalLerpRate;
	}

	void Update () {
		LerpPosRot ();
	}

	void FixedUpdate () {
		TransmitPosRot ();
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
	
	[Command]
	void CmdProvidePosRotToServer (Vector3 pos, Quaternion charRot, Quaternion camRot) {
		syncPos = pos;
		syncMyRot = charRot;
		syncCamRot = camRot;
	}

	void LerpPosRot () {
		if (!isLocalPlayer) {
			if (useHistoricalLerping)
				HistoricalLerping ();
			else
				OrdinaryLerping ();
		}
	}

	void OrdinaryLerping () {
		myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpPosRate);
		myTransform.rotation = Quaternion.Slerp (myTransform.rotation, syncMyRot, Time.deltaTime * lerpRotRate);
		camTransform.rotation = Quaternion.Slerp (camTransform.rotation, syncCamRot, Time.deltaTime * lerpRotRate);
	}

	void HistoricalLerping () {
		if (syncPosList.Count > 0) {
			myTransform.position = Vector3.Lerp (myTransform.position, syncPosList[0], Time.deltaTime * lerpPosRate);

			if (Vector3.Distance (myTransform.position, syncPosList[0]) < posCloseEnough) {
				syncPosList.RemoveAt (0);
			}

			if (syncPosList.Count > 10) {
				lerpPosRate = fasterLerpRate;
			}
			else {
				lerpPosRate = normalLerpRate;
			}
		}

		if (syncMyRotList.Count > 0) {
			myTransform.rotation = Quaternion.Slerp (myTransform.rotation, syncMyRotList[0], Time.deltaTime * lerpRotRate);

			if (Quaternion.Angle (myTransform.rotation, syncMyRotList[0]) < rotCloseEnough) {
				syncMyRotList.RemoveAt (0);
			}

			if (syncMyRotList.Count > 10) {
				lerpRotRate = fasterLerpRate;
			}
			else {
				lerpRotRate = normalLerpRate;
			}
		}

		if (syncCamRotList.Count > 0) {
			camTransform.rotation = Quaternion.Slerp (camTransform.rotation, syncCamRotList[0], Time.deltaTime * lerpRotRate);

			if (Quaternion.Angle (camTransform.rotation, syncCamRotList[0]) < rotCloseEnough) {
				syncCamRotList.RemoveAt (0);
			}

			if (syncCamRotList.Count > 10) {
				lerpRotRate = fasterLerpRate;
			}
			else {
				lerpRotRate = normalLerpRate;
			}
		}
	}

	// ==========

	[Client]
	void SyncPosValues (Vector3 latestPos) {
		syncPos = latestPos;
		syncPosList.Add (latestPos);
	}

	[Client]
	void SyncMyRotValues (Quaternion latestRot) {
		syncMyRot = latestRot;
		syncMyRotList.Add (latestRot);
	}

	[Client]
	void SyncCamRotValues (Quaternion latestRot) {
		syncCamRot = latestRot;
		syncCamRotList.Add (latestRot);
	}
}
