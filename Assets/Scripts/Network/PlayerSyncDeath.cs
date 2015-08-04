using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSyncDeath : NetworkBehaviour {

	PlayerHP hpScript;

	void Start () {
		hpScript = GetComponent<PlayerHP> ();
		hpScript.EventDie += DisablePlayer;
	}

	void DisablePlayer () {
		ShowPlayer (false);

		hpScript.isDead = true;
		GetComponentInChildren<Weapon> ().enabled = false;

		if (isLocalPlayer) {
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = false;
			GetComponent<CharacterController> ().enabled = false;
			GetComponentInChildren<Camera>().enabled = false;
			GameObject.Find ("SceneCamera").GetComponent<Camera>().enabled = true;
		}
	}

	void ShowPlayer (bool show) {
		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer> ())
			renderer.enabled = show;

		foreach (SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer> ())
			renderer.enabled = show;
	}
}
