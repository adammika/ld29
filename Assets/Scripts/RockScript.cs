using UnityEngine;
using System.Collections;

public class RockScript : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			GameManager.Stop();

			Color fadeColor = new Color();
			fadeColor.r = 27 / 255.0f;
			fadeColor.g = 38 / 255.0f;
			fadeColor.b = 50 / 255.0f;
			fadeColor.a = 1.0f;
			AutoFade.LoadLevel("GameOverScene", 1.0f, 1.0f, fadeColor);
		}
	}
}
