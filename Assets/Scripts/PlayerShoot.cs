using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

	Transform gunTop;

	void Start () {
		Cursor.visible = false;
		gunTop = transform.Find ("Character/Weapon/GunTop");
	}
	
	void Update () {
		Debug.DrawRay (
			gunTop.position,
			gunTop.forward * 100f,
			Color.green
		);
	}
}
