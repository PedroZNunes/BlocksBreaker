using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExplosiveBallEffect : MonoBehaviour {

	private AudioSource audioSource;
	[SerializeField] private AudioClip tickSound, explosionSound;
	private float radius = 0.7f;
	private float finalRadius = 0f;
	[SerializeField] LayerMask layerMask;
	private float explosionSize;

	void Start(){
		audioSource = GetComponent<AudioSource> ();
	}

	public void Explode(){
		transform.localScale *= explosionSize;
		audioSource.PlayOneShot (explosionSound, 0.4f);
		Collider2D[] blocksHit = Physics2D.OverlapCircleAll (this.transform.position, finalRadius, layerMask);
		if (blocksHit.Length > 0) {
			for (int i = 0; i < blocksHit.Length; i++) {
//				if (blocksHit [i].transform == this.transform.parent)
//					continue;
				Block block = blocksHit [i].GetComponent<Block> ();
				block.TakeHit ();
			}
		}
		Destroy (gameObject, audioSource.clip.length);
	}

	public void PlayTickSound(){
		audioSource.PlayOneShot (tickSound);
	}

	public void SetStackAttributes(float multiplier){
		finalRadius = radius * multiplier;
		explosionSize = multiplier;
	}


}
