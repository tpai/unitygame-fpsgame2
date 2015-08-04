using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHP : NetworkBehaviour {

	[SyncVar (hook="OnHPChanged")] public int nowHP;
	int maxHP = 100;
	Text hpText;

	private bool shouldDie = false;
	public bool isDead = false;

	public delegate void DieDelegate ();
	public event DieDelegate EventDie;

	void Start () {
		nowHP = maxHP;
		SetHealthText ();
	}

	void Update () {
		CheckCondition ();
	}

	void CheckCondition () {
		if (!shouldDie && !isDead && nowHP <= 0)
			shouldDie = true;

		if (shouldDie && nowHP <= 0) {
			if (EventDie != null) {
				EventDie ();
			}
			shouldDie = false;
		}
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
