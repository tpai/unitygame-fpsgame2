using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public int bulletCount;
	public int bulletMaxCount = 30;

	void Start () {
		bulletCount = bulletMaxCount;
	}
	
	public bool AddBullet (int amt) {
		bulletCount += amt;
		if (bulletCount <= 0) {
			bulletCount = 0;
			return false;
		}
		return true;
	}
}
