using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

	[SyncVar] public string playerUniqueName;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;

	public override void OnStartLocalPlayer ()
	{
		GetNetIdentity ();
		SetIdentity ();
	}

	[Client]
	void GetNetIdentity () {
		playerNetID = GetComponent<NetworkIdentity> ().netId;
		CmdTellServerMyIdentity ("Player " + playerNetID.ToString ());
	}

	[Client]
	void SetIdentity () {
		if (!isLocalPlayer) {
			myTransform.name = playerUniqueName;
			myTransform.tag = "Enemy";
		} else {
			myTransform.name = "Player " + playerNetID.ToString ();
		}
	}

	[Command]
	void CmdTellServerMyIdentity (string identity) {
		playerUniqueName = identity;
	}

	void Update () {
		if (myTransform.name == "" || myTransform.name == "Player(Clone)") {
			SetIdentity ();
		}
	}

	[SerializeField] Camera fpsCamera;
	[SerializeField] AudioListener audioListener;

	void Awake () {
		myTransform = transform;
	}

	void Start () {
		if (isLocalPlayer) {
			// turn off scene camera
			GameObject.Find ("SceneCamera").GetComponent<Camera>().enabled = false;
			GameObject.Find ("PlayerHUD").GetComponent<Canvas> ().enabled = true;
			// turn character on
			fpsCamera.enabled = true;
			audioListener.enabled = true;
			GetComponent <PlayerAnim> ().enabled = true;
			GetComponent <UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
		}
	}
}