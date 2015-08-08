using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameManagerZombieSpawner : NetworkBehaviour {

	[SerializeField] GameObject zombiePrefab;
	[SerializeField] Transform zombieSpawn;
	public int numberOfZombies = 10;
	int counter;

	public override void OnStartServer () {

		StartCoroutine ( SpawnZombie () );
	}

	IEnumerator SpawnZombie () {
		for (int i=0; i<numberOfZombies; i++) {
			GameObject go = (GameObject)Instantiate (zombiePrefab, zombieSpawn.position, Quaternion.identity);
			go.GetComponent<ZombieNetworkSetup> ().zombieID = "Zombie "+(i+1);
			NetworkServer.Spawn (go);
			yield return new WaitForSeconds (1f);
		}
	}
}
