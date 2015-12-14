using UnityEngine;
using System.Collections;

	// ParticleSoundTrigger allows to trigger sounds on particle collisions on any object of the world
	// attach the script to
	// As particle collisions can be very greedy setting CollideSoundPercent allow to reduce the percentage of collisions which effectively trigger sound samples
	// At each particle collision the script chooses among a max of ten AudioClips (choose very short sounds if possible and to avoid clicks each sample should be shaped with it’s own enveloppe)

public class ParticleSoundTrigger : MonoBehaviour {
	public AudioClip [] sounds = new AudioClip[10]; // le tableau de choix des sons
	public int CollideSoundPercent = 10;
	private int monson = 0;
	private int x;



	// Update is called once per frame
	void OnParticleCollision(GameObject other) {

		x = Random.Range (0, 100);
				if (x < CollideSoundPercent) {

						monson = Random.Range (0, sounds.Length);

						AudioSource.PlayClipAtPoint(sounds [monson], new Vector3(5, 1, 2));
						//collidersound =(sounds[monson]);
						GetComponent<AudioSource>().pitch = Random.Range (0.5F, 2F);
						GetComponent<AudioSource>().volume = 0.2F;
				}
	}
}
 