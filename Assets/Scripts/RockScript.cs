﻿using UnityEngine;
using System.Collections;

public class RockScript : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			GameManager.Stop();
			Application.LoadLevel("GameOverScene");
		}
	}
}
