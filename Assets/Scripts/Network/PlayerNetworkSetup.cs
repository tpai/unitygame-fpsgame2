using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {
	
	[SerializeField] Camera fpsCamera;
	[SerializeField] AudioListener audioListener;
	
	void Start () {
		if (isLocalPlayer) {
			// turn off scene camera
			GameObject.Find ("SceneCamera").SetActive (false);
			// turn character on
			fpsCamera.enabled = true;
			audioListener.enabled = true;
			GetComponent <CharacterController>().enabled = true;
			GetComponent <UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
		}
	}
}