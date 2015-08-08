using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class HP : NetworkBehaviour {

	[SyncVar (hook="OnHPChanged")] public int nowHP;
	public int maxHP = 100;
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

		if (transform.tag != "Player")
			return;

		SetHealthText ();
	}

	void SetHealthText () {
		if (hpText == null)
			hpText = GameObject.Find ("HealthText").GetComponent<Text> ();

		if (isLocalPlayer)
			hpText.text = nowHP.ToString ();
	}

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
			isDead = true;

			if (transform.tag == "Bot") {
				Destroy (gameObject);
				return ;
			}

			if (EventDie != null) {
				EventDie ();
			}

			Invoke ("ResetHealth", 3f);
		}
		 
		if (isDead && nowHP > 0) {
			isDead = false;

			if (transform.tag == "Bot")return ;

			if (EventRespawn != null) {
				EventRespawn ();
			}
		}
	}

	public void ResetHealth () {
		nowHP = maxHP;
	}
}
