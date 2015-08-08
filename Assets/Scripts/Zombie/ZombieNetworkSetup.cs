using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZombieNetworkSetup : NetworkBehaviour {

	[SyncVar] public string zombieID;
	private Transform myTransform;

	void Awake () {
		myTransform = transform;
	}
	
	void Update () {
		SetIdentity ();
	}

	void SetIdentity () {
		if (myTransform.name == "" || myTransform.name == "Zombie(Clone)") {
			myTransform.name = zombieID;
		}
	}
}
