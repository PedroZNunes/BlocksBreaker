using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SpriteRenderer))]
public class BallTrailFade : MonoBehaviour {

	[SerializeField] float fadePerFrame = 0.02f;

	private SpriteRenderer spriteRenderer;


	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void Update () {
		if (spriteRenderer.color.a > 0f) {
			Color newColor = spriteRenderer.color;
			newColor.a -= fadePerFrame;
			spriteRenderer.color = newColor;
		} else {
			Destroy (gameObject);
		}
	}
}
