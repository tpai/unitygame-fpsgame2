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

	private bool shouldDie = false;
	public bool isDead = false;
	
	public delegate void DieDelegate ();
	public event DieDelegate EventDie;

	public delegate void RespawnDelegate ();
	public event RespawnDelegate EventRespawn;
	
	void Update () {
		CheckCondition ();
	}
	
	void CheckCondition () {
		if (!isDead && nowHP <= 0) {
			if (EventDie != null) {
				EventDie ();
			}
			isDead = true;

			Invoke ("ResetHealth", 3f);
		}
		 
		if (isDead && nowHP > 0) {
			if (EventRespawn != null) {
				EventRespawn ();
			}
			isDead = false;
		}
	}

	public void ResetHealth () {
		nowHP = maxHP;
	}
}
