using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSyncDeath : NetworkBehaviour {

	PlayerHP hpScript;

	void Start () {
		hpScript = GetComponent<PlayerHP> ();
		hpScript.EventDie += DisablePlayerNow;
		hpScript.EventRespawn += EnablePlayerNow;
	}

	void EnablePlayerNow () {
		DisablePlayer (false);
	}

	void DisablePlayerNow () {
		DisablePlayer (true);
	}

	void DisablePlayer (bool b) {
		ShowPlayer (!b);

		hpScript.isDead = b;
		GetComponentInChildren<Weapon> ().enabled = !b;

		if (isLocalPlayer) {
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = !b;
			GetComponent<CharacterController> ().enabled = !b;
			GetComponentInChildren<Camera>().enabled = !b;
			GameObject.Find ("PlayerHUD").GetComponent<Canvas>().enabled = !b;
			GameObject.Find ("SceneCamera").GetComponent<Camera>().enabled = b;
		}

		if (b) {
			NetworkStartPosition[] points = GameObject.Find ("SpawnPoints").GetComponentsInChildren<NetworkStartPosition> ();
			transform.position = points[Random.Range (0, points.Length)].transform.position;
		}
	}

	void ShowPlayer (bool show) {
		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer> ())
			renderer.enabled = show;

		foreach (SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer> ())
			renderer.enabled = show;
	}
}
