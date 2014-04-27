using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	[SerializeField] private GUIText _highScoreLabel;
	
	void Update() {
		if (Input.GetKeyDown("space")) {
			Application.LoadLevel("GameScene");
		}
		if (Input.GetKeyDown("t")) {
			Application.LoadLevel("Tutorial1");
		}
	}

	void OnGUI() {
		this._highScoreLabel.text = "HIGHSCORE " + PlayerPrefs.GetInt("HighScore");
	}
}
