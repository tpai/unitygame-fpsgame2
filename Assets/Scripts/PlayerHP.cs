using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHP : NetworkBehaviour {

	[SyncVar (hook="OnHPChanged")] public int nowHP;
	int maxHP = 100;
	Text hpText;

	void Start () {
		nowHP = maxHP;
		SetHealthText ();
	}

	public void AddHP (int amt) {
		nowHP += amt;

		if (nowHP > maxHP)
			nowHP = maxHP;
		if (nowHP < 0)
			nowHP = 0;
	}

	void OnHPChanged (int hp) {
		nowHP = hp;
		SetHealthText ();
	}

	void SetHealthText () {
		if (hpText == null)
			hpText = GameObject.Find ("HealthText").GetComponent<Text> ();

		if (isLocalPlayer)
			hpText.text = nowHP.ToString ();
	}
}
