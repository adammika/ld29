using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	[SerializeField] private GUIText _highScoreLabel;
	
	void Update() {
		if (Input.GetKeyDown("space")) {
			Color fadeColor = new Color();
			fadeColor.r = 27 / 255.0f;
			fadeColor.g = 38 / 255.0f;
			fadeColor.b = 50 / 255.0f;
			fadeColor.a = 1.0f;
			AutoFade.LoadLevel("GameScene", 1.0f, 1.0f, fadeColor);
		}
		if (Input.GetKeyDown("t")) {
			Application.LoadLevel("Tutorial1");
		}
	}

	void OnGUI() {
		this._highScoreLabel.text = "HIGHSCORE " + PlayerPrefs.GetInt("HighScore");
	}
}
