using UnityEngine;
using System.Collections;

public class Backpack : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			transform.Find ("Character/M16").gameObject.SetActive (false);
			transform.Find ("Character/Magnum").gameObject.SetActive (true);
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			transform.Find ("Character/Magnum").gameObject.SetActive (false);
			transform.Find ("Character/M16").gameObject.SetActive (true);
		}
	}
}
