using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	[SerializeField] private GUIText _scoreLabel;
	[SerializeField] private GUIText _newHighScoreLabel;

	private int _gameScore;
	private int _highScore;

	// Use this for initialization
	void Start () {
		_gameScore = PlayerPrefs.GetInt("GameScore");
		_highScore = PlayerPrefs.GetInt("HighScore");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Application.LoadLevel("Title");
		}
	}

	void OnGUI() {
		this._scoreLabel.text = "SCORE " + _gameScore;

		if (_gameScore == _highScore) {
			_newHighScoreLabel.text = "NEW HIGHSCORE";
		}
		else {
			_newHighScoreLabel.text = "";
		}

	}
}
