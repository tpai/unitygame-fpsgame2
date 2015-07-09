using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class WeaponSound : MonoBehaviour {

	AudioSource audioSource;
	public AudioClip[] bulletSounds;
	public AudioClip reloadSound;
	public AudioClip noAmmoSound;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	public void PlaySound (string clip) {
		switch (clip) {
		case "Bullet":
			audioSource.PlayOneShot (bulletSounds [Random.Range (0, bulletSounds.Length)]);
			break;
		case "Reload":
			audioSource.PlayOneShot (reloadSound);
			break;
		case "NoAmmo":
			audioSource.PlayOneShot (noAmmoSound);
			break;
		}
	}
}
