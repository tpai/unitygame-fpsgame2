using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSyncShoot : NetworkBehaviour {

	[Command]
	public void CmdTellServerWhoWasShot (string uniqueID, int damage) {
		GameObject go = GameObject.Find (uniqueID);
		go.GetComponent<HP> ().AddHP (-damage);
	}
}
