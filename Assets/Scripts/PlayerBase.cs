using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {

	PlayerAnim m_PlayerAnim;
	public PlayerAnim PlayerAnim {
		get {
			if (m_PlayerAnim == null) {
				m_PlayerAnim = GetComponent<PlayerAnim> ();
			}
			return m_PlayerAnim;
		}
	}

	Backpack m_Backpack;
	public Backpack Backpack {
		get {
			if (m_Backpack == null) {
				m_Backpack = GetComponent<Backpack> ();
			}
			return m_Backpack;
		}
	}

	Weapon m_Weapon;
	public Weapon Weapon {
		get {
			if (m_Weapon == null) {
				m_Weapon = GetComponentInChildren<Weapon> ();
			}
			return m_Weapon;
		}
		set {
			m_Weapon = value;
		}
	}

	// ===== Network =====

	PlayerSyncShoot m_PlayerSyncShoot;
	public PlayerSyncShoot PlayerSyncShoot {
		get {
			if (m_PlayerSyncShoot == null) {
				m_PlayerSyncShoot = GetComponentInParent <PlayerSyncShoot> ();
			}
			return m_PlayerSyncShoot;
		}
	}
}
